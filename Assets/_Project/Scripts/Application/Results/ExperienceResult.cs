using RvCulturas.Domain;

namespace RvCulturas.Application.Results
{
    public enum ExperienceResultKind { Started, Accepted, Rejected, Paused, Resumed, GuidancePresented, Completed, Restarted, Exited, ContextPresented, InvalidContent }

    public class ExperienceResult
    {
        public ExperienceResult(ExperienceResultKind kind, string message, GuidanceContent guidance, int currentStepOrdinal, string sourceReference = null)
        {
            Kind = kind;
            Message = message;
            Guidance = guidance;
            CurrentStepOrdinal = currentStepOrdinal;
            SourceReference = sourceReference;
        }

        public ExperienceResultKind Kind { get; }
        public string Message { get; }
        public GuidanceContent Guidance { get; }
        public int CurrentStepOrdinal { get; }
        public string SourceReference { get; }
    }
}
