using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using UnityEngine;
using Unity.Mathematics;
using Unity.Transforms;

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
