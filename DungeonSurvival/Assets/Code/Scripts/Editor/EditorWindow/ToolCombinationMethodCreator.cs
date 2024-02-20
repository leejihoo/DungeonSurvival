using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

public class ToolCombinationMethodCreator : EditorWindow
{
    [MenuItem("PrefabCreator/CombinationMethod/Tool")]
    public static void OpenToolCombinationMethodCreatorWindow()
    {
        var toolCombinationMethodCreator = GetWindow<ToolCombinationMethodCreator>();
        toolCombinationMethodCreator.titleContent = new GUIContent("ToolCombinationMethodCreator");
    }

    private void CreateGUI()
    {
        var root = rootVisualElement;
        ObjectField combinationSO = new ObjectField("CombinationSO");
        combinationSO.objectType = typeof(CombinationSO);
        root.Add(combinationSO);

        Button button = new Button();
        button.text = "Create";
        button.clicked += () =>
        {
            if (combinationSO.value == null)
            {
                Debug.LogWarning("조합식 정보를 등록해주세요.");
                return;
            }
            
            GameObject frame =
                PrefabUtility.LoadPrefabContents("Assets/Prefabs/PrefabsUI/Combination/ToolCombinationMethodFrameUI.prefab");
            
            var toolCombinationMethodController = frame.GetComponent<ToolCombinationMethodController>();
            toolCombinationMethodController.CombinationSO = (CombinationSO)combinationSO.value;
            
            var ingredientContainer = frame.GetComponent<IngredientContainer>();

            if (toolCombinationMethodController.CombinationSO.TargetItemSO == null)
            {
                Debug.LogWarning("조합식에 조합 아이템 정보를 등록해주세요.");
                return;
            }
            ingredientContainer.TargetItem.sprite =
                toolCombinationMethodController.CombinationSO.TargetItemSO.ItemSprite;
            ingredientContainer.TargetItemName.text =
                toolCombinationMethodController.CombinationSO.TargetItemSO.DisplayName;
            
            if (toolCombinationMethodController.CombinationSO.FirstIngredientSO == null)
            {
                Debug.LogWarning("조합식에 첫 번째 재료 아이템 정보를 등록해주세요.");
                return;
            }
            ingredientContainer.FirstIngredient.sprite =
                toolCombinationMethodController.CombinationSO.FirstIngredientSO.ItemSprite;
            
            if (toolCombinationMethodController.CombinationSO.SecondIngredientSO == null)
            {
                if (((CombinationSO)combinationSO.value).SecondIngredientRequiredCount != 0)
                {
                    Debug.LogWarning("조합식에 두 번째 재료 아이템 정보를 등록해주세요.");
                    return;
                }
                
                toolCombinationMethodController.SecondIngredientCount.gameObject.SetActive(false);
                ingredientContainer.SecondIngredient.gameObject.SetActive(false);
            }
            else
            {
                ingredientContainer.SecondIngredient.sprite =
                    toolCombinationMethodController.CombinationSO.SecondIngredientSO.ItemSprite;
            }
            
            if (toolCombinationMethodController.CombinationSO.ThirdIngredientSO == null)
            {
                if (((CombinationSO)combinationSO.value).ThirdIngredientRequiredCount != 0)
                {
                    Debug.LogWarning("조합식에 세 번째 재료 아이템 정보를 등록해주세요.");
                    return;
                }
                
                toolCombinationMethodController.ThirdIngredientCount.gameObject.SetActive(false);
                ingredientContainer.ThirdIngredient.gameObject.SetActive(false);
            }
            else
            {
                ingredientContainer.ThirdIngredient.sprite =
                    toolCombinationMethodController.CombinationSO.ThirdIngredientSO.ItemSprite;
            }
            
            
            DestroyImmediate(ingredientContainer);
            PrefabUtility.SaveAsPrefabAsset(frame,AssetDatabase.GenerateUniqueAssetPath($"Assets/Prefabs/PrefabsUI/Combination/{toolCombinationMethodController.CombinationSO.TargetItemSO.Id}CombinationMethodUI.prefab"));
            
            Debug.Log("프리팹이 완성되었습니다. ");
        };

        root.Add(button);
    }
}
