/*************************************************
  * 名稱：LocalTransform
  * 作者：RyanHsu
  * 功能說明：將localPosition及localScale顯示在Inspector內
  * ***********************************************/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//禁止多載         [DisallowMultipleComponent]
#if UNITY_EDITOR
[ExecuteAlways]
#endif
//依賴項           [RequireComponent(typeof CLASS)]
public class LocalTransform : MonoBehaviour
{
    public Vector3 m_localPosition;
    public Vector3 m_localRotation;
    public Vector3 m_localScale;

    void Start() { }

    void Update()
    {
        m_localPosition = transform.localPosition;
        m_localRotation = transform.localEulerAngles;
        m_localScale = transform.localScale;
    }

#if UNITY_EDITOR
    //void OnValidate() { }
    //void OnDrawGizmos() { }
    //private void OnDrawGizmosSelected() { }
#endif

}
