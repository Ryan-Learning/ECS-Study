/*************************************************
  * 名稱：Header
  * 作者：RyanHsu
  * 功能說明：在Inspector內做註解
  * ***********************************************/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;

public class Header : MonoBehaviour
{
    [Header("程式註解")]
    public List<headerInfo> headers;

    [Serializable]
    public struct headerInfo
    {
        [HideInInspector] public string name;
        public UnityEngine.Object obj;
        [TextArea] public string header;
    }


#if UNITY_EDITOR
    private void OnValidate()
    {
        headers.ForEach(m => { if (m.obj) { m.name = m.obj.name; } });
    }
#endif

}
