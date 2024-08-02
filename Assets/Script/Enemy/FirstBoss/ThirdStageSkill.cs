using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ThirdStageSkill : MonoBehaviour
{
    [Header("���")]
    public EnemyStageControl selfEnemyStageControl;
    public EnemyControl selfEnemyControl;
    public Rigidbody2D rb;
    public Vector3 position1;
    public Vector3 movePosition1;//ͷ��ֱ���ƶ�����
    public Vector3 movePosition2;
    public Vector3 movePosition3;//��߼����ƶ�����
    public Vector3 movePosition4;
    public Vector3 movePosition5;//�ұ߼����ƶ�����
    public Vector3 movePosition6;
    public float leftX;    // ���xֵ��ֵ
    public float rightX;   // �ұ�xֵ��ֵ
    public float bottomY;  // �±�yֵ��ֵ
    public float topY;     // �ϱ�yֵ��ֵ
    public GameObject laserPointPrefab;
    public GameObject ScattingBulletPrefab;

    //public Transform playerTransform;

    [Header("����")]
    public float Maxhealth;
    public float currentHealth;
    public float stageLastTime;
    public float redLaserLength;
    public float fireInterval;// ÿ���ӵ�֮����ӳ�
    public float bulletSpeed;// �ӵ��ٶ�
    public float flyDuration;
    private Coroutine moveCoroutine;
    public float d = 5f; // �趨�ľ�����ֵ
    public float baseSpeed = 1f; // �����ٶ�����
    [Header("״̬")]
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
                //�����ֹͣһ�׶ε�����Э��Ȼ��ת�׶�
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
            // ���������dʱ����������ķ�ʽ�ƶ���������dԶ��
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
        // �����ӵ�ʵ��
        //GameObject bullet = Instantiate(ScattingBulletPrefab, startPosition, Quaternion.identity);
        GameObject bullet = ObjectPool.Instance.CreateObject("BossStar",ScattingBulletPrefab, startPosition, Quaternion.identity);
        //Debug.Log("Create");
        // �����ӵ����ٶ�
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
