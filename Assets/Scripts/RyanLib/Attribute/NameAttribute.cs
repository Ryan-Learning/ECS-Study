/*************************************************
  * 名稱：NameAttribute
  * 作者：RyanHsu
  * 功能說明：取string name的Attribute標籤屬性格式
  * ***********************************************/
using UnityEngine;
using System;

[AttributeUsage(AttributeTargets.Field)]
public class NameAttribute : PropertyAttribute
{

    public NameAttribute() { }
}