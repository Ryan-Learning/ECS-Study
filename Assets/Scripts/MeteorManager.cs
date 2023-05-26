///**************************************************************
///
/// Classic System 肚参╰参
/// Classic System + Job System肚参╰参 + 穨╰参  <=****
/// ECS + Job System龟砰舱ン╰参 + 穨╰参
/// ECS + Job System + Burst Compiler 龟砰舱ン╰参 + 穨╰参 + Burst絪亩竟
/// 
///**************************************************************


using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using UnityEngine;
using UnityEngine.UI;
using Unity.Jobs;
using Unity.Burst;
using Unity.Mathematics;

public class MeteorManager : MonoBehaviour
{
    public GameObject meteorPrefab;
    public Transform parent;

    [Range(0, 5000f)] public int numMeteors;
    NativeArray<MeteorsData> meteorsDatas;
    Transform[] meteorTransforms;
    Text[] meteorText;

    void Start()
    {
        meteorsDatas = new NativeArray<MeteorsData>(numMeteors, Allocator.Persistent);
        meteorTransforms = new Transform[numMeteors];
        meteorText = new Text[numMeteors];

        for (int i = 0; i < numMeteors; i++) {
            meteorsDatas[i] = new MeteorsData(Vector3.zero, RandomVector(UnityEngine.Random.Range(6f, 10f)), UnityEngine.Random.insideUnitSphere, 0f, UnityEngine.Random.Range(0.1f, 1f));
            GameObject meteor = Instantiate(meteorPrefab, meteorsDatas[i].forwardVector * 10f, Quaternion.identity);
            meteor.transform.SetParent(parent);
            meteorTransforms[i] = meteor.transform;
            meteorText[i] = meteor.GetComponentInChildren<Text>();
        }
        parent.name = $"Empty({numMeteors})";
    }

    Vector3 RandomVector(float num) => new Vector3(num, num, num);

    void Update()
    {
        JobHandle jobHandle = DoMoveMeteorsJob();
        jobHandle.Complete();
    }

    private JobHandle DoMoveMeteorsJob()
    {
        MoveMeteorsJob job = new MoveMeteorsJob(Time.deltaTime, meteorsDatas);
        return job.Schedule(numMeteors, 32);
    }

    private void LateUpdate()
    {
        for (int i = 0; i < numMeteors; i++) {
            meteorTransforms[i].position = meteorsDatas[i].position;
            meteorTransforms[i].localScale = meteorsDatas[i].scale;
            meteorText[i].text = meteorsDatas[i].distance.ToString();
        }
    }

    private void OnDestroy()
    {
        meteorsDatas.Dispose();
        parent.name = "Empty";
    }

    [BurstCompile]
    private struct MoveMeteorsJob : IJobParallelFor
    {
        public float deltaTime;
        public NativeArray<MeteorsData> datas;

        public MoveMeteorsJob(float deltaTime, NativeArray<MeteorsData> datas)
        {
            this.deltaTime = deltaTime;
            this.datas = datas;
        }

        public void Execute(int index)
        {
            datas[index] = datas[index].Calculate(deltaTime);
        }
    }
}

public struct MeteorsData
{
    public Vector3 position;
    public Vector3 scale;
    public Vector3 forwardVector;
    public float distance;
    public float speed;

    public MeteorsData(Vector3 position, Vector3 scale, Vector3 forwardVector, float distance, float speed)
    {
        this.position = position;
        this.scale = scale;
        this.forwardVector = forwardVector;
        this.distance = distance;
        this.speed = speed;
    }

    public MeteorsData Calculate(float deltaTime)
    {
        distance = Vector3.Distance(Vector3.zero, position);
        if (distance > 1000f) position = Vector3.zero;
        position += forwardVector * speed * deltaTime;
        return this;
    }
}
