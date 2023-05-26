/*************************************************
  * 名稱：JsonAttribute
  * 作者：RyanHsu
  * 功能說明：SceneDrawer的Attribute標籤屬性格式
  * ***********************************************/
using UnityEngine;
using System;

/// <summary>將Asset檔案路徑填入string中</summary>
[AttributeUsage(AttributeTargets.Field)]
public class JsonAttribute : PropertyAttribute
{
    public JsonAttribute() { }
}