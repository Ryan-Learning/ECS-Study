/*************************************************
  * 名稱：DynamicRangeAttribute
  * 作者：RyanHsu
  * 功能說明：DynamicRange的Attribute標籤屬性格式
  * min & max 放入propertyName字串時，property=float或int，以property值做為設定值
  * min & max 放入propertyName字串時，property=List或Array，以陣列長度做為設定值
  * ***********************************************/
using UnityEngine;
using System.Collections;
using System;

[Serializable]
public class DynamicRangeAttribute : PropertyAttribute
{
    public float min;
    public float max;
    public string str_min = "";
    public string str_max = "";
    public string str_vector2 = "";

    /// <summary>以 min max 做為Slider上下限</summary>
    /// <param name="min">下限值的float或int</param>
    /// <param name="max">上限值的float或int</param>
    public DynamicRangeAttribute(float min, float max)
    {
        this.min = min;
        this.max = max;
    }

    /// <summary>以 min max 做為Slider上下限</summary>
    /// <param name="str_min">下限值的float或int名稱</param>
    /// <param name="max">上限值的float或int</param>
    public DynamicRangeAttribute(string str_min, float max)
    {
        this.str_min = str_min;
        this.max = max;
    }

    /// <summary>以 min max 做為Slider上下限</summary>
    /// <param name="min">下限值的float或int</param>
    /// <param name="str_max">上限值的float或int名稱</param>
    public DynamicRangeAttribute(float min, string str_max)
    {
        this.min = min;
        this.str_max = str_max;
    }

    /// <summary>以 min max 做為Slider上下限</summary>
    /// <param name="str_min">下限值的float或int名稱</param>
    /// <param name="str_max">上限值的float或int名稱</param>
    public DynamicRangeAttribute(string str_min, string str_max)
    {
        this.str_min = str_min;
        this.str_max = str_max;
    }

    /// <summary>以 Vector2變數 做為Slider上下限</summary>
    /// <param name="str_vector2">Vector2的名稱</param>
    public DynamicRangeAttribute(string str_vector2)
    {
        this.str_vector2 = str_vector2;
    }
}