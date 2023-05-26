/*************************************************
  * 名稱：MixBoxCollider
  * 作者：RyanHsu
  * 功能說明：(AABB) 將子項Collider範圍加總至m_TargetBoxCollider
  * ***********************************************/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using ExtensionMethods;

#if UNITY_EDITOR
[ExecuteAlways]
#endif
public class MixBoxCollider : MonoBehaviour
{
    [CantBeNull]public BoxCollider m_TargetBoxCollider;
    [Header("包含未激活項目")] public bool includeInactive;
    public Collider[] m_Colliders;
    DirtyInt childCount_dirty = new DirtyInt();

    void Update()
    {
        if (!m_TargetBoxCollider) return;

        ////列出Child Collider列表，且不包含自已的Collider
        //if (childCount_dirty.isDirty(transform.childCount))
        //    m_Colliders = gameObject.GetComponentsInChildren<Collider>(true).Where(m => m.gameObject != gameObject).ToArray();

        if (m_Colliders.Length > 0) {
            Bounds bounds = UnionBounds(m_Colliders);
            SetBoxCollider(m_TargetBoxCollider, bounds);
        }
    }

    //聯集Collider
    Bounds UnionBounds(Collider[] baseCollider)
    {
        baseCollider.ForEach(m => m.isTrigger = false);//關閉所有Collider的isTrigger
        Collider[] colliders = includeInactive ? baseCollider : baseCollider.Where(m => m.gameObject.activeInHierarchy).ToArray();//未激活項目篩選

        Bounds[] bounds = colliders.Select(m => m.bounds).ToArray();
        Bounds firstBound = bounds[0];
        int max = bounds.Length - 1;

        for (int i = 0; i < max; i++) {
            firstBound.Encapsulate(bounds[i + 1]);
        }

        return firstBound;
    }

    //BoxCollider Set
    void SetBoxCollider(BoxCollider bc, Bounds bounds)
    {
        Bounds box_Bounds = bc.bounds;
        box_Bounds.SetMinMax(bounds.min, bounds.max);
        bc.center = transform.InverseTransformPoint(box_Bounds.center);
        bc.size = box_Bounds.size;
    }
}