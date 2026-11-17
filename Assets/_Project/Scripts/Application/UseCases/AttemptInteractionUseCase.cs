using RvCulturas.Application.Results;
using RvCulturas.Domain;

namespace RvCulturas.Application.UseCases
{
    public sealed class AttemptInteractionUseCase
    {
        public ExperienceResult Execute(ExperienceSessionState session, ExperienceDefinition experience, string targetId, ExpectedAction action)
        {
            var activeStep = experience.GetStep(session.CurrentStepOrdinal);
            if (activeStep == null) return new ExperienceResult(ExperienceResultKind.Rejected, "No hay un paso activo.", null, session.CurrentStepOrdinal);
            if (!session.TryCompleteCurrentStep(experience, targetId, action)) return new InteractionRejectedResult(activeStep.CorrectionFeedback, session.CurrentStepOrdinal);
            var completed = session.Lifecycle == ExperienceLifecycle.Completed;
            var nextStep = completed ? null : experience.GetStep(session.CurrentStepOrdinal);
            return new InteractionAcceptedResult(completed ? activeStep.SuccessFeedback : nextStep.VisualInstruction, session.CurrentStepOrdinal, completed);
        }
    }
}
