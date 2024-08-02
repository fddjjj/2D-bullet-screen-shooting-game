using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ThirdStageSkill : MonoBehaviour
{
    [Header("组件")]
    public EnemyStageControl selfEnemyStageControl;
    public EnemyControl selfEnemyControl;
    public Rigidbody2D rb;
    public Vector3 position1;
    public Vector3 movePosition1;//头顶直线移动坐标
    public Vector3 movePosition2;
    public Vector3 movePosition3;//左边激光移动坐标
    public Vector3 movePosition4;
    public Vector3 movePosition5;//右边激光移动坐标
    public Vector3 movePosition6;
    public float leftX;    // 左边x值阈值
    public float rightX;   // 右边x值阈值
    public float bottomY;  // 下边y值阈值
    public float topY;     // 上边y值阈值
    public GameObject laserPointPrefab;
    public GameObject ScattingBulletPrefab;

    //public Transform playerTransform;

    [Header("属性")]
    public float Maxhealth;
    public float currentHealth;
    public float stageLastTime;
    public float redLaserLength;
    public float fireInterval;// 每个子弹之间的延迟
    public float bulletSpeed;// 子弹速度
    public float flyDuration;
    private Coroutine moveCoroutine;
    public float d = 5f; // 设定的距离阈值
    public float baseSpeed = 1f; // 基础速度因子
    [Header("状态")]
    public bool isStart = true;
    public bool isFollow;
    public bool isEnter = true;
    public bool isNeedMove = false;
    private void Awake()
    {
        selfEnemyControl = GetComponent<EnemyControl>();
        selfEnemyStageControl = GetComponent<EnemyStageControl>();
        rb = GetComponent<Rigidbody2D>();
        stageLastTime = 40;
        isFollow = true;
    }
    private void Update()
    {
        if (selfEnemyStageControl.currentStage != Stage.ThirdStage) return;
        stageLastTime -= Time.deltaTime;
        currentHealth = selfEnemyStageControl.EnemyHealth;
        if (isEnter)
        {

        }else
        {
            if (stageLastTime <= 0 ||  currentHealth<= 0)
            {
                //在这边停止一阶段的所有协程然后转阶段
                if (moveCoroutine != null)
                    StopCoroutine(moveCoroutine);
                ObjectPool.Instance.SetFalse("RedLaserPointPrefab");
                ObjectPool.Instance.SetFalse("BossStar");
                selfEnemyStageControl.ChangeStage(Stage.FourthStage);

            }
            else
            {
                if (isFollow)
                {
                    FollowPlayer();
                    if (!isStart)
                    {
                        isStart = true;
                        moveCoroutine = StartCoroutine(StageOne());
                    }
                }
                else
                {
                    if (isNeedMove)
                    {
                        rb.velocity = Vector3.zero;
                        //isStart = true;
                        StartCoroutine(Move());
                    }
                    if (!isStart && ! isNeedMove)
                    {
                        isStart = true;
                        moveCoroutine = StartCoroutine(StageTwo());
                    }
                }
            }
        }
    }

    public void OnEnter()
    {
        currentHealth = Maxhealth;
        stageLastTime = 40;
        StartCoroutine(Move());
        
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

            Vector3 targetPosition = new Vector3(playerPosition.x + moveDistance,selfPosition.y, selfPosition.z);
            float moveSpeed = baseSpeed * Mathf.Abs(targetPosition.x - selfPosition.x);
            rb.velocity = new Vector2(moveSpeed * moveDir, rb.velocity.y);
            //rb.MovePosition(targetPosition);
        }
    }
    IEnumerator  StageOne()
    {
        InstantiateLaserPoint(movePosition1, movePosition2,180);
        InstantiateLaserPoint(movePosition2, movePosition1, 180);
        yield return ShootBullet();
        yield return ShootBullet();
        yield return new WaitForSeconds(4f);
        InstantiateLaserPoint(movePosition1, movePosition2, 180);
        InstantiateLaserPoint(movePosition2, movePosition1, 180);
        yield return ShootBullet();
        yield return ShootBullet();
        yield return new WaitForSeconds(4f);
        InstantiateLaserPoint(movePosition1, movePosition2, 180);
        InstantiateLaserPoint(movePosition2, movePosition1, 180);
        yield return ShootBullet();
        yield return ShootBullet();
        yield return new WaitForSeconds(0.2f);
        InstantiateLaserPoint(movePosition1, movePosition2, 180);
        InstantiateLaserPoint(movePosition2, movePosition1, 180);
        yield return new WaitForSeconds(0.2f);
        InstantiateLaserPoint(movePosition1, movePosition2, 180);
        InstantiateLaserPoint(movePosition2, movePosition1, 180);
        yield return new WaitForSeconds(0.2f);
        InstantiateLaserPoint(movePosition1, movePosition2, 180);
        InstantiateLaserPoint(movePosition2, movePosition1, 180);
        yield return new WaitForSeconds(0.2f);
        isFollow =false;
        isStart = false;
        isNeedMove = true;
        yield break;
    }
    IEnumerator StageTwo()
    {
        yield return ShootBullet();
        yield return ShootBullet();
        InstantiateLaserPoint(movePosition4,movePosition3,90);
        yield return new WaitForSeconds(3f);
        yield return ShootBullet();
        yield return ShootBullet();
        InstantiateLaserPoint(movePosition6, movePosition5,270);
        yield return new WaitForSeconds(3f);
        isStart = false;
        yield break;

    }
    void InstantiateLaserPoint(Vector3 targetPosition,Vector3 startPosition,float angle)
    {
        //GameObject laserPoint = Instantiate(laserPointPrefab, startPosition, Quaternion.identity);
        GameObject laserPoint = ObjectPool.Instance.CreateObject("RedLaserPointPrefab", laserPointPrefab, startPosition, Quaternion.identity);
        LaserRedPointController controller = laserPoint.GetComponent<LaserRedPointController>();
        if (controller != null)
        {
            controller.targetPosition = targetPosition;
            controller.deflectionAngle = angle;
            controller.isFixedAngleLaser = false;
        }
    }
    IEnumerator ShootBullet()
    {
        Vector3 direction = new Vector3(Random.Range(leftX, rightX), Random.Range(bottomY, topY),0);
        Vector2 dir = (PlayerStateManager.Instance.playerTransform.position - direction).normalized;
        FireBulletInDirection(dir, direction);
        yield break;
    }
    private void FireBulletInDirection(Vector2 direction,Vector3 startPosition)
    {
        // 创建子弹实例
        //GameObject bullet = Instantiate(ScattingBulletPrefab, startPosition, Quaternion.identity);
        GameObject bullet = ObjectPool.Instance.CreateObject("BossStar",ScattingBulletPrefab, startPosition, Quaternion.identity);
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
        isNeedMove = false;
        isEnter = false;
        yield break;
    }
}
