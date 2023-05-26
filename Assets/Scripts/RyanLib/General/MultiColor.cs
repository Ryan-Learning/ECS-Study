/*************************************************
  * 名稱：MuiltColor
  * 作者：RyanHsu
  * 功能說明：
  * ***********************************************/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using ExtensionMethods;

//禁止多載         [DisallowMultipleComponent]
//#if UNITY_EDITOR
//[ExecuteAlways]
//#endif
//依賴項           [RequireComponent(typeof CLASS)]
public class MultiColor : MonoBehaviour
{
    public Graphic[] targetGraphic;
    public Color[] colors = new Color[] { Color.white };
    [DynamicRange("colors")] public int m_colorIndex;
    DirtyParams<int> dirty = new DirtyParams<int>();

    void Awake()
    {

    }

    void OnDirty(int[] a, int[] b)
    {

    }

    void Update()
    {
        if (dirty.isDirty(m_colorIndex)) SetColor(m_colorIndex);
    }

    public void SetColor(int index) => targetGraphic.ForEach(m => m.color = colors.CorrectValue(index));

#if UNITY_EDITOR
    void OnValidate()
    {
        if (dirty.isDirty(m_colorIndex)) SetColor(m_colorIndex);
    }
    //void OnDrawGizmos() { }
    //private void OnDrawGizmosSelected() { }
#endif

}
