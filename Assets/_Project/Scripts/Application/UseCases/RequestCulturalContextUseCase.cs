using RvCulturas.Application.Results;
using RvCulturas.Domain;

namespace RvCulturas.Application.UseCases
{
    public sealed class RequestCulturalContextUseCase
    {
        public ExperienceResult Execute(ExperienceSessionState session, ExperienceDefinition experience, string elementId)
        {
            var element = experience.GetElement(elementId);
            if (element == null) return new ExperienceResult(ExperienceResultKind.Rejected, "No hay información para este elemento.", null, session.CurrentStepOrdinal);
            var guidance = new GuidanceContent(element.Explanation, string.Empty, "contexto", string.Empty, true);
            return new ExperienceResult(ExperienceResultKind.ContextPresented, element.DisplayName, guidance, session.CurrentStepOrdinal);
        }
    }
}
