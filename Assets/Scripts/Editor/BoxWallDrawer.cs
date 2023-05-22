/*************************************************
  * 名稱：NewBehaviourScript
  * 作者：RyanHsu
  * 功能說明：
  * ***********************************************/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;


[CustomEditor(typeof(BoxWall))]
public class BoxWallDrawer : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        BoxWall m_target = (BoxWall)target;

        GUILayout.Space(20f);

        if (GUILayout.Button("CreatBox")) {
            m_target.CreatBox();
        }

        if (GUILayout.Button("ClearBox")) {
            m_target.ClearBox();
        }

    }

}
