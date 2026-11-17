using System;
using System.Collections.Generic;

namespace RvCulturas.Domain
{
    public sealed class ExperienceSessionState
    {
        private readonly HashSet<string> completedStepIds = new HashSet<string>();

        public ExperienceLifecycle Lifecycle { get; private set; } = ExperienceLifecycle.Intro;
        public int CurrentStepOrdinal { get; private set; }
        public IReadOnlyCollection<string> CompletedStepIds => completedStepIds;

        public void Start(ExperienceDefinition experience)
        {
            if (experience == null) throw new ArgumentNullException(nameof(experience));
            if (experience.Steps.Count == 0) throw new InvalidOperationException("No hay pasos para iniciar.");
            completedStepIds.Clear();
            CurrentStepOrdinal = 1;
            Lifecycle = ExperienceLifecycle.Active;
        }

        public bool TryCompleteCurrentStep(ExperienceDefinition experience, string targetId, ExpectedAction action)
        {
            if (Lifecycle != ExperienceLifecycle.Active || experience == null) return false;
            var step = experience.GetStep(CurrentStepOrdinal);
            if (step == null || !step.Matches(targetId, action)) return false;
            if (!completedStepIds.Add(step.Id)) return false;
            if (CurrentStepOrdinal >= experience.Steps.Count) Lifecycle = ExperienceLifecycle.Completed;
            else CurrentStepOrdinal++;
            return true;
        }

        public bool Pause() { if (Lifecycle != ExperienceLifecycle.Active) return false; Lifecycle = ExperienceLifecycle.Paused; return true; }
        public bool Resume() { if (Lifecycle != ExperienceLifecycle.Paused) return false; Lifecycle = ExperienceLifecycle.Active; return true; }
        public void Restart() { completedStepIds.Clear(); CurrentStepOrdinal = 0; Lifecycle = ExperienceLifecycle.Intro; }
        public void Exit() { completedStepIds.Clear(); CurrentStepOrdinal = 0; Lifecycle = ExperienceLifecycle.Exited; }
    }
}
