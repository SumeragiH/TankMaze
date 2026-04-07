using UnityEngine;

public class PLayerTankMountLookAt : MonoBehaviour
{
    [Header("俯仰角限制")]
    public float minPitchAngle = -15f; // 炮口最大下俯角度
    public float maxPitchAngle = 30f;  // 炮口最大上仰角度

    [Header("旋转平滑系数")]
    public float pitchSmoothing = 5f;

    private Camera mainCamera;
    private Plane groundPlane = new Plane(Vector3.up, Vector3.zero);

    void Start()
    {
        mainCamera = Camera.main;
    }

    void Update()
    {
        if (mainCamera == null) return;

        Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
        float enterDistance;

        if (groundPlane.Raycast(ray, out enterDistance))
        {
            Vector3 worldMousePosition = ray.GetPoint(enterDistance);

            // 计算炮口指向的目标方向（这次需要考虑Y轴高度差，以实现俯仰）
            Vector3 aimDirection = worldMousePosition - transform.position;

            // 将世界空间的方向向量转换到炮台基座的局部空间
            // 这样，X轴分量就代表了炮口在局部空间中需要的前后倾斜（俯仰）
            Vector3 localAimDirection = transform.parent.InverseTransformDirection(aimDirection);

            // 使用Mathf.Atan2计算在局部XZ平面内，指向目标时所需的俯仰角度（绕X轴旋转）
            // Atan2(y, z) 返回的是弧度，乘以 Mathf.Rad2Deg 转换为角度
            float targetPitchAngle = -Mathf.Atan2(localAimDirection.y, localAimDirection.z) * Mathf.Rad2Deg;

            // 限制俯仰角度在设定的范围内
            targetPitchAngle = Mathf.Clamp(targetPitchAngle, minPitchAngle, maxPitchAngle);

            // 平滑地应用绕X轴的旋转（俯仰）到炮口旋转轴
            float smoothedPitch = Mathf.LerpAngle(transform.localEulerAngles.x, targetPitchAngle, pitchSmoothing * Time.deltaTime);
            transform.localRotation = Quaternion.Euler(smoothedPitch, 0f, 0f);
        }
    }
}