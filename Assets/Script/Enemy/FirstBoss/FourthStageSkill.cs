using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FourthStageSkill : MonoBehaviour
{
    [Header("组件")]
    public EnemyStageControl selfEnemyStageControl;
    public EnemyControl selfEnemyControl;
    public Rigidbody2D rb;
    public Vector3 position1;
    public GameObject ScattingBulletPrefab;
    public Transform MoonTransform; // 圆心物体
    public Transform ShootTransform;
    //public Transform playerTransform;

    [Header("属性")]
    public float health;
    public float stageLastTime;
    public float shootCooldown;// 每个子弹之间的延迟
    public float bulletSpeed;// 子弹速度
    public float flyDuration;
    private Coroutine moveCoroutine;
    public float d = 5f; // 设定的距离阈值
    public float baseSpeed = 1f; // 基础速度因子

    public float rotationSpeed; // 自转速度，每秒旋转角度
    public float shootRotationSpeed;
    public float moonRadius; // 轨道半径
    public float shootRadius;//射击轨道半径
    public float orbitSpeed; // 轨道运动速度，每秒运动的角度
    public float shootOrbitSpeed;
    public float growDuration; // 变大过程的持续时间
    private float currentMoonAngle; // 当前角度
    private float currentShootAngle;
    private Vector3 initialScale; // 初始大小
    private float growTimer = 0f; // 变大计时器
    private float shootTimer = 0f;//射击计时器
    [Header("状态")]
    public bool isStart;
    public bool isFollow;
    private bool isGrowing = true; // 是否在变大
    private void Awake()
    {
        selfEnemyControl = GetComponent<EnemyControl>();
        selfEnemyStageControl = GetComponent<EnemyStageControl>();
        rb = GetComponent<Rigidbody2D>();
        health = 1000;
        stageLastTime = 40;

        isFollow = true;

        //FIXME:调试用代码记得删
        rb.position = position1;
    }
    void Start()
    {
        initialScale = MoonTransform.localScale; // 记录初始大小
        MoonTransform.localScale = Vector3.zero; // 初始时大小为0
        MoonTransform.position = transform.position; // 初始位置设为圆心位置
    }
    private void Update()
    {
        if (selfEnemyStageControl.currentStage != Stage.FourthStage) return;
        stageLastTime -= Time.deltaTime;
        FollowPlayer();
        if (isGrowing)
        {
            MoonTransform.gameObject.SetActive(true);
            GrowAndMoveToOrbit();
        }
        else
        {
            ShootTransform.gameObject.SetActive(true);
            MoveInCircularPath();
            MoveShootPosition();
            ShootBullets();
        }

        //if (stageLastTime <= 0 || health <= 0)
        //{
        //    //在这边停止一阶段的所有协程然后转阶段
        //    if (moveCoroutine != null)
        //        StopCoroutine(moveCoroutine);

        //}
        //else
        //{
        //    if (!isStart)
        //    {
        //        //StartCoroutine();
        //        isStart = true;
        //        //moveCoroutine = StartCoroutine(GenerateShootingPointsCoroutine());
        //    }

        //}
    }

    private void GrowAndMoveToOrbit()
    {
        currentMoonAngle += orbitSpeed * Time.deltaTime;
        growTimer += Time.deltaTime;
        float t = growTimer / growDuration;
        MoonTransform.localScale = Vector3.Lerp(Vector3.zero, initialScale, t); // 逐渐变大
        MoonTransform.position = Vector3.Lerp(transform.position, GetOrbitPosition(currentMoonAngle,moonRadius), t); // 逐渐移动到轨道
        // 自转效果
        MoonTransform.Rotate(Vector3.forward, rotationSpeed * Time.deltaTime);

        if (growTimer >= growDuration)
        {
            isGrowing = false;
        }
        
    }

    private Vector3 GetOrbitPosition(float angle,float radius)
    {
        float newX = transform.position.x + Mathf.Cos(angle * Mathf.Deg2Rad) * radius;
        float newY = transform.position.y + Mathf.Sin(angle * Mathf.Deg2Rad) * radius;
        return new Vector3(newX, newY, 0);
    }

    public void MoveInCircularPath()
    {
        // 计算轨道运动
        currentMoonAngle += orbitSpeed * Time.deltaTime;
        MoonTransform.position = GetOrbitPosition(currentMoonAngle,moonRadius);

        // 自转效果
        MoonTransform.Rotate(Vector3.forward, rotationSpeed * Time.deltaTime);
    }
    public void MoveShootPosition()
    {
        // 计算轨道运动
        currentShootAngle += shootOrbitSpeed * Time.deltaTime;
        ShootTransform.position = GetOrbitPosition(currentShootAngle, shootRadius);

        // 自转效果
        ShootTransform.Rotate(Vector3.forward, shootRotationSpeed * Time.deltaTime);
    }
    public void OnEnter()
    {
        health = 1000;
        stageLastTime = 40;
    }
    public void FollowPlayer()
    {
        Vector3 selfPosition = transform.position;
        Vector3 playerPosition = PlayerStateManager.Instance.playerTransform.position;

        float distanceX = Mathf.Abs(selfPosition.x - playerPosition.x);

        if (distanceX <= d)
        {
            rb.velocity = Vector2.zero;
        }
        else
        {
            // 当距离大于d时，根据最近的方式移动到距离我d远处
            float moveDir;
            float moveDistance;
            if (selfPosition.x > playerPosition.x)
            {
                moveDir = -1;
                moveDistance = d;
            }
            else
            {
                moveDir = 1;
                moveDistance = -d;
            }

            Vector3 targetPosition = new Vector3(playerPosition.x + moveDistance, selfPosition.y, selfPosition.z);
            float moveSpeed = baseSpeed * Mathf.Abs(targetPosition.x - selfPosition.x);
            rb.velocity = new Vector2(moveSpeed * moveDir, rb.velocity.y);
            //rb.MovePosition(targetPosition);
        }
    }
    private void ShootBullets()
    {
        shootTimer += Time.deltaTime;
        if (shootTimer >= shootCooldown)
        {
            shootTimer = 0f;
            FireBullet(0);
            FireBullet(120);
            FireBullet(240);
        }
    }
    private void FireBullet(float angle)
    {
        // 计算子弹发射方向，考虑自转影响
        float adjustedAngle = angle + ShootTransform.eulerAngles.z;
        Vector2 direction = new Vector2(Mathf.Cos(adjustedAngle * Mathf.Deg2Rad), Mathf.Sin(adjustedAngle * Mathf.Deg2Rad));
        FireBulletInDirection(direction, ShootTransform.position);
    }
    private void FireBulletInDirection(Vector2 direction, Vector3 startPosition)
    {
        // 创建子弹实例
        GameObject bullet = Instantiate(ScattingBulletPrefab, startPosition, Quaternion.identity);
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
}
