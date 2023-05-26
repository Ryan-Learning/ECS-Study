/*************************************************
  * 名稱：PointAtMouse
  * 作者：RyanHsu
  * 功能說明：3D物件向著鼠標旋轉
  * ***********************************************/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PointAtMouse : MonoBehaviour
{

    Vector3 pointingTarget;

    void Update()
    {
        pointingTarget = Camera.main.ScreenToWorldPoint(Input.mousePosition + Vector3.back * Camera.main.transform.position.z);
        transform.LookAt(pointingTarget, Vector3.back);
    }

    void OnDrawGizmos()
    {
        Gizmos.DrawSphere(pointingTarget, 0.2f);
        Gizmos.DrawLine(transform.position, pointingTarget);
    }
}
