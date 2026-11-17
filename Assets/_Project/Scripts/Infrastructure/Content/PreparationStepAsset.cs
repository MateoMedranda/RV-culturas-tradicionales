using UnityEngine;
using RvCulturas.Domain;

namespace RvCulturas.Infrastructure.Content
{
    [CreateAssetMenu(menuName = "RV Culturas/Preparation Step", fileName = "PreparationStep")]
    public sealed class PreparationStepAsset : ScriptableObject
    {
        public string stepId;
        public int ordinal = 1;
        public string title;
        public string[] requiredTargets;
        public ExpectedAction expectedAction;
        public GuidanceData visualInstruction;
        public GuidanceData successFeedback;
        public GuidanceData correctionFeedback;

        public PreparationStep ToDomain() => new PreparationStep(stepId, ordinal, title, visualInstruction.ToDomain(), requiredTargets, expectedAction, successFeedback.ToDomain(), correctionFeedback.ToDomain());
    }
}
