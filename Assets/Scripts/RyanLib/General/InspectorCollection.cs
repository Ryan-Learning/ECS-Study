/*************************************************
  * 名稱：InspectorCollection
  * 作者：RyanHsu
  * 功能說明：Inspector Collection 群組整理工具
  * ***********************************************/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;


//執行階段執行     [ExecuteAlways]
//依賴項           [RequireComponent(typeof CLASS)]
[DisallowMultipleComponent]
public class InspectorCollection : MonoBehaviour
{
    public string CollectionName;
    int m_LineCount = 25;
    string m_LineStr = "=";


#if UNITY_EDITOR
    void OnValidate()
    {
        int count = m_LineCount - Mathf.FloorToInt(CollectionName.Length * 0.5f);
        string draw = drawTxt(m_LineStr, count);
        name = String.Format("{0}({1}){0}", draw, CollectionName);
    }

    string drawTxt(string str, int count)
    {
        string s = "";
        for (int i = 0; i < count; i++) { s += str; }
        return s;
    }
    //void OnDrawGizmos() { }
    //private void OnDrawGizmosSelected() { }
#endif
}
