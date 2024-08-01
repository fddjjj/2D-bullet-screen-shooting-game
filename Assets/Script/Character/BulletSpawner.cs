using UnityEngine;
using UnityEngine.InputSystem;

public class BulletSpawner : MonoBehaviour
{
    [Header("�ӵ�Ԥ����")]
    public GameObject bulletScattingPrefab; 
    public GameObject bulletCastPrefab;
    public GameObject bulletLaserPrefab;
    public GameObject bulletTracezsePrefab;
    [Header("�ӵ�����")]
    public Transform shootPoint; // �����
    public float bulletSpeed = 10f; // �ӵ��ٶ�
    public CharacterControl playerState;
    public float attackCoolTime;
    public float attackDeltaTime;

    [Header("�ӵ����Ʋ���")]
    public AttackData_SO Scattering;
    public AttackData_SO Cast;
    public AttackData_SO Tracezse;
    public AttackData_SO Laser;

    public GameObject laserObject = null;//ʵ�ʼ���
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
        // ��ȡ���λ�ò�ת��Ϊ��������
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue());
        mousePosition.z = 0;

        // ���������㵽���λ�õķ���
        Vector2 direction = (mousePosition - shootPoint.position).normalized;

        // �����м�ָ�����λ�õ��ӵ�
        SpawnScatteringBullet(shootPoint.position, direction);

        // ����ƫת����
        float angleOffset = 10f;
        Vector2 directionLeft = Quaternion.Euler(0, 0, angleOffset) * direction;
        Vector2 directionRight = Quaternion.Euler(0, 0, -angleOffset) * direction;

        // ������ƫ10�Ⱥ���ƫ10�ȵ��ӵ�
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
        // ��ȡ���λ�ò�ת��Ϊ��������
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue());
        mousePosition.z = 0;
        Vector2 direction = (mousePosition - shootPoint.position).normalized;

        // �����м�ָ�����λ�õ��ӵ�
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
        // ��ȡ���λ�ò�ת��Ϊ��������
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue());
        mousePosition.z = 0;
        // ���������㵽���λ�õķ���
        Vector3 direction = (mousePosition - shootPoint.position).normalized;
        // ����ָ��������ֱ���ϵļн�
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        // ���ü����λ�ú���ת
        laserObject.transform.position = shootPoint.position;
        laserController.ray = direction;
        laserObject.transform.rotation = Quaternion.Euler(0, 0, angle - 90); // ��ȥ90�ȣ�ʹ0����ת��ֱ����
        laserController.isAttacking = playerState.isShooting;
        
    }
    void FireTracezseBullets()
    {
        // ��ȡ���λ�ò�ת��Ϊ��������
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue());
        mousePosition.z = 0;
        // ���������㵽���λ�õķ���
        Vector2 direction = (mousePosition - shootPoint.position).normalized;

        // �����м�ָ�����λ�õ��ӵ�
        SpawnTracezseBullet(shootPoint.position, direction);

        // ����ƫת����
        float angleOffset = 30f;
        Vector2 directionLeft = Quaternion.Euler(0, 0, angleOffset) * direction;
        Vector2 directionRight = Quaternion.Euler(0, 0, -angleOffset) * direction;

        // ������ƫ10�Ⱥ���ƫ10�ȵ��ӵ�
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
