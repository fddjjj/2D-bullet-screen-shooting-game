using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class RaycastAndCircleIntersection2D : MonoBehaviour
{
    public Transform point; // �����ʼ��
    public float circleRadius;// Բ�İ뾶
    public Transform shootPoint;
    public Transform backArmBone;
    public Animator playerAnimator;

    private void Awake()
    {
        playerAnimator = GetComponent<Animator>();
        backArmBone = playerAnimator.transform.Find("Skeletal/bone_1/bone_2/bone_3/bone_4/bone_8");
        if (backArmBone == null)
        {
            Debug.LogError("Bone not found: " + "bone_8");
            return;
        }
    }

    void Update()
    {
        // ��ȡ�������Ļ�ϵ�λ��
        Vector3 mousePosition = Mouse.current.position.ReadValue();

        // ����Ļ����ת��Ϊ��������
        Vector3 worldMousePosition = Camera.main.ScreenToWorldPoint(new Vector3(mousePosition.x, mousePosition.y, 0));

        // ֻ��Ҫx��y�������
        worldMousePosition.z = 0;

        // �������߷���
        Vector2 rayDirection = (worldMousePosition - point.position).normalized;

        // ��������
        RaycastHit2D hit = Physics2D.Raycast(point.position, rayDirection);

        //if (hit.collider != null)
        //{
        //    ���������ײ�����壬���������ﴦ����ײ�¼�
        //    Debug.Log("Ray hit: " + hit.collider.name);
        //}

        // ����������Բ�Ľ���
        Vector2 circleCenter = point.position;
        Vector2 rayOrigin = point.position;

        // ����������Բ�Ľ���
        Vector2 intersectionPoint1, intersectionPoint2;
        bool hasIntersection = GetRayCircleIntersections(circleCenter, circleRadius, rayOrigin, rayDirection, out intersectionPoint1, out intersectionPoint2);

        if (hasIntersection)
        {
            //Debug.Log("Intersection points: " + intersectionPoint1 + " and " + intersectionPoint2);
            shootPoint.position = intersectionPoint1;

            //// ����ָ�򽻵�ĽǶ�
            //Vector2 direction = intersectionPoint1 - (Vector2)backArmBone.position;
            //float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

            //// ������״�����ת
            //backArmBone.rotation = Quaternion.Euler(0, 0, angle);
            //Debug.Log("set Finished");
        }

        // ����Բ����Scene��ͼ�л��ƣ�
        DebugDrawCircle(point.position, circleRadius, Color.red);
    }

    bool GetRayCircleIntersections(Vector2 circleCenter, float radius, Vector2 rayOrigin, Vector2 rayDirection, out Vector2 intersection1, out Vector2 intersection2)
    {
        intersection1 = Vector2.zero;
        intersection2 = Vector2.zero;

        Vector2 diff = rayOrigin - circleCenter;
        float a = Vector2.Dot(rayDirection, rayDirection);
        float b = 2 * Vector2.Dot(rayDirection, diff);
        float c = Vector2.Dot(diff, diff) - radius * radius;

        float discriminant = b * b - 4 * a * c;

        if (discriminant < 0)
        {
            return false;
        }

        float sqrtDiscriminant = Mathf.Sqrt(discriminant);

        float t1 = (-b + sqrtDiscriminant) / (2 * a);
        float t2 = (-b - sqrtDiscriminant) / (2 * a);

        intersection1 = rayOrigin + t1 * rayDirection;
        intersection2 = rayOrigin + t2 * rayDirection;

        return true;
    }

    // �ڳ�����ͼ�л���Բ�ķ���ʾ��
    void DebugDrawCircle(Vector3 center, float radius, Color color)
    {
        Vector3 previousPoint = center + new Vector3(radius, 0, 0);
        int segments = 50;

        for (int i = 1; i <= segments; i++)
        {
            float angle = i / (float)segments * 360f;
            Vector3 nextPoint = center + new Vector3(Mathf.Cos(Mathf.Deg2Rad * angle) * radius, Mathf.Sin(Mathf.Deg2Rad * angle) * radius, 0);

            Debug.DrawLine(previousPoint, nextPoint, color);

            previousPoint = nextPoint;
        }
    }
}
