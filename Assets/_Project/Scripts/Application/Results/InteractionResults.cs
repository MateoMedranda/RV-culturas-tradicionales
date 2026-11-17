using RvCulturas.Domain;

namespace RvCulturas.Application.Results
{
    public sealed class InteractionAcceptedResult : ExperienceResult
    {
        public InteractionAcceptedResult(GuidanceContent guidance, int currentStepOrdinal, bool completed) : base(completed ? ExperienceResultKind.Completed : ExperienceResultKind.Accepted, completed ? "Recorrido completado." : "Acción correcta.", guidance, currentStepOrdinal) { }
    }

    public sealed class InteractionRejectedResult : ExperienceResult
    {
        public InteractionRejectedResult(GuidanceContent guidance, int currentStepOrdinal) : base(ExperienceResultKind.Rejected, "Completa primero el paso indicado.", guidance, currentStepOrdinal) { }
    }
}
