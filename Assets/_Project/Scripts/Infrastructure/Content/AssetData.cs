using System;
using UnityEngine;
using RvCulturas.Domain;

namespace RvCulturas.Infrastructure.Content
{
    [Serializable]
    public sealed class GuidanceData
    {
        [TextArea] public string text;
        public string iconKey;
        public string visualCue;
        public string audioCue;
        public bool repeatable = true;
        public GuidanceContent ToDomain() => new GuidanceContent(text, iconKey, visualCue, audioCue, repeatable);
    }

}
