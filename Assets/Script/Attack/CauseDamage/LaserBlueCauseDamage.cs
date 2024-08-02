using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserBlueCauseDamage : CauseDamage
{
    public override void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy") && type != BulletOwner.Enemy)
        {
            collision.gameObject.GetComponent<EnemyStageControl>()?.GetHurt(damage);
        }
    }
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy") && type != BulletOwner.Enemy)
        {
            collision.gameObject.GetComponent<EnemyStageControl>()?.GetHurt(damage);
        }
    }
}
