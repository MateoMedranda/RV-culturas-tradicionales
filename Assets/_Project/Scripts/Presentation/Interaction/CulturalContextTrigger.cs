using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactables;
using RvCulturas.Presentation.Bootstrap;

namespace RvCulturas.Presentation.Interaction
{
    [RequireComponent(typeof(XRSimpleInteractable))]
    public sealed class CulturalContextTrigger : MonoBehaviour
    {
        [SerializeField] private ExperienceBootstrapper bootstrapper;
        [SerializeField] private string elementId;
        [SerializeField] private XRSimpleInteractable interactable;

        public void Configure(ExperienceBootstrapper configuredBootstrapper, string configuredElementId, XRSimpleInteractable configuredInteractable)
        {
            bootstrapper = configuredBootstrapper;
            elementId = configuredElementId;
            interactable = configuredInteractable;
        }

        private void Awake()
        {
            if (interactable == null) interactable = GetComponent<XRSimpleInteractable>();
            if (interactable != null) interactable.selectEntered.AddListener(OnSelected);
        }

        private void OnDestroy()
        {
            if (interactable != null) interactable.selectEntered.RemoveListener(OnSelected);
        }

        private void OnMouseDown() => Open();
        private void OnSelected(SelectEnterEventArgs _) => Open();
        private void Open() => bootstrapper?.ShowCulturalContext(elementId);
    }
}
