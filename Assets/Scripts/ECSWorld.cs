using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using UnityEngine;
using Unity.Mathematics;
using Unity.Transforms;

public class ECSWorld : MonoBehaviour
{
    private World ecsWorld;
    private EntityManager entityManager;

    private void Awake()
    {
        // 创建ECS世界
        ecsWorld = World.DefaultGameObjectInjectionWorld;
        entityManager = ecsWorld.EntityManager;

        // 添加渲染系统
        RenderSystem renderSystem = ecsWorld.GetOrCreateSystem<RenderSystem>();
        if (ecsWorld.GetExistingSystem<RenderSystem>() == null) {
            ecsWorld.AddSystem(new RenderSystem());
        }

        // 添加渲染系统
        RenderingSystem renderingSystem = ecsWorld.GetOrCreateSystem<RenderingSystem>();
        if (ecsWorld.GetExistingSystem<RenderingSystem>() == null) {
            ecsWorld.AddSystem(renderingSystem);
        }
    }

    private void Update()
    {
        // 更新ECS世界
        ecsWorld.Update();
    }
}

public struct RenderData : IComponentData
{
    public float3 position;
    public quaternion rotation;
    public float3 scale;
}