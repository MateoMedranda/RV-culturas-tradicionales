using UnityEngine;
using RvCulturas.Domain;

namespace RvCulturas.Infrastructure.Content
{
    [CreateAssetMenu(menuName = "RV Culturas/Asset Manifest", fileName = "AssetManifest")]
    public sealed class AssetManifestAsset : ScriptableObject
    {
        public string assetKey;
        public string assetType;
        public string source;
        public string licenseOrPermission;
        public ReviewStatus culturalReview;
        public ReviewStatus optimizationStatus;

        public AssetManifestEntry ToDomain() => new AssetManifestEntry(assetKey, assetType, source, licenseOrPermission, culturalReview, optimizationStatus);
    }
}
