/*************************************************
  * 名稱：PrefabPoolEditor
  * 作者：RyanHsu
  * 功能說明：
  * ***********************************************/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(PrefabPool))]
public class PrefabPoolEditor : Editor
{
    PrefabPool m_target;
    void OnEnable()
    {
        m_target = (PrefabPool)target;
    }

    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();
        //
        EditorGUILayout.Space(20f);
        //
        EditorGUILayout.BeginHorizontal();
        {
            if (GUILayout.Button("Creat Item Prefab")) {
                m_target.CreatItemPrefab();
            }

            if (GUILayout.Button("Creat Item Clone")) {
                m_target.CreatItem();
            }
        }
        EditorGUILayout.EndHorizontal();

        if (GUILayout.Button("Destory Item")) {
            if (m_target.transform.childCount > 0)
                m_target.DestoryItem(m_target.transform.GetChild(m_target.transform.childCount - 1).gameObject);
        }

        EditorGUILayout.BeginHorizontal();
        {
            if (GUILayout.Button("Clear Items")) {
                if (m_target.transform.childCount > 0)
                    m_target.ClearAll();
            }

            if (GUILayout.Button("Clear Pool")) {
                if (m_target.m_PoolCount > 0)
                    m_target.ClearPool();
            }
        }
        EditorGUILayout.EndHorizontal();

        if (GUILayout.Button("ClearAll")) {
            m_target.ClearAll();
            m_target.ClearPool();
        }
    }
}
