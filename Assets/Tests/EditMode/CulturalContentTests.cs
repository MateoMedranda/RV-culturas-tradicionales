using NUnit.Framework;
using RvCulturas.Application.UseCases;
using RvCulturas.Domain;

namespace RvCulturas.Tests.EditMode
{
    public class CulturalContentTests
    {
        [Test]
        public void ContextRequestKeepsCurrentStep()
        {
            var experience = TestData.Experience();
            var element = new CulturalElement("utensil", CulturalElementCategory.Utensil, "Utensilio", "Explicación", "asset", "Fuente", ReviewStatus.Draft);
            experience = new ExperienceDefinition("test", "Prueba", "Contexto", "Fuente", ReviewStatus.Draft, experience.Steps, new[] { element }, experience.Assets);
            var session = new ExperienceSessionState();
            new StartExperienceUseCase().Execute(session, experience);
            var result = new RequestCulturalContextUseCase().Execute(session, experience, "utensil");
            Assert.That(result.CurrentStepOrdinal, Is.EqualTo(1));
            Assert.That(result.Message, Is.EqualTo("Utensilio"));
        }
    }
}
