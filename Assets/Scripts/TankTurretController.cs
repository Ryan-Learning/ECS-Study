using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TankTurretController : MonoBehaviour
{
    Camera mainCamera; // 主摄像机
    Transform turret; // 坦克炮管
    float offsetAngle = 0f; // 偏移角度

    public void Awake()
    {
        mainCamera = Camera.main;
        turret = transform;
        Cursor.visible = false;
    }

    void Update()
    {
        if (!isActiveAndEnabled) return;

        Vector3 mousePosition = Input.mousePosition;

        Vector3 worldMousePosition = mainCamera.ScreenToWorldPoint(new Vector3(mousePosition.x, mousePosition.y, mainCamera.transform.position.y));

        Vector3 direction = worldMousePosition - turret.position;
        float angle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg;
        
        //angle = Mathf.Clamp(angle, -45f+ offsetAngle, 45f+ offsetAngle);
        angle += offsetAngle;

        turret.rotation = Quaternion.Euler(0f, angle, 0f);
    }

    public void OnDrawGizmos()
    {
        Vector3 startPosition = transform.position;
        Vector3 endPosition = transform.position + transform.forward * 10f;
        Gizmos.color = Color.red;

        Gizmos.DrawLine(startPosition, endPosition);
    }
}
