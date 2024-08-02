using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StraightLineBulletCauseDamage :CauseDamage
{
    StraightLineBulletDestory straightLineBulletDestory;

    private void Awake()
    {
        straightLineBulletDestory = GetComponent<StraightLineBulletDestory>();
    }
    public override void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy") && type != BulletOwner.Enemy)
        {
            collision.gameObject.GetComponent<EnemyStageControl>()?.GetHurt(damage);
            straightLineBulletDestory.DestorySelf();
        }
    }
}
