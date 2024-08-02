using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TracezseBulletCauseDamage : CauseDamage
{
    TracezseBulletDestory tracezseBulletDestory;
    private void Awake()
    {
        tracezseBulletDestory = GetComponent<TracezseBulletDestory>();
    }
    public override void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy") && type != BulletOwner.Enemy)
        {
            collision.gameObject.GetComponent<EnemyStageControl>()?.GetHurt(damage);
            tracezseBulletDestory.DestorySelf();
        }
    }
}
