using System;
using System.Collections.Generic;
using System.Linq;

namespace RvCulturas.Domain
{
    public sealed class ExperienceDefinition
    {
        public ExperienceDefinition(string id, string title, string culturalContext, string sourceReference, ReviewStatus reviewStatus, IEnumerable<PreparationStep> steps, IEnumerable<CulturalElement> culturalElements, IEnumerable<AssetManifestEntry> assets)
        {
            Id = id ?? string.Empty;
            Title = title ?? string.Empty;
            CulturalContext = culturalContext ?? string.Empty;
            SourceReference = sourceReference ?? string.Empty;
            ReviewStatus = reviewStatus;
            Steps = (steps ?? Array.Empty<PreparationStep>()).OrderBy(step => step.Ordinal).ToArray();
            CulturalElements = (culturalElements ?? Array.Empty<CulturalElement>()).ToArray();
            Assets = (assets ?? Array.Empty<AssetManifestEntry>()).ToArray();
        }

        public string Id { get; }
        public string Title { get; }
        public string CulturalContext { get; }
        public string SourceReference { get; }
        public ReviewStatus ReviewStatus { get; }
        public IReadOnlyList<PreparationStep> Steps { get; }
        public IReadOnlyList<CulturalElement> CulturalElements { get; }
        public IReadOnlyList<AssetManifestEntry> Assets { get; }

        public IReadOnlyList<string> Validate(bool requirePublishable)
        {
            var issues = new List<string>();
            if (string.IsNullOrWhiteSpace(Id)) issues.Add("La experiencia requiere identificador.");
            if (string.IsNullOrWhiteSpace(Title)) issues.Add("La experiencia requiere título.");
            if (string.IsNullOrWhiteSpace(CulturalContext)) issues.Add("La experiencia requiere contexto cultural.");
            if (Steps.Count == 0) issues.Add("La experiencia requiere mínimo un paso.");
            if (Steps.Select(step => step.Ordinal).Distinct().Count() != Steps.Count || Steps.Select(step => step.Ordinal).OrderBy(value => value).Where((value, index) => value != index + 1).Any()) issues.Add("Los ordinales son únicos y consecutivos.");
            foreach (var step in Steps) issues.AddRange(step.Validate());
            if (requirePublishable)
            {
                if (string.IsNullOrWhiteSpace(SourceReference)) issues.Add("La experiencia publicable requiere fuente.");
                if (ReviewStatus != ReviewStatus.Validated) issues.Add("La experiencia publicable requiere revisión validada.");
                foreach (var element in CulturalElements) issues.AddRange(element.ValidateForPublication());
                foreach (var asset in Assets) issues.AddRange(asset.ValidateForAcceptance());
            }
            return issues;
        }

        public PreparationStep GetStep(int ordinal) => Steps.FirstOrDefault(step => step.Ordinal == ordinal);
        public CulturalElement GetElement(string id) => CulturalElements.FirstOrDefault(element => element.Id == id);
    }
}
