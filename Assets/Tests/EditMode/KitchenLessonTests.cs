using NUnit.Framework;
using UnityEditor;
using RvCulturas.Domain;
using RvCulturas.Infrastructure.Content;

namespace RvCulturas.Tests.EditMode
{
    public sealed class KitchenLessonTests
    {
        private ExperienceDefinition Load()
        {
            var asset = AssetDatabase.LoadAssetAtPath<ExperienceDefinitionAsset>("Assets/_Project/Content/Kitchen/Lesson.asset");
            Assert.That(asset, Is.Not.Null, "Construye ChichaKitchen antes de ejecutar las pruebas.");
            return asset.ToDomain();
        }
        [Test]
        public void SixOrderedActionsCompleteAndRestartCleanly()
        {
            var lesson = Load();
            Assert.That(lesson.Validate(false), Is.Empty);
            Assert.That(lesson.Steps.Count, Is.EqualTo(6));
            var session = new ExperienceSessionState(); session.Start(lesson);
            foreach (var step in lesson.Steps)
            {
                Assert.That(session.CurrentStepOrdinal, Is.EqualTo(step.Ordinal));
                Assert.That(session.TryCompleteCurrentStep(lesson, step.RequiredTargets[0], ExpectedAction.Use), Is.True);
            }
            Assert.That(session.Lifecycle, Is.EqualTo(ExperienceLifecycle.Completed));
            Assert.That(session.TryCompleteCurrentStep(lesson, "serve", ExpectedAction.Use), Is.False);
            session.Restart(); Assert.That(session.CompletedStepIds, Is.Empty); Assert.That(session.Lifecycle, Is.EqualTo(ExperienceLifecycle.Intro));
        }
        [Test]
        public void WrongObjectAndPauseCannotAdvanceProgress()
        {
            var lesson = Load(); var session = new ExperienceSessionState(); session.Start(lesson);
            Assert.That(session.TryCompleteCurrentStep(lesson, "serve", ExpectedAction.Use), Is.False);
            Assert.That(session.CurrentStepOrdinal, Is.EqualTo(1));
            session.Pause(); Assert.That(session.TryCompleteCurrentStep(lesson, "corn", ExpectedAction.Use), Is.False);
            session.Resume(); Assert.That(session.TryCompleteCurrentStep(lesson, "corn", ExpectedAction.Use), Is.True);
        }
    }
}
