using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine.UIElements;

public class EditorTest : EditorWindow
{

    [MenuItem("EditorTest/Test1/EditorTest1")]
    public static void ShowExample()
    {
        EditorTest wnd = GetWindow<EditorTest>();
        wnd.titleContent = new GUIContent("EditorTest1");
    }
    
    public void CreateGUI()
    {
        
        // Each editor window contains a root VisualElement object
        VisualElement root = rootVisualElement;

        // VisualElements objects can contain other VisualElement following a tree hierarchy
        Label label = new Label("Hello World!");
        root.Add(label);

        // Create toggle
        Toggle toggle = new Toggle();
        toggle.name = "toggle";
        toggle.label = "Toggle";
        root.Add(toggle);

        var temp = new ObjectField();
        temp.objectType = typeof(ScriptableObject);
        temp.label = "Select an object:";
        root.Add(temp);
        
       
        // Create button
        Button button = new Button();
        button.name = "button";
        button.text = "Button";
        button.clicked += () =>
        {
            if (temp.value == null)
            {
                Debug.LogWarning("Please select a ScriptableObject first.");
                return;
            }
            
            // Create a prefab from the selected ScriptableObject
            string prefabPath = "Assets/Prefabs/MyPrefab.prefab";
            prefabPath = AssetDatabase.GenerateUniqueAssetPath(prefabPath);
            GameObject prefab = new GameObject("MyPrefab");
            var scriptComponent = prefab.AddComponent<SOContainer>();
            scriptComponent.item = (ItemSO)temp.value;
            
            PrefabUtility.SaveAsPrefabAsset(prefab, prefabPath);
            DestroyImmediate(prefab);
        
            Debug.Log("Prefab created at: " + prefabPath);
        };
        
        root.Add(button);
        
        
    }
    
}
