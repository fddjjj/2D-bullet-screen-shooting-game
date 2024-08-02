using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RedLaserPointCauseDamage : CauseDamage
{
    public override void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player") && type != BulletOwner.Player)
        {
            PlayerStateManager.Instance.GetHurt();
            //gameObject.SetActive(false);
        }
    }
}
