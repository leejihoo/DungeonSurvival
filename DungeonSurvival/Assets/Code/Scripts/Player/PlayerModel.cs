using System;
using UnityEngine;

[Serializable]
public class PlayerModel
{
    #region Inspector Fields
    
    [field: SerializeField] public float Speed { get; set; }
    [field: SerializeField] public float AtkSpeed { get; set; }
    [field: SerializeField] public int Atk { get; set; }
    [field: SerializeField] public int Def { get; set; }
    [field: SerializeField] public float AtkRange { get; set; }
    [field: SerializeField] public float ProjectileSpeed { get; set; }
    [field: SerializeField] public float PrecisionShootingTime { get; set; }
    [field: SerializeField] public float StoneCollectingSpeed { get; set; }
    [field: SerializeField] public float BranchCollectionSpeed { get; set; }
    [field: SerializeField] public int Hp { get; set; }
    [field: SerializeField] public int Fullness { get; set; }
    
    #endregion

    #region Constructor
    
    public PlayerModel(PlayerSO playerSO)
    {
        if (playerSO != null)
        {
            // PlayerSO와 PlayerModel의 속성 이름이 일치하는 경우 반복문을 사용하여 자동으로 할당
            foreach (var property in typeof(PlayerModel).GetProperties())
            {
                var playerSOProperty = typeof(PlayerSO).GetProperty(property.Name);
                if (playerSOProperty != null)
                {
                    var value = playerSOProperty.GetValue(playerSO);
                    property.SetValue(this, value);
                }
            }
        }
        else
        {
            Debug.LogError("Invalid PlayerSO provided to PlayerModel constructor.");
        }
    }
    
    #endregion
}
