using UnityEngine;
using RvCulturas.Application.Results;
using RvCulturas.Application.UseCases;
using RvCulturas.Domain;
using RvCulturas.Infrastructure.Content;
using RvCulturas.Presentation.Interaction;
using RvCulturas.Presentation.UI;

namespace RvCulturas.Presentation.Bootstrap
{
    public sealed class ExperienceBootstrapper : MonoBehaviour
    {
        [SerializeField] private ExperienceDefinitionAsset experienceAsset;
        [SerializeField] private AccessibleGuidancePresenter guidancePresenter;
        [SerializeField] private AccessibleFeedbackPresenter feedbackPresenter;
        [SerializeField] private CulturalContextPanelPresenter contextPresenter;
        [SerializeField] private SessionControlsPresenter controlsPresenter;
        [SerializeField] private XrInteractionAdapter interactionAdapter;

        private readonly ExperienceSessionState session = new ExperienceSessionState();
        private readonly StartExperienceUseCase startUseCase = new StartExperienceUseCase();
        private readonly AttemptInteractionUseCase interactionUseCase = new AttemptInteractionUseCase();
        private readonly RequestCulturalContextUseCase contextUseCase = new RequestCulturalContextUseCase();
        private readonly SessionControlUseCases controlsUseCases = new SessionControlUseCases();
        private ExperienceDefinition experience;

        public void Configure(ExperienceDefinitionAsset source, AccessibleGuidancePresenter guidance, AccessibleFeedbackPresenter feedback, CulturalContextPanelPresenter context, SessionControlsPresenter controls, XrInteractionAdapter adapter)
        {
            experienceAsset = source;
            guidancePresenter = guidance;
            feedbackPresenter = feedback;
            contextPresenter = context;
            controlsPresenter = controls;
            interactionAdapter = adapter;
        }

        private void Awake()
        {
            if (experienceAsset == null) return;
            experience = experienceAsset.ToDomain();
            if (controlsPresenter != null) controlsPresenter.Configure(this);
        }

        private void Start()
        {
            if (interactionAdapter == null) return;
            foreach (var target in interactionAdapter.Targets)
            {
                target.InteractionRequested -= HandleInteraction;
                target.InteractionRequested += HandleInteraction;
            }
        }

        private void OnDestroy()
        {
            if (interactionAdapter == null) return;
            foreach (var target in interactionAdapter.Targets) target.InteractionRequested -= HandleInteraction;
        }

        public void StartExperience() => Present(startUseCase.Execute(session, experience));
        public void RepeatGuidance() => Present(controlsUseCases.RepeatGuidance(session, experience));
        public void RestartExperience() => Present(controlsUseCases.Restart(session));
        public void ExitExperience() => Present(controlsUseCases.Exit(session));

        public void TogglePause()
        {
            var result = session.Lifecycle == ExperienceLifecycle.Paused ? controlsUseCases.Resume(session) : controlsUseCases.Pause(session);
            Present(result);
        }

        public void ShowCulturalContext(string elementId)
        {
            var result = contextUseCase.Execute(session, experience, elementId);
            if (result.Kind == ExperienceResultKind.ContextPresented && contextPresenter != null) contextPresenter.Present(result.Message, result.Guidance.Text);
            else Present(result);
        }

        private void HandleInteraction(string targetId, ExpectedAction action)
        {
            Present(interactionUseCase.Execute(session, experience, targetId, action));
        }

        private void Present(ExperienceResult result)
        {
            if (result == null) return;
            if (result.Guidance != null) guidancePresenter?.Present(result.Guidance);
            var accepted = result.Kind == ExperienceResultKind.Accepted || result.Kind == ExperienceResultKind.Completed || result.Kind == ExperienceResultKind.Started;
            feedbackPresenter?.Present(result.Message, result.Guidance, accepted);
        }
    }
}
