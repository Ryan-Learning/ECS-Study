using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Unity.Mathematics;

public class RotationSelf : MonoBehaviour
{
    [Range(0, 10f)] public float speed = 10;

    public void SetRotation(float3 rotate)
    {
        transform.eulerAngles = new Vector3(rotate.x, rotate.y, rotate.z);
    }

}