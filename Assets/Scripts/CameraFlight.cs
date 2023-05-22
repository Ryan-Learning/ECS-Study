using UnityEngine;

public class CameraFlight : MonoBehaviour
{
    public float speed = 5f;
    public float rotationSpeed = 3f;

    private void Update()
    {
        // 获取按键输入
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");
        float rotationInput = Input.GetAxis("Mouse X");

        // 计算移动方向
        Vector3 moveDirection = new Vector3(horizontalInput, 0f, verticalInput).normalized;

        // 将移动方向转换为世界空间
        moveDirection = transform.TransformDirection(moveDirection);

        // 移动相机
        transform.position += moveDirection * speed * Time.deltaTime;

        // 鼠标右键旋转
        if (Input.GetMouseButton(1)) {
            float rotationAmount = rotationInput * rotationSpeed;
            transform.Rotate(Vector3.up, rotationAmount);
        }
    }
}