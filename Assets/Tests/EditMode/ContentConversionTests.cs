using NUnit.Framework;
using UnityEngine;
using RvCulturas.Domain;
using RvCulturas.Infrastructure.Content;

namespace RvCulturas.Tests.EditMode
{
    public class ContentConversionTests
    {
        [Test]
        public void PreparationStepAssetConvertsAllInteractionFields()
        {
            var asset = ScriptableObject.CreateInstance<PreparationStepAsset>();
            asset.stepId = "step";
            asset.ordinal = 1;
            asset.title = "Usar utensilio";
            asset.requiredTargets = new[] { "utensil" };
            asset.expectedAction = ExpectedAction.Use;
            asset.visualInstruction = Guidance("Instrucción");
            asset.successFeedback = Guidance("Correcto");
            asset.correctionFeedback = Guidance("Reintenta");

            var result = asset.ToDomain();

            Assert.That(result.Id, Is.EqualTo("step"));
            Assert.That(result.RequiredTargets, Is.EquivalentTo(new[] { "utensil" }));
            Assert.That(result.ExpectedAction, Is.EqualTo(ExpectedAction.Use));
            Object.DestroyImmediate(asset);
        }

        [Test]
        public void CulturalElementAndManifestAssetsPreserveProvenance()
        {
            var element = ScriptableObject.CreateInstance<CulturalElementAsset>();
            element.elementId = "element";
            element.category = CulturalElementCategory.Utensil;
            element.displayName = "Utensilio";
            element.explanation = "Descripción";
            element.assetKey = "asset-key";
            element.sourceReference = "Fuente verificable";
            element.reviewStatus = ReviewStatus.Draft;
            var manifest = ScriptableObject.CreateInstance<AssetManifestAsset>();
            manifest.assetKey = "asset-key";
            manifest.assetType = "Greybox";
            manifest.source = "Proyecto";
            manifest.licenseOrPermission = "Propio";
            manifest.culturalReview = ReviewStatus.Draft;
            manifest.optimizationStatus = ReviewStatus.Draft;

            Assert.That(element.ToDomain().SourceReference, Is.EqualTo("Fuente verificable"));
            Assert.That(manifest.ToDomain().LicenseOrPermission, Is.EqualTo("Propio"));
            Object.DestroyImmediate(element);
            Object.DestroyImmediate(manifest);
        }

        [Test]
        public void ExperienceAssetAndRepositoryLoadTheSameConvertedExperience()
        {
            var step = ScriptableObject.CreateInstance<PreparationStepAsset>();
            step.stepId = "step";
            step.ordinal = 1;
            step.title = "Paso";
            step.requiredTargets = new[] { "utensil" };
            step.expectedAction = ExpectedAction.Use;
            step.visualInstruction = Guidance("Instrucción");
            step.successFeedback = Guidance("Correcto");
            step.correctionFeedback = Guidance("Reintenta");
            var experience = ScriptableObject.CreateInstance<ExperienceDefinitionAsset>();
            experience.experienceId = "experience";
            experience.title = "Experiencia";
            experience.culturalContext = "Contexto";
            experience.sourceReference = "Fuente";
            experience.reviewStatus = ReviewStatus.Draft;
            experience.steps = new[] { step };
            experience.culturalElements = new CulturalElementAsset[0];
            experience.assets = new AssetManifestAsset[0];

            var loaded = new ScriptableObjectExperienceRepository(experience).Load("experience");

            Assert.That(loaded.Id, Is.EqualTo("experience"));
            Assert.That(loaded.Steps[0].VisualInstruction.Text, Is.EqualTo("Instrucción"));
            Assert.That(new ScriptableObjectExperienceRepository(experience).Load("missing"), Is.Null);
            Object.DestroyImmediate(experience);
            Object.DestroyImmediate(step);
        }

        private static GuidanceData Guidance(string text) => new GuidanceData
        {
            text = text,
            iconKey = "icon",
            visualCue = "cue",
            audioCue = string.Empty,
            repeatable = true
        };
    }
}
