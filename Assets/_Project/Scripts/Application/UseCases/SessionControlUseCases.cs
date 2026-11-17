using RvCulturas.Application.Results;
using RvCulturas.Domain;

namespace RvCulturas.Application.UseCases
{
    public sealed class SessionControlUseCases
    {
        public ExperienceResult Pause(ExperienceSessionState session) => session.Pause() ? new ExperienceResult(ExperienceResultKind.Paused, "Recorrido en pausa.", null, session.CurrentStepOrdinal) : new ExperienceResult(ExperienceResultKind.Rejected, "No se puede pausar en este estado.", null, session.CurrentStepOrdinal);
        public ExperienceResult Resume(ExperienceSessionState session) => session.Resume() ? new ExperienceResult(ExperienceResultKind.Resumed, "Recorrido reanudado.", null, session.CurrentStepOrdinal) : new ExperienceResult(ExperienceResultKind.Rejected, "No se puede reanudar en este estado.", null, session.CurrentStepOrdinal);
        public ExperienceResult Restart(ExperienceSessionState session) { session.Restart(); return new ExperienceResult(ExperienceResultKind.Restarted, "Recorrido reiniciado.", null, 0); }
        public ExperienceResult Exit(ExperienceSessionState session) { session.Exit(); return new ExperienceResult(ExperienceResultKind.Exited, "Recorrido finalizado.", null, 0); }
        public ExperienceResult RepeatGuidance(ExperienceSessionState session, ExperienceDefinition experience)
        {
            var step = experience.GetStep(session.CurrentStepOrdinal);
            return step == null ? new ExperienceResult(ExperienceResultKind.Rejected, "No hay guía activa.", null, session.CurrentStepOrdinal) : new ExperienceResult(ExperienceResultKind.GuidancePresented, step.Title, step.VisualInstruction, step.Ordinal);
        }
    }
}
