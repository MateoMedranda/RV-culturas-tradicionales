using System;
using System.Collections.Generic;

namespace RvCulturas.Domain
{
    public sealed class AssetManifestEntry
    {
        public AssetManifestEntry(string assetKey, string assetType, string source, string licenseOrPermission, ReviewStatus culturalReview, ReviewStatus optimizationStatus)
        {
            AssetKey = assetKey ?? string.Empty;
            AssetType = assetType ?? string.Empty;
            Source = source ?? string.Empty;
            LicenseOrPermission = licenseOrPermission ?? string.Empty;
            CulturalReview = culturalReview;
            OptimizationStatus = optimizationStatus;
        }

        public string AssetKey { get; }
        public string AssetType { get; }
        public string Source { get; }
        public string LicenseOrPermission { get; }
        public ReviewStatus CulturalReview { get; }
        public ReviewStatus OptimizationStatus { get; }

        public IReadOnlyList<string> ValidateForAcceptance()
        {
            var issues = new List<string>();
            if (string.IsNullOrWhiteSpace(AssetKey)) issues.Add("El recurso requiere identificador.");
            if (string.IsNullOrWhiteSpace(Source)) issues.Add("El recurso requiere procedencia.");
            if (string.IsNullOrWhiteSpace(LicenseOrPermission)) issues.Add("El recurso requiere permiso o licencia.");
            if (CulturalReview != ReviewStatus.Validated) issues.Add("El recurso requiere revisión cultural.");
            if (OptimizationStatus != ReviewStatus.Validated) issues.Add("El recurso requiere optimización aprobada.");
            return issues;
        }
    }
}
