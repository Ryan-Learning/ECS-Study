using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Entities;
using Unity.Rendering;
using Unity.Transforms;
using System;

public class PureECS : MonoBehaviour
{
    public Mesh mesh;
    public Material material;

    public void Start()
    {
        EntityManager entityManager = World.DefaultGameObjectInjectionWorld.EntityManager;

        EntityArchetype playerArchetype = entityManager.CreateArchetype(
            typeof(Translation),
            typeof(RenderMesh),
            typeof(RenderBounds),
            typeof(LocalToWorld),
            typeof(PlayerData)
        );

        Entity playerEntity = entityManager.CreateEntity(playerArchetype);

        entityManager.SetSharedComponentData(playerEntity, new RenderMesh {
            mesh = mesh,
            material = material,
            layerMask = 1
        });

    }
}

[Serializable]
public struct PlayerData : IComponentData
{
    public float speed;
}


public partial class PlayerMovementSystem : SystemBase
{
    protected override void OnUpdate()
    {
        float deltaTime = Time.DeltaTime;
        Entities.ForEach((ref Translation pos, in PlayerData playerData) => {
            pos.Value.x += deltaTime * playerData.speed;
        }).ScheduleParallel();
    }
}