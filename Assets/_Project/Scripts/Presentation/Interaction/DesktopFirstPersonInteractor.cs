using UnityEngine;
using UnityEngine.InputSystem;
using RvCulturas.Presentation.Bootstrap;

namespace RvCulturas.Presentation.Interaction
{
    /// <summary>Interacción de escritorio accesible que emula apuntar y seleccionar en primera persona.</summary>
    public sealed class DesktopFirstPersonInteractor : MonoBehaviour
    {
        private static DesktopFirstPersonInteractor current;
        private ExperienceBootstrapper bootstrapper;
        private Camera view;
        private InteractionTargetPresenter hoveredTarget;
        private BoundsOutline outline;
        private GUIStyle tooltipStyle;

        public static bool IsActive => current != null && current.isActiveAndEnabled;

        public void Configure(ExperienceBootstrapper configuredBootstrapper, Camera configuredView)
        {
            bootstrapper = configuredBootstrapper;
            view = configuredView;
        }

        private void Awake()
        {
            current = this;
            outline = new BoundsOutline(transform);
        }

        private void OnDestroy()
        {
            if (current == this) current = null;
        }

        private void Update()
        {
            if (view == null) view = Camera.main;
            if (view == null) return;
            HandleExperienceControls();
            UpdateHover();
            var mouse = Mouse.current;
            if (mouse != null && mouse.leftButton.wasPressedThisFrame && hoveredTarget != null)
                hoveredTarget.AttemptInteraction();
        }

        private void HandleExperienceControls()
        {
            var keyboard = Keyboard.current;
            if (keyboard == null || bootstrapper == null) return;
            if (keyboard.spaceKey.wasPressedThisFrame) bootstrapper.StartExperience();
            if (keyboard.pKey.wasPressedThisFrame) bootstrapper.TogglePause();
            if (keyboard.gKey.wasPressedThisFrame) bootstrapper.RepeatGuidance();
            if (keyboard.rKey.wasPressedThisFrame) bootstrapper.RestartExperience();
            if (keyboard.cKey.wasPressedThisFrame) bootstrapper.CloseCulturalContext();
        }

        private void UpdateHover()
        {
            var mouse = Mouse.current;
            if (mouse == null) return;
            var point = Cursor.lockState == CursorLockMode.Locked
                ? new Vector2(Screen.width * 0.5f, Screen.height * 0.5f)
                : mouse.position.ReadValue();
            var ray = view.ScreenPointToRay(point);
            InteractionTargetPresenter candidate = null;
            if (Physics.Raycast(ray, out var hit, 20f)) candidate = hit.collider.GetComponentInParent<InteractionTargetPresenter>();
            if (candidate == hoveredTarget) return;
            hoveredTarget = candidate;
            if (hoveredTarget == null) outline.Hide();
            else outline.Show(hoveredTarget.GetComponent<Renderer>().bounds);
        }

        private void OnGUI()
        {
            if (hoveredTarget == null || view == null) return;
            if (tooltipStyle == null)
            {
                tooltipStyle = new GUIStyle(GUI.skin.box)
                {
                    fontSize = 30,
                    fontStyle = FontStyle.Bold,
                    alignment = TextAnchor.MiddleCenter,
                    normal = { textColor = new Color(1f, 0.9f, 0.2f) }
                };
            }

            var bounds = hoveredTarget.GetComponent<Renderer>().bounds;
            var screen = view.WorldToScreenPoint(bounds.center + Vector3.up * (bounds.extents.y + 0.22f));
            if (screen.z < 0f) return;
            var label = DisplayName(hoveredTarget.TargetId);
            GUI.backgroundColor = new Color(0.03f, 0.05f, 0.08f, 0.94f);
            GUI.Box(new Rect(screen.x - 170f, Screen.height - screen.y - 48f, 340f, 48f), label, tooltipStyle);
        }

        private static string DisplayName(string id)
        {
            if (id == "ingredient") return "Ingrediente de práctica";
            if (id == "bowl") return "Recipiente de práctica";
            if (id == "utensil") return "Utensilio de práctica";
            return "Elemento interactivo";
        }

        private sealed class BoundsOutline
        {
            private readonly LineRenderer[] edges = new LineRenderer[12];
            private readonly Material material;

            public BoundsOutline(Transform parent)
            {
                var shader = Shader.Find("Universal Render Pipeline/Unlit") ?? Shader.Find("Sprites/Default");
                material = new Material(shader) { color = new Color(1f, 0.9f, 0.05f) };
                if (material.HasProperty("_BaseColor")) material.SetColor("_BaseColor", new Color(1f, 0.9f, 0.05f));
                for (var index = 0; index < edges.Length; index++)
                {
                    var edge = new GameObject("HoverOutlineEdge");
                    edge.transform.SetParent(parent, false);
                    var line = edge.AddComponent<LineRenderer>();
                    line.positionCount = 2;
                    line.startWidth = 0.025f;
                    line.endWidth = 0.025f;
                    line.material = material;
                    line.startColor = Color.yellow;
                    line.endColor = Color.yellow;
                    line.useWorldSpace = true;
                    line.enabled = false;
                    edges[index] = line;
                }
            }

            public void Show(Bounds bounds)
            {
                var min = bounds.min;
                var max = bounds.max;
                var p = new[]
                {
                    new Vector3(min.x, min.y, min.z), new Vector3(max.x, min.y, min.z),
                    new Vector3(max.x, min.y, max.z), new Vector3(min.x, min.y, max.z),
                    new Vector3(min.x, max.y, min.z), new Vector3(max.x, max.y, min.z),
                    new Vector3(max.x, max.y, max.z), new Vector3(min.x, max.y, max.z)
                };
                var pairs = new[,] { { 0, 1 }, { 1, 2 }, { 2, 3 }, { 3, 0 }, { 4, 5 }, { 5, 6 }, { 6, 7 }, { 7, 4 }, { 0, 4 }, { 1, 5 }, { 2, 6 }, { 3, 7 } };
                for (var index = 0; index < edges.Length; index++)
                {
                    edges[index].SetPosition(0, p[pairs[index, 0]]);
                    edges[index].SetPosition(1, p[pairs[index, 1]]);
                    edges[index].enabled = true;
                }
            }

            public void Hide()
            {
                foreach (var edge in edges) edge.enabled = false;
            }
        }
    }
}
