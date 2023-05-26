/*************************************************
  * 名稱：TransformEulaAngle
  * 作者：RyanHsu
  * 功能說明：顯示GameObject的TransformEulaAngle
  * ***********************************************/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RyanLib.SupportTransform
{
    public class TransformEulaAngle : MonoBehaviour
    {
        public bool SyncEulaAngle;
        public Vector3 EulaAngle = Vector3.zero;
        DirtyVector dirty = new DirtyVector();

        private void FixedUpdate()
        {
            if (!isActiveAndEnabled) return;
            if (SyncEulaAngle)
            {
                if (dirty.isDirty(EulaAngle)) updatePose();
            }
            else
            {
                //EulaAngle = transform.localEulerAngles;
            }
        }

        private void updatePose()
        {
            Vector3 dA = EulaAngle - (Vector3)dirty.obsoleteValue;
            transform.RotateAround(transform.position, transform.right, dA.x);
            transform.RotateAround(transform.position, transform.up, dA.y);
            transform.RotateAround(transform.position, transform.forward, dA.z);
        }

#if UNITY_EDITOR
        private void OnDrawGizmos()
        {
            FixedUpdate();
        }
#endif
    }
}
