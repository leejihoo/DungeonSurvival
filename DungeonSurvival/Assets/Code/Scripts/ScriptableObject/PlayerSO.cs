using UnityEngine;

[CreateAssetMenu( menuName = "ScriptableObject/CreatePlayerSO", fileName = "PlayerSO")]
public class PlayerSO : ScriptableObject
{
    [field: SerializeField] public float Speed { get; set; }
    [field: SerializeField] public float AtkSpeed { get; set; }
    [field: SerializeField] public int Atk { get; set; }
    [field: SerializeField] public int Def { get; set; }
    [field: SerializeField] public float AtkRange { get; set; }
    [field: SerializeField] public float ProjectileSpeed { get; set; }
    [field: SerializeField] public float PrecisionShootingTime { get; set; }
    [field: SerializeField] public float StoneCollectingSpeed { get; set; }
    [field: SerializeField] public float BranchCollectingSpeed { get; set; }
    [field: SerializeField] public int Hp { get; set; }
    [field: SerializeField] public int Fullness { get; set; }
}
