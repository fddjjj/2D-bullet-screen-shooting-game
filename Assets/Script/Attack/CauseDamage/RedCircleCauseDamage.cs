using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RedCircleCauseDamage : CauseDamage
{
    RedCircleDestory redCircleDestory;
    private void Awake()
    {
        redCircleDestory = GetComponent<RedCircleDestory>();
    }
    public override void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player") && type != BulletOwner.Player)
        {
            PlayerStateManager.Instance.GetHurt();
            redCircleDestory.DestorySelf();
            //gameobject.setactive(false);
        }
    }

}
