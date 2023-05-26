/*************************************************
  * 名稱：ReadOnlyAttribute
  * 作者：RyanHsu
  * 功能說明：Inspector ReadOnly套件
  * ***********************************************/
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class ReadOnlyAttribute : PropertyAttribute
{
    public string str_value = "";
    public List<int> enum_index = new List<int> { 0 };

    /// <summary>設定Inspector上的property唯讀</summary>
    public ReadOnlyAttribute() { }

    /// <summary>設定Inspector上的property唯讀，依照bool或Enum變數切換</summary>
    /// <param name="str_value">以 Enum 或 bool 變數 str_value 決定 property 是否唯讀，加入"!"可反轉</param>
    /// <param name="enum_index"> Enum 的 SelectedIndex </param>
    public ReadOnlyAttribute(string str_value, int enum_index)
    {
        this.str_value = str_value;
        this.enum_index = new List<int> { enum_index };
    }

    /// <summary>設定Inspector上的property唯讀，依照bool或Enum變數切換</summary>
    /// <param name="str_value">以 Enum 或 bool 變數 str_value 決定 property 是否唯讀，加入"!"可反轉</param>
    /// <param name="enum_index"> Enum 的 SelectedIndex </param>
    public ReadOnlyAttribute(string str_value, params int[] enum_index)
    {
        this.str_value = str_value;
        this.enum_index = enum_index.ToList();
    }
}
