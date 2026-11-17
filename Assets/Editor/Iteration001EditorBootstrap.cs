using System.IO;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;
using Unity.XR.CoreUtils;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Inputs.Simulation;
using UnityEngine.XR.Interaction.Toolkit.Interactables;
using UnityEngine.XR.Interaction.Toolkit.Interactors;
using UnityEngine.XR.Interaction.Toolkit.Interactors.Visuals;
using UnityEngine.XR.Interaction.Toolkit.Inputs;
using UnityEngine.XR.Interaction.Toolkit.Inputs.Readers;
using UnityEngine.InputSystem;
using RvCulturas.Domain;
using RvCulturas.Infrastructure.Content;
using RvCulturas.Presentation.Bootstrap;
using RvCulturas.Presentation.Interaction;
using RvCulturas.Presentation.UI;

public static class Iteration001EditorBootstrap
{
    private const string ContentPath = "Assets/_Project/Content/Experiences";
    private const string ScenePath = "Assets/_Project/Scenes/Iteration001.unity";
    private const string InputActionsPath = "Assets/XR/Input/Iteration001.inputactions";

    [MenuItem("RV Culturas/Generate Iteration 001")]
    public static void Generate()
    {
        EnsureFolder("Assets/_Project");
        EnsureFolder("Assets/_Project/Content");
        EnsureFolder(ContentPath);
        EnsureFolder("Assets/_Project/Scenes");
        EnsureFolder("Assets/_Project/Prefabs");
        EnsureFolder("Assets/_Project/Prefabs/Ingredients");
        EnsureFolder("Assets/_Project/Prefabs/Utensils");
        EnsureFolder("Assets/_Project/UI");
        CreatePrefabAssets();
        var experience = CreateExperience();
        CreateScene(experience);
        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
        Debug.Log("Iteración 001 generada.");
    }

    public static void GenerateAndExit()
    {
        Generate();
        EditorApplication.Exit(0);
    }

    private static ExperienceDefinitionAsset CreateExperience()
    {
        var path = ContentPath + "/Iteration001Experience.asset";
        AssetDatabase.DeleteAsset(path);
        var experience = ScriptableObject.CreateInstance<ExperienceDefinitionAsset>();
        experience.experienceId = "iteration001-chicha-draft";
        experience.title = "Preparación guiada: paso de práctica";
        experience.culturalContext = "Contenido educativo en revisión cultural. La variante y procedencia se confirmarán antes de publicación.";
        experience.sourceReference = "Borrador interno: requiere validación cultural antes de publicación.";
        experience.reviewStatus = ReviewStatus.Draft;

        var step = ScriptableObject.CreateInstance<PreparationStepAsset>();
        step.name = "Paso_01_Usar_Utensilio";
        step.stepId = "step-use-utensil";
        step.ordinal = 1;
        step.title = "Usa el utensilio indicado";
        step.requiredTargets = new[] { "utensil" };
        step.expectedAction = ExpectedAction.Use;
        step.visualInstruction = Guidance("Selecciona el utensilio sobre la mesa.", "mano", "instruccion");
        step.successFeedback = Guidance("Correcto. Has completado el paso de práctica.", "check", "correcto");
        step.correctionFeedback = Guidance("Aún no. Selecciona el utensilio indicado.", "flecha", "reintentar");
        CreateOrReplaceAsset(step, ContentPath + "/Iteration001Step01.asset");

        var ingredient = Element("ingredient", CulturalElementCategory.Ingredient, "Ingrediente de práctica", "Elemento provisional para validar la experiencia. Requiere revisión cultural.", "ingredient-greybox");
        var bowl = Element("bowl", CulturalElementCategory.Utensil, "Recipiente de práctica", "Recipiente provisional para validar la interacción.", "bowl-greybox");
        var utensil = Element("utensil", CulturalElementCategory.Utensil, "Utensilio de práctica", "Utensilio provisional para validar el paso.", "utensil-greybox");
        CreateOrReplaceAsset(ingredient, ContentPath + "/Iteration001Ingredient.asset");
        CreateOrReplaceAsset(bowl, ContentPath + "/Iteration001Bowl.asset");
        CreateOrReplaceAsset(utensil, ContentPath + "/Iteration001Utensil.asset");

        var manifest = ScriptableObject.CreateInstance<AssetManifestAsset>();
        manifest.name = "Iteration001AssetManifest";
        manifest.assetKey = "iteration001-greybox";
        manifest.assetType = "Primitive greybox";
        manifest.source = "Creado dentro del proyecto para prototipado";
        manifest.licenseOrPermission = "Activo propio de prototipo";
        manifest.culturalReview = ReviewStatus.Draft;
        manifest.optimizationStatus = ReviewStatus.Draft;
        CreateOrReplaceAsset(manifest, ContentPath + "/Iteration001AssetManifest.asset");

        experience.steps = new[] { step };
        experience.culturalElements = new[] { ingredient, bowl, utensil };
        experience.assets = new[] { manifest };
        AssetDatabase.CreateAsset(experience, path);
        return experience;
    }

    private static GuidanceData Guidance(string text, string icon, string visualCue)
    {
        return new GuidanceData { text = text, iconKey = icon, visualCue = visualCue, audioCue = string.Empty, repeatable = true };
    }

    private static CulturalElementAsset Element(string id, CulturalElementCategory category, string name, string explanation, string assetKey)
    {
        var element = ScriptableObject.CreateInstance<CulturalElementAsset>();
        element.name = id;
        element.elementId = id;
        element.category = category;
        element.displayName = name;
        element.explanation = explanation;
        element.assetKey = assetKey;
        element.sourceReference = "Borrador interno, revisión cultural pendiente.";
        element.reviewStatus = ReviewStatus.Draft;
        return element;
    }

    private static void CreateScene(ExperienceDefinitionAsset experience)
    {
        var scene = EditorSceneManager.NewScene(NewSceneSetup.DefaultGameObjects, NewSceneMode.Single);
        var root = new GameObject("Iteration001");
        var interactionManager = root.AddComponent<XRInteractionManager>();
        var inputActions = AssetDatabase.LoadAssetAtPath<InputActionAsset>(InputActionsPath);
        var inputActionManager = root.AddComponent<InputActionManager>();
        inputActionManager.actionAssets = new List<InputActionAsset> { inputActions };
        var adapter = root.AddComponent<XrInteractionAdapter>();
        var camera = Camera.main;
        var xrOriginObject = new GameObject("XR Origin (Simulation Ready)");
        var xrOrigin = xrOriginObject.AddComponent<XROrigin>();
        camera.transform.position = new Vector3(0f, 1.6f, -5f);
        camera.transform.rotation = Quaternion.Euler(8f, 0f, 0f);
        camera.transform.SetParent(xrOriginObject.transform, true);
        xrOrigin.Camera = camera;
        var simulator = xrOriginObject.AddComponent<XRInteractionSimulator>();
        simulator.cameraTransform = camera.transform;
        var selectAction = inputActions.FindAction("XRInteraction/Select", true);
        CreateXrInteractor(xrOriginObject.transform, interactionManager, selectAction, "Left Direct Interactor", new Vector3(-0.18f, -0.18f, 0.35f), true);
        CreateXrInteractor(xrOriginObject.transform, interactionManager, selectAction, "Right Ray Interactor", new Vector3(0.18f, -0.18f, 0.35f), false);
        var directionalLight = Object.FindFirstObjectByType<Light>();
        if (directionalLight != null) directionalLight.lightmapBakeType = LightmapBakeType.Baked;

        var guidancePanel = CreatePanel("GuidancePanel", new Vector3(-2.5f, 2.3f, 0.25f), new Vector3(3.8f, 1.1f, 1f), new Color(0.06f, 0.08f, 0.12f, 1f));
        var guidanceObject = CreateText("Guidance", new Vector3(-2.5f, 2.3f, 0f), 0.22f, Color.white);
        var guidance = guidanceObject.AddComponent<AccessibleGuidancePresenter>();
        guidance.Configure(guidanceObject.GetComponent<TextMesh>(), guidancePanel);
        var feedbackPanel = CreatePanel("FeedbackPanel", new Vector3(-2.5f, 1.65f, 0.25f), new Vector3(3.8f, 0.65f, 1f), new Color(0.12f, 0.12f, 0.12f, 1f));
        var feedbackObject = CreateText("Feedback", new Vector3(-2.5f, 1.7f, 0f), 0.18f, Color.yellow);
        var feedback = feedbackObject.AddComponent<AccessibleFeedbackPresenter>();
        var feedbackIndicator = CreatePrimitive("FeedbackIndicator", PrimitiveType.Quad, new Vector3(-3.8f, 1.7f, 0.15f), new Vector3(0.3f, 0.3f, 1f), new Color(0.85f, 0.45f, 0.15f));
        var feedbackSymbol = CreateText("FeedbackSymbol", new Vector3(-3.55f, 1.7f, 0f), 0.11f, Color.white).GetComponent<TextMesh>();
        feedback.Configure(feedbackObject.GetComponent<TextMesh>(), feedbackIndicator.GetComponent<Renderer>(), feedbackSymbol);
        var contextPanel = CreatePanel("CulturalContextPanel", new Vector3(2.5f, 2.3f, 0.25f), new Vector3(3.8f, 1.7f, 1f), new Color(0.04f, 0.12f, 0.16f, 1f));
        var contextObject = CreateText("CulturalContext", new Vector3(2.5f, 2.3f, 0f), 0.16f, Color.cyan);
        var context = contextObject.AddComponent<CulturalContextPanelPresenter>();
        context.Configure(contextObject.GetComponent<TextMesh>(), contextPanel);
        contextPanel.SetActive(false);
        var overviewPanel = CreatePanel("ExperienceOverviewPanel", new Vector3(0f, 3.25f, 0.25f), new Vector3(5.8f, 1.1f, 1f), new Color(0.08f, 0.08f, 0.14f, 1f));
        var overview = CreateText("ExperienceOverview", new Vector3(-2.6f, 3.25f, 0f), 0.18f, Color.white).AddComponent<ExperienceOverviewPresenter>();
        overview.Configure(overview.GetComponent<TextMesh>(), overviewPanel);
        var controlsPanel = CreatePanel("ControlsPanel", new Vector3(-2.5f, 0.8f, 0.25f), new Vector3(3.8f, 0.55f, 1f), new Color(0.08f, 0.08f, 0.08f, 1f));
        var controls = CreateText("Controls", new Vector3(-2.5f, 0.8f, 0f), 0.13f, Color.white).AddComponent<SessionControlsPresenter>();

        var floor = CreatePrimitive("Floor", PrimitiveType.Plane, Vector3.zero, new Vector3(1.5f, 1f, 1.5f), new Color(0.35f, 0.25f, 0.15f));
        var table = CreatePrimitive("Table", PrimitiveType.Cube, new Vector3(0f, 0.75f, 0f), new Vector3(3.2f, 1.2f, 1.5f), new Color(0.35f, 0.18f, 0.08f));
        GameObjectUtility.SetStaticEditorFlags(floor, StaticEditorFlags.ContributeGI | StaticEditorFlags.BatchingStatic);
        GameObjectUtility.SetStaticEditorFlags(table, StaticEditorFlags.ContributeGI | StaticEditorFlags.BatchingStatic);
        var ingredient = InstantiateTarget("Assets/_Project/Prefabs/Ingredients/Ingredient_Greybox.prefab", "Ingredient", new Vector3(-0.8f, 1.55f, 0f));
        var bowl = InstantiateTarget("Assets/_Project/Prefabs/Utensils/Bowl_Greybox.prefab", "Bowl", new Vector3(0f, 1.55f, 0f));
        var utensil = InstantiateTarget("Assets/_Project/Prefabs/Utensils/Utensil_Greybox.prefab", "Utensil", new Vector3(0.8f, 1.55f, 0f));
        adapter.SetTargets(new[] { ingredient, bowl, utensil });

        var bootstrap = root.AddComponent<ExperienceBootstrapper>();
        bootstrap.Configure(experience, guidance, feedback, context, overview, controls, adapter);
        controls.Configure(bootstrap, inputActions);
        CreateCulturalContextButton(bootstrap, "IngredientInfo", "ingredient", new Vector3(-1.25f, 1.25f, 0f));
        CreateCulturalContextButton(bootstrap, "BowlInfo", "bowl", new Vector3(0f, 1.25f, 0f));
        CreateCulturalContextButton(bootstrap, "UtensilInfo", "utensil", new Vector3(1.25f, 1.25f, 0f));
        EditorSceneManager.SaveScene(scene, ScenePath);
        EditorBuildSettings.scenes = new[] { new EditorBuildSettingsScene(ScenePath, true) };
    }

    private static void CreateXrInteractor(Transform parent, XRInteractionManager interactionManager, InputAction selectAction, string name, Vector3 localPosition, bool direct)
    {
        var controllerObject = new GameObject(name);
        controllerObject.transform.SetParent(parent, false);
        controllerObject.transform.localPosition = localPosition;
        controllerObject.AddComponent<ActionBasedController>();
        XRBaseInputInteractor interactor;
        if (direct) interactor = controllerObject.AddComponent<XRDirectInteractor>();
        else interactor = controllerObject.AddComponent<XRRayInteractor>();
        interactor.interactionManager = interactionManager;
        interactor.selectInput = new XRInputButtonReader("Select", inputSourceMode: XRInputButtonReader.InputSourceMode.InputAction)
        {
            inputActionPerformed = selectAction
        };
        if (!direct) controllerObject.AddComponent<XRInteractorLineVisual>();
    }

    private static void CreateCulturalContextButton(ExperienceBootstrapper bootstrapper, string name, string elementId, Vector3 position)
    {
        var button = CreatePrimitive(name, PrimitiveType.Cube, position, new Vector3(0.45f, 0.12f, 0.08f), new Color(0.15f, 0.5f, 0.65f));
        var interactable = button.AddComponent<XRSimpleInteractable>();
        button.AddComponent<CulturalContextTrigger>().Configure(bootstrapper, elementId, interactable);
        var label = CreateText(name + "Label", position + new Vector3(-0.18f, 0f, -0.06f), 0.08f, Color.white).GetComponent<TextMesh>();
        label.text = "INFO";
    }

    private static InteractionTargetPresenter CreateTarget(string name, PrimitiveType primitive, Vector3 position, Color color, string id, ExpectedAction action)
    {
        var gameObject = CreatePrimitive(name, primitive, position, Vector3.one * 0.45f, color);
        var target = gameObject.AddComponent<InteractionTargetPresenter>();
        target.Configure(id, action);
        var interactable = gameObject.AddComponent<XRSimpleInteractable>();
        gameObject.AddComponent<XrSelectRelay>().Configure(target, interactable);
        return target;
    }

    private static void CreatePrefabAssets()
    {
        CreateInteractablePrefab("Ingredient_Greybox", PrimitiveType.Sphere, new Color(0.85f, 0.65f, 0.15f), "ingredient", ExpectedAction.Take, "Assets/_Project/Prefabs/Ingredients/Ingredient_Greybox.prefab");
        CreateInteractablePrefab("Bowl_Greybox", PrimitiveType.Cylinder, new Color(0.35f, 0.55f, 0.8f), "bowl", ExpectedAction.Place, "Assets/_Project/Prefabs/Utensils/Bowl_Greybox.prefab");
        CreateInteractablePrefab("Utensil_Greybox", PrimitiveType.Capsule, new Color(0.75f, 0.35f, 0.2f), "utensil", ExpectedAction.Use, "Assets/_Project/Prefabs/Utensils/Utensil_Greybox.prefab");

        var guidancePanel = GameObject.CreatePrimitive(PrimitiveType.Quad);
        guidancePanel.name = "AccessibleGuidanceCanvas";
        guidancePanel.transform.localScale = new Vector3(3.8f, 1.3f, 1f);
        guidancePanel.GetComponent<Renderer>().sharedMaterial.color = new Color(0.06f, 0.08f, 0.12f, 1f);
        PrefabUtility.SaveAsPrefabAsset(guidancePanel, "Assets/_Project/UI/AccessibleGuidanceCanvas.prefab");
        Object.DestroyImmediate(guidancePanel);

        var contextPanel = GameObject.CreatePrimitive(PrimitiveType.Quad);
        contextPanel.name = "CulturalContextPanel";
        contextPanel.transform.localScale = new Vector3(3.8f, 1.3f, 1f);
        contextPanel.GetComponent<Renderer>().sharedMaterial.color = new Color(0.04f, 0.12f, 0.16f, 1f);
        PrefabUtility.SaveAsPrefabAsset(contextPanel, "Assets/_Project/UI/CulturalContextPanel.prefab");
        Object.DestroyImmediate(contextPanel);

        var controlsPanel = GameObject.CreatePrimitive(PrimitiveType.Quad);
        controlsPanel.name = "SessionControlsCanvas";
        controlsPanel.transform.localScale = new Vector3(3.8f, 0.6f, 1f);
        controlsPanel.GetComponent<Renderer>().sharedMaterial.color = new Color(0.08f, 0.08f, 0.08f, 1f);
        PrefabUtility.SaveAsPrefabAsset(controlsPanel, "Assets/_Project/UI/SessionControlsCanvas.prefab");
        Object.DestroyImmediate(controlsPanel);
    }

    private static void CreateInteractablePrefab(string name, PrimitiveType primitive, Color color, string id, ExpectedAction action, string path)
    {
        var source = CreateTarget(name, primitive, Vector3.zero, color, id, action);
        PrefabUtility.SaveAsPrefabAsset(source.gameObject, path);
        Object.DestroyImmediate(source.gameObject);
    }

    private static InteractionTargetPresenter InstantiateTarget(string prefabPath, string sceneName, Vector3 position)
    {
        var prefab = AssetDatabase.LoadAssetAtPath<GameObject>(prefabPath);
        var instance = (GameObject)PrefabUtility.InstantiatePrefab(prefab);
        instance.name = sceneName;
        instance.transform.position = position;
        return instance.GetComponent<InteractionTargetPresenter>();
    }

    private static GameObject CreatePrimitive(string name, PrimitiveType primitive, Vector3 position, Vector3 scale, Color color)
    {
        var gameObject = GameObject.CreatePrimitive(primitive);
        gameObject.name = name;
        gameObject.transform.position = position;
        gameObject.transform.localScale = scale;
        gameObject.GetComponent<Renderer>().sharedMaterial.color = color;
        return gameObject;
    }

    private static GameObject CreatePanel(string name, Vector3 position, Vector3 scale, Color color)
    {
        return CreatePrimitive(name, PrimitiveType.Quad, position, scale, color);
    }

    private static GameObject CreateText(string name, Vector3 position, float size, Color color)
    {
        var gameObject = new GameObject(name);
        gameObject.transform.position = position;
        var text = gameObject.AddComponent<TextMesh>();
        text.text = string.Empty;
        text.characterSize = size;
        text.fontSize = 60;
        text.anchor = TextAnchor.MiddleLeft;
        text.color = color;
        return gameObject;
    }

    private static void EnsureFolder(string path)
    {
        if (AssetDatabase.IsValidFolder(path)) return;
        var parent = Path.GetDirectoryName(path).Replace("\\", "/");
        var name = Path.GetFileName(path);
        EnsureFolder(parent);
        AssetDatabase.CreateFolder(parent, name);
    }

    private static void CreateOrReplaceAsset(Object asset, string path)
    {
        AssetDatabase.DeleteAsset(path);
        AssetDatabase.CreateAsset(asset, path);
    }
}
