using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FourthStageSkill : MonoBehaviour
{
    [Header("���")]
    public EnemyStageControl selfEnemyStageControl;
    public EnemyControl selfEnemyControl;
    public Rigidbody2D rb;
    public Vector3 position1;
    public GameObject ScattingBulletPrefab;
    public Transform MoonTransform; // Բ������
    public Transform ShootTransform;
    //public Transform playerTransform;

    [Header("����")]
    public float health;
    public float stageLastTime;
    public float shootCooldown;// ÿ���ӵ�֮����ӳ�
    public float bulletSpeed;// �ӵ��ٶ�
    public float flyDuration;
    private Coroutine moveCoroutine;
    public float d = 5f; // �趨�ľ�����ֵ
    public float baseSpeed = 1f; // �����ٶ�����

    public float rotationSpeed; // ��ת�ٶȣ�ÿ����ת�Ƕ�
    public float shootRotationSpeed;
    public float moonRadius; // ����뾶
    public float shootRadius;//�������뾶
    public float orbitSpeed; // ����˶��ٶȣ�ÿ���˶��ĽǶ�
    public float shootOrbitSpeed;
    public float growDuration; // �����̵ĳ���ʱ��
    private float currentMoonAngle; // ��ǰ�Ƕ�
    private float currentShootAngle;
    private Vector3 initialScale; // ��ʼ��С
    private float growTimer = 0f; // ����ʱ��
    private float shootTimer = 0f;//�����ʱ��
    [Header("״̬")]
    public bool isStart;
    public bool isFollow;
    private bool isGrowing = true; // �Ƿ��ڱ��
    private void Awake()
    {
        selfEnemyControl = GetComponent<EnemyControl>();
        selfEnemyStageControl = GetComponent<EnemyStageControl>();
        rb = GetComponent<Rigidbody2D>();
        health = 1000;
        stageLastTime = 40;

        isFollow = true;

        //FIXME:�����ô���ǵ�ɾ
        rb.position = position1;
    }
    void Start()
    {
        initialScale = MoonTransform.localScale; // ��¼��ʼ��С
        MoonTransform.localScale = Vector3.zero; // ��ʼʱ��СΪ0
        MoonTransform.position = transform.position; // ��ʼλ����ΪԲ��λ��
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
        //    //�����ֹͣһ�׶ε�����Э��Ȼ��ת�׶�
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
        MoonTransform.localScale = Vector3.Lerp(Vector3.zero, initialScale, t); // �𽥱��
        MoonTransform.position = Vector3.Lerp(transform.position, GetOrbitPosition(currentMoonAngle,moonRadius), t); // ���ƶ������
        // ��תЧ��
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
        // �������˶�
        currentMoonAngle += orbitSpeed * Time.deltaTime;
        MoonTransform.position = GetOrbitPosition(currentMoonAngle,moonRadius);

        // ��תЧ��
        MoonTransform.Rotate(Vector3.forward, rotationSpeed * Time.deltaTime);
    }
    public void MoveShootPosition()
    {
        // �������˶�
        currentShootAngle += shootOrbitSpeed * Time.deltaTime;
        ShootTransform.position = GetOrbitPosition(currentShootAngle, shootRadius);

        // ��תЧ��
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
        // �����ӵ����䷽�򣬿�����תӰ��
        float adjustedAngle = angle + ShootTransform.eulerAngles.z;
        Vector2 direction = new Vector2(Mathf.Cos(adjustedAngle * Mathf.Deg2Rad), Mathf.Sin(adjustedAngle * Mathf.Deg2Rad));
        FireBulletInDirection(direction, ShootTransform.position);
    }
    private void FireBulletInDirection(Vector2 direction, Vector3 startPosition)
    {
        // �����ӵ�ʵ��
        GameObject bullet = Instantiate(ScattingBulletPrefab, startPosition, Quaternion.identity);
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
}
