using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserPointController : MonoBehaviour
{
    public Transform redLaser;
    public Transform blueLaser;
    public Transform collisionPoint; //碰撞点

    public Vector3 targetPosition;
    public float deflectionAngle;
    public float moveDuration = 2f; // 到达目标位置的时间
    public float scaleMultiplier = 2f; // 到达目标位置后的缩放倍数
    public float laserLength;

    private void Start()
    {
        // 开始移动到目标位置
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

        // 调整红色激光
        AdjustLaser(redLaser);

        // 启用红色激光 1 秒，然后启用蓝色激光
        redLaser.gameObject.SetActive(true);
        yield return new WaitForSeconds(1f);

        AdjustLaser(blueLaser);
        blueLaser.gameObject.SetActive(true);

        // 5 秒后禁用蓝色激光
        yield return new WaitForSeconds(5f);
        StartCoroutine(FadeOutAndDestroy());
    }

    private void AdjustLaser(Transform laser)
    {
        // 根据偏转角度旋转激光
        laser.rotation = Quaternion.Euler(0, 0,  - deflectionAngle);

        // 根据地面调整激光长度
        Vector2 direction = new Vector2(Mathf.Sin(Mathf.Deg2Rad * deflectionAngle), Mathf.Cos(Mathf.Deg2Rad * deflectionAngle));
        RaycastHit2D hit = Physics2D.Raycast(transform.position, direction, Mathf.Infinity, LayerMask.GetMask("Ground"));
        if (hit.collider != null)
        {
            Debug.Log("find Ground");
            Vector3 scale = laser.localScale;
            scale.y = hit.distance / laserLength; // 调整激光长度
            laser.localScale = scale;

            // 设置碰撞点位置
            collisionPoint.position = hit.point;
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
        Destroy(gameObject); // 销毁 LaserPoint 对象
    }
}
