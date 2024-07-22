using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserPointController : MonoBehaviour
{
    public Transform redLaser;
    public Transform blueLaser;
    public Transform collisionPoint; //��ײ��

    public Vector3 targetPosition;
    public float deflectionAngle;
    public float moveDuration = 2f; // ����Ŀ��λ�õ�ʱ��
    public float scaleMultiplier = 2f; // ����Ŀ��λ�ú�����ű���
    public float laserLength;
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
            yield return null;
        }

        transform.position = targetPosition;
        transform.localScale *= scaleMultiplier;
        dir = (PlayerStateManager.Instance.playerTransform.position - transform.position).normalized;
        // ������ɫ����
        AdjustLaser(redLaser);

        // ���ú�ɫ���� 1 �룬Ȼ��������ɫ����
        redLaser.gameObject.SetActive(true);
        yield return new WaitForSeconds(1f);
        redLaser.gameObject.SetActive(false);
        AdjustLaser(blueLaser);
        blueLaser.gameObject.SetActive(true);

        // 5 ��������ɫ����
        yield return new WaitForSeconds(3f);
        StartCoroutine(FadeOutAndDestroy());
    }

    private void AdjustLaser(Transform laser)
    {
        if (!isFixedAngleLaser)
        {
             // ����ƫת�Ƕ���ת����
            laser.rotation = Quaternion.Euler(0, 0,  - deflectionAngle);

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
            }
        }else
        {
            float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
            laser.rotation = Quaternion.Euler(0, 0, angle - 90);
            RaycastHit2D hit = Physics2D.Raycast(transform.position, dir, Mathf.Infinity, LayerMask.GetMask("Ground"));
            Debug.DrawRay(transform.position, dir * 1000, Color.red,100f);
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

    private IEnumerator FadeOutAndDestroy()
    {
        float fadeDuration = 0.2f;
        float elapsedTime = 0f;
        Vector3 initialScale = blueLaser.localScale;
        Vector3 targetScale = new Vector3(0, initialScale.y, initialScale.z);

        while (elapsedTime < fadeDuration)
        {
            blueLaser.localScale = Vector3.Lerp(initialScale, targetScale, elapsedTime / fadeDuration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        blueLaser.localScale = targetScale;
        Destroy(gameObject); // ���� LaserPoint ����
    }
}