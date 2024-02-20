using UnityEngine;

[CreateAssetMenu(menuName = "CreateSO",fileName = "Combination")]
public class CombinationSO : ScriptableObject
{
   public ItemSO TargetItemSO;
   public ItemSO FirstIngredientSO;
   public ItemSO SecondIngredientSO;
   public ItemSO ThirdIngredientSO;

   public int FirstIngredientRequiredCount;
   public int SecondIngredientRequiredCount;
   public int ThirdIngredientRequiredCount;
   
}
