using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObject/CreateProjectile/ProjectileSO", fileName = "ProjectileSO")]
public class ProjectileSO : ScriptableObject
{
    [field: SerializeField] public int Atk { get; private set; }
    [field: SerializeField] public float Speed { get; private set; }
    [field: SerializeField] public AudioClip AttackSound { get; private set; }
}
