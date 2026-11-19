using System;
using System.Linq;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using RvCulturas.Presentation.Interaction;

/// <summary>Updates only the corn and the window, preserving other scene edits.</summary>
public static class KitchenAssetUpgrade
{
    public const string CornPath = "Assets/Models/corn/cornV2.glb";
    public const string HousePath = "Assets/Models/casaPaja/casa_paja.fbx";
    private const string Art = "Assets/_Project/Art/Kitchen/";

    [MenuItem("RV Culturas/Update Corn V2 and Exterior Window")]
    public static void Apply()
    {
        var scene = UnityEngine.SceneManagement.SceneManager.GetActiveScene();
        if (EditorApplication.isPlaying || scene.path != KitchenSceneBuilder.ScenePath)
            throw new InvalidOperationException("Abre ChichaKitchen y detén Play antes de actualizar.");
        if (AssetDatabase.LoadAssetAtPath<GameObject>(CornPath) == null || AssetDatabase.LoadAssetAtPath<GameObject>(HousePath) == null)
            throw new InvalidOperationException("Espera a que Unity termine de importar cornV2 y casa_paja.");
        var target = UnityEngine.Object.FindObjectsByType<KitchenTarget>(FindObjectsSortMode.None).Single(t => t.targetId == "corn");
        var environment = GameObject.Find("COCINA · Entorno").transform;
        Undo.IncrementCurrentGroup(); var group = Undo.GetCurrentGroup(); Undo.SetCurrentGroupName("Corn V2 y ventana exterior");
        Undo.RecordObject(target.transform, "Normalizar raíz del maíz");
        foreach (var child in target.transform.Cast<Transform>().ToArray()) Undo.DestroyObjectImmediate(child.gameObject);
        var position = target.transform.position; position.y = 0.99f;
        target.transform.SetPositionAndRotation(position, Quaternion.identity); target.transform.localScale = Vector3.one;
        var visual = ImportCorn(position);
        Undo.RegisterCreatedObjectUndo(visual, "Maíz completo"); visual.transform.SetParent(target.transform, true);
        FitCollider(target);
        CreateWindow(environment);
        EditorSceneManager.MarkSceneDirty(scene); AssetDatabase.SaveAssets(); EditorSceneManager.SaveScene(scene);
        Undo.CollapseUndoOperations(group);
    }

    public static GameObject ImportCorn(Vector3 position)
    {
        var source = AssetDatabase.LoadAssetAtPath<GameObject>(CornPath);
        if (source == null) throw new InvalidOperationException("No se importó cornV2.glb.");
        var go = UnityEngine.Object.Instantiate(source); go.name = "cornV2 · malla completa";
        Normalize(go, position, 0.32f);
        return go;
    }

    private static void FitCollider(KitchenTarget target)
    {
        var collider = target.GetComponent<BoxCollider>();
        if (collider == null) collider = Undo.AddComponent<BoxCollider>(target.gameObject);
        Undo.RecordObject(collider, "Collider maíz completo");
        var b = BoundsOf(target.gameObject); collider.center = target.transform.InverseTransformPoint(b.center);
        collider.size = b.size + Vector3.one * 0.025f;
    }

    public static void CreateWindow(Transform environment)
    {
        var names = new[] {"Pared del fondo", "Ventana al paisaje", "Marco ventana", "Ventana exterior · Casa de paja", "EXTERIOR · Casa de paja"};
        foreach (var t in environment.Cast<Transform>().Where(t => names.Contains(t.name)).ToArray()) Undo.DestroyObjectImmediate(t.gameObject);
        var root = new GameObject("Ventana exterior · Casa de paja"); root.transform.SetParent(environment, false); Undo.RegisterCreatedObjectUndo(root, "Ventana real");
        var cream = AssetDatabase.LoadAssetAtPath<Material>(Art + "Marfil.mat"); var wood = AssetDatabase.LoadAssetAtPath<Material>(Art + "Madera.mat");
        // Wall spans x=-4.5..4.5, y=0..3.6. Opening: x=-3.8..-1, y=1.1..3.
        Box(root.transform, "Muro izquierdo", new Vector3(-4.15f,1.8f,2.8f), new Vector3(0.7f,3.6f,0.16f), cream);
        Box(root.transform, "Muro derecho", new Vector3(1.75f,1.8f,2.8f), new Vector3(5.5f,3.6f,0.16f), cream);
        Box(root.transform, "Muro bajo ventana", new Vector3(-2.4f,0.55f,2.8f), new Vector3(2.8f,1.1f,0.16f), cream);
        Box(root.transform, "Dintel", new Vector3(-2.4f,3.3f,2.8f), new Vector3(2.8f,0.6f,0.16f), cream);
        foreach (var x in new[] {-3.8f,-1f}) Box(root.transform,"Marco vertical",new Vector3(x,2.05f,2.7f),new Vector3(0.08f,1.98f,0.22f),wood);
        foreach (var y in new[] {1.1f,3f}) Box(root.transform,"Marco horizontal",new Vector3(-2.4f,y,2.7f),new Vector3(2.88f,0.08f,0.22f),wood);
        Box(root.transform,"Alféizar",new Vector3(-2.4f,1.08f,2.61f),new Vector3(3f,0.09f,0.42f),wood);
        var barrier = new GameObject("Límite invisible de la ventana"); barrier.transform.SetParent(root.transform); barrier.transform.position = new Vector3(-2.4f,2.05f,2.8f);
        barrier.AddComponent<BoxCollider>().size = new Vector3(2.8f,1.9f,0.12f);

        var exterior = new GameObject("EXTERIOR · Casa de paja"); exterior.transform.SetParent(environment, false); Undo.RegisterCreatedObjectUndo(exterior,"Exterior");
        var grass = Material("Exterior césped",new Color(0.25f,0.39f,0.14f));
        Box(exterior.transform,"Terreno exterior",new Vector3(-4,-0.12f,13),new Vector3(32,0.2f,22),grass);
        var source = AssetDatabase.LoadAssetAtPath<GameObject>(HousePath);
        if (source == null) throw new InvalidOperationException("No se importó casa_paja.fbx.");
        var house = UnityEngine.Object.Instantiate(source); house.name = "Casa de paja"; house.transform.SetParent(exterior.transform, true);
        // The source contains modelling helpers and a distant loose stick, not part of the house.
        foreach (var t in house.transform.Cast<Transform>().ToArray()) if (t.name != "atap_house") UnityEngine.Object.DestroyImmediate(t.gameObject);
        house.transform.rotation = Quaternion.Euler(0,180,0);
        Normalize(house,new Vector3(-5.25f,-0.015f,8.6f),5.2f);
        var material = Material("Casa de paja",new Color(0.8f,0.68f,0.44f));
        var textures = AssetDatabase.FindAssets("t:Texture2D",new[]{"Assets/Models/casaPaja"});
        if (textures.Length > 0) { material.SetTexture("_BaseMap",AssetDatabase.LoadAssetAtPath<Texture2D>(AssetDatabase.GUIDToAssetPath(textures[0]))); material.SetColor("_BaseColor",Color.white); EditorUtility.SetDirty(material); }
        foreach (var renderer in house.GetComponentsInChildren<Renderer>()) renderer.sharedMaterial = material;
        var path = Material("Exterior camino",new Color(0.48f,0.36f,0.21f));
        Box(exterior.transform,"Camino",new Vector3(-5.25f,-0.01f,5.8f),new Vector3(1.1f,0.02f,4.2f),path);
    }
    private static Bounds BoundsOf(GameObject go)
    {
        var renderers = go.GetComponentsInChildren<Renderer>();
        if (renderers.Length == 0) throw new InvalidOperationException("Modelo sin renderers: " + go.name);
        var bounds = renderers[0].bounds; foreach (var r in renderers) bounds.Encapsulate(r.bounds); return bounds;
    }
    private static void Normalize(GameObject go,Vector3 bottomCenter,float largestDimension)
    {
        var b=BoundsOf(go); go.transform.localScale *= largestDimension / Mathf.Max(b.size.x,b.size.y,b.size.z);
        b=BoundsOf(go); go.transform.position += bottomCenter-new Vector3(b.center.x,b.min.y,b.center.z);
    }
    private static GameObject Box(Transform parent,string name,Vector3 position,Vector3 scale,Material material)
    {
        var go=GameObject.CreatePrimitive(PrimitiveType.Cube); go.name=name; go.transform.SetParent(parent,false); go.transform.position=position; go.transform.localScale=scale; go.GetComponent<Renderer>().sharedMaterial=material; return go;
    }
    private static Material Material(string name,Color color)
    {
        var path=Art+name+".mat"; var material=AssetDatabase.LoadAssetAtPath<Material>(path);
        if(material==null) {material=new Material(Shader.Find("Universal Render Pipeline/Lit"));AssetDatabase.CreateAsset(material,path);}
        material.SetColor("_BaseColor",color); material.SetFloat("_Smoothness",0.15f); EditorUtility.SetDirty(material); return material;
    }
}
