using System.Collections;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.TestTools;
using RvCulturas.Presentation.Bootstrap;
using RvCulturas.Presentation.Interaction;

namespace RvCulturas.Tests.PlayMode
{
    public class AudioIndependentGuidanceTests
    {
        [UnityTest]
        public IEnumerator TextGuidanceAndCorrectionWorkWithAudioListenerDisabled()
        {
            SceneManager.LoadScene("Iteration001");
            yield return null;
            var listener = Object.FindFirstObjectByType<AudioListener>();
            if (listener != null) listener.enabled = false;
            var bootstrapper = Object.FindFirstObjectByType<ExperienceBootstrapper>();
            bootstrapper.StartExperience();
            GameObject.Find("Ingredient").GetComponent<InteractionTargetPresenter>().AttemptInteraction();
            Assert.That(GameObject.Find("Guidance").GetComponent<TextMesh>().text, Is.Not.Empty);
            Assert.That(GameObject.Find("Feedback").GetComponent<TextMesh>().text, Does.Contain("Completa primero"));
            Assert.That(GameObject.Find("FeedbackSymbol").GetComponent<TextMesh>().text, Does.Contain("REVISA"));
        }
    }
}
