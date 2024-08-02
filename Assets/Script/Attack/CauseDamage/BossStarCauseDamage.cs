using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossStarCauseDamage : CauseDamage
{
    BossStarDestory bossStarDestory;
    private void Awake()
    {
        bossStarDestory = GetComponent<BossStarDestory>();
    }
    public override void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player") && type != BulletOwner.Player)
        {
            PlayerStateManager.Instance.GetHurt();
            bossStarDestory.DestorySelf();
            //gameobject.setactive(false);
        }
        if(collision.gameObject.CompareTag("Shelter"))
        {
            bossStarDestory.DestorySelf();
        }
    }
}
