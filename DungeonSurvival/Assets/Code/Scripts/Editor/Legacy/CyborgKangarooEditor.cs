using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(CyborgKangarooDebug))]
public class CyborgKangarooEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();
        CyborgKangarooDebug eventChannel = (CyborgKangarooDebug) target;
        if (GUILayout.Button("데미지 발생"))
        {
            CyborgKangarooCyborgKangarooController.OnDamage.Invoke(eventChannel.tempdamage);
            //eventChannel.OnDamage.Invoke(eventChannel.tempdamage);
        }
    }
}
