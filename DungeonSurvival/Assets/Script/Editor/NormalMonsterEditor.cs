using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(NormalMonsterDebug))]
public class NormalMonsterEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        NormalMonsterDebug eventChannel = (NormalMonsterDebug) target;
        if (GUILayout.Button("데미지 발생"))
        {
            eventChannel.controller.OnDamage.Invoke(eventChannel.tempDamage);
        }
    }
}
