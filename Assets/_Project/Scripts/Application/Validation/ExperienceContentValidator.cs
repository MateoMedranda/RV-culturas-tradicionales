using System.Collections.Generic;
using RvCulturas.Domain;

namespace RvCulturas.Application.Validation
{
    public sealed class ExperienceContentValidator
    {
        public IReadOnlyList<string> ValidateDraft(ExperienceDefinition experience) => experience == null ? new[] { "La experiencia no existe." } : experience.Validate(false);
        public IReadOnlyList<string> ValidateForPublication(ExperienceDefinition experience) => experience == null ? new[] { "La experiencia no existe." } : experience.Validate(true);
    }
}
