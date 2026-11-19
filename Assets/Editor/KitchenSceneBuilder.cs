using System;
using System.Linq;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.Rendering;
using RvCulturas.Domain;
using RvCulturas.Infrastructure.Content;
using RvCulturas.Presentation.Interaction;
using RvCulturas.Presentation.UI;

/// <summary>Reproducible composition of the kitchen, leaving Iteration001 untouched.</summary>
public static class KitchenSceneBuilder
{
    private const string Art = "Assets/_Project/Art/Kitchen";
    public const string ScenePath = "Assets/_Project/Scenes/ChichaKitchen.unity";
    private static Transform environment;
    private static Material wood, cream, green, clay, metal, dark, gold;

    [MenuItem("RV Culturas/Build Chicha Kitchen")]
    public static void Build()
    {
        if (EditorApplication.isPlaying) throw new InvalidOperationException("Sal del modo Play antes de construir la cocina.");
        // Preserve current manual work before composing the separate scene.
        EditorSceneManager.SaveOpenScenes();
        Folder(Art);
        var scene = EditorSceneManager.NewScene(NewSceneSetup.EmptyScene, NewSceneMode.Single);
        environment = new GameObject("COCINA · Entorno").transform;
        wood = Mat("Madera", new Color(0.26f, 0.12f, 0.055f));
        cream = Mat("Marfil", new Color(0.83f, 0.78f, 0.65f));
        green = Mat("Salvia", new Color(0.17f, 0.32f, 0.25f));
        clay = Mat("Terracota", new Color(0.55f, 0.22f, 0.1f));
        metal = Mat("Acero", new Color(0.31f, 0.35f, 0.33f));
        dark = Mat("Carbón", new Color(0.055f, 0.075f, 0.065f));
        gold = Mat("Mostaza", new Color(0.86f, 0.55f, 0.12f));
        RenderSettings.ambientMode = AmbientMode.Trilight;
        RenderSettings.ambientSkyColor = new Color(0.65f, 0.75f, 0.79f);
        RenderSettings.ambientEquatorColor = new Color(0.5f, 0.47f, 0.37f);
        RenderSettings.ambientGroundColor = new Color(0.22f, 0.18f, 0.13f);
        var sunlight = new GameObject("Luz de mañana").AddComponent<Light>();
        sunlight.type = LightType.Directional; sunlight.intensity = 0.95f; sunlight.color = new Color(1, 0.95f, 0.85f);
        sunlight.transform.rotation = Quaternion.Euler(48, -35, 0); sunlight.shadows = LightShadows.Soft;
        Box("Suelo", new Vector3(0, -0.12f, 0), new Vector3(9, 0.24f, 8), clay);
        for (int i = -4; i <= 4; i++) Box("Junta del suelo", new Vector3(i, 0.004f, 0), new Vector3(0.015f, 0.008f, 8), cream, false);
        for (int i = -3; i <= 3; i++) Box("Junta del suelo", new Vector3(0, 0.004f, i), new Vector3(9, 0.008f, 0.015f), cream, false);
        Box("Pared del fondo", new Vector3(0, 1.8f, 2.8f), new Vector3(9, 3.6f, 0.16f), cream);
        Box("Pared izquierda", new Vector3(-4.5f, 1.8f, 0), new Vector3(0.16f, 3.6f, 8), cream);
        Box("Pared derecha", new Vector3(4.5f, 1.8f, 0), new Vector3(0.16f, 3.6f, 8), cream);
        Box("Límite posterior", new Vector3(0, 1, -4), new Vector3(9, 2, 0.16f), cream);
        Box("Zócalo", new Vector3(0, 0.15f, 2.66f), new Vector3(9, 0.28f, 0.06f), wood);
        for (int i = -3; i <= 3; i++) { var beam = Box("Viga", new Vector3(i * 1.3f, 3.4f, 0), new Vector3(0.18f, 0.2f, 7.5f), wood); beam.GetComponent<Renderer>().shadowCastingMode = ShadowCastingMode.Off; }
        Box("Ventana al paisaje", new Vector3(-2.6f, 2.02f, 2.69f), new Vector3(1.9f, 1.35f, 0.08f), Mat("Cielo", new Color(0.45f, 0.74f, 0.79f)));
        foreach (float x in new[] {-3.58f, -2.6f, -1.62f}) Box("Marco ventana", new Vector3(x, 2.02f, 2.6f), new Vector3(0.065f, 1.5f, 0.12f), wood);
        foreach (float y in new[] {1.3f, 2.72f}) Box("Marco ventana", new Vector3(-2.6f, y, 2.6f), new Vector3(2.05f, 0.07f, 0.12f), wood);
        Box("Encimera trasera", new Vector3(0, 0.93f, 2.16f), new Vector3(6.9f, 0.12f, 0.92f), wood);
        for (int i = -3; i <= 3; i++)
        {
            Box("Alacena", new Vector3(i * 0.94f, 0.46f, 2.2f), new Vector3(0.89f, 0.85f, 0.8f), green);
            Box("Tirador", new Vector3(i * 0.94f, 0.74f, 1.77f), new Vector3(0.3f, 0.028f, 0.05f), gold);
        }
        Box("Mesa accesible", new Vector3(0, 0.9f, 0.7f), new Vector3(4.6f, 0.12f, 1.1f), wood);
        foreach (var x in new[] {-2.05f, 2.05f}) foreach (var z in new[] {0.28f, 1.12f}) Box("Pata", new Vector3(x, 0.42f, z), new Vector3(0.13f, 0.84f, 0.13f), green);
        Box("Camino de mesa", new Vector3(0, 0.966f, 0.67f), new Vector3(4.25f, 0.008f, 0.74f), cream, false);
        for (int i = -10; i <= 10; i++) Box("Bordado geométrico", new Vector3(i * 0.19f, 0.973f, 0.34f), new Vector3(0.09f, 0.006f, 0.04f), clay, false);
        Label("TALLER DE CHICHA", new Vector3(0, 2.6f, 2.62f), 0.11f, dark);
        Label("Explora · prepara · comparte", new Vector3(0, 2.34f, 2.62f), 0.055f, dark);
        Label("Recorrido ilustrativo · pendiente de revisión cultural", new Vector3(0, 2.14f, 2.62f), 0.028f, dark);
        var shelf = Import("spices", new Vector3(2.4f, 1.46f, 2.45f), 0.85f, null);
        shelf.name = "Estante de especias · volvor";
        Import("fruit", new Vector3(-2.4f, 1.0f, 2.1f), 0.2f, "apple03_lr_apple03_lr_0").name = "Fruta decorativa";
        // A decorative plant made from low-cost primitives, no extra external resources.
        Cylinder("Maceta", new Vector3(-3.1f, 1.09f, 2.1f), new Vector3(0.3f, 0.14f, 0.3f), clay);
        for (int i = 0; i < 6; i++) { var leaf = Box("Hoja", new Vector3(-3.1f + Mathf.Sin(i) * 0.1f, 1.39f, 2.1f + Mathf.Cos(i) * 0.09f), new Vector3(0.06f, 0.4f, 0.09f), green, false); leaf.transform.rotation = Quaternion.Euler(i * 9, i * 60, 25); }
        var pot = Cylinder("Olla de cocción", new Vector3(0, 1.14f, 1.02f), new Vector3(0.62f, 0.16f, 0.62f), metal);
        Cylinder("Interior olla", new Vector3(0, 1.305f, 1.02f), new Vector3(0.54f, 0.006f, 0.54f), dark, false);
        foreach (float x in new[] {-0.39f, 0.39f}) Box("Asa olla", new Vector3(x, 1.22f, 1.02f), new Vector3(0.2f, 0.05f, 0.12f), dark);
        var liquid = Cylinder("Mezcla", new Vector3(0, 1.314f, 1.02f), new Vector3(0.51f, 0.004f, 0.51f), Mat("Chicha", new Color(0.65f, 0.43f, 0.16f)), false).GetComponent<Renderer>();
        var lesson = Lesson();
        var root = new GameObject("RECORRIDO · Preparación guiada");
        var presenter = root.AddComponent<KitchenLessonPresenter>(); presenter.lesson = lesson; presenter.liquid = liquid;
        var targets = new List<KitchenTarget>();
        var corn = Import("corn", new Vector3(-1.75f, 0.99f, 0.5f), 0.27f, "Object_2");
        foreach (var r in corn.GetComponentsInChildren<Renderer>()) r.sharedMaterial = gold;
        targets.Add(Target(corn, "corn", "1 · Maíz", new Vector3(-0.15f, 1.32f, 1.02f)));
        var jug = Cylinder("Jarra de agua", new Vector3(-1.05f, 1.13f, 0.5f), new Vector3(0.21f, 0.17f, 0.21f), Mat("Cerámica azul", new Color(0.15f, 0.4f, 0.51f)));
        targets.Add(Target(jug, "water", "2 · Agua", new Vector3(-0.4f, 1.38f, 1.02f)));
        var panelaMaterial = Mat("Panela", new Color(0.4f, 0.19f, 0.065f));
        var texture = AssetDatabase.LoadAssetAtPath<Texture2D>("Assets/Models/panela/texture/panela texture.jpg");
        if (texture != null) { panelaMaterial.SetTexture("_BaseMap", texture); EditorUtility.SetDirty(panelaMaterial); }
        var panela = Box("Panela", new Vector3(-0.37f, 1.02f, 0.5f), new Vector3(0.25f, 0.12f, 0.18f), panelaMaterial);
        targets.Add(Target(panela, "panela", "3 · Panela", new Vector3(0.1f, 1.32f, 1.02f)));
        var spice = Cylinder("Especias", new Vector3(0.4f, 1.06f, 0.5f), new Vector3(0.17f, 0.1f, 0.17f), clay);
        Cylinder("Tapa especias", new Vector3(0.4f, 1.17f, 0.5f), new Vector3(0.19f, 0.02f, 0.19f), wood);
        targets.Add(Target(spice, "spices", "4 · Especias", new Vector3(0.3f, 1.38f, 1.02f)));
        var spoon = Box("Cuchara de madera", new Vector3(1.1f, 1.0f, 0.5f), new Vector3(0.07f, 0.045f, 0.32f), wood);
        var scoop = Cylinder("Cabeza cuchara", new Vector3(1.1f, 1.0f, 0.69f), new Vector3(0.14f, 0.02f, 0.16f), wood, false); scoop.transform.SetParent(spoon.transform, true);
        targets.Add(Target(spoon, "mix", "5 · Mezclar", new Vector3(0, 1.35f, 1.02f)));
        var cup = Cylinder("Vaso de servicio", new Vector3(1.75f, 1.08f, 0.5f), new Vector3(0.2f, 0.12f, 0.2f), clay);
        Cylinder("Interior vaso", new Vector3(1.75f, 1.205f, 0.5f), new Vector3(0.165f, 0.003f, 0.165f), dark, false).transform.SetParent(cup.transform, true);
        targets.Add(Target(cup, "serve", "6 · Servir", new Vector3(0.65f, 1.08f, 0.5f)));
        presenter.targets = targets.ToArray();
        var rig = new GameObject("JUGADOR · Primera persona y XR"); rig.transform.position = new Vector3(0, 0.03f, -1.3f);
        var cc = rig.AddComponent<CharacterController>(); cc.height = 1.7f; cc.center = new Vector3(0, 0.85f, 0); cc.radius = 0.24f;
        var camera = new GameObject("Main Camera").AddComponent<Camera>(); camera.tag = "MainCamera"; camera.transform.SetParent(rig.transform, false); camera.transform.localPosition = new Vector3(0, 1.65f, 0); camera.fieldOfView = 70; camera.nearClipPlane = 0.04f; camera.farClipPlane = 40; camera.AdditionalSetup();
        var player = rig.AddComponent<KitchenPlayer>(); player.view = camera; presenter.player = player;
        BuildHands(player, rig.transform);
        var xrPanel = Box("Guía XR", new Vector3(0, 1.94f, 1.35f), new Vector3(2.1f, 0.95f, 0.03f), dark, false);
        var xrText = Label("Guía XR texto", new Vector3(-0.95f, 2.34f, 1.32f), 0.04f, cream).GetComponent<TextMesh>(); xrText.anchor = TextAnchor.UpperLeft; xrText.alignment = TextAlignment.Left;
        xrPanel.transform.SetParent(xrText.transform, true); presenter.xrGuide = xrText;
        AssetDatabase.SaveAssets();
        EditorSceneManager.SaveScene(scene, ScenePath);
        var builds = EditorBuildSettings.scenes.Where(s => s.path != ScenePath).ToList(); builds.Insert(0, new EditorBuildSettingsScene(ScenePath, true)); EditorBuildSettings.scenes = builds.ToArray();
        SceneView.lastActiveSceneView?.LookAt(new Vector3(0, 1.1f, 0.8f), Quaternion.Euler(22, 0, 0), 5);
        Debug.Log("ChichaKitchen creada: 6 acciones, modelos locales, manos, colisiones y accesibilidad.");
    }
    private static void AdditionalSetup(this Camera camera) { camera.backgroundColor = new Color(0.57f, 0.72f, 0.75f); camera.clearFlags = CameraClearFlags.SolidColor; camera.gameObject.AddComponent<AudioListener>(); }
    private static KitchenTarget Target(GameObject go, string id, string label, Vector3 destination)
    {
        go.name = label;
        var t = go.AddComponent<KitchenTarget>(); t.targetId = id; t.displayName = label;
        var d = new GameObject("Destino · " + id).transform; d.position = destination; t.destination = d;
        if (go.GetComponent<Collider>() == null)
        {
            var b = t.VisualBounds; var collider = go.AddComponent<BoxCollider>(); collider.center = go.transform.InverseTransformPoint(b.center); collider.size = new Vector3(b.size.x / go.transform.lossyScale.x, b.size.y / go.transform.lossyScale.y, b.size.z / go.transform.lossyScale.z);
        }
        Label(label, new Vector3(go.transform.position.x, 1.44f, 0.48f), 0.034f, dark);
        return t;
    }
    private static ExperienceDefinitionAsset Lesson()
    {
        var ids = new[] {"corn", "water", "panela", "spices", "mix", "serve"};
        var titles = new[] {"Preparar el maíz", "Añadir el agua", "Añadir la panela", "Añadir especias", "Mezclar", "Servir"};
        var text = new[] {"Selecciona el maíz (1) para llevarlo a la olla. Representa maíz previamente preparado.", "Selecciona la jarra de agua (2). Se representa su incorporación a la olla.", "Selecciona la panela (3) para añadir el endulzante a la mezcla.", "Selecciona las especias (4). Su uso y proporción dependen de la receta local.", "Selecciona la cuchara (5) para mezclar. La cocción y el enfriado se omiten en esta práctica.", "Selecciona el vaso (6) para terminar el servicio simulado. No se enseña fermentación real."};
        var steps = new PreparationStepAsset[6];
        Folder("Assets/_Project/Content/Kitchen");
        for (int i = 0; i < 6; i++)
        {
            var path = "Assets/_Project/Content/Kitchen/Step" + (i + 1) + ".asset";
            var step = AssetDatabase.LoadAssetAtPath<PreparationStepAsset>(path) ?? ScriptableObject.CreateInstance<PreparationStepAsset>();
            step.stepId = ids[i]; step.ordinal = i + 1; step.title = titles[i]; step.requiredTargets = new[] {ids[i]}; step.expectedAction = ExpectedAction.Use;
            step.visualInstruction = new GuidanceData {text = text[i], visualCue = "Número y nombre del objeto", iconKey = ids[i]};
            step.successFeedback = new GuidanceData {text = "Acción completada", visualCue = "Confirmación textual"}; step.correctionFeedback = new GuidanceData {text = "Sigue el paso indicado; puedes volver a intentar.", visualCue = "Guía persistente"};
            if (!AssetDatabase.Contains(step)) AssetDatabase.CreateAsset(step, path); EditorUtility.SetDirty(step); steps[i] = step;
        }
        var lessonPath = "Assets/_Project/Content/Kitchen/Lesson.asset";
        var lesson = AssetDatabase.LoadAssetAtPath<ExperienceDefinitionAsset>(lessonPath) ?? ScriptableObject.CreateInstance<ExperienceDefinitionAsset>();
        lesson.experienceId = "chicha-kitchen-practice"; lesson.title = "Taller de chicha · recorrido ilustrativo"; lesson.culturalContext = "Las preparaciones varían entre comunidades; este contenido requiere revisión cultural."; lesson.reviewStatus = ReviewStatus.Draft; lesson.steps = steps;
        if (!AssetDatabase.Contains(lesson)) AssetDatabase.CreateAsset(lesson, lessonPath); EditorUtility.SetDirty(lesson); return lesson;
    }
    private static GameObject Import(string folder, Vector3 position, float size, string node)
    {
        var prefab = AssetDatabase.LoadAssetAtPath<GameObject>("Assets/Models/" + folder + "/scene.gltf");
        if (prefab == null) throw new InvalidOperationException("Importa glTFast y espera a que Unity importe: " + folder);
        var root = UnityEngine.Object.Instantiate(prefab); root.name = folder;
        if (node != null) foreach (var r in root.GetComponentsInChildren<Renderer>()) if (r.name != node) r.gameObject.SetActive(false);
        root.transform.rotation = folder == "spices" ? Quaternion.Euler(0, 90, 0) : Quaternion.identity;
        var renderers = root.GetComponentsInChildren<Renderer>(); var bounds = renderers[0].bounds; foreach (var r in renderers) bounds.Encapsulate(r.bounds);
        root.transform.localScale *= size / Mathf.Max(bounds.size.x, bounds.size.y, bounds.size.z);
        bounds = renderers[0].bounds; foreach (var r in renderers) bounds.Encapsulate(r.bounds);
        root.transform.position += position - new Vector3(bounds.center.x, bounds.min.y, bounds.center.z);
        return root;
    }
    private static void BuildHands(KitchenPlayer player, Transform rig)
    {
        var source = Import("hands", Vector3.zero, 0.65f, null);
        var renderer = source.GetComponentInChildren<SkinnedMeshRenderer>();
        if (renderer == null) throw new InvalidOperationException("El modelo de manos requiere SkinnedMeshRenderer.");
        var baked = new Mesh(); renderer.BakeMesh(baked);
        var vertices = baked.vertices;
        for (var i = 0; i < vertices.Length; i++) vertices[i] = renderer.transform.TransformPoint(vertices[i]);
        baked.vertices = vertices; baked.RecalculateBounds();
        float center = baked.bounds.center.x;
        for (int side = 0; side < 2; side++)
        {
            var triangles = new List<int>(); var original = baked.triangles;
            for (int i = 0; i < original.Length; i += 3)
            {
                var x = (vertices[original[i]].x + vertices[original[i+1]].x + vertices[original[i+2]].x) / 3;
                if ((side == 0 && x < center) || (side == 1 && x >= center)) { triangles.Add(original[i]); triangles.Add(original[i+1]); triangles.Add(original[i+2]); }
            }
            var mesh = UnityEngine.Object.Instantiate(baked); mesh.triangles = triangles.ToArray();
            var used = triangles.Select(i => vertices[i]).ToArray(); var b = new Bounds(used[0], Vector3.zero); foreach (var v in used) b.Encapsulate(v);
            var handVertices = mesh.vertices;
            for (int i = 0; i < handVertices.Length; i++) handVertices[i] = (handVertices[i] - b.center) * (0.24f / Mathf.Max(b.size.x,b.size.y,b.size.z));
            mesh.vertices = handVertices; mesh.RecalculateNormals(); mesh.RecalculateBounds();
            var meshPath = Art + "/Hand" + side + ".asset";
            var old = AssetDatabase.LoadAssetAtPath<Mesh>(meshPath);
            if (old != null) { EditorUtility.CopySerialized(mesh, old); UnityEngine.Object.DestroyImmediate(mesh); mesh = old; } else AssetDatabase.CreateAsset(mesh, meshPath);
            var hand = new GameObject(side == 0 ? "Mano izquierda" : "Mano derecha"); hand.transform.SetParent(rig, false);
            hand.AddComponent<MeshFilter>().sharedMesh = mesh; hand.AddComponent<MeshRenderer>().sharedMaterial = renderer.sharedMaterial;
            if (side == 0) player.leftHand = hand.transform; else player.rightHand = hand.transform;
        }
        UnityEngine.Object.DestroyImmediate(baked); UnityEngine.Object.DestroyImmediate(source);
    }
    private static void Folder(string path) { if (AssetDatabase.IsValidFolder(path)) return; var parent = System.IO.Path.GetDirectoryName(path).Replace('\\','/'); Folder(parent); AssetDatabase.CreateFolder(parent, System.IO.Path.GetFileName(path)); }
    private static Material Mat(string name, Color color)
    {
        var path = Art + "/" + name + ".mat"; var m = AssetDatabase.LoadAssetAtPath<Material>(path);
        if (m == null) { m = new Material(Shader.Find("Universal Render Pipeline/Lit")); AssetDatabase.CreateAsset(m, path); }
        m.SetColor("_BaseColor", color); m.SetFloat("_Smoothness", 0.18f); EditorUtility.SetDirty(m); return m;
    }
    private static GameObject Box(string name, Vector3 p, Vector3 s, Material m, bool collision = true) => Primitive(name, PrimitiveType.Cube, p, s, m, collision);
    private static GameObject Cylinder(string name, Vector3 p, Vector3 s, Material m, bool collision = true) => Primitive(name, PrimitiveType.Cylinder, p, s, m, collision);
    private static GameObject Primitive(string name, PrimitiveType type, Vector3 p, Vector3 s, Material m, bool collision)
    {
        var go = GameObject.CreatePrimitive(type); go.name = name; go.transform.SetParent(environment, false); go.transform.position = p; go.transform.localScale = s; go.GetComponent<Renderer>().sharedMaterial = m;
        if (!collision) UnityEngine.Object.DestroyImmediate(go.GetComponent<Collider>()); return go;
    }
    private static GameObject Label(string text, Vector3 p, float size, Material color)
    {
        var go = new GameObject(text); go.transform.SetParent(environment, false); go.transform.position = p;
        var tm = go.AddComponent<TextMesh>(); tm.text = text; tm.fontSize = 64; tm.characterSize = size * 0.16f; tm.anchor = TextAnchor.MiddleCenter; tm.alignment = TextAlignment.Center; tm.color = color.color;
        go.GetComponent<Renderer>().shadowCastingMode = ShadowCastingMode.Off;
        return go;
    }
}
