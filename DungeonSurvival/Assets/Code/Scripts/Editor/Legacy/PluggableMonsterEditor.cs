using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(PluggableMonsterController))]
public class PluggableMonsterEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        PluggableMonsterController eventChannel = (PluggableMonsterController) target;
        if (GUILayout.Button("데미지 발생"))
        {
            //CyborgKangarooCyborgKangarooController.OnDamage.Invoke(eventChannel.tempdamage);
            eventChannel.OnDamage.Invoke(5);
        }
    }
}
