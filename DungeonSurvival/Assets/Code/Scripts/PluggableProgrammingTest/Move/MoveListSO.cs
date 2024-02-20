using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObject/CreateList/MoveList", fileName = "MoveList")]
public class MoveListSO : ScriptableObject
{
    public List<MoveSO> moveList;
}
