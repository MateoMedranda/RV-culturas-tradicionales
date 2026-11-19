using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using RvCulturas.Domain;
using RvCulturas.Application.UseCases;
using RvCulturas.Infrastructure.Content;
using RvCulturas.Presentation.Interaction;
using UnityEngine.XR;
using XrUsages = UnityEngine.XR.CommonUsages;

namespace RvCulturas.Presentation.UI
{
    /// <summary>Unity adapter: lesson rules remain in the existing domain/use cases.</summary>
    [DefaultExecutionOrder(100)]
    public sealed class KitchenLessonPresenter : MonoBehaviour
    {
        public ExperienceDefinitionAsset lesson;
        public KitchenPlayer player;
        public KitchenTarget[] targets;
        public Renderer liquid;
        public TextMesh xrGuide;
        private readonly ExperienceSessionState session = new ExperienceSessionState();
        private readonly AttemptInteractionUseCase attempt = new AttemptInteractionUseCase();
        private ExperienceDefinition definition;
        private KitchenTarget hovered;
        private LineRenderer outline;
        private string feedback = "Bienvenida/o. Explora a tu ritmo; no hay límite de tiempo.";
        private bool settings, busy, sound = true;
        private float textScale = 1;
        private GUIStyle title, body, small, button;
        private AudioSource audioSource;
        private AudioClip cue;
        private readonly Vector3[] outlinePoints = new Vector3[16];
        private float styledScale;
        private bool previousPrimary, previousSecondary, previousLeftPrimary;
        private LineRenderer xrPointer;
        private TextMesh xrHover;
        public ExperienceLifecycle Lifecycle => session.Lifecycle;
        public int Step => session.CurrentStepOrdinal;
        private void Awake()
        {
            definition = lesson.ToDomain();
            var issues = definition.Validate(false);
            if (issues.Count > 0) { Debug.LogError(string.Join("; ", issues)); enabled = false; return; }
            outline = new GameObject("Selección amarilla").AddComponent<LineRenderer>();
            outline.transform.SetParent(transform);
            outline.sharedMaterial = new Material(Shader.Find("Universal Render Pipeline/Unlit"));
            outline.sharedMaterial.SetColor("_BaseColor", new Color(1, 0.82f, 0.05f));
            outline.widthMultiplier = 0.009f; outline.positionCount = 16; outline.enabled = false;
            xrPointer = new GameObject("Puntero XR").AddComponent<LineRenderer>(); xrPointer.transform.SetParent(transform);
            xrPointer.sharedMaterial = outline.sharedMaterial; xrPointer.positionCount = 2; xrPointer.widthMultiplier = 0.003f;
            xrHover = new GameObject("Nombre del objeto XR").AddComponent<TextMesh>(); xrHover.transform.SetParent(transform);
            xrHover.fontSize = 64; xrHover.characterSize = 0.006f; xrHover.anchor = TextAnchor.MiddleCenter; xrHover.color = new Color(1,0.85f,0.15f);
            audioSource = gameObject.AddComponent<AudioSource>();
            cue = AudioClip.Create("Confirmación suave", 4410, 1, 44100, false);
            var samples = new float[4410];
            for (var i = 0; i < samples.Length; i++) samples[i] = Mathf.Sin(i * 2 * Mathf.PI * 660 / 44100) * 0.08f * (1 - i / 4410f);
            cue.SetData(samples, 0);
            if (liquid != null) liquid.enabled = false;
        }
        private void Update()
        {
            if (player.IsXr)
            {
                var right = InputDevices.GetDeviceAtXRNode(XRNode.RightHand); var left = InputDevices.GetDeviceAtXRNode(XRNode.LeftHand);
                right.TryGetFeatureValue(XrUsages.primaryButton, out var primary);
                right.TryGetFeatureValue(XrUsages.secondaryButton, out var secondary);
                left.TryGetFeatureValue(XrUsages.primaryButton, out var leftPrimary);
                if (primary && !previousPrimary) Pause();
                if (secondary && !previousSecondary) textScale = textScale < 1.1f ? 1.2f : 1;
                if (leftPrimary && !previousLeftPrimary) player.seated = !player.seated;
                previousPrimary = primary; previousSecondary = secondary; previousLeftPrimary = leftPrimary;
            }
            var key = Keyboard.current;
            if (key != null)
            {
                if (key.spaceKey.wasPressedThisFrame && session.Lifecycle == ExperienceLifecycle.Intro) Begin();
                if (key.escapeKey.wasPressedThisFrame || key.pKey.wasPressedThisFrame) Pause();
                if (key.rKey.wasPressedThisFrame && !busy) ResetLesson();
                if (key.f1Key.wasPressedThisFrame) settings = !settings;
                if (key.digit1Key.wasPressedThisFrame) SelectIndex(0);
                if (key.digit2Key.wasPressedThisFrame) SelectIndex(1);
                if (key.digit3Key.wasPressedThisFrame) SelectIndex(2);
                if (key.digit4Key.wasPressedThisFrame) SelectIndex(3);
                if (key.digit5Key.wasPressedThisFrame) SelectIndex(4);
                if (key.digit6Key.wasPressedThisFrame) SelectIndex(5);
            }
            player.movementLocked = settings || session.Lifecycle == ExperienceLifecycle.Paused;
            hovered = null;
            if (!settings && Physics.Raycast(player.Pointer, out var hit, 3.2f, ~0, QueryTriggerInteraction.Ignore)) hovered = hit.collider.GetComponentInParent<KitchenTarget>();
            outline.enabled = hovered != null;
            xrPointer.enabled = player.IsXr;
            xrHover.gameObject.SetActive(player.IsXr && hovered != null);
            if (player.IsXr)
            {
                xrPointer.SetPosition(0, player.Pointer.origin); xrPointer.SetPosition(1, player.Pointer.GetPoint(hovered != null ? Vector3.Distance(player.Pointer.origin, hovered.VisualBounds.center) : 2));
                if (hovered != null) { xrHover.text = hovered.displayName; xrHover.transform.SetPositionAndRotation(hovered.VisualBounds.center + Vector3.up * (hovered.VisualBounds.extents.y + 0.12f), player.view.transform.rotation); }
            }
            if (hovered != null) DrawOutline(hovered.VisualBounds);
            if (player.Selected && (player.IsXr || IsWorldPointer()))
            {
                if (player.IsXr && session.Lifecycle == ExperienceLifecycle.Intro) Begin();
                else if (player.IsXr && session.Lifecycle == ExperienceLifecycle.Completed) ResetLesson();
                else if (hovered != null) Select(hovered);
            }
            if (xrGuide != null)
            {
                xrGuide.gameObject.SetActive(player.IsXr);
                xrGuide.characterSize = 0.0064f * textScale;
                var step = definition.GetStep(Step);
                xrGuide.text = session.Lifecycle == ExperienceLifecycle.Intro ? "CHICHA · TALLER DE PRÁCTICA\nGatillo: comenzar y seleccionar.\nA: pausa · B: ampliar texto · X: altura\nSin límite de tiempo." :
                    session.Lifecycle == ExperienceLifecycle.Completed ? "¡Práctica completada!\nPulsa el gatillo para volver a la bienvenida.\nSimulación educativa; receta pendiente de revisión." :
                    session.Lifecycle == ExperienceLifecycle.Paused ? "EN PAUSA\nPulsa el botón primario derecho para continuar." :
                    "PASO " + Step + "/6 · " + step?.Title + "\n" + Wrap(step?.VisualInstruction.Text, 48) + "\n" + Wrap(feedback, 48);
            }
        }
        private bool IsWorldPointer()
        {
            if (settings || Mouse.current == null) return false;
            var scale = Mathf.Min(Screen.width / 1440f, Screen.height / 900f);
            var y = Screen.height - Mouse.current.position.ReadValue().y;
            return y > 140 * scale && y < Screen.height - 245 * scale;
        }
        public void Begin() { session.Start(definition); feedback = "Sigue la indicación. Puedes seleccionar con ratón, E o números 1–6."; }
        public void SelectIndex(int index) { if (!settings && index >= 0 && index < targets.Length) Select(targets[index]); }
        public void Select(KitchenTarget target)
        {
            if (busy || session.Lifecycle != ExperienceLifecycle.Active) return;
            var step = definition.GetStep(Step);
            var result = attempt.Execute(session, definition, target.targetId, ExpectedAction.Use);
            if (result.Kind == RvCulturas.Application.Results.ExperienceResultKind.Rejected) { feedback = "Prueba con: " + step.Title + ". El progreso se conserva."; return; }
            feedback = "Correcto: " + target.displayName + ".";
            if (sound) audioSource.PlayOneShot(cue);
            StartCoroutine(AnimateAction(target));
        }
        private IEnumerator AnimateAction(KitchenTarget target)
        {
            busy = true;
            if (target.destination != null)
            {
                var start = target.transform.position; var rotation = target.transform.rotation;
                for (float t = 0; t < 1; t += Time.deltaTime * 1.6f)
                {
                    target.transform.position = Vector3.Lerp(start, target.destination.position, t) + Vector3.up * Mathf.Sin(t * Mathf.PI) * 0.18f;
                    target.transform.rotation = Quaternion.Slerp(rotation, target.destination.rotation, t);
                    yield return null;
                }
                target.Complete();
            }
            if (liquid != null) { liquid.enabled = true; liquid.material.color = Color.Lerp(new Color(0.72f, 0.56f, 0.27f), new Color(0.5f, 0.25f, 0.07f), Step / 6f); }
            busy = false;
        }
        public void Pause() { if (busy) return; if (session.Lifecycle == ExperienceLifecycle.Active) session.Pause(); else session.Resume(); }
        public void ResetLesson() { if (busy) return; session.Restart(); foreach (var t in targets) t.ResetPosition(); if (liquid != null) liquid.enabled = false; feedback = "Todo preparado para volver a practicar."; }
        private void DrawOutline(Bounds b)
        {
            b.Expand(0.035f); var a = b.min; var z = b.max;
            outlinePoints[0]=new Vector3(a.x,a.y,a.z); outlinePoints[1]=new Vector3(z.x,a.y,a.z); outlinePoints[2]=new Vector3(z.x,a.y,z.z); outlinePoints[3]=new Vector3(a.x,a.y,z.z);
            outlinePoints[4]=outlinePoints[0]; outlinePoints[5]=new Vector3(a.x,z.y,a.z); outlinePoints[6]=new Vector3(z.x,z.y,a.z); outlinePoints[7]=outlinePoints[1];
            outlinePoints[8]=outlinePoints[6]; outlinePoints[9]=new Vector3(z.x,z.y,z.z); outlinePoints[10]=outlinePoints[2]; outlinePoints[11]=outlinePoints[9];
            outlinePoints[12]=new Vector3(a.x,z.y,z.z); outlinePoints[13]=outlinePoints[3]; outlinePoints[14]=outlinePoints[12]; outlinePoints[15]=outlinePoints[5];
            outline.SetPositions(outlinePoints);
        }
        private void OnGUI()
        {
            if (player.IsXr) return;
            float scale = Mathf.Min(Screen.width / 1440f, Screen.height / 900f);
            GUI.matrix = Matrix4x4.TRS(Vector3.zero, Quaternion.identity, Vector3.one * scale);
            float width = Screen.width / scale, height = Screen.height / scale;
            if (title == null || styledScale != textScale)
            {
            styledScale = textScale;
            title = new GUIStyle(GUI.skin.label) {fontSize = Mathf.RoundToInt(30 * textScale), fontStyle = FontStyle.Bold, wordWrap = true, normal = {textColor = new Color(1, 0.85f, 0.46f)}};
            body = new GUIStyle(GUI.skin.label) {fontSize = Mathf.RoundToInt(24 * textScale), wordWrap = true, normal = {textColor = Color.white}};
            small = new GUIStyle(body) {fontSize = 21}; button = new GUIStyle(GUI.skin.button) {fontSize = 22, wordWrap = true};
            }
            Panel(new Rect(24, 22, width - 48, 76));
            GUI.Label(new Rect(44, 35, 830, 52), "SABERES A LA MESA  /  CHICHA", title);
            if (GUI.Button(new Rect(width - 294, 36, 250, 44), "Accesibilidad · F1", button)) settings = !settings;
            if (session.Lifecycle == ExperienceLifecycle.Intro || session.Lifecycle == ExperienceLifecycle.Completed)
            {
                const string introduction = "Recorrido ilustrativo de preparación con maíz, agua, panela y especias.\n\nLas recetas varían por comunidad. Esta simulación está pendiente de revisión cultural y no enseña fermentación real.";
                float contentHeight = body.CalcHeight(new GUIContent(introduction), 595);
                float buttonY = Mathf.Max(482, 284 + contentHeight);
                Panel(new Rect(40, 132, 650, buttonY - 132 + 80));
                GUI.Label(new Rect(64, 154, 600, 96), session.Lifecycle == ExperienceLifecycle.Intro ? "Una cocina para aprender\na tu ritmo" : "¡Completaste la práctica!", title);
                GUI.Label(new Rect(64, 260, 595, contentHeight + 8), introduction, body);
                if (GUI.Button(new Rect(64, buttonY, 590, 58), session.Lifecycle == ExperienceLifecycle.Intro ? "Comenzar · Espacio" : "Volver a practicar", button)) { if (session.Lifecycle == ExperienceLifecycle.Intro) Begin(); else ResetLesson(); }
            }
            else
            {
                Panel(new Rect(24, height - 245, width - 48, 222));
                var step = definition.GetStep(Step);
                GUI.Label(new Rect(48, height - 235, width - 96, 58), session.Lifecycle == ExperienceLifecycle.Paused ? "EN PAUSA · P para continuar" : "PASO " + Step + " / 6 · " + step?.Title, title);
                GUI.Label(new Rect(48, height - 174, width - 440, 82), step?.VisualInstruction.Text, body);
                GUI.Label(new Rect(48, height - 91, width - 440, 55), feedback, small);
                if (GUI.Button(new Rect(width - 356, height - 160, 300, 54), "Pausar / continuar · P", button)) Pause();
                if (GUI.Button(new Rect(width - 356, height - 96, 300, 54), "Reiniciar · R", button)) ResetLesson();
            }
            Panel(new Rect(24, 100, width - 48, 42));
            GUI.Label(new Rect(30, 104, width - 60, 34), "WASD / flechas: caminar   ·   clic derecho + ratón: mirar   ·   clic / E: usar   ·   1–6: selección asistida", small);
            if (hovered != null && session.Lifecycle == ExperienceLifecycle.Active)
            {
                var point = player.view.WorldToScreenPoint(hovered.VisualBounds.center + Vector3.up * (hovered.VisualBounds.extents.y + 0.12f));
                var x = Mathf.Clamp(point.x / scale - 190, 16, width - 396);
                var y = Mathf.Clamp((Screen.height - point.y) / scale - 65, 145, height - 330);
                Panel(new Rect(x, y, 380, 65)); GUI.Label(new Rect(x + 12, y + 7, 356, 54), hovered.displayName + " · usar", body);
            }
            if (settings)
            {
                Panel(new Rect(width / 2 - 330, 155, 660, 470));
                GUI.Label(new Rect(width / 2 - 300, 180, 600, 60), "ACCESIBILIDAD Y CONFORT", title);
                if (GUI.Button(new Rect(width / 2 - 300, 255, 600, 54), "Texto: " + (textScale < 1.1f ? "grande" : "extra grande"), button)) textScale = textScale < 1.1f ? 1.2f : 1;
                if (GUI.Button(new Rect(width / 2 - 300, 325, 600, 54), "Vista: " + (player.seated ? "sentado/a" : "de pie"), button)) player.seated = !player.seated;
                if (GUI.Button(new Rect(width / 2 - 300, 395, 600, 54), "Confirmaciones sonoras: " + (sound ? "sí" : "no"), button)) sound = !sound;
                GUI.Label(new Rect(width / 2 - 300, 465, 600, 70), "Sin temporizador. El sonido es opcional.\nNúmeros 1–6 permiten jugar sin apuntar.", small);
                if (GUI.Button(new Rect(width / 2 - 300, 550, 600, 54), "Cerrar · F1", button)) settings = false;
            }
            GUI.matrix = Matrix4x4.identity;
        }
        private static void Panel(Rect rect) { var color = GUI.color; GUI.color = new Color(0.035f, 0.065f, 0.075f, 0.97f); GUI.DrawTexture(rect, Texture2D.whiteTexture); GUI.color = color; }
        private static string Wrap(string value, int width)
        {
            if (string.IsNullOrEmpty(value)) return "";
            var words = value.Split(' '); var result = ""; var column = 0;
            foreach (var word in words) { if (column + word.Length > width) { result += "\n"; column = 0; } result += word + " "; column += word.Length + 1; }
            return result;
        }
        private void OnDestroy() { if (cue != null) Destroy(cue); if (outline != null) Destroy(outline.sharedMaterial); }
    }
}
