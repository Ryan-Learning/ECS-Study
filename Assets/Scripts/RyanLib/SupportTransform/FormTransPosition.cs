/*************************************************
  * 名稱：TransformEulaAngle
  * 作者：RyanHsu
  * 功能說明：將本地position實時轉換為FormTransTarget space座標並顯示
  * ***********************************************/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RyanLib.SupportTransform
{
    public class FormTransPosition : MonoBehaviour
    {
        public bool SyncEulaAngle;
        public Transform FormTransTarget;
        public Vector3 TargetTransform;

        private void Start() { }

#if UNITY_EDITOR
        private void OnDrawGizmos()
        {
            if (!isActiveAndEnabled) return;

            if (FormTransTarget) {
                if (SyncEulaAngle) {
                    transform.localPosition = transform.parent.InverseTransformPoint(FormTransTarget.TransformPoint(TargetTransform));
                } else {
                    TargetTransform = FormTransTarget.InverseTransformPoint(transform.TransformPoint(Vector3.zero));
                }
            } else {
                TargetTransform = Vector3.zero;
            }
        }
#endif
    }
}