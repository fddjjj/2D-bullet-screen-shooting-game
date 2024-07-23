using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserRedPointController : MonoBehaviour
{
    float redLaserLength;
    public Transform redLaser;
    public Transform collisionPoint;

    public Vector3 targetPosition;
    public float deflectionAngle;
    public float moveDuration = 5f; // ����Ŀ��λ�õ�ʱ��
    public float laserLength;
    public float staticLaserLength;
    public bool isFixedAngleLaser; // �Ƿ��ǹ̶��Ƕȼ���
    public Vector2 dir;

    private void Start()
    {
        // ��ʼ�ƶ���Ŀ��λ��
        StartCoroutine(MoveToTarget());
    }
    private IEnumerator MoveToTarget()
    {

        Vector3 startPosition = transform.position;
        float elapsedTime = 0f;

        while (elapsedTime < moveDuration)
        {
            transform.position = Vector3.Lerp(startPosition, targetPosition, elapsedTime / moveDuration);
            elapsedTime += Time.deltaTime;
            // ������ɫ����
            AdjustLaser(redLaser);
            yield return null;
        }

        transform.position = targetPosition;

        Destroy(gameObject); // ���� LaserPoint ����
    }

    private void AdjustLaser(Transform laser)
    {
        if (!isFixedAngleLaser)
        {
            // ����ƫת�Ƕ���ת����
            laser.rotation = Quaternion.Euler(0, 0, -deflectionAngle);
            // ���ݵ���������ⳤ��
            Vector2 direction = new Vector2(Mathf.Sin(Mathf.Deg2Rad * deflectionAngle), Mathf.Cos(Mathf.Deg2Rad * deflectionAngle));
            RaycastHit2D hit = Physics2D.Raycast(transform.position, direction, Mathf.Infinity, LayerMask.GetMask("Ground"));
            if (hit.collider != null)
            {
                //Debug.Log("find Ground");
                Vector3 scale = laser.localScale;
                scale.y = hit.distance / laserLength; // �������ⳤ��
                laser.localScale = scale;

                // ������ײ��λ��
                collisionPoint.position = hit.point;
            }else
            {
                Vector3 scale = laser.localScale;
                scale.y = staticLaserLength / laserLength; // �������ⳤ��
                laser.localScale = scale;
            }
        }
        else
        {
            float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
            laser.rotation = Quaternion.Euler(0, 0, angle - 90);
            RaycastHit2D hit = Physics2D.Raycast(transform.position, dir, Mathf.Infinity, LayerMask.GetMask("Ground"));
            Debug.DrawRay(transform.position, dir * 1000, Color.red, 100f);
            if (hit.collider != null)
            {
                Debug.Log("find Ground");
                Vector3 scale = laser.localScale;
                scale.y = hit.distance / laserLength; // �������ⳤ��
                laser.localScale = scale;

                // ������ײ��λ��
                collisionPoint.position = hit.point;
            }
        }
    }
}
