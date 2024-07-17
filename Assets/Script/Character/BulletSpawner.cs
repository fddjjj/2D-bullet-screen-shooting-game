using UnityEngine;
using UnityEngine.InputSystem;

public class BulletSpawner : MonoBehaviour
{
    public GameObject bulletScattingPrefab; // �ӵ�Ԥ����
    public GameObject bulletCastPrefab;
    public Transform shootPoint; // �����
    public float bulletSpeed = 10f; // �ӵ��ٶ�
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
    }
    void FireCastBullets()
    {
        // ��ȡ���λ�ò�ת��Ϊ��������
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue());
        mousePosition.z = 0;

        // ���������㵽���λ�õķ���
        Vector2 direction = (mousePosition - shootPoint.position).normalized;

        // �����м�ָ�����λ�õ��ӵ�
        SpawnCastBullet(shootPoint.position, direction);

    }

    void SpawnCastBullet(Vector2 position, Vector2 direction)
    {
        GameObject bullet = Instantiate(bulletCastPrefab, position, Quaternion.identity);
        Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();
        rb.velocity = direction * bulletSpeed;
    }
}
