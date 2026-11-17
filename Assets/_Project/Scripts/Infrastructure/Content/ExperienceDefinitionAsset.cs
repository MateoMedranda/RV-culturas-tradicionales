using System.Linq;
using UnityEngine;
using RvCulturas.Domain;

namespace RvCulturas.Infrastructure.Content
{
    [CreateAssetMenu(menuName = "RV Culturas/Experience Definition", fileName = "ExperienceDefinition")]
    public sealed class ExperienceDefinitionAsset : ScriptableObject
    {
        public string experienceId;
        public string title;
        [TextArea] public string culturalContext;
        [TextArea] public string sourceReference;
        public ReviewStatus reviewStatus;
        public PreparationStepAsset[] steps;
        public CulturalElementAsset[] culturalElements;
        public AssetManifestAsset[] assets;

        public ExperienceDefinition ToDomain() => new ExperienceDefinition(
            experienceId,
            title,
            culturalContext,
            sourceReference,
            reviewStatus,
            steps?.Where(step => step != null).Select(step => step.ToDomain()),
            culturalElements?.Where(element => element != null).Select(element => element.ToDomain()),
            assets?.Where(asset => asset != null).Select(asset => asset.ToDomain()));
    }
}
