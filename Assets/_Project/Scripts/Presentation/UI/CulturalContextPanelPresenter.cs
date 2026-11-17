using UnityEngine;

namespace RvCulturas.Presentation.UI
{
    public sealed class CulturalContextPanelPresenter : MonoBehaviour
    {
        [SerializeField] private TextMesh textMesh;
        [SerializeField] private GameObject panel;

        public void Configure(TextMesh configuredTextMesh, GameObject configuredPanel = null)
        {
            textMesh = configuredTextMesh;
            panel = configuredPanel;
        }

        public void Present(string title, string explanation, string sourceReference)
        {
            if (!DesktopSimulationHelpOverlay.IsActive && panel != null) panel.SetActive(true);
            if (textMesh != null) textMesh.text = title + "\n" + explanation + "\nFuente: " + sourceReference + "\n[Cerrar contexto]";
            DesktopSimulationHelpOverlay.PublishContext(title + ": " + explanation + " Fuente: " + sourceReference);
        }

        public void Close()
        {
            if (panel != null) panel.SetActive(false);
            DesktopSimulationHelpOverlay.ClearContext();
        }
    }
}
