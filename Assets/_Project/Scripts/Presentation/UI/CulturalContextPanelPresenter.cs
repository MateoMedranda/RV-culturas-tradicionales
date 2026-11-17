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

        public void Present(string title, string explanation)
        {
            if (panel != null) panel.SetActive(true);
            if (textMesh != null) textMesh.text = title + "\n" + explanation;
        }

        public void Close()
        {
            if (panel != null) panel.SetActive(false);
        }
    }
}
