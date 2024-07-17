using UnityEngine;
using UnityEngine.InputSystem;

public class BulletSpawner : MonoBehaviour
{
    public GameObject bulletScattingPrefab; // 子弹预制体
    public GameObject bulletCastPrefab;
    public Transform shootPoint; // 射击点
    public float bulletSpeed = 10f; // 子弹速度
    public CharacterControl playerState;
    public float attackCoolTime;
    public float attackDeltaTime;
        
    public AttackData_SO Scattering;
    public AttackData_SO Cast;
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
            switch(playerState.currentAttackType)
            {
                case AttackTypes.scattering:
                    attackCoolTime = Scattering.attackCooldown;
                    bulletSpeed = Scattering.bullerSpeed;
                    if(attackDeltaTime <=0)
                    {
                        FireScatteringBullets();
                        attackDeltaTime = attackCoolTime;
                    }
                        
                    break;
                case AttackTypes.cast:
                    attackCoolTime = Cast.attackCooldown;
                    bulletSpeed = Cast.bullerSpeed;
                    if(attackDeltaTime <= 0)
                    {
                        FireCastBullets();
                        attackDeltaTime = attackCoolTime;
                    }
                    break;
                case AttackTypes.laser: 
                    break;
                case AttackTypes.tracezse: 
                    break;
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
    }
    void FireCastBullets()
    {
        // 获取鼠标位置并转换为世界坐标
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue());
        mousePosition.z = 0;

        // 计算从射击点到鼠标位置的方向
        Vector2 direction = (mousePosition - shootPoint.position).normalized;

        // 发射中间指向鼠标位置的子弹
        SpawnCastBullet(shootPoint.position, direction);

    }

    void SpawnCastBullet(Vector2 position, Vector2 direction)
    {
        GameObject bullet = Instantiate(bulletCastPrefab, position, Quaternion.identity);
        Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();
        rb.velocity = direction * bulletSpeed;
    }
}
