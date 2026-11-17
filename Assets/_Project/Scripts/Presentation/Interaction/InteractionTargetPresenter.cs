using System;
using UnityEngine;
using RvCulturas.Domain;

namespace RvCulturas.Presentation.Interaction
{
    [RequireComponent(typeof(Collider))]
    public sealed class InteractionTargetPresenter : MonoBehaviour
    {
        [SerializeField] private string targetId;
        [SerializeField] private ExpectedAction expectedAction = ExpectedAction.Use;
        public event Action<string, ExpectedAction> InteractionRequested;

        public string TargetId => targetId;
        public ExpectedAction ExpectedAction => expectedAction;

        public void Configure(string id, ExpectedAction action)
        {
            targetId = id;
            expectedAction = action;
        }

        public void AttemptInteraction()
        {
            InteractionRequested?.Invoke(targetId, expectedAction);
        }

        private void OnMouseDown()
        {
            AttemptInteraction();
        }
    }
}
