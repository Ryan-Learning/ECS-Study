/*************************************************
  * 名稱：boxWall
  * 作者：RyanHsu
  * 功能說明：
  * ***********************************************/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using Unity.Mathematics;

using Unity.Collections;
using Unity.Jobs;

//禁止多載         [DisallowMultipleComponent]
//#if UNITY_EDITOR
//執行階段執行     [ExecuteAlways]
//#endif
[RequireComponent(typeof(BoxCollider))]
//[ExecuteAlways]
public class BoxWall : MonoBehaviour
{
    public GameObject box;
    [Min(1)] public int heightCount = 500;
    [Min(1)] public int weightCount = 500;
    [Min(1)] public int depthCount = 100;
    public int totalCount;
    public bool isRandom;
    public List<RotationSelf> items;
    //ESC
    public List<BoxData> m_SelfDatas;
    //ESC

    Vector3 ConvertToUnit(Collider collider)
    {
        Bounds bounds = collider.bounds;
        Vector3 v = bounds.max - bounds.min;
        return new Vector3(heightCount == 1 ? 1 : (v.x / (heightCount - 1)), weightCount == 1 ? 1 : (v.y / (weightCount - 1)), depthCount == 1 ? 1 : (v.z / (depthCount - 1)));
    }

    void Start() { }

    void Update()
    {
        //ESC
        m_SelfDatas = new List<BoxData>();
        items.ForEach(m => m_SelfDatas.Add(new BoxData(m.transform, Time.deltaTime)));
        //ESC

        //DOTS
        NativeArray<BoxData> nativeList = new NativeArray<BoxData>(m_SelfDatas.ToArray(), Allocator.TempJob);
        BoxDataJob job = new BoxDataJob(nativeList);
        for (int i = 0; i < nativeList.Length; i++) {
            job.Execute(i);
        }

        JobHandle jobHandle = job.Schedule(nativeList.Length, 100);
        jobHandle.Complete();

        float n = Time.deltaTime * 10f * 100f;
        for (int i = 0; i < nativeList.Length; i++) {
            items[i].transform.eulerAngles += new Vector3(n, n, n);
        }

        nativeList.Dispose();
        //DOTS
    }

    public void CreatBox()
    {
        ClearBox();
        Collider collider = GetComponent<Collider>();
        totalCount = heightCount * weightCount * depthCount;

        Vector3 uniV3 = ConvertToUnit(collider);
        items = new List<RotationSelf>();

        for (int i = 0; i < heightCount; i++) {
            for (int j = 0; j < weightCount; j++) {
                for (int k = 0; k < depthCount; k++) {
                    Quaternion randomRotation = isRandom ? Quaternion.Euler(new Vector3(UnityEngine.Random.Range(0f, 360f), UnityEngine.Random.Range(0f, 360f), UnityEngine.Random.Range(0f, 360f))) : new Quaternion();
                    GameObject go = Instantiate(box, collider.bounds.min + new Vector3(i * uniV3.x, j * uniV3.y, k * uniV3.z), randomRotation, transform);
                    items.Add(go.GetComponent<RotationSelf>());
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

}


public struct BoxData
{
    public float _speed;
    public float _delfaTime;
    public float3 _rotation;

    public BoxData(Transform self, float deltaTime)
    {
        _speed = UnityEngine.Random.Range(1, 15);
        _delfaTime = deltaTime;
        _rotation = self.eulerAngles;
    }

    public void CalculateUpdate()
    {
        _rotation += new float3(_delfaTime * _speed * 100f);
    }
}

public struct BoxDataJob : IJobParallelFor
{
    public NativeArray<BoxData> dataList;

    public BoxDataJob(NativeArray<BoxData> list)
    {
        dataList = list;
    }

    public void Execute(int index)
    {
        dataList[index].CalculateUpdate();
    }
}