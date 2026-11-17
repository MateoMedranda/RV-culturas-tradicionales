using System.Collections;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.TestTools;
using RvCulturas.Presentation.Bootstrap;
using RvCulturas.Presentation.Interaction;

namespace RvCulturas.Tests.PlayMode
{
    public class GuidedPreparationFlowTests
    {
        [UnityTest]
        public IEnumerator StartThenCorrectAndIncorrectActionsProduceVisibleFeedback()
        {
            SceneManager.LoadScene("Iteration001");
            yield return null;
            var bootstrapper = Object.FindFirstObjectByType<ExperienceBootstrapper>();
            Assert.That(bootstrapper, Is.Not.Null);

            bootstrapper.StartExperience();
            var guidance = GameObject.Find("Guidance").GetComponent<TextMesh>();
            Assert.That(guidance.text, Does.Contain("utensilio"));

            GameObject.Find("Ingredient").GetComponent<InteractionTargetPresenter>().AttemptInteraction();
            var feedback = GameObject.Find("Feedback").GetComponent<TextMesh>();
            Assert.That(feedback.text, Does.Contain("Completa primero"));

            GameObject.Find("Utensil").GetComponent<InteractionTargetPresenter>().AttemptInteraction();
            Assert.That(feedback.text, Does.Contain("Recorrido completado"));
        }
    }
}
