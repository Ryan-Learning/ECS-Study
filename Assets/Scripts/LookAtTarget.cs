using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[ExecuteAlways]
public class LookAtTarget : MonoBehaviour
{
    public Transform m_Target;
    void Start()
    {

    }

    void Update()
    {
        transform.LookAt(m_Target);
    }
}
