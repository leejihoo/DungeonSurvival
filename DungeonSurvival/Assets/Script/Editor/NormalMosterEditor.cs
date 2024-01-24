using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(NormalMonsterDebug))]
public class NormalMosterEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();
        NormalMonsterDebug eventChannel = (NormalMonsterDebug) target;
        if (GUILayout.Button("데미지 발생"))
        {
            NormalMosterController.OnDamage.Invoke(eventChannel.tempdamage);
            //eventChannel.OnDamage.Invoke(eventChannel.tempdamage);
        }
    }
}
