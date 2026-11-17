using UnityEngine;
using RvCulturas.Domain;

namespace RvCulturas.Presentation.UI
{
    public sealed class AccessibleFeedbackPresenter : MonoBehaviour
    {
        [SerializeField] private TextMesh textMesh;
        [SerializeField] private Renderer visualIndicator;
        [SerializeField] private TextMesh statusSymbol;

        public void Configure(TextMesh configuredTextMesh, Renderer configuredVisualIndicator = null, TextMesh configuredStatusSymbol = null)
        {
            textMesh = configuredTextMesh;
            visualIndicator = configuredVisualIndicator;
            statusSymbol = configuredStatusSymbol;
        }

        public void Present(string message, GuidanceContent guidance, bool accepted)
        {
            if (textMesh != null) textMesh.text = message + (guidance == null ? string.Empty : "\n" + guidance.Text);
            if (visualIndicator != null) visualIndicator.material.color = accepted ? new Color(0.2f, 0.65f, 0.35f) : new Color(0.85f, 0.45f, 0.15f);
            if (statusSymbol != null) statusSymbol.text = accepted ? "✓ CORRECTO" : "↻ REVISA";
            DesktopSimulationHelpOverlay.PublishFeedback(message, accepted);
        }
    }
}
