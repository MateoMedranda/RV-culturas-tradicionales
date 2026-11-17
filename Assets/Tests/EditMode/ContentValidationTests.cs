using NUnit.Framework;
using RvCulturas.Domain;

namespace RvCulturas.Tests.EditMode
{
    public class ContentValidationTests
    {
        [Test]
        public void DraftRequiresConsecutiveOrdinals()
        {
            var first = Step("one", 1);
            var third = Step("three", 3);
            var experience = Experience(new[] { first, third });
            Assert.That(experience.Validate(false), Does.Contain("Los ordinales son únicos y consecutivos."));
        }

        [Test]
        public void EssentialGuidanceRequiresTextAndVisualCue()
        {
            var guidance = new GuidanceContent(string.Empty, string.Empty, string.Empty, string.Empty, true);
            Assert.That(guidance.Validate(true), Has.Count.EqualTo(2));
        }

        [Test]
        public void PublicationRequiresSourceAndReview()
        {
            var experience = Experience(new[] { Step("one", 1) });
            Assert.That(experience.Validate(true), Is.Not.Empty);
        }

        private static ExperienceDefinition Experience(PreparationStep[] steps) => new ExperienceDefinition("experience", "Prueba", "Contexto", string.Empty, ReviewStatus.Draft, steps, new CulturalElement[0], new AssetManifestEntry[0]);
        private static PreparationStep Step(string id, int ordinal)
        {
            var guidance = new GuidanceContent("Texto", string.Empty, "visual", string.Empty, true);
            return new PreparationStep(id, ordinal, id, guidance, new[] { "target" }, ExpectedAction.Use, guidance, guidance);
        }
    }
}
