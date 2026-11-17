using UnityEngine;
using RvCulturas.Domain;

namespace RvCulturas.Presentation.UI
{
    public sealed class AccessibleGuidancePresenter : MonoBehaviour
    {
        [SerializeField] private TextMesh textMesh;
        [SerializeField] private GameObject panel;

        public void Configure(TextMesh configuredTextMesh, GameObject configuredPanel = null)
        {
            textMesh = configuredTextMesh;
            panel = configuredPanel;
        }

        public void Present(GuidanceContent guidance)
        {
            if (guidance == null) return;
            if (panel != null) panel.SetActive(true);
            if (textMesh != null) textMesh.text = guidance.Text;
        }

        public void Hide()
        {
            if (panel != null) panel.SetActive(false);
        }
    }
}
