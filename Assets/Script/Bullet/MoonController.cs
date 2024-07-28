using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class MoonController : MonoBehaviour
{
    public GameObject bulletPrefab;
    Rigidbody2D rb;
    private bool isChange = false;//确保球撞击地面后只改变一次速度
    private bool startShoot = false;
    public float shootCooldown;
    private float shootTimer = 0;
    public float bulletSpeed;
    public float bulletCount;
    public float radius;     // 发射子弹的半径
    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }
    void Update()
    {
        shootTimer += Time.deltaTime;
        if(startShoot && shootTimer >= shootCooldown)
        {
            shootTimer = 0;
            //TODO:shoot
            ShootBullets();
        }   
    }
    void OnTriggerEnter2D(Collider2D other)
    {
        if(other.gameObject.CompareTag("Ground")&& !isChange)
        {
            isChange = true;
            startShoot = true;
            rb.velocity = rb.velocity * 0.1f;
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Ground") && isChange)
            Destroy(gameObject);
    }
    void ShootBullets()
    {
        float angleStep = 360f / bulletCount;
        for (int i = 0; i < bulletCount; i++)
        {
            float angle = i * angleStep;
            float angleRad = angle * Mathf.Deg2Rad; // 将角度转换为弧度
            Vector3 spawnPosition = transform.position + Quaternion.Euler(0f, angle, 0f) * Vector3.forward * radius;
            // 实例化子弹
            GameObject bullet = Instantiate(bulletPrefab,spawnPosition, Quaternion.identity);
            Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();

            // 设置子弹速度
            if (rb != null)
            {
                Vector2 direction = new Vector2(Mathf.Cos(angleRad), Mathf.Sin(angleRad));
                rb.velocity = direction * bulletSpeed;
            }
        }
    }
}
