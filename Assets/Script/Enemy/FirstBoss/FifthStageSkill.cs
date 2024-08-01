using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FifthStageSkill : MonoBehaviour
{
    [Header("组件")]
    public EnemyStageControl selfEnemyStageControl;
    public EnemyControl selfEnemyControl;
    public Rigidbody2D rb;
    public Vector3 position1;
    public GameObject ScattingBulletPrefab;
    public GameObject RedBulletPrefab;
    public GameObject MoonPrefab;

    [Header("属性")]
    public float Maxhealth;
    public float currentHealth;
    public float stageLastTime;
    public float shootCooldown;// 每个子弹之间的延迟
    public float moonShootCooldown;
    public float bulletSpeed;// 子弹速度
    public float RedbulletSpeed;
    public float MoonbulletSpeed;
    public float RedbulletCount;
    public float flyDuration;
    private Coroutine moveCoroutine;
    public float d = 5f; // 设定的距离阈值
    public float baseSpeed = 1f; // 基础速度
    private float shootTimer = 0f;//射击计时器
    private float moonShootTimer = 0f;
    public float leftX;    // 左边x值阈值
    public float rightX;   // 右边x值阈值
    public float bottomY;  // 下边y值阈值
    public float topY;     // 上边y值阈值
    [Header("状态")]
    public bool isStart;
    public bool isFollow;
    public bool isEnter = true;
    private void Awake()
    {
        selfEnemyControl = GetComponent<EnemyControl>();
        selfEnemyStageControl = GetComponent<EnemyStageControl>();
        rb = GetComponent<Rigidbody2D>();
        stageLastTime = 40;
        isEnter = true;
        isFollow = false;

    }
    private void Update()
    {
        if (selfEnemyStageControl.currentStage != Stage.FifthStage) return;
        stageLastTime -= Time.deltaTime;
        shootTimer += Time.deltaTime;
        moonShootTimer += Time.deltaTime;
        currentHealth = selfEnemyStageControl.EnemyHealth;
        if (isEnter)
        {

        }else
        {
            if (isFollow)
            {
                FollowPlayer();
                if (shootTimer > shootCooldown)
                {
                    shootTimer = 0f;
                    StartCoroutine(ShootBullet());
                }
                if (moonShootTimer > moonShootCooldown)
                {
                    moonShootTimer = 0f;
                    StartCoroutine(MoonShoot());
                }
            }
            else
            {
                if (!isStart)
                    moveCoroutine = StartCoroutine(ShootRedBullet());
            }
            if (stageLastTime <= 0 || currentHealth <= 0)
            {
                //在这边停止一阶段的所有协程然后转阶段
                if (moveCoroutine != null)
                    StopCoroutine(moveCoroutine);
                selfEnemyStageControl.ChangeStage(Stage.SixthStage);
            }
        }
       
    }
    public void OnEnter()
    {
        currentHealth = Maxhealth;
        stageLastTime = 40;
        StartCoroutine(Move());
    }
    private IEnumerator ShootRedBullet()
    {
        isStart = true;
        float angleStep = 360f / RedbulletCount;
        for (int i = 0; i < RedbulletCount; i++)
        {
            float angle = i * angleStep;
            float angleRad = angle * Mathf.Deg2Rad; // 将角度转换为弧度
            // 实例化子弹
            Vector2 direction = new Vector2(Mathf.Cos(angleRad), Mathf.Sin(angleRad));
            FireRedBulletInDirection(direction, transform.position);
        }
        yield return new WaitForSeconds(1f);
        for (int i = 0; i < RedbulletCount; i++)
        {
            float angle = i * angleStep;
            float angleRad = angle * Mathf.Deg2Rad; // 将角度转换为弧度
            // 实例化子弹
            Vector2 direction = new Vector2(Mathf.Cos(angleRad), Mathf.Sin(angleRad));
            FireRedBulletInDirection(direction, transform.position);
        }
        yield return new WaitForSeconds(1f);
        for (int i = 0; i < RedbulletCount; i++)
        {
            float angle = i * angleStep;
            float angleRad = angle * Mathf.Deg2Rad; // 将角度转换为弧度
            // 实例化子弹
            Vector2 direction = new Vector2(Mathf.Cos(angleRad), Mathf.Sin(angleRad));
            FireRedBulletInDirection(direction, transform.position);
        }
        yield return new WaitForSeconds(1f);
        for (int i = 0; i < RedbulletCount; i++)
        {
            float angle = i * angleStep;
            float angleRad = angle * Mathf.Deg2Rad; // 将角度转换为弧度
            // 实例化子弹
            Vector2 direction = new Vector2(Mathf.Cos(angleRad), Mathf.Sin(angleRad));
            FireRedBulletInDirection(direction, transform.position);
        }
        yield return new WaitForSeconds(1f);
        isFollow = true;
        isStart = false;
        yield break;

    }

    private IEnumerator MoonShoot()
    {
        Vector3 direction = new Vector3(Random.Range(leftX, rightX), Random.Range(bottomY, topY), 0);
        Vector2 dir = (PlayerStateManager.Instance.playerTransform.position - direction).normalized;
        GameObject bullet = Instantiate(MoonPrefab, direction, Quaternion.identity);
        //Debug.Log("Create");
        // 设置子弹的速度
        Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            rb.velocity = dir.normalized * MoonbulletSpeed;
        }
        yield break;
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
    private void FireRedBulletInDirection(Vector2 direction, Vector3 startPosition)
    {
        // 创建子弹实例
        GameObject bullet = Instantiate(RedBulletPrefab, startPosition, Quaternion.identity);
        //Debug.Log("Create");
        // 设置子弹的速度
        Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            rb.velocity = direction.normalized * RedbulletSpeed;
        }
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
    IEnumerator ShootBullet()
    {
        Vector3 direction = new Vector3(Random.Range(leftX, rightX), Random.Range(bottomY, topY), 0);
        Vector2 dir = (PlayerStateManager.Instance.playerTransform.position - direction).normalized;
        FireBulletInDirection(dir, direction);
        yield return new WaitForSeconds(0.1f);
        FireBulletInDirection(dir, direction);
        yield return new WaitForSeconds(0.1f);
        FireBulletInDirection(dir, direction);
        yield break;
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
    private IEnumerator Move()
    {
        Vector2 startPosition = rb.position;
        float duringTime = 0f;
        while (duringTime < flyDuration)
        {
            duringTime += Time.deltaTime;
            float t = duringTime / flyDuration;
            Vector2 newPosition = Vector2.Lerp(startPosition, position1, t);
            rb.MovePosition(newPosition);
            yield return null;
        }
        rb.MovePosition(position1);
        rb.velocity = Vector3.zero;
        isStart = false;
        isEnter = false;
        yield break;
    }
}
