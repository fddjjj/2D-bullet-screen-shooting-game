using UnityEngine;
using UnityEngine.InputSystem;

public class BulletSpawner : MonoBehaviour
{
    [Header("子弹预制体")]
    public GameObject bulletScattingPrefab; 
    public GameObject bulletCastPrefab;
    public GameObject bulletLaserPrefab;
    public GameObject bulletTracezsePrefab;
    [Header("子弹属性")]
    public Transform shootPoint; // 射击点
    public float bulletSpeed = 10f; // 子弹速度
    public CharacterControl playerState;
    public float attackCoolTime;
    public float attackDeltaTime;

    [Header("子弹控制参数")]
    public AttackData_SO Scattering;
    public AttackData_SO Cast;
    public AttackData_SO Tracezse;
    public AttackData_SO Laser;

    public GameObject laserObject = null;//实际激光
    public LaserSelfController laserController = null;
    private void Awake()
    {
        playerState =  GetComponent<CharacterControl>();
        attackDeltaTime = 0;
    }
    void Update()
    {
        attackDeltaTime -= Time.deltaTime;
        if (playerState.isShooting)
        {
            switch (playerState.currentAttackType)
            {
                case AttackTypes.scattering:
                    attackCoolTime = Scattering.attackCooldown;
                    bulletSpeed = Scattering.bullerSpeed;
                    if (attackDeltaTime <=0)
                    {
                        FireScatteringBullets();
                        attackDeltaTime = attackCoolTime;
                    }

                    break;
                case AttackTypes.cast:
                    attackCoolTime = Cast.attackCooldown;
                    bulletSpeed = Cast.bullerSpeed;
                    if (attackDeltaTime <= 0)
                    {
                        FireCastBullets();
                        attackDeltaTime = attackCoolTime;
                    }
                    break;
                case AttackTypes.laser:
                    if (laserObject == null)
                        laserObject = Instantiate(bulletLaserPrefab, shootPoint.position, Quaternion.identity);
                    laserController = laserObject.GetComponent<LaserSelfController>();
                    CauseDamage cd = laserObject.GetComponent<CauseDamage>();
                    cd.damage = Laser.attackDamage;
                    FireLaserBullets();
                    break;
                case AttackTypes.tracezse:
                    attackCoolTime = Tracezse.attackCooldown;
                    bulletSpeed = Tracezse.bullerSpeed;
                    if(attackDeltaTime <= 0)
                    {
                        FireTracezseBullets();
                        attackDeltaTime = attackCoolTime;
                    }
                    break;
            }
        }
        else
        {
            if (laserObject != null)
            {
                laserObject.SetActive(false);
            }
        }
    }

    void FireScatteringBullets()
    {
        // 获取鼠标位置并转换为世界坐标
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue());
        mousePosition.z = 0;

        // 计算从射击点到鼠标位置的方向
        Vector2 direction = (mousePosition - shootPoint.position).normalized;

        // 发射中间指向鼠标位置的子弹
        SpawnScatteringBullet(shootPoint.position, direction);

        // 计算偏转方向
        float angleOffset = 10f;
        Vector2 directionLeft = Quaternion.Euler(0, 0, angleOffset) * direction;
        Vector2 directionRight = Quaternion.Euler(0, 0, -angleOffset) * direction;

        // 发射左偏10度和右偏10度的子弹
        SpawnScatteringBullet(shootPoint.position, directionLeft);
        SpawnScatteringBullet(shootPoint.position, directionRight);
    }

    void SpawnScatteringBullet(Vector2 position, Vector2 direction)
    {
        GameObject bullet = Instantiate(bulletScattingPrefab, position, Quaternion.identity);
        Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();
        rb.velocity = direction * bulletSpeed;
        CauseDamage cd = bullet.GetComponent<CauseDamage>();
        cd.damage = Scattering.attackDamage;
    }
    void FireCastBullets()
    {
        // 获取鼠标位置并转换为世界坐标
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue());
        mousePosition.z = 0;
        Vector2 direction = (mousePosition - shootPoint.position).normalized;

        // 发射中间指向鼠标位置的子弹
        SpawnCastBullet(shootPoint.position, direction);

    }

    void SpawnCastBullet(Vector2 position, Vector2 direction)
    {
        GameObject bullet = Instantiate(bulletCastPrefab, position, Quaternion.identity);
        Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();
        rb.velocity = direction * bulletSpeed;
        CauseDamage cd = bullet.GetComponent<CauseDamage>();
        cd.damage = Cast.attackDamage;
    }

    void FireLaserBullets()
    {
        laserObject.SetActive(true);
        // 获取鼠标位置并转换为世界坐标
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue());
        mousePosition.z = 0;
        // 计算从射击点到鼠标位置的方向
        Vector3 direction = (mousePosition - shootPoint.position).normalized;
        // 计算指向方向与竖直向上的夹角
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        // 设置激光的位置和旋转
        laserObject.transform.position = shootPoint.position;
        laserController.ray = direction;
        laserObject.transform.rotation = Quaternion.Euler(0, 0, angle - 90); // 减去90度，使0度旋转竖直向上
        laserController.isAttacking = playerState.isShooting;
        
    }
    void FireTracezseBullets()
    {
        // 获取鼠标位置并转换为世界坐标
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue());
        mousePosition.z = 0;
        // 计算从射击点到鼠标位置的方向
        Vector2 direction = (mousePosition - shootPoint.position).normalized;

        // 发射中间指向鼠标位置的子弹
        SpawnTracezseBullet(shootPoint.position, direction);

        // 计算偏转方向
        float angleOffset = 30f;
        Vector2 directionLeft = Quaternion.Euler(0, 0, angleOffset) * direction;
        Vector2 directionRight = Quaternion.Euler(0, 0, -angleOffset) * direction;

        // 发射左偏10度和右偏10度的子弹
        SpawnTracezseBullet(shootPoint.position, directionLeft);
        SpawnTracezseBullet(shootPoint.position, directionRight);
    }

    void SpawnTracezseBullet(Vector2 position, Vector2 direction)
    {
        GameObject bullet = Instantiate(bulletTracezsePrefab, position, Quaternion.identity);
        bullet.GetComponent<TracezseSelfControl>().bulletSpeed = bulletSpeed;
        bullet.GetComponent<TracezseSelfControl>().startControl = 0.5f;
        Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();
        rb.velocity = direction * bulletSpeed * 2;
        CauseDamage cd = bullet.GetComponent<CauseDamage>();
        cd.damage = Tracezse.attackDamage;
    }
}
