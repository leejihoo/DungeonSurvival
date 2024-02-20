using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObject/CreateList/AttackList", fileName = "AttackList")]
public class AttackListSO : ScriptableObject
{
    public List<AttackSO> attackList;
}
