/*************************************************
  * 名稱：ListToPopupAttribute
  * 作者：RyanHsu
  * 功能說明：以List做為類enum的popup顯示在inspector中
  * ***********************************************/
using UnityEngine;
using System.Collections;
using System;

/// <summary> 
/// 以List做為類enum的popup顯示在inspector中 。
/// Property必需為float、int或string
/// listPropertyName = 同Class中，Public List<string> 的PropertyName
/// </summary>
[Serializable]
public class ListToPopupAttribute : PropertyAttribute
{
    public string m_ListName;
    public int m_SelectedIndex = 0;

    public ListToPopupAttribute(string listPropertyName)
    {
        m_ListName = listPropertyName;
    }
}