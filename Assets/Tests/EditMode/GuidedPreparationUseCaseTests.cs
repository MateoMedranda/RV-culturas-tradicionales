using NUnit.Framework;
using RvCulturas.Application.UseCases;
using RvCulturas.Domain;

namespace RvCulturas.Tests.EditMode
{
    public class GuidedPreparationUseCaseTests
    {
        [Test]
        public void CorrectInteractionCompletesOnlyStep()
        {
            var experience = TestData.Experience();
            var session = new ExperienceSessionState();
            new StartExperienceUseCase().Execute(session, experience);
            var result = new AttemptInteractionUseCase().Execute(session, experience, "utensil", ExpectedAction.Use);
            Assert.That(result.Message, Is.EqualTo("Recorrido completado."));
            Assert.That(session.Lifecycle, Is.EqualTo(ExperienceLifecycle.Completed));
        }

        [Test]
        public void IncorrectInteractionDoesNotAdvance()
        {
            var experience = TestData.Experience();
            var session = new ExperienceSessionState();
            new StartExperienceUseCase().Execute(session, experience);
            var result = new AttemptInteractionUseCase().Execute(session, experience, "ingredient", ExpectedAction.Take);
            Assert.That(result.Message, Is.EqualTo("Completa primero el paso indicado."));
            Assert.That(session.CurrentStepOrdinal, Is.EqualTo(1));
        }
    }

    internal static class TestData
    {
        public static ExperienceDefinition Experience()
        {
            var guidance = new GuidanceContent("Usa el utensilio", "icon", "visual", string.Empty, true);
            return new ExperienceDefinition("test", "Prueba", "Contexto", "Fuente", ReviewStatus.Draft,
                new[] { new PreparationStep("step", 1, "Usar", guidance, new[] { "utensil" }, ExpectedAction.Use, guidance, guidance) },
                new CulturalElement[0], new AssetManifestEntry[0]);
        }
    }
}
