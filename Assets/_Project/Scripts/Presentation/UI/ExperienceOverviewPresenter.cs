using UnityEngine;

namespace RvCulturas.Presentation.UI
{
    public sealed class ExperienceOverviewPresenter : MonoBehaviour
    {
        [SerializeField] private TextMesh textMesh;
        [SerializeField] private GameObject panel;

        public void Configure(TextMesh configuredTextMesh, GameObject configuredPanel = null)
        {
            textMesh = configuredTextMesh;
            panel = configuredPanel;
        }

        public void PresentIntroduction(string title, string context)
        {
            if (!DesktopSimulationHelpOverlay.IsActive && panel != null) panel.SetActive(true);
            if (textMesh != null) textMesh.text = title + "\n" + context + "\nSelecciona Iniciar para comenzar.";
            DesktopSimulationHelpOverlay.PublishOverview(title + ": " + context + " Pulsa Espacio para iniciar.");
        }

        public void PresentClosing(string summary)
        {
            if (!DesktopSimulationHelpOverlay.IsActive && panel != null) panel.SetActive(true);
            if (textMesh != null) textMesh.text = "Resumen del recorrido\n" + summary + "\nGracias por contribuir a preservar y respetar esta práctica cultural.";
            DesktopSimulationHelpOverlay.PublishOverview("Resumen: " + summary + " Gracias por respetar esta práctica cultural.");
        }

        public void Hide()
        {
            if (panel != null) panel.SetActive(false);
        }
    }
}
