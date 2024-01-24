using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CreateAssetMenu( menuName = "ScriptableObject/CreateMosterSO", fileName = "MosterSO")]
public class MosterSO : ScriptableObject
{
    [field: SerializeField] public string MosterName { get; set; }
    [field: SerializeField] public string Id { get; set; }
    [field: SerializeField] public int Hp { get; set; }
    [field: SerializeField] public int MoveSpeed { get; set; }
    [field: SerializeField] public int ProjectileSpeed { get; set; }
    [field: SerializeField] public int Atk { get; set; }
    [field: SerializeField] public int Def { get; set; }
    [field: SerializeField] public ScriptableObject DropItem { get; set; }
    [field: SerializeField] public int PatrolRange { get; set; }
    [field: SerializeField] public int PatrolFov { get; set; }
    [field: SerializeField] public int ChaseTime { get; set; }
    [field: SerializeField] public int ViewDistance { get; set; }
    [field: SerializeField] public AudioClip DeadSound { get; set; }
    [field: SerializeField] public AudioClip AttackSound { get; set; }
}
