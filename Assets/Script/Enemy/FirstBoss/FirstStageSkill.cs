using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class FirstStageSkill : MonoBehaviour
{
    [Header("组件")]
    public EnemyStageControl selfEnemyStageControl;
    public EnemyControl selfEnemyControl;
    public Vector3 position1;
    public Vector3 position2;
    public GameObject LaserRedPrefab;
    public GameObject laserPointPrefab;
    public GameObject ScattingBulletPrefab;
    //public Transform playerTransform;

    [Header("属性")]
    public float health;
    public float stageLastTime;
    public float sideOffsetAngle = 10f; // 左右偏移角度
    public float redLaserLength;
    public float LaserPointOffsetX;
    public float LaserPointOffsetY;
    [Header("状态")]
    public bool selectPosition = true;
    public bool isStart;
    private void Awake()
    {
        selfEnemyControl = GetComponent<EnemyControl>();
        selfEnemyStageControl = GetComponent<EnemyStageControl>();
        health = 1000;
        stageLastTime = 40;
    }
    private void Update()
    {
        if (selfEnemyStageControl.currentStage != Stage.FirstStage) return;
        stageLastTime -= Time.deltaTime;
        if (stageLastTime <= 0 || health <= 0)
        {
            //在这边停止一阶段的所有协程然后转阶段
            //    StopCoroutine();
        }
        else
        {
            if(!isStart)
            {
                //StartCoroutine();
                isStart = true;
                selectPosition = !selectPosition;
                StartCoroutine(SpawnLaserPoints());
                StartCoroutine(StartScatteringBullet());
            }
            
        }
    }

    public void OnEnter()
    {
        health = 1000;
        stageLastTime = 40;
    }
    IEnumerator SpawnLaserPoints()
    {
        Vector3 startPosition = transform.position;

        // 生成左侧的3个LaserPoint
        for (int i = 1; i < 4; i++)
        {
            Vector3 targetPosition = startPosition + new Vector3(-LaserPointOffsetX * i, LaserPointOffsetY * i, 0);
            InstantiateLaserPoint(targetPosition,150);
            Vector3 targetPosition1 = startPosition + new Vector3(LaserPointOffsetX * i, LaserPointOffsetY * i, 0);
            InstantiateLaserPoint(targetPosition1, 210);
            yield return new WaitForSeconds(0.5f); // 每个生成之间的延迟，可根据需要调整
        }
    }

    void InstantiateLaserPoint(Vector3 targetPosition,float deflectionAngle)
    {
        GameObject laserPoint = Instantiate(laserPointPrefab, transform.position, Quaternion.identity);
        LaserPointController controller = laserPoint.GetComponent<LaserPointController>();
        if (controller != null)
        {
            controller.targetPosition = targetPosition;
            controller.deflectionAngle = deflectionAngle;
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
        float angleOffset = 10f;
        Vector2 directionLeft = Quaternion.Euler(0, 0, angleOffset) * direction;
        Vector2 directionRight = Quaternion.Euler(0, 0, -angleOffset) * direction;
        float deflectionAngle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

        // 调整激光
        AdjustLaser(mainLaser.transform,direction);
        AdjustLaser(leftLaser.transform,directionLeft);
        AdjustLaser(rightLaser.transform,directionRight);
        yield return new WaitForSeconds(1f);
        Destroy(mainLaser);
        Destroy(leftLaser);
        Destroy(rightLaser);
        //isStart = false;
        yield break;
    }
    private void AdjustLaser(Transform laser,Vector2 dir)
    {
        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        // 根据偏转角度旋转激光
        laser.rotation = Quaternion.Euler(0, 0, angle - 90);

        RaycastHit2D hit = Physics2D.Raycast(transform.position,dir, Mathf.Infinity, LayerMask.GetMask("Ground"));
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
}
