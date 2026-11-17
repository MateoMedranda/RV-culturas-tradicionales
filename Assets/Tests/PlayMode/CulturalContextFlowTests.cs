using System.Collections;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.TestTools;
using RvCulturas.Presentation.Bootstrap;

namespace RvCulturas.Tests.PlayMode
{
    public class CulturalContextFlowTests
    {
        [UnityTest]
        public IEnumerator CulturalContextIsShownWithoutChangingTheActiveGuidance()
        {
            SceneManager.LoadScene("Iteration001");
            yield return null;
            var bootstrapper = Object.FindFirstObjectByType<ExperienceBootstrapper>();
            bootstrapper.StartExperience();
            var before = GameObject.Find("Guidance").GetComponent<TextMesh>().text;
            bootstrapper.ShowCulturalContext("ingredient");
            var context = GameObject.Find("CulturalContext").GetComponent<TextMesh>().text;
            Assert.That(context, Does.Contain("Ingrediente de práctica"));
            Assert.That(GameObject.Find("Guidance").GetComponent<TextMesh>().text, Is.EqualTo(before));
        }
    }
}
