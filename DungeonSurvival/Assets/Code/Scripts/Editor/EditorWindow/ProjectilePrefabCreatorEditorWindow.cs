using System.Linq;
using UnityEditor;
using UnityEditor.Animations;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

public class ProjectilePrefabCreatorEditorWindow : EditorWindow
{
    [MenuItem("PrefabCreator/Projectile/projectile")]
    public static void OpenWindow()
    {
        var window = GetWindow<ProjectilePrefabCreatorEditorWindow>();
        window.titleContent = new GUIContent("ProjectilePrefabCreator");
    }

    private void CreateGUI()
    {
        var root = rootVisualElement;
        root.style.paddingLeft = 5;
        root.style.paddingRight = 5;
        root.style.paddingTop = 10;

        Label projectileInfoLabel = new Label("투사체 정보");
        projectileInfoLabel.style.fontSize = 15;
        projectileInfoLabel.style.paddingBottom = 5;
        root.Add(projectileInfoLabel);

        Box projectileInfoBox = new Box();
        projectileInfoBox.style.paddingTop = 5;
        projectileInfoBox.style.paddingBottom = 5;
        
        ObjectField projectileFrame = new ObjectField("투사체 프레임");
        projectileFrame.objectType = typeof(GameObject);
        projectileFrame.style.paddingTop = 5;
        projectileInfoBox.Add(projectileFrame);

        ObjectField projectileSprite = new ObjectField("투사체 스프라이트");
        projectileSprite.objectType = typeof(Sprite);
        projectileSprite.style.paddingTop = 5;
        projectileInfoBox.Add(projectileSprite);

        ObjectField projectileAnimator = new ObjectField("애니메이터");
        projectileAnimator.objectType = typeof(AnimatorController);
        projectileAnimator.style.paddingTop = 5;
        projectileInfoBox.Add(projectileAnimator);

        ObjectField projectileSO = new ObjectField("투사체 SO");
        projectileSO.objectType = typeof(ProjectileSO);
        projectileSO.style.paddingTop = 5;
        projectileInfoBox.Add(projectileSO);
        
        root.Add(projectileInfoBox);

        Button createButton = new Button();
        createButton.text = "생성";
        createButton.style.marginTop = 10;
        createButton.style.marginLeft = Length.Percent(35);
        createButton.style.marginRight = Length.Percent(35);
        createButton.style.height = 30;
        createButton.clicked += () =>
        {
            ClickCreationButton(projectileInfoBox);
        };
        root.Add(createButton);
    }

    public void ClickCreationButton(Box projectileInfoBox)
    {
        var nullReferenceField = projectileInfoBox.Children().Where(child => ((ObjectField)child).value == null).ToList();
        if (nullReferenceField.Count > 0)
        {
            foreach (var field in nullReferenceField)
            {
                Debug.LogWarning(((ObjectField)field).label + "에 대한 정보가 없습니다.");
            }
            ShowNotification(new GUIContent("생성에 실패하였습니다. 콘솔 창을 확인해주세요."));
            return;
        }

        var projectileFrame = Instantiate((GameObject)((ObjectField)projectileInfoBox[0]).value);
        var projectileSprite = (Sprite)((ObjectField)projectileInfoBox[1]).value;
        var projectileAnimator = (AnimatorController)((ObjectField)projectileInfoBox[2]).value;
        var projectileSO = (ProjectileSO)((ObjectField)projectileInfoBox[3]).value;
        projectileFrame.GetComponent<SpriteRenderer>().sprite = projectileSprite;
        projectileFrame.GetComponent<Animator>().runtimeAnimatorController = projectileAnimator;
        projectileFrame.GetComponent<ProjectileController>().projectileSO = projectileSO;
        projectileFrame.AddComponent<PolygonCollider2D>().isTrigger = true;

        var prefabPath = AssetDatabase.GenerateUniqueAssetPath($"Assets/Prefabs/Projectile/{projectileSO.name}.prefab");
        PrefabUtility.SaveAsPrefabAsset(projectileFrame, prefabPath);
        ShowNotification(new GUIContent("성공적으로 생성했습니다."));
    }
}
