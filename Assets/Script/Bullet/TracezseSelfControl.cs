using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TracezseSelfControl : MonoBehaviour
{
    Rigidbody2D rb;
    public float bulletSpeed;
    public float startControl;
    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }
    private void Update()
    {
        if(startControl > 0)
        {
            startControl -= Time.deltaTime;
        }else
        {
            Transform enemyTransform = EnemyManager.Instance.minDistanceOfEnemy(transform);
            if (enemyTransform != null)
            {
                Vector2 dir = (Vector2)(enemyTransform.position - transform.position).normalized;
                rb.velocity = dir * bulletSpeed;
            }
        }

    }
}
