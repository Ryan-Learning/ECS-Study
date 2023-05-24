/*************************************************
  * 名稱：boxWall
  * 作者：RyanHsu
  * 功能說明：
  * ***********************************************/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

//禁止多載         [DisallowMultipleComponent]
//#if UNITY_EDITOR
//執行階段執行     [ExecuteAlways]
//#endif
[RequireComponent(typeof(BoxCollider))]
[ExecuteAlways]
public class BoxWall : MonoBehaviour
{
    public GameObject box;
    [Min(1)] public int heightCount = 500;
    [Min(1)] public int weightCount = 500;
    [Min(1)] public int depthCount = 100;
    public int totalCount;
    public bool isRandom;

    public float bx;
    public float by;
    public float bz;

    Vector3 ConvertToUnit(Collider collider)
    {
        Bounds bounds = collider.bounds;
        Vector3 v = bounds.max - bounds.min;
        return new Vector3(heightCount == 1 ? 1 : (v.x / (heightCount - 1)), weightCount == 1 ? 1 : (v.y / (weightCount - 1)), depthCount == 1 ? 1 : (v.z / (depthCount - 1)));
    }

    void Start() { }

    void Update()
    {
        bx = GetComponent<Collider>().bounds.size.x;
        by = GetComponent<Collider>().bounds.size.y;
        bz = GetComponent<Collider>().bounds.size.z;
    }

    public void CreatBox()
    {
        ClearBox();
        Collider collider = GetComponent<Collider>();
        totalCount = heightCount * weightCount * depthCount;

        Vector3 uniV3 = ConvertToUnit(collider);
        for (int i = 0; i < heightCount; i++) {
            for (int j = 0; j < weightCount; j++) {
                for (int k = 0; k < depthCount; k++) {
                    Quaternion randomRotation = isRandom ? Quaternion.Euler(new Vector3(Random.Range(0f, 360f), Random.Range(0f, 360f), Random.Range(0f, 360f))) : new Quaternion();
                    Instantiate(box, collider.bounds.min + new Vector3(i * uniV3.x, j * uniV3.y, k * uniV3.z), randomRotation, transform);
                }
            }
        }
    }

    public void ClearBox()
    {
        while (transform.childCount > 0) {
            DestroyImmediate(transform.GetChild(0).gameObject);
        }
    }

#if UNITY_EDITOR
    void OnValidate()
    {
        //CreatBox();
    }
    //void OnDrawGizmos() { }
    //private void OnDrawGizmosSelected() { }
#endif

}
