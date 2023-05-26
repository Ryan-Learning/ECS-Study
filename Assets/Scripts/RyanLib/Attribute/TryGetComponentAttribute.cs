/*************************************************
  * 名稱：TryGetComponentAttribute
  * 作者：RyanHsu
  * 功能說明：Inspector TryGetComponentAttribute套件
  * ***********************************************/
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;

public class TryGetComponentAttribute : PropertyAttribute
{
    public Type type = null;

    public TryGetComponentAttribute(Type type)
    {
        this.type = type;
    }
}
