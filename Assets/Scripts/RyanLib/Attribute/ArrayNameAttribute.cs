/*************************************************
  * 名稱：ArrayNameAttribute
  * 作者：RyanHsu
  * 功能說明：輔助Array或List於Inspector內自訂List顯示名稱
  * ***********************************************/
using UnityEngine;
using System;

public class ArrayNameAttribute : PropertyAttribute
{
    public readonly string[] names;
    public ArrayNameAttribute(params string[] names)
    {
        this.names = names;
    }
}