using System.Collections.Generic;
using UnityEngine;

namespace RvCulturas.Presentation.Interaction
{
    public sealed class XrInteractionAdapter : MonoBehaviour
    {
        [SerializeField] private InteractionTargetPresenter[] targets;
        private readonly List<InteractionTargetPresenter> registeredTargets = new List<InteractionTargetPresenter>();

        public void Register(InteractionTargetPresenter target)
        {
            if (target == null || registeredTargets.Contains(target)) return;
            registeredTargets.Add(target);
        }

        public void SetTargets(InteractionTargetPresenter[] values)
        {
            targets = values;
        }

        private void Awake()
        {
            if (targets == null) return;
            foreach (var target in targets) Register(target);
        }

        public IEnumerable<InteractionTargetPresenter> Targets => registeredTargets;
    }
}
