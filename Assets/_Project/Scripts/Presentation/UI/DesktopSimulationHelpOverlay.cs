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
            GUI.Box(new Rect(16, 16, width, 104), GUIContent.none);
            GUI.Label(new Rect(32, 24, width - 32, 28), "SIMULADOR DE ESCRITORIO — RECORRIDO DE PRÁCTICA", titleStyle);
            GUI.Label(new Rect(32, 62, width - 32, 46), "W A S D: mover · mantener clic derecho y mover el ratón: mirar en primera persona · clic izquierdo: seleccionar · Espacio: iniciar · P: pausa · G: repetir · R: reiniciar · C: cerrar ficha", bodyStyle);

            GUI.Box(new Rect(16, 136, width * 0.56f, 172), GUIContent.none);
            GUI.Label(new Rect(32, 150, width * 0.56f - 32, 32), "GUÍA", headingStyle);
            GUI.Label(new Rect(32, 188, width * 0.56f - 32, 72), guidance, contentStyle);
            GUI.Label(new Rect(32, 265, width * 0.56f - 32, 35), feedbackAccepted ? "✓ " + feedback : "↻ " + feedback, contentStyle);

            GUI.Box(new Rect(16 + width * 0.59f, 136, width * 0.41f, 172), GUIContent.none);
            GUI.Label(new Rect(32 + width * 0.59f, 150, width * 0.41f - 32, 32), "RECORRIDO", headingStyle);
            GUI.Label(new Rect(32 + width * 0.59f, 188, width * 0.41f - 32, 100), overview, contentStyle);

            if (!string.IsNullOrWhiteSpace(context))
            {
                GUI.Box(new Rect(16, 324, width, 130), GUIContent.none);
                GUI.Label(new Rect(32, 338, width - 32, 32), "CONTEXTO CULTURAL", headingStyle);
                GUI.Label(new Rect(32, 376, width - 32, 66), context, contentStyle);
            }
        }

        private void EnsureStyles()
        {
            if (titleStyle != null) return;
            titleStyle = new GUIStyle(GUI.skin.label)
            {
                fontSize = 48,
                fontStyle = FontStyle.Bold,
                normal = { textColor = new Color(1f, 0.8f, 0.25f) }
            };
            bodyStyle = new GUIStyle(GUI.skin.label)
            {
                fontSize = 32,
                wordWrap = true,
                normal = { textColor = Color.white }
            };
            headingStyle = new GUIStyle(titleStyle)
            {
                fontSize = 42,
                normal = { textColor = new Color(0.45f, 0.9f, 1f) }
            };
            contentStyle = new GUIStyle(bodyStyle)
            {
                fontSize = 40,
                normal = { textColor = new Color(0.96f, 0.96f, 0.96f) }
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
