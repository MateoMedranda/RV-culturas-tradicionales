using System.Collections;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.TestTools;
using RvCulturas.Presentation.Bootstrap;
using RvCulturas.Presentation.Interaction;
using UnityEngine.XR.Interaction.Toolkit.Interactors;
using UnityEngine.XR.Interaction.Toolkit.Interactables;

namespace RvCulturas.Tests.PlayMode
{
    public class GuidedPreparationFlowTests
    {
        [UnityTest]
        public IEnumerator SceneProvidesDirectAndRayXrSelectionForInteractableTargets()
        {
            SceneManager.LoadScene("Iteration001");
            yield return null;

            Assert.That(Object.FindFirstObjectByType<XRDirectInteractor>(), Is.Not.Null);
            Assert.That(Object.FindFirstObjectByType<XRRayInteractor>(), Is.Not.Null);
            Assert.That(Object.FindFirstObjectByType<XRRayInteractor>().selectInput.inputActionPerformed, Is.Not.Null);
            Assert.That(GameObject.Find("Utensil").GetComponent<XRSimpleInteractable>(), Is.Not.Null);
            Assert.That(GameObject.Find("Utensil").GetComponent<XrSelectRelay>(), Is.Not.Null);
        }

        [UnityTest]
        public IEnumerator StartThenCorrectAndIncorrectActionsProduceVisibleFeedback()
        {
            SceneManager.LoadScene("Iteration001");
            yield return null;
            var bootstrapper = Object.FindFirstObjectByType<ExperienceBootstrapper>();
            Assert.That(bootstrapper, Is.Not.Null);
            Assert.That(GameObject.Find("ExperienceOverview").GetComponent<TextMesh>().text, Does.Contain("Selecciona Iniciar"));

            bootstrapper.StartExperience();
            var guidance = GameObject.Find("Guidance").GetComponent<TextMesh>();
            Assert.That(guidance.text, Does.Contain("utensilio"));

            GameObject.Find("Ingredient").GetComponent<InteractionTargetPresenter>().AttemptInteraction();
            var feedback = GameObject.Find("Feedback").GetComponent<TextMesh>();
            Assert.That(feedback.text, Does.Contain("Completa primero"));

            GameObject.Find("Utensil").GetComponent<InteractionTargetPresenter>().AttemptInteraction();
            Assert.That(feedback.text, Does.Contain("Recorrido completado"));
            Assert.That(GameObject.Find("ExperienceOverview").GetComponent<TextMesh>().text, Does.Contain("Resumen del recorrido"));
        }
    }
}
