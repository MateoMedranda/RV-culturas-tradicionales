using System.Collections;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.TestTools;
using RvCulturas.Presentation.Bootstrap;

namespace RvCulturas.Tests.PlayMode
{
    public class SessionControlsFlowTests
    {
        [UnityTest]
        public IEnumerator PauseRepeatAndRestartKeepTheExperienceControllable()
        {
            SceneManager.LoadScene("Iteration001");
            yield return null;
            var bootstrapper = Object.FindFirstObjectByType<ExperienceBootstrapper>();
            bootstrapper.StartExperience();
            bootstrapper.TogglePause();
            bootstrapper.RepeatGuidance();
            Assert.That(GameObject.Find("Feedback").GetComponent<TextMesh>().text, Does.Contain("Usa el utensilio"));
            bootstrapper.TogglePause();
            bootstrapper.RestartExperience();
            Assert.That(GameObject.Find("Feedback").GetComponent<TextMesh>().text, Does.Contain("reiniciado"));
        }
    }
}
