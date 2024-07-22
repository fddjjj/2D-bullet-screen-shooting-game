using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class SecondStageSkill : MonoBehaviour
{
    [Header("���")]
    public EnemyStageControl selfEnemyStageControl;
    public EnemyControl selfEnemyControl;
    public Rigidbody2D rb;
    public Vector3 position1;
    public GameObject LaserRedPrefab;
    public GameObject laserPointPrefab;
    public GameObject ScattingBulletPrefab;

    //public Transform playerTransform;

    [Header("����")]
    public float health;
    public float stageLastTime;
    public float sideOffsetAngle; // Ԥ����������ƫ�ƽǶ�
    public float redLaserLength;
    public float fireInterval;// ÿ���ӵ�֮����ӳ�
    public float spreadAngle;// �ӵ���������ƫ�ƽǶȷ�Χ
    public float rotationDuration;// �Ƕ�ƽ��ת����ʱ��
    public float bulletSpeed;// �ӵ��ٶ�
    public float flyDuration;
    public float radius = 10f; // ��������Բ�İ뾶
    private Coroutine moveCoroutine;
    [Header("״̬")]
    public bool isStart;
    private void Awake()
    {
        selfEnemyControl = GetComponent<EnemyControl>();
        selfEnemyStageControl = GetComponent<EnemyStageControl>();
        rb = GetComponent<Rigidbody2D>();
        health = 1000;
        stageLastTime = 40;

        //FIXME:�����ô���ǵ�ɾ
        rb.position = position1;
    }
    private void Update()
    {
        if (selfEnemyStageControl.currentStage != Stage.SecondStage) return;
        stageLastTime -= Time.deltaTime;
        if (stageLastTime <= 0 || health <= 0)
        {
            //�����ֹͣһ�׶ε�����Э��Ȼ��ת�׶�
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
        // ���ü���ָ���ɫλ��
        Vector2 direction = (PlayerStateManager.Instance.playerTransform.position - transform.position).normalized;
        //Debug.DrawRay(transform.position, direction * 1000, Color.blue, 100f);
        // ����ƫת����
        float angleOffset = sideOffsetAngle;
        Vector2 directionLeft = Quaternion.Euler(0, 0, angleOffset) * direction;
        Vector2 directionRight = Quaternion.Euler(0, 0, -angleOffset) * direction;
        float deflectionAngle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

        // ��������
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
        // ����ƫת�Ƕ���ת����
        laser.rotation = Quaternion.Euler(0, 0, angle - 90);

        RaycastHit2D hit = Physics2D.Raycast(transform.position, dir, Mathf.Infinity, LayerMask.GetMask("Ground"));
        // ���ƿɼ�������
        //Debug.DrawRay(transform.position, dir * 1000, Color.red,100f);
        if (hit.collider != null)
        {
            Debug.Log("find");
            Vector3 scale = laser.localScale;
            scale.y = hit.distance/redLaserLength; // �������ⳤ��
            laser.localScale = scale;
        }
    }
    private IEnumerator FireBullets(Vector2 dir, float Angle)
    {
        float elapsedTime = 0f;
        float count = 0;
        while (elapsedTime < rotationDuration)
        {
            // ���㵱ǰ�ķ���Ƕ�
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
            // ���㵱ǰ�ķ���Ƕ�
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
        //// ȷ�����һ���ӵ���������ƫ��20�ȵ�λ��
        //Vector2 finalDirection = RotateVector(dir, Angle);
        // FireBulletInDirection(finalDirection);
        yield break;
    }
    private void FireBulletInDirection(Vector2 direction)
    {
        // �����ӵ�ʵ��
        GameObject bullet = Instantiate(ScattingBulletPrefab, transform.position, Quaternion.identity);
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
    private IEnumerator GenerateShootingPointsCoroutine()
    {
        float angleStep = 90f / 6f; // ÿ��ĽǶȲ��� (90�� / 6 = 15��)
        float leftStartAngle = -225f; // ��ߵ���ʼ�Ƕ�
        float rightStartAngle = 45f; // �ұߵ���ʼ�Ƕ�

        // �������ߵ������
        for (int i = 0; i < 6; i++)
        {
            float angle = leftStartAngle + i * angleStep;
            Vector3 position = CalculatePosition(angle);
            InstantiateLaserPoint(position);    
            float angle2 = rightStartAngle - i * angleStep;
            Vector3 position2 = CalculatePosition(angle2);
            InstantiateLaserPoint(position2);
            yield return new WaitForSeconds(0.5f); // ����ӳ�
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
        // ���Ƕ�ת��Ϊ����
        float radian = angle * Mathf.Deg2Rad;

        // ��������������
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

            // ���Բ�ֵ���㵱ǰλ��
            Vector2 newPosition = Vector2.Lerp(startPosition, target, t);
            rb.MovePosition(newPosition);

            yield return null;
        }

        // ȷ�����徫ȷ����Ŀ���
        rb.MovePosition(target);
        yield break;
    }
}
