using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CastBulletCauseDamage : CauseDamage
{
    CastBulletDestory castBulletDestory;
    private void Awake()
    {
        castBulletDestory = GetComponent<CastBulletDestory>();
    }
    public override void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy") && type != BulletOwner.Enemy)
        {
            collision.gameObject.GetComponent<EnemyStageControl>()?.GetHurt(damage);
            castBulletDestory.DestorySelf();
        }
    }
}
