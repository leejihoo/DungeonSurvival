using System;
using UnityEngine;

[Serializable]
public class NormalMosterModel
{
    #region Inspector Fields
    
    [field: Header("기본 정보")]
    [field: SerializeField] public string MosterName { get; set; }
    [field: SerializeField] public string Id { get; set; }
    [field: Header("스텟")]
    [field: Range(0,30)]
    [field: SerializeField] public int Hp { get; set; }
    [field: Range(0,10)]
    [field: SerializeField] public int Atk { get; set; }
    [field: Range(0,10)]
    [field: SerializeField] public int Def { get; set; }
    [field: SerializeField] public int AttackRange { get; set; }
    [field: Header("속도")]
    [field: Range(0,10)]
    [field: SerializeField] public int MoveSpeed { get; set; }
    [field: Range(0,10)]
    [field: SerializeField] public int ProjectileSpeed { get; set; }
    [field: Header("정찰 정보")]
    [field: Range(0,10)]
    [field: SerializeField] public int PatrolRange { get; set; }
    [field: Range(0,90)]
    [field: SerializeField] public int PatrolFov { get; set; }
    [field: Range(0,10)]
    [field: SerializeField] public int ViewDistance { get; set; }
    [field: Range(0,10)]
    [field: SerializeField] public int ChaseTime { get; set; }
    [field: Header("드랍 아이템 정보")]

    [field: SerializeField] public ScriptableObject DropItem { get; set; }
    
    #endregion

    #region Constructor
    
    public NormalMosterModel(MonsterSO monsterSo)
    {
        if (monsterSo != null)
        {
            foreach (var property in typeof(NormalMosterModel).GetProperties())
            {
                var mosterSOProperty = typeof(MonsterSO).GetProperty(property.Name);
                var value = mosterSOProperty.GetValue(monsterSo);
                property.SetValue(this,value);
            }
        }
        else
        {
            Debug.LogError("사용 불가능한 SO입니다.");
        }
    }
    
    #endregion
}
