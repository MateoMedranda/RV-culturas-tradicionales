using UnityEngine;
using RvCulturas.Domain;

namespace RvCulturas.Infrastructure.Content
{
    [CreateAssetMenu(menuName = "RV Culturas/Cultural Element", fileName = "CulturalElement")]
    public sealed class CulturalElementAsset : ScriptableObject
    {
        public string elementId;
        public CulturalElementCategory category;
        public string displayName;
        [TextArea] public string explanation;
        public string assetKey;
        [TextArea] public string sourceReference;
        public ReviewStatus reviewStatus;

        public CulturalElement ToDomain() => new CulturalElement(elementId, category, displayName, explanation, assetKey, sourceReference, reviewStatus);
    }
}
