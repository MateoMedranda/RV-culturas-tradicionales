using UnityEngine;

namespace RvCulturas.Presentation.UI
{
    /// <summary>Ayuda visible únicamente para la prueba en Game View sin visor XR.</summary>
    public sealed class DesktopSimulationHelpOverlay : MonoBehaviour
    {
        private static DesktopSimulationHelpOverlay current;
        private GUIStyle titleStyle;
        private GUIStyle bodyStyle;
        private GUIStyle headingStyle;
        private GUIStyle contentStyle;
        private GUIStyle crosshairStyle;
        private string overview = "Pulsa ESPACIO para comenzar la práctica.";
        private string guidance = "Selecciona el utensilio indicado sobre la mesa.";
        private string feedback = "Preparado para iniciar.";
        private string context;
        private bool feedbackAccepted;

        public static bool IsActive => current != null;

        public static void PublishOverview(string value) => current?.SetOverview(value);
        public static void PublishGuidance(string value) => current?.SetGuidance(value);
        public static void PublishFeedback(string value, bool accepted) => current?.SetFeedback(value, accepted);
        public static void PublishContext(string value) => current?.SetContext(value);
        public static void ClearContext() => current?.SetContext(string.Empty);

        private void Awake() => current = this;
        private void OnDestroy()
        {
            if (current == this) current = null;
        }

        private void OnGUI()
        {
            EnsureStyles();
            var width = Mathf.Min(Screen.width - 32, 1120);
            GUI.backgroundColor = new Color(0.04f, 0.08f, 0.14f, 0.96f);
            GUI.Box(new Rect(16, 16, width, 138), GUIContent.none);
            GUI.Label(new Rect(32, 25, width - 32, 54), "PRÁCTICA GUIADA · SIMULADOR DE ESCRITORIO", titleStyle);
            GUI.Label(new Rect(32, 83, width - 32, 60), "W A S D: caminar · mantener clic derecho y mover ratón: mirar en primera persona · apunta con la mira central y usa clic izquierdo: seleccionar · Espacio: iniciar", bodyStyle);

            GUI.Box(new Rect(16, 172, width * 0.56f, 198), GUIContent.none);
            GUI.Label(new Rect(32, 185, width * 0.56f - 32, 52), "GUÍA", headingStyle);
            GUI.Label(new Rect(32, 237, width * 0.56f - 32, 90), guidance, contentStyle);
            GUI.Label(new Rect(32, 326, width * 0.56f - 32, 38), feedbackAccepted ? "✓ " + feedback : "↻ " + feedback, contentStyle);

            GUI.Box(new Rect(16 + width * 0.59f, 172, width * 0.41f, 198), GUIContent.none);
            GUI.Label(new Rect(32 + width * 0.59f, 185, width * 0.41f - 32, 52), "RECORRIDO", headingStyle);
            GUI.Label(new Rect(32 + width * 0.59f, 237, width * 0.41f - 32, 120), overview, contentStyle);

            if (!string.IsNullOrWhiteSpace(context))
            {
                GUI.Box(new Rect(16, 388, width, 150), GUIContent.none);
                GUI.Label(new Rect(32, 402, width - 32, 52), "CONTEXTO CULTURAL", headingStyle);
                GUI.Label(new Rect(32, 454, width - 32, 72), context, contentStyle);
            }

            GUI.Label(new Rect(Screen.width * 0.5f - 20f, Screen.height * 0.5f - 26f, 40f, 52f), "+", crosshairStyle);
        }

        private void EnsureStyles()
        {
            if (titleStyle != null) return;
            titleStyle = new GUIStyle(GUI.skin.label)
            {
                fontSize = 46,
                fontStyle = FontStyle.Bold,
                normal = { textColor = new Color(1f, 0.8f, 0.25f) }
            };
            bodyStyle = new GUIStyle(GUI.skin.label)
            {
                fontSize = 30,
                wordWrap = true,
                normal = { textColor = Color.white }
            };
            headingStyle = new GUIStyle(titleStyle)
            {
                fontSize = 38,
                normal = { textColor = new Color(0.45f, 0.9f, 1f) }
            };
            contentStyle = new GUIStyle(bodyStyle)
            {
                fontSize = 36,
                normal = { textColor = new Color(0.96f, 0.96f, 0.96f) }
            };
            crosshairStyle = new GUIStyle(GUI.skin.label)
            {
                fontSize = 44,
                fontStyle = FontStyle.Bold,
                alignment = TextAnchor.MiddleCenter,
                normal = { textColor = Color.white }
            };
        }

        private void SetOverview(string value) => overview = value ?? string.Empty;
        private void SetGuidance(string value) => guidance = value ?? string.Empty;
        private void SetFeedback(string value, bool accepted)
        {
            feedback = value ?? string.Empty;
            feedbackAccepted = accepted;
        }
        private void SetContext(string value) => context = value ?? string.Empty;
    }
}
