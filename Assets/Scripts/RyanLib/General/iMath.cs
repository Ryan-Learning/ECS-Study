/*************************************************
  * 名稱：運算函數庫集合
  * 作者：RyanHsu
  * 功能說明：將向量運算或三角函數、數學公式集合在此做簡易的調用
  * ***********************************************/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class iMath
{
    /// <summary>
    /// 計算A-B與C-D是否相交
    /// </summary>
    /// <param name="a"></param><param name="b"></param>
    /// <param name="c"></param><param name="d"></param>
    /// <returns></returns>
    static public bool hasIntersect(Vector2 a, Vector2 b, Vector2 c, Vector2 d)
    {
        //Step1,過濾絕無相交可能
        if (Mathf.Max(a.x, b.x) < Mathf.Min(c.x, d.x) || Mathf.Max(a.y, b.y) < Mathf.Min(c.y, d.y) || Mathf.Max(c.x, d.x) < Mathf.Min(a.x, b.x) || Mathf.Max(c.y, d.y) < Mathf.Min(a.y, b.y))
            return false;
        //Step2,利用向量積計算C,D兩點是否在L1兩側
        float c1 = crossProduct(a, b, c);
        float c2 = crossProduct(a, b, d);
        float c3 = crossProduct(c, d, a);
        float c4 = crossProduct(c, d, b);
        // 向量積相乘 < 0,代表線的端點在另一條線的兩端
        if (c1 * c2 < 0 || c3 * c4 < 0) {
            return true;
        }

        //Step3,若在同一條線，則判斷點是否在線上

        if (c1 == 0) {
            return pointIntersect(c, a, b);
        }
        if (c2 == 0) {
            return pointIntersect(d, a, b);
        }
        if (c3 == 0) {
            return pointIntersect(a, c, d);
        }
        if (c4 == 0) {
            return pointIntersect(b, c, d);
        }
        //Step4,其餘狀況皆為不相交
        return false;
    }

    /// <summary>計算A-B與A-C的角度</summary>
    static public float crossProduct(Vector2 a, Vector2 b, Vector2 c)
    {
        return (b.x - a.x) * (c.y - a.y) - (c.x - a.x) * (b.y - a.y);
    }

    /// <summary>計算P點是否在A-B線段上</summary>
    static public bool pointIntersect(Vector2 p, Vector2 a, Vector2 b)
    {
        return (p.x >= Mathf.Min(a.x, b.x)) && (p.x <= Mathf.Max(a.y, b.y)) && (p.y >= Mathf.Min(a.x, b.x)) && (p.y <= Mathf.Max(a.y, b.y));
    }

    /// <summary>計算A-B線與C-D線的交點座標</summary>
    static public Vector2 IntersectSlope(Vector2 a, Vector2 b, Vector2 c, Vector2 d)
    {
        return Intersect(a, Slope(a, b), c, Slope(c, d));
    }

    /// <summary>計算兩點的斜率</summary>
    static public float Slope(Vector2 p1, Vector2 p2)
    {
        Vector2 vp = Vector2.zero;
        float K = (p1.y - p2.y) / (p1.x - p2.x);
        return K;
    }

    /// <summary>計算兩點的旋轉角</summary>
    static public float Angel(Vector2 p1, Vector2 p2)
    {
        return Mathf.Atan2(p1.y - p2.y, p1.x - p2.x) / Mathf.PI;
    }

    /// <summary>計算P1、P2斜率上的交點座標</summary>
    static public Vector2 Intersect(Vector2 p1, float slope1, Vector2 p2, float slope2)
    {
        Vector2 vp = Vector2.zero;
        float K = slope1;
        float M = slope2;
        float B = p1.y - K * p1.x;
        float D = p2.y - M * p2.x;
        vp.x = (D - B) / (K - M);
        vp.y = K * (D - B) / (K - M) + B;
        return vp;
    }

    /// <summary>計算Point以Pivot圓心旋轉Angles角度後的座標</summary>
    /// <param name="point">target</param>
    /// <param pivot="pivot">圓心</param>
    /// <param angles="angles">轉角</param>
    static public Vector3 RotatePointAroundPivot(Vector3 point, Vector3 pivot, Vector3 angles)
    {
        Vector3 dir = point - pivot; // get point direction relative to pivot
        dir = Quaternion.Euler(angles) * dir; // rotate it
        point = dir + pivot; // calculate rotated point
        return point; // return it
    }

    /// <summary>將Vector做四捨五入運算</summary>
    /// <param name="d">小數點精度</param>
    static public Vector3 Round(Vector3 v3, int d = 0)
    {
        float pow = Mathf.Pow(10, d);
        v3 = v3 * pow;
        return new Vector3(Mathf.Round(v3.x), Mathf.Round(v3.y), Mathf.Round(v3.z)) / pow;
    }

    /// <summary>計算Vector3是否為XYZ某方向的純量</summary>
    /// <param name="inner">輸入向量</param>
    /// <param name="azimuth">單一方向向量</param>
    static public bool NormalizeAzimuth(Vector3 inner, Vector3 azimuth)
    {
        return inner.normalized == azimuth;
    }
}
