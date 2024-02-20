using UnityEngine;

[CreateAssetMenu(menuName = "CreateItemData", fileName = "Item")]
public class ItemSO : ScriptableObject
{
    public string Id;
    public string DisplayName;
    public Sprite ItemSprite;
    public int MaxStackSize = 99;
    public bool Consumable;
    public GameObject VisualWorldPrefab;
    public GameObject UIPrefab;
    public AudioClip[] UseSound;
    public string Description;
    public string Usage;
    public int RecoveryHP;
    public int RecoveryHunger;
}

