using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Entities;
using Unity.Rendering;
using Unity.Transforms;
using System;

public class RotationSelf : MonoBehaviour
{
    public PlayerData data = new PlayerData();

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(Vector3.left, Time.deltaTime * data.speed);
    }

}


public partial class RotationSelf_ECS : SystemBase
{
    protected override void OnUpdate()
    {
        float deltaTime = Time.DeltaTime;
        Entities.ForEach((ref Translation pos, in PlayerData playerData) => {
            pos.Value.x += deltaTime * playerData.speed;
        }).ScheduleParallel();
    }
}