using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class SecondStageSkill : MonoBehaviour
{
    [Header("组件")]
    public EnemyStageControl selfEnemyStageControl;
    public EnemyControl selfEnemyControl;
    public Rigidbody2D rb;
    public Vector3 position1;
    public GameObject LaserRedPrefab;
    public GameObject laserPointPrefab;
    public GameObject ScattingBulletPrefab;

    //public Transform playerTransform;

    [Header("属性")]
    public float health;
    public float stageLastTime;
    public float sideOffsetAngle; // 预警激光左右偏移角度
    public float redLaserLength;
    public float fireInterval;// 每个子弹之间的延迟
    public float spreadAngle;// 子弹发射左右偏移角度范围
    public float rotationDuration;// 角度平滑转动的时间
    public float bulletSpeed;// 子弹速度
    public float flyDuration;
    public float radius = 10f; // 激光生成圆的半径
    private Coroutine moveCoroutine;
    [Header("状态")]
    public bool isStart;
    private void Awake()
    {
        selfEnemyControl = GetComponent<EnemyControl>();
        selfEnemyStageControl = GetComponent<EnemyStageControl>();
        rb = GetComponent<Rigidbody2D>();
        health = 1000;
        stageLastTime = 40;

        //FIXME:调试用代码记得删
        rb.position = position1;
    }
    private void Update()
    {
        if (selfEnemyStageControl.currentStage != Stage.SecondStage) return;
        stageLastTime -= Time.deltaTime;
        if (stageLastTime <= 0 || health <= 0)
        {
            //在这边停止一阶段的所有协程然后转阶段
            if (moveCoroutine != null)
                StopCoroutine(moveCoroutine);

        }
        else
        {
            if (!isStart)
            {
                //StartCoroutine();
                isStart = true;
                moveCoroutine = StartCoroutine(GenerateShootingPointsCoroutine());
            }

        }
    }

    public void OnEnter()
    {
        health = 1000;
        stageLastTime = 40;
    }
    void InstantiateLaserPoint(Vector3 targetPosition)
    {
        GameObject laserPoint = Instantiate(laserPointPrefab, transform.position, Quaternion.identity);
        LaserPointController controller = laserPoint.GetComponent<LaserPointController>();
        if (controller != null)
        {
            controller.targetPosition = targetPosition;
            controller.isFixedAngleLaser = true;
        }
    }
    IEnumerator StartScatteringBullet()
    {

        GameObject mainLaser;
        GameObject leftLaser;
        GameObject rightLaser;
        mainLaser = Instantiate(LaserRedPrefab, transform.position, Quaternion.identity, transform);
        leftLaser = Instantiate(LaserRedPrefab, transform.position, Quaternion.identity, transform);
        rightLaser = Instantiate(LaserRedPrefab, transform.position, Quaternion.identity, transform);
        // 设置激光指向角色位置
        Vector2 direction = (PlayerStateManager.Instance.playerTransform.position - transform.position).normalized;
        //Debug.DrawRay(transform.position, direction * 1000, Color.blue, 100f);
        // 计算偏转方向
        float angleOffset = sideOffsetAngle;
        Vector2 directionLeft = Quaternion.Euler(0, 0, angleOffset) * direction;
        Vector2 directionRight = Quaternion.Euler(0, 0, -angleOffset) * direction;
        float deflectionAngle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

        // 调整激光
        AdjustLaser(mainLaser.transform, direction);
        AdjustLaser(leftLaser.transform, directionLeft);
        AdjustLaser(rightLaser.transform, directionRight);
        yield return new WaitForSeconds(1f);
        Destroy(mainLaser);
        Destroy(leftLaser);
        Destroy(rightLaser);
        yield return FireBullets(direction, spreadAngle);
        //yield return FireBullets(direction, -spreadAngle);
        yield return FireBullets(direction, spreadAngle);
        //yield return FireBullets(direction, -spreadAngle);
        //isStart = false;
        yield break;
    }
    private void AdjustLaser(Transform laser, Vector2 dir)
    {
        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        // 根据偏转角度旋转激光
        laser.rotation = Quaternion.Euler(0, 0, angle - 90);

        RaycastHit2D hit = Physics2D.Raycast(transform.position, dir, Mathf.Infinity, LayerMask.GetMask("Ground"));
        // 绘制可见的射线
        //Debug.DrawRay(transform.position, dir * 1000, Color.red,100f);
        if (hit.collider != null)
        {
            Debug.Log("find");
            Vector3 scale = laser.localScale;
            scale.y = hit.distance/redLaserLength; // 调整激光长度
            laser.localScale = scale;
        }
    }
    private IEnumerator FireBullets(Vector2 dir, float Angle)
    {
        float elapsedTime = 0f;
        float count = 0;
        while (elapsedTime < rotationDuration)
        {
            // 计算当前的发射角度
            float angle = Mathf.Lerp(0, Angle, elapsedTime / rotationDuration);
            float angle2 = Mathf.Lerp(0, -Angle, elapsedTime / rotationDuration);
            Vector2 direction = RotateVector(dir, angle);
            Vector2 direction2 = RotateVector(dir, angle2);
            FireBulletInDirection(direction);
            FireBulletInDirection(direction2);
            count++;
            //Debug.Log(elapsedTime);
            elapsedTime += fireInterval;
            //Debug.Log(elapsedTime);
            yield return new WaitForSeconds(fireInterval);
        }
        elapsedTime = 0f;
        while (elapsedTime < rotationDuration)
        {
            // 计算当前的发射角度
            float angle = Mathf.Lerp(Angle,0, elapsedTime / rotationDuration);
            float angle2 = Mathf.Lerp(-Angle, 0, elapsedTime / rotationDuration);
            Vector2 direction = RotateVector(dir, angle);
            Vector2 direction2 = RotateVector(dir, angle2);
            FireBulletInDirection(direction);
            FireBulletInDirection(direction2);
            count++;
            //Debug.Log(elapsedTime);
            elapsedTime += fireInterval;
            //Debug.Log(elapsedTime);
            yield return new WaitForSeconds(fireInterval);
        }
        //Debug.Log(count);
        //// 确保最后一个子弹发射在右偏移20度的位置
        //Vector2 finalDirection = RotateVector(dir, Angle);
        // FireBulletInDirection(finalDirection);
        yield break;
    }
    private void FireBulletInDirection(Vector2 direction)
    {
        // 创建子弹实例
        GameObject bullet = Instantiate(ScattingBulletPrefab, transform.position, Quaternion.identity);
        //Debug.Log("Create");
        // 设置子弹的速度
        Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            rb.velocity = direction.normalized * bulletSpeed;
        }
    }

    private Vector2 RotateVector(Vector2 vector, float angle)
    {
        float radians = angle * Mathf.Deg2Rad;
        float cos = Mathf.Cos(radians);
        float sin = Mathf.Sin(radians);
        return new Vector2(
            vector.x * cos - vector.y * sin,
            vector.x * sin + vector.y * cos
        );
    }
    private IEnumerator GenerateShootingPointsCoroutine()
    {
        float angleStep = 90f / 6f; // 每侧的角度步长 (90° / 6 = 15°)
        float leftStartAngle = -225f; // 左边的起始角度
        float rightStartAngle = 45f; // 右边的起始角度

        // 生成两边的射击点
        for (int i = 0; i < 6; i++)
        {
            float angle = leftStartAngle + i * angleStep;
            Vector3 position = CalculatePosition(angle);
            InstantiateLaserPoint(position);    
            float angle2 = rightStartAngle - i * angleStep;
            Vector3 position2 = CalculatePosition(angle2);
            InstantiateLaserPoint(position2);
            yield return new WaitForSeconds(0.5f); // 添加延迟
        }
        yield return new WaitForSeconds(2f);
        yield return StartScatteringBullet();
        yield return new WaitForSeconds(1f);
        yield return StartScatteringBullet();
        yield return new WaitForSeconds(1f);
        yield return StartScatteringBullet();
        isStart = false;
        yield break;
    }

    private Vector3 CalculatePosition(float angle)
    {
        // 将角度转换为弧度
        float radian = angle * Mathf.Deg2Rad;

        // 计算射击点的坐标
        float x = radius * Mathf.Cos(radian);
        float y = radius * Mathf.Sin(radian);

        return new Vector3(x, y, 0) + transform.position;
    }
    private IEnumerator MoveToTarget(Vector2 target, float time)
    {
        Vector2 startPosition = rb.position;
        float elapsedTime = 0f;

        while (elapsedTime < time)
        {
            elapsedTime += Time.deltaTime;
            float t = elapsedTime / time;

            // 线性插值计算当前位置
            Vector2 newPosition = Vector2.Lerp(startPosition, target, t);
            rb.MovePosition(newPosition);

            yield return null;
        }

        // 确保物体精确到达目标点
        rb.MovePosition(target);
        yield break;
    }
}
