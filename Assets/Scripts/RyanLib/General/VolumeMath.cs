/*************************************************
  * 名稱：運算函數庫集合
  * 作者：RyanHsu
  * 功能說明：關於體積方面的計算公式
  * ***********************************************/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class VolumeMath
{
    /// <summary>同心圓等效直徑計算</summary>
    /// <param name="outer_d">外圓直徑</param>
    /// <param name="inner_d">內圓直徑</param>
    /// <returns>等效直徑</returns>
    public static float Circle_Equivalent_Diameter(float outer_d, float inner_d) => Mathf.Sqrt((outer_d * outer_d - inner_d * inner_d));

    /// <summary>中空圓台等效直徑計算，體積換算</summary>
    /// <param name="D1">圓台底直徑</param>
    /// <param name="D2">圓台頂直徑</param>
    /// <param name="h">圓台高</param>
    /// <param name="d">內圓直徑</param>
    /// <return>等效直徑</return>
    public static float Frustum_Equivalent_Diameter(float D1, float D2, float h, float d)
    {
        //計算圓台截面積A
        float A0 = Mathf.PI * D1 * D1 / 4;
        float A1 = Mathf.PI * D2 * D2 / 4;
        float A2 = Mathf.Sqrt((D1 - D2) * (D1 - D2) / 4 + h * h) * (D1 + D2) / 2;
        float A = A0 + A1 + A2;
        //計算內圓面積
        float a = Mathf.PI * d * d / 4;
        //計算有效面積
        float Aeff = A - a;
        //計算等效直徑
        float deff = Mathf.Sqrt(4 * Aeff / Mathf.PI);

        return deff;
    }
}
