using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine.UIElements;

public class ItemPrefabCreatorEditorWindow : EditorWindow
{
    [MenuItem("PrefabCreator/Item/item")]
    public static void OpenCreatorWindow()
    {
        ItemPrefabCreatorEditorWindow itemPrefabCreatorEditorWindow = GetWindow<ItemPrefabCreatorEditorWindow>();
        itemPrefabCreatorEditorWindow.titleContent = new GUIContent("ItemPrefabCreator");
    }
    
    private void CreateGUI()
    {
        VisualElement root = rootVisualElement;
        ObjectField itemSO = new ObjectField("ItemSO");
        itemSO.objectType = typeof(ItemSO);
        root.Add(itemSO);
        
        HelpBox helpBox = new HelpBox("아이템 이름과 이미지를 넣지 않으면 itemSO에 등록된 정보를 입력합니다. ", HelpBoxMessageType.Warning);
        root.Add(helpBox);
        
        TextField text = new TextField("Item Name");
        root.Add(text);

        ObjectField sprite = new ObjectField("Item Sprite");
        sprite.objectType = typeof(Sprite);
        root.Add(sprite);

        Button creationButton = new Button();
        creationButton.text = "Create";
        creationButton.clicked += () =>
        {
            if (itemSO.value == null)
            {
                Debug.LogWarning("아이템 정보를 넣어주세요.");
                return;        
            }
            
            if (String.IsNullOrEmpty(text.value))
            {
                text.value = ((ItemSO)itemSO.value)?.Id;
                if (String.IsNullOrEmpty(text.value))
                {
                    Debug.LogWarning("아이템 이름을 작성해주세요.");
                    return;
                }
            }

            if (sprite.value == null)
            {
                sprite.value = ((ItemSO)itemSO.value)?.ItemSprite;
                if (sprite.value == null)
                {
                    Debug.LogWarning("아이템 이미지를 넣어주세요.");
                    return;       
                }
            }
            
            GameObject frameUI = PrefabUtility.LoadPrefabContents("Assets/Prefabs/PrefabsUI/Item/FrameUI.prefab");
            GameObject frameWorld = PrefabUtility.LoadPrefabContents("Assets/Prefabs/PrefabsWorld/FrameWorld.prefab");

            frameUI.GetComponent<UnityEngine.UI.Image>().sprite = (Sprite)sprite.value;
            frameWorld.GetComponent<SpriteRenderer>().sprite = (Sprite)sprite.value;
            frameWorld.GetComponent<SOContainer>().item = (ItemSO)itemSO.value;

            var frameUIPath = AssetDatabase.GenerateUniqueAssetPath($"Assets/Prefabs/PrefabsUI/{text.value}UI.prefab");
            PrefabUtility.SaveAsPrefabAsset(frameUI, frameUIPath);
            ((ItemSO)itemSO.value).UIPrefab = AssetDatabase.LoadAssetAtPath(frameUIPath,typeof(GameObject)) as GameObject;
            
            var frameWorldPath =
                AssetDatabase.GenerateUniqueAssetPath($"Assets/Prefabs/PrefabsWorld/{text.value}World.prefab");
            PrefabUtility.SaveAsPrefabAsset(frameWorld, frameWorldPath);
            ((ItemSO)itemSO.value).VisualWorldPrefab = AssetDatabase.LoadAssetAtPath(frameWorldPath,typeof(GameObject)) as GameObject;
            // var temp = new SerializedObject((ItemSO)itemSO.value);
            // temp.ApplyModifiedProperties();
        };
        
        root.Add(creationButton);
    }
    
}
