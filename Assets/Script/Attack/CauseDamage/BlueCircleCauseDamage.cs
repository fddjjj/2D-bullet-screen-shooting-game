using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlueCircleCauseDamage : CauseDamage
{
    BlueCircleDestory blueCircleDestory;
    private void Awake()
    {
        blueCircleDestory = GetComponent<BlueCircleDestory>();
    }
    public override void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player") && type != BulletOwner.Player)
        {
            PlayerStateManager.Instance.GetHurt();
            blueCircleDestory.DestorySelf();
            //gameobject.setactive(false);
        }
    }
}
