using RvCulturas.Application.Results;
using RvCulturas.Domain;

namespace RvCulturas.Application.UseCases
{
    public sealed class StartExperienceUseCase
    {
        public ExperienceResult Execute(ExperienceSessionState session, ExperienceDefinition experience)
        {
            var issues = experience.Validate(false);
            if (issues.Count > 0) return new ExperienceResult(ExperienceResultKind.InvalidContent, issues[0], null, 0);
            session.Start(experience);
            var step = experience.GetStep(session.CurrentStepOrdinal);
            return new ExperienceResult(ExperienceResultKind.Started, step.Title, step.VisualInstruction, step.Ordinal);
        }
    }
}
