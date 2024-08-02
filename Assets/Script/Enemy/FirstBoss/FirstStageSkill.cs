using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Cinemachine.CinemachineTargetGroup;
using static UnityEngine.GraphicsBuffer;

public class FirstStageSkill : MonoBehaviour
{
    [Header("���")]
    public EnemyStageControl selfEnemyStageControl;
    public EnemyControl selfEnemyControl;
    public Rigidbody2D rb;
    public Vector3 position1;
    public Vector3 position2;
    public GameObject LaserRedPrefab;
    public GameObject laserPointPrefab;
    public GameObject ScattingBulletPrefab;

    //public Transform playerTransform;

    [Header("����")]
    public float Maxhealth;
    public float currentHealth;
    public float stageLastTime;
    public float sideOffsetAngle; // Ԥ����������ƫ�ƽǶ�
    public float redLaserLength;
    public float LaserPointOffsetX;
    public float LaserPointOffsetY;
    public float fireInterval;// ÿ���ӵ�֮����ӳ�
    public float spreadAngle;// �ӵ���������ƫ�ƽǶȷ�Χ
    public float rotationDuration;// �Ƕ�ƽ��ת����ʱ��
    public float bulletSpeed;// �ӵ��ٶ�
    public float flyDuration;

    private Coroutine moveCoroutine;
    [Header("״̬")]
    public bool selectFirstPosition = true;
    public bool isStart;
    private void Awake()
    {
        selfEnemyControl = GetComponent<EnemyControl>();
        selfEnemyStageControl = GetComponent<EnemyStageControl>();
        rb = GetComponent<Rigidbody2D>();
        stageLastTime = 40;
        isStart = true;
    }
    private void Update()
    {
        if (selfEnemyStageControl.currentStage != Stage.FirstStage) return;
        stageLastTime -= Time.deltaTime;
        currentHealth = selfEnemyStageControl.EnemyHealth;
        if (stageLastTime <= 0 || currentHealth <= 0)
        {
            //�����ֹͣһ�׶ε�����Э��Ȼ��ת�׶�
            if(moveCoroutine != null)
                StopCoroutine(moveCoroutine);
            ObjectPool.Instance.SetFalse("BlueLaserPointPrefab");
            ObjectPool.Instance.SetFalse("RedLaserWarningPrefab");
            ObjectPool.Instance.SetFalse("BossStar");
            selfEnemyStageControl.ChangeStage(Stage.SecondStage);

        }
        else
        {
            if(!isStart)
            {
                //StartCoroutine();
                isStart = true;
                moveCoroutine = StartCoroutine(MoveToTarget(selectFirstPosition? position1:position2,flyDuration));
                selectFirstPosition = !selectFirstPosition;
            }
            
        }
    }

    public void OnEnter()
    {
        currentHealth = Maxhealth;
        stageLastTime = 40;
        StartCoroutine(Move());
    }
    IEnumerator SpawnLaserPoints()
    {
        Vector3 startPosition = transform.position;

        // �������Ҳ��3��LaserPoint
        for (int i = 1; i < 4; i++)
        {
            Vector3 targetPosition = startPosition + new Vector3(-LaserPointOffsetX * i, LaserPointOffsetY * i, 0);
            InstantiateLaserPoint(targetPosition,150);
            Vector3 targetPosition1 = startPosition + new Vector3(LaserPointOffsetX * i, LaserPointOffsetY * i, 0);
            InstantiateLaserPoint(targetPosition1, 210);
            yield return new WaitForSeconds(0.5f); // ÿ������֮����ӳ٣��ɸ�����Ҫ����
        }
    }

    void InstantiateLaserPoint(Vector3 targetPosition,float deflectionAngle)
    {
        //GameObject laserPoint = Instantiate(laserPointPrefab, transform.position, Quaternion.identity);
        //Debug.Log("Start Create");
        GameObject laserPoint = ObjectPool.Instance.CreateObject("BlueLaserPointPrefab", laserPointPrefab, transform.position, Quaternion.identity);
        //Debug.Log("Create");
        //Debug.Log(laserPoint);
        LaserPointController controller = laserPoint.GetComponent<LaserPointController>();
        if (controller != null)
        {
            controller.targetPosition = targetPosition;
            controller.deflectionAngle = deflectionAngle;
            controller.isFixedAngleLaser = false;
        }
    }
    IEnumerator StartScatteringBullet()
    {

        GameObject mainLaser;
        GameObject leftLaser;
        GameObject rightLaser;
        //mainLaser = Instantiate(LaserRedPrefab, transform.position, Quaternion.identity);
        //leftLaser = Instantiate(LaserRedPrefab, transform.position, Quaternion.identity);
        //rightLaser = Instantiate(LaserRedPrefab, transform.position, Quaternion.identity);
        mainLaser = ObjectPool.Instance.CreateObject("RedLaserWarningPrefab",LaserRedPrefab, transform.position, Quaternion.identity);
        leftLaser = ObjectPool.Instance.CreateObject("RedLaserWarningPrefab", LaserRedPrefab, transform.position, Quaternion.identity);
        rightLaser = ObjectPool.Instance.CreateObject("RedLaserWarningPrefab", LaserRedPrefab, transform.position, Quaternion.identity);
        // ���ü���ָ���ɫλ��
        Vector2 direction = (PlayerStateManager.Instance.playerTransform.position - transform.position).normalized;
        //Debug.DrawRay(transform.position, direction * 1000, Color.blue, 100f);
        // ����ƫת����
        float angleOffset = 10f;
        Vector2 directionLeft = Quaternion.Euler(0, 0, angleOffset) * direction;
        Vector2 directionRight = Quaternion.Euler(0, 0, -angleOffset) * direction;
        float deflectionAngle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

        // ��������
        AdjustLaser(mainLaser.transform,direction);
        AdjustLaser(leftLaser.transform,directionLeft);
        AdjustLaser(rightLaser.transform,directionRight);
        yield return new WaitForSeconds(1f);
        //Destroy(mainLaser);
        //Destroy(leftLaser);
        //Destroy(rightLaser);
        ObjectPool.Instance.CollectObject(mainLaser);
        ObjectPool.Instance.CollectObject(leftLaser);
        ObjectPool.Instance.CollectObject(rightLaser);
        yield return FireBullets(direction, spreadAngle);
        yield return FireBullets(direction,-spreadAngle);
        yield return FireBullets(direction, spreadAngle);
        yield return FireBullets(direction,-spreadAngle);
        //isStart = false;
        yield break;
    }
    private void AdjustLaser(Transform laser,Vector2 dir)
    {
        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        // ����ƫת�Ƕ���ת����
        laser.rotation = Quaternion.Euler(0, 0, angle - 90);

        RaycastHit2D hit = Physics2D.Raycast(transform.position,dir, Mathf.Infinity, LayerMask.GetMask("Ground"));
        // ���ƿɼ�������
        //Debug.DrawRay(transform.position, dir * 1000, Color.red,100f);
        if (hit.collider != null)
        {
            //Debug.Log("find");
            Vector3 scale = laser.localScale;
            scale.y = hit.distance/redLaserLength; // �������ⳤ��
            laser.localScale = scale;
        }
    }
    private IEnumerator FireBullets(Vector2 dir,float Angle)
    {
        float elapsedTime = 0f;
        float count = 0;
        while (elapsedTime < rotationDuration)
        {
            // ���㵱ǰ�ķ���Ƕ�
            float angle = Mathf.Lerp(-Angle, Angle, elapsedTime / rotationDuration);
            Vector2 direction = RotateVector(dir, angle);
            FireBulletInDirection(direction);
            count ++;
            //Debug.Log(elapsedTime);
            elapsedTime += fireInterval;
            //Debug.Log(elapsedTime);
            yield return new WaitForSeconds(fireInterval);
        }
        //Debug.Log(count);
       //// ȷ�����һ���ӵ���������ƫ��20�ȵ�λ��
       //Vector2 finalDirection = RotateVector(dir, Angle);
       // FireBulletInDirection(finalDirection);
        yield break;
    }
    private void FireBulletInDirection(Vector2 direction)
    {
        // �����ӵ�ʵ��
        //GameObject bullet = Instantiate(ScattingBulletPrefab, transform.position, Quaternion.identity);
        GameObject bullet = ObjectPool.Instance.CreateObject("BossStar", ScattingBulletPrefab,transform.position, Quaternion.identity);
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
    private IEnumerator MoveToTarget(Vector2 target, float time)
    {
        Vector2 startPosition = rb.position;
        float elapsedTime = 0f;

        while (elapsedTime < time)
        {
            elapsedTime += Time.deltaTime;
            float t = elapsedTime / time;

            // ���Բ�ֵ���㵱ǰλ��
            Vector2 newPosition = Vector2.Lerp(startPosition, target, t);
            rb.MovePosition(newPosition);

            yield return null;
        }

        // ȷ�����徫ȷ����Ŀ���
        rb.MovePosition(target);
        yield return SpawnLaserPoints();
        yield return StartScatteringBullet();
        isStart = false;
        yield break;
    }

    private IEnumerator Move()
    {
        Vector2 startPosition = rb.position;
        float duringTime = 0f;
        while(duringTime < flyDuration)
        {
            duringTime += Time.deltaTime;
            float t = duringTime / flyDuration;
            Vector2 newPosition = Vector2.Lerp(startPosition, position1, t);
            rb.MovePosition(newPosition);
            yield return null;
        }
        rb.MovePosition(position1);
        isStart = false;
        yield break;
    }
}
