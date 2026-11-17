using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactables;

namespace RvCulturas.Presentation.Interaction
{
    [RequireComponent(typeof(XRSimpleInteractable))]
    public sealed class XrSelectRelay : MonoBehaviour
    {
        [SerializeField] private InteractionTargetPresenter target;
        [SerializeField] private XRSimpleInteractable interactable;

        public void Configure(InteractionTargetPresenter configuredTarget, XRSimpleInteractable configuredInteractable)
        {
            target = configuredTarget;
            interactable = configuredInteractable;
        }

        private void Awake()
        {
            if (interactable == null) interactable = GetComponent<XRSimpleInteractable>();
            if (target == null) target = GetComponent<InteractionTargetPresenter>();
            if (interactable != null) interactable.selectEntered.AddListener(OnSelected);
        }

        private void OnDestroy()
        {
            if (interactable != null) interactable.selectEntered.RemoveListener(OnSelected);
        }

        private void OnSelected(SelectEnterEventArgs _) => target?.AttemptInteraction();
    }
}
