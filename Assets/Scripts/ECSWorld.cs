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

public partial class RenderingSystem : SystemBase
{
    protected override void OnUpdate()
    {
        Entities.ForEach((in RenderData renderData) =>
        {
            // 使用渲染数据进行渲染
            GameObject obj = GetGameObjectFromRenderData(renderData);
            // ...
        }).WithoutBurst().Run();
    }

    private GameObject GetGameObjectFromRenderData(RenderData renderData)
    {
        // 根据渲染数据创建或获取 GameObject
        GameObject gameObject = GameObject.CreatePrimitive(PrimitiveType.Cube);
        Debug.Log("創建Cube");

        // 设置位置、旋转、缩放等属性
        gameObject.transform.position = renderData.position;
        gameObject.transform.rotation = renderData.rotation;
        gameObject.transform.localScale = renderData.scale;

        // 返回创建或获取的 GameObject
        return gameObject;
    }
}

public partial class RenderSystem : SystemBase
{
    protected override void OnUpdate()
    {
        Entities.ForEach((ref RenderData renderData, in Translation translation, in Rotation rotation, in NonUniformScale scale) =>
        {
            // 更新渲染数据
            renderData.position = translation.Value;
            renderData.rotation = rotation.Value;
            renderData.scale = scale.Value;
        }).Schedule();
    }
}

public struct RenderData : IComponentData
{
    public float3 position;
    public quaternion rotation;
    public float3 scale;
}