using UnityEngine;

public class PlayerTankLookAt : MonoBehaviour
{
    [Header("旋转平滑系数")]
    public float rotationSmoothing = 5f; // 值越大，转向越迅速

    private Camera mainCamera;
    private Plane groundPlane = new Plane(Vector3.up, Vector3.zero); // 定义一个在地面（Y=0）的平面用于射线检测

    void Start()
    {
        mainCamera = Camera.main;
    }

    void Update()
    {
        if (mainCamera == null) return;

        // 创建一条从摄像机发出，穿过鼠标位置的射线
        Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
        float enterDistance;

        // 如果射线与地面平面相交
        if (groundPlane.Raycast(ray, out enterDistance))
        {
            // 获取射线与地面的交点坐标
            Vector3 worldMousePosition = ray.GetPoint(enterDistance);

            // 计算炮台应该朝向的目标方向（忽略Y轴高度差，只考虑水平方向）
            Vector3 lookAtDirection = worldMousePosition - transform.position;
            lookAtDirection.y = 0; // 锁定Y轴，确保炮台只在水平面上旋转

            // 如果方向有效（鼠标不在炮台正上方），则让炮台平滑地转向目标方向
            if (lookAtDirection != Vector3.zero)
            {
                Quaternion targetRotation = Quaternion.LookRotation(lookAtDirection);
                transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSmoothing * Time.deltaTime);
            }
        }
    }
}