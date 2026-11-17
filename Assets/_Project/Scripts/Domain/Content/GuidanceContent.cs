using System;
using System.Collections.Generic;

namespace RvCulturas.Domain
{
    public sealed class GuidanceContent
    {
        public GuidanceContent(string text, string iconKey, string visualCue, string audioCue, bool repeatable)
        {
            Text = text ?? string.Empty;
            IconKey = iconKey ?? string.Empty;
            VisualCue = visualCue ?? string.Empty;
            AudioCue = audioCue ?? string.Empty;
            Repeatable = repeatable;
        }

        public string Text { get; }
        public string IconKey { get; }
        public string VisualCue { get; }
        public string AudioCue { get; }
        public bool Repeatable { get; }

        public IReadOnlyList<string> Validate(bool essential)
        {
            var issues = new List<string>();
            if (string.IsNullOrWhiteSpace(Text)) issues.Add("La guía requiere texto.");
            if (essential && string.IsNullOrWhiteSpace(VisualCue)) issues.Add("La guía esencial requiere señal visual.");
            return issues;
        }
    }
}
