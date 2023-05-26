using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using UnityEngine;
using Unity.Mathematics;
using Unity.Transforms;

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