using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEditor.Animations;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

public class MonsterPrefabCreatorEditorWindow : EditorWindow
{
    [MenuItem("PrefabCreator/Monster/monster")]
    public static void OpenWindow()
    {
        var window = GetWindow<MonsterPrefabCreatorEditorWindow>();
        window.titleContent = new GUIContent("MonsterPrefabCreator");
    }
    
    private void CreateGUI()
    {
        VisualElement root = rootVisualElement;
        root.style.paddingLeft = new StyleLength(5);
        root.style.paddingRight = new StyleLength(5);
        
        Box monsterInfoBox = new Box();
        ObjectField monsterPrefabFrame = new ObjectField("몬스터 프리팹");
        Button createButton = new Button();
        
        ObjectField projectile = new ObjectField("투사체");
        projectile.objectType = typeof(GameObject);
        projectile.style.paddingTop = new StyleLength(5);

        ObjectField attackAnimationClip = new ObjectField("공격 애니메이션 클립");
        attackAnimationClip.objectType = typeof(AnimationClip);
        attackAnimationClip.style.paddingTop = 5;
        
        List<string> option = new List<string> { "타입을 선택해주세요.","돌진 타입", "추격 타입", "투척 타입" };
        DropdownField monsterTypeDropdownField = new DropdownField("마물 타입", option,0);
        monsterTypeDropdownField.style.paddingTop = new StyleLength(10);
        monsterTypeDropdownField.RegisterValueChangedCallback(value => SelectDropdownMenu(value,monsterInfoBox,projectile, monsterPrefabFrame, createButton, attackAnimationClip));
        root.Add(monsterTypeDropdownField);

        Label monsterInfoBoxLabel = new Label("몬스터 정보 설정");
        monsterInfoBoxLabel.style.fontSize = 15;
        monsterInfoBoxLabel.style.paddingTop = 10;
        monsterInfoBoxLabel.style.paddingBottom = 5;
        root.Add(monsterInfoBoxLabel);

        monsterInfoBox.style.paddingBottom = new StyleLength(10);
        monsterInfoBox.SetEnabled(false);
        createButton.SetEnabled(false);
        
        monsterPrefabFrame.objectType = typeof(GameObject);
        monsterPrefabFrame.style.paddingTop = new StyleLength(5);
        monsterInfoBox.Add(monsterPrefabFrame);
        
        ObjectField monsterSO = new ObjectField("몬스터 SO");
        monsterSO.objectType = typeof(MonsterSO);
        monsterSO.style.paddingTop = new StyleLength(5);
        monsterInfoBox.Add(monsterSO);

        ObjectField monsterSprite = new ObjectField("몬스터 초기 이미지");
        monsterSprite.tooltip = "에디터 상에서 표시될 몬스터의 이미지를 선택해주세요.";
        monsterSprite.objectType = typeof(Sprite);
        monsterSprite.style.paddingTop = new StyleLength(5);
        monsterInfoBox.Add(monsterSprite);
        
        ObjectField monsterAnimator = new ObjectField("몬스터 애니메이터");
        monsterAnimator.objectType = typeof(AnimatorController);
        monsterAnimator.style.paddingTop = new StyleLength(5);
        monsterInfoBox.Add(monsterAnimator);
        
        root.Add(monsterInfoBox);
        
        createButton.text = "생성";
        createButton.style.marginTop = 10;
        createButton.style.height = 30;
        createButton.style.unityTextAlign = new StyleEnum<TextAnchor>(TextAnchor.MiddleCenter);
        createButton.style.fontSize = 15;
        createButton.style.marginLeft = Length.Percent(35);
        createButton.style.marginRight = Length.Percent(35);
        createButton.clicked += () => { ClickCreationButton(monsterInfoBox); };
        root.Add(createButton);
    }

    public void ClickCreationButton(Box monsterInfoBox)
    {
        var nullReferenceField = monsterInfoBox.Children().Where(child => ((ObjectField)child).value == null).ToList();
        if (nullReferenceField.Count > 0)
        {
            foreach (var field in nullReferenceField)
            {
                Debug.LogWarning(((ObjectField)field).label + "에 대한 정보를 넣어주세요.");
            }
            ShowNotification(new GUIContent("누락된 정보가 존재하여 생성에 실패하였습니다. 디버그 창을 확인해주세요."));
            return;
        }

        var prefab = (GameObject)((ObjectField)monsterInfoBox[0]).value;
        prefab.GetComponent<NormalMonsterController>().monsterSO = (MonsterSO)((ObjectField)monsterInfoBox[1]).value;
        prefab.GetComponentInChildren<SpriteRenderer>().sprite = (Sprite)((ObjectField)monsterInfoBox[2]).value;
        prefab.GetComponentInChildren<Animator>().runtimeAnimatorController = (AnimatorController)((ObjectField)monsterInfoBox[3]).value;
        prefab.transform.GetChild(0).gameObject.AddComponent<PolygonCollider2D>();
        
        // 투척형일 경우
        if (monsterInfoBox.childCount == 6)
        {
            prefab.GetComponent<ThrowTypeMonsterController>().projectile = (GameObject)((ObjectField)monsterInfoBox[4]).value;
            prefab.GetComponentInChildren<ThrowTypeMonsterVisual>().attackAnimationClip = (AnimationClip)((ObjectField)monsterInfoBox[5]).value;
        }
        
        var monsterPrefabCreationPath = AssetDatabase.GenerateUniqueAssetPath($"Assets/Prefabs/Monster/{prefab.GetComponent<NormalMonsterController>().monsterSO.name}.prefab");
        PrefabUtility.SaveAsPrefabAsset(prefab, monsterPrefabCreationPath);
        ShowNotification(new GUIContent("생성에 성공했습니다."));
    }

    public void SelectDropdownMenu(ChangeEvent<string> value, Box box, ObjectField projectile, ObjectField prefabField, Button createButton, ObjectField attackAnimationClip)
    {
        string selectedOption = value.newValue;
        bool isProjectileExist = box.Contains(projectile);
        bool isAttackAnimationClipExist = box.Contains(attackAnimationClip);
        
        GameObject prefab;
        
        Action removeProjectile = () =>
        {
            if (isProjectileExist)
            {
                box.Remove(projectile);
            }
        };
        
        Action removeAttackAnimationClip = () =>
        {
            if (isAttackAnimationClipExist)
            {
                box.Remove(attackAnimationClip);
            }
        };
        
        switch (selectedOption)
        {
            case "타입을 선택해주세요.":
                removeProjectile();
                removeAttackAnimationClip();
                prefabField.value = null;
                box.SetEnabled(false);
                createButton.SetEnabled(false);
                break;
            case "돌진 타입":
                prefab = PrefabUtility.LoadPrefabContents("Assets/Prefabs/Monster/Frame/RushTypeMonsterPrefabFrame.prefab");
                prefabField.value = prefab;
                removeProjectile();
                removeAttackAnimationClip();
                box.SetEnabled(true);
                createButton.SetEnabled(true);
                break;
            case "추격 타입":
                prefab = PrefabUtility.LoadPrefabContents("Assets/Prefabs/Monster/Frame/ChaseTypeMonsterPrefabFrame.prefab");
                prefabField.value = prefab;
                removeProjectile();
                removeAttackAnimationClip();
                box.SetEnabled(true);
                createButton.SetEnabled(true);
                break;
            case "투척 타입":
                prefab = PrefabUtility.LoadPrefabContents("Assets/Prefabs/Monster/Frame/ThrowTypeMonsterPrefabFrame.prefab");
                prefabField.value = prefab;
                
                if (!isProjectileExist)
                {
                    box.Add(projectile);
                }

                if (!isAttackAnimationClipExist)
                {
                    box.Add(attackAnimationClip);
                }
                
                box.SetEnabled(true);
                createButton.SetEnabled(true);
                break;
        }
    }
}
