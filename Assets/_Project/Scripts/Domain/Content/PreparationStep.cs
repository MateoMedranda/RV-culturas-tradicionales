using System;
using System.Collections.Generic;
using System.Linq;

namespace RvCulturas.Domain
{
    public sealed class PreparationStep
    {
        public PreparationStep(string id, int ordinal, string title, GuidanceContent visualInstruction, IEnumerable<string> requiredTargets, ExpectedAction expectedAction, GuidanceContent successFeedback, GuidanceContent correctionFeedback)
        {
            Id = id ?? string.Empty;
            Ordinal = ordinal;
            Title = title ?? string.Empty;
            VisualInstruction = visualInstruction;
            RequiredTargets = (requiredTargets ?? Array.Empty<string>()).Where(value => !string.IsNullOrWhiteSpace(value)).Distinct().ToArray();
            ExpectedAction = expectedAction;
            SuccessFeedback = successFeedback;
            CorrectionFeedback = correctionFeedback;
        }

        public string Id { get; }
        public int Ordinal { get; }
        public string Title { get; }
        public GuidanceContent VisualInstruction { get; }
        public IReadOnlyList<string> RequiredTargets { get; }
        public ExpectedAction ExpectedAction { get; }
        public GuidanceContent SuccessFeedback { get; }
        public GuidanceContent CorrectionFeedback { get; }

        public bool Matches(string targetId, ExpectedAction action) => RequiredTargets.Contains(targetId) && ExpectedAction == action;

        public IReadOnlyList<string> Validate()
        {
            var issues = new List<string>();
            if (string.IsNullOrWhiteSpace(Id)) issues.Add("El paso requiere identificador.");
            if (Ordinal < 1) issues.Add("El ordinal empieza en 1.");
            if (string.IsNullOrWhiteSpace(Title)) issues.Add("El paso requiere título.");
            if (RequiredTargets.Count == 0) issues.Add("El paso requiere al menos un objetivo.");
            if (VisualInstruction == null) issues.Add("El paso requiere instrucción visual.");
            else issues.AddRange(VisualInstruction.Validate(true));
            if (SuccessFeedback == null) issues.Add("El paso requiere retroalimentación de éxito.");
            else issues.AddRange(SuccessFeedback.Validate(true));
            if (CorrectionFeedback == null) issues.Add("El paso requiere retroalimentación de corrección.");
            else issues.AddRange(CorrectionFeedback.Validate(true));
            return issues;
        }
    }
}
