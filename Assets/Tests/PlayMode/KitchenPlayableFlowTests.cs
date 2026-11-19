using System.Collections;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.TestTools;
using RvCulturas.Domain;
using RvCulturas.Presentation.UI;
using RvCulturas.Presentation.Interaction;

namespace RvCulturas.Tests.PlayMode
{
    public sealed class KitchenPlayableFlowTests
    {
        [UnityTest]
        public IEnumerator CornV2HasCompleteVisualAndWindowHasNoOpaqueWall()
        {
            SceneManager.LoadScene("ChichaKitchen"); yield return null;
            var p = Object.FindFirstObjectByType<KitchenLessonPresenter>();
            var corn = p.targets[0];
            Assert.That(corn.GetComponentsInChildren<MeshFilter>().Length, Is.EqualTo(1), "El maíz debe usar la malla unida de cornV2.");
            var b = corn.VisualBounds;
            Assert.That(Mathf.Max(b.size.x,b.size.y,b.size.z), Is.InRange(0.31f,0.33f));
            Assert.That(corn.GetComponent<BoxCollider>().bounds.Contains(b.center), Is.True);
            var hits = Physics.RaycastAll(new Vector3(-2.4f,2.05f,2.4f),Vector3.forward,0.8f);
            foreach (var hit in hits) Assert.That(hit.collider.GetComponent<Renderer>(), Is.Null, "Una superficie opaca bloquea la ventana.");
            var house = GameObject.Find("Casa de paja");
            Assert.That(house, Is.Not.Null);
            Assert.That(house.GetComponentsInChildren<Camera>(), Is.Empty);
            Assert.That(house.GetComponentsInChildren<Light>(), Is.Empty);
        }
        [UnityTest]
        public IEnumerator SixAnimatedActionsRejectErrorsPauseCompleteAndReset()
        {
            SceneManager.LoadScene("ChichaKitchen"); yield return null;
            var p = Object.FindFirstObjectByType<KitchenLessonPresenter>();
            Assert.That(p, Is.Not.Null); Assert.That(p.targets.Length, Is.EqualTo(6));
            p.Begin(); p.SelectIndex(5); Assert.That(p.Step, Is.EqualTo(1));
            p.Pause(); p.SelectIndex(0); Assert.That(p.Step, Is.EqualTo(1)); p.Pause();
            for (int i = 0; i < 6; i++)
            {
                p.SelectIndex(i); yield return new WaitForSeconds(0.85f);
                Assert.That(Vector3.Distance(p.targets[i].transform.position, p.targets[i].destination.position), Is.LessThan(0.01f));
            }
            Assert.That(p.Lifecycle, Is.EqualTo(ExperienceLifecycle.Completed));
            p.ResetLesson(); Assert.That(p.Lifecycle, Is.EqualTo(ExperienceLifecycle.Intro)); Assert.That(p.liquid.enabled, Is.False);
        }
        [UnityTest]
        public IEnumerator BodyCannotWalkThroughTableAndBothHandsHaveMeshes()
        {
            SceneManager.LoadScene("ChichaKitchen"); yield return null;
            var player = Object.FindFirstObjectByType<KitchenPlayer>();
            Assert.That(player.leftHand.GetComponent<MeshFilter>().sharedMesh.vertexCount, Is.GreaterThan(0));
            Assert.That(player.rightHand.GetComponent<MeshFilter>().sharedMesh.vertexCount, Is.GreaterThan(0));
            var cc = player.GetComponent<CharacterController>();
            for (int i = 0; i < 40; i++) { cc.Move(Vector3.forward * 0.1f); yield return null; }
            Assert.That(player.transform.position.z, Is.LessThan(0.15f), "La cápsula no debe atravesar la mesa.");
        }
    }
}
