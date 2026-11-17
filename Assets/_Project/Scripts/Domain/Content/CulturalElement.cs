using System;
using System.Collections.Generic;

namespace RvCulturas.Domain
{
    public sealed class CulturalElement
    {
        public CulturalElement(string id, CulturalElementCategory category, string displayName, string explanation, string assetKey, string sourceReference, ReviewStatus reviewStatus)
        {
            Id = id ?? string.Empty;
            Category = category;
            DisplayName = displayName ?? string.Empty;
            Explanation = explanation ?? string.Empty;
            AssetKey = assetKey ?? string.Empty;
            SourceReference = sourceReference ?? string.Empty;
            ReviewStatus = reviewStatus;
        }

        public string Id { get; }
        public CulturalElementCategory Category { get; }
        public string DisplayName { get; }
        public string Explanation { get; }
        public string AssetKey { get; }
        public string SourceReference { get; }
        public ReviewStatus ReviewStatus { get; }

        public IReadOnlyList<string> ValidateForPublication()
        {
            var issues = new List<string>();
            if (string.IsNullOrWhiteSpace(Id)) issues.Add("El elemento cultural requiere identificador.");
            if (string.IsNullOrWhiteSpace(DisplayName)) issues.Add("El elemento cultural requiere nombre.");
            if (string.IsNullOrWhiteSpace(Explanation)) issues.Add("El elemento cultural requiere explicación.");
            if (string.IsNullOrWhiteSpace(SourceReference)) issues.Add("El elemento cultural requiere fuente.");
            if (ReviewStatus != ReviewStatus.Validated) issues.Add("El elemento cultural no está validado.");
            return issues;
        }
    }
}
