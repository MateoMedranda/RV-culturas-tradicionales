using RvCulturas.Domain;

namespace RvCulturas.Application.Results
{
    public enum ExperienceResultKind { Started, Accepted, Rejected, Paused, Resumed, GuidancePresented, Completed, Restarted, Exited, ContextPresented, InvalidContent }

    public class ExperienceResult
    {
        public ExperienceResult(ExperienceResultKind kind, string message, GuidanceContent guidance, int currentStepOrdinal)
        {
            Kind = kind;
            Message = message;
            Guidance = guidance;
            CurrentStepOrdinal = currentStepOrdinal;
        }

        public ExperienceResultKind Kind { get; }
        public string Message { get; }
        public GuidanceContent Guidance { get; }
        public int CurrentStepOrdinal { get; }
    }
}
