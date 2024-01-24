using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class NormalMosterModel
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

    public NormalMosterModel(MosterSO mosterSO)
    {
        if (mosterSO != null)
        {
            foreach (var property in typeof(NormalMosterModel).GetProperties())
            {
                var mosterSOProperty = typeof(MosterSO).GetProperty(property.Name);
                var value = mosterSOProperty.GetValue(mosterSO);
                property.SetValue(this,value);
            }
        }
        else
        {
            Debug.LogError("사용 불가능한 SO입니다.");
        }
    }
}
