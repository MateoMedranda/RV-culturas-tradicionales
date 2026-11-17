using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit.Interactors;
using UnityEngine.XR.Interaction.Toolkit.Inputs.Simulation;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactors.Visuals;
using RvCulturas.Presentation.UI;

namespace RvCulturas.Presentation.Interaction
{
    /// <summary>Hace legible la escena existente mientras se migra la interfaz 3D a HUD de escritorio.</summary>
    public static class DesktopSimulationSceneRepair
    {
        private static readonly string[] LegacyText =
        {
            "Guidance", "Feedback", "FeedbackSymbol", "CulturalContext", "ExperienceOverview", "Controls",
            "IngredientInfoLabel", "BowlInfoLabel", "UtensilInfoLabel"
        };

        private static readonly string[] LegacyPanels =
        {
            "GuidancePanel", "FeedbackPanel", "FeedbackIndicator", "CulturalContextPanel", "ExperienceOverviewPanel", "ControlsPanel"
        };

        public static void Apply(GameObject root)
        {
            if (root.GetComponent<DesktopSimulationHelpOverlay>() == null) root.AddComponent<DesktopSimulationHelpOverlay>();
            ApplyColor("Floor", new Color(0.16f, 0.31f, 0.19f));
            ApplyColor("Table", new Color(0.38f, 0.15f, 0.045f));
            ApplyColor("Ingredient", new Color(0.93f, 0.66f, 0.12f));
            ApplyColor("Bowl", new Color(0.12f, 0.42f, 0.75f));
            ApplyColor("Utensil", new Color(0.72f, 0.2f, 0.12f));
            ApplyColor("IngredientInfo", new Color(0.06f, 0.48f, 0.67f));
            ApplyColor("BowlInfo", new Color(0.06f, 0.48f, 0.67f));
            ApplyColor("UtensilInfo", new Color(0.06f, 0.48f, 0.67f));

            foreach (var name in LegacyText) HideRenderer(name);
            foreach (var name in LegacyPanels) HideObject(name);
            HideObject("IngredientInfo");
            HideObject("BowlInfo");
            HideObject("UtensilInfo");

            var camera = Camera.main;
            if (camera != null)
            {
                camera.clearFlags = CameraClearFlags.SolidColor;
                camera.backgroundColor = new Color(0.07f, 0.14f, 0.23f);
                var origin = GameObject.Find("XR Origin (Simulation Ready)");
                if (origin != null)
                {
                    var movement = origin.GetComponent<DesktopSimulationLocomotion>() ?? origin.AddComponent<DesktopSimulationLocomotion>();
                    movement.Configure(origin.transform, camera.transform);
                    var simulator = origin.GetComponent<XRInteractionSimulator>();
                    if (simulator != null && UnityEngine.Application.isEditor) simulator.enabled = false;
                }

                var ray = Object.FindFirstObjectByType<XRRayInteractor>();
                if (ray != null)
                {
                    ray.transform.SetParent(camera.transform, false);
                    ray.transform.localPosition = new Vector3(0.12f, -0.08f, 0.1f);
                    ray.transform.localRotation = Quaternion.identity;
                    var controller = ray.GetComponent<ActionBasedController>();
                    if (controller != null && UnityEngine.Application.isEditor) controller.enabled = false;
                    var lineVisual = ray.GetComponent<XRInteractorLineVisual>();
                    if (lineVisual != null && UnityEngine.Application.isEditor) lineVisual.enabled = false;
                }
                var direct = Object.FindFirstObjectByType<XRDirectInteractor>();
                if (direct != null && UnityEngine.Application.isEditor) direct.enabled = false;
            }
        }

        private static void ApplyColor(string objectName, Color color)
        {
            var target = GameObject.Find(objectName);
            if (target == null) return;
            var renderer = target.GetComponent<Renderer>();
            if (renderer == null) return;
            var shader = Shader.Find("Sprites/Default") ?? Shader.Find("Unlit/Color");
            var material = new Material(shader) { color = color };
            if (material.HasProperty("_BaseColor")) material.SetColor("_BaseColor", color);
            renderer.material = material;
        }

        private static void HideRenderer(string objectName)
        {
            var target = GameObject.Find(objectName);
            if (target == null) return;
            var renderer = target.GetComponent<Renderer>();
            if (renderer != null) renderer.enabled = false;
        }

        private static void HideObject(string objectName)
        {
            var target = GameObject.Find(objectName);
            if (target != null) target.SetActive(false);
        }
    }
}
