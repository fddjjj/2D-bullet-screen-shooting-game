using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum BulletOwner {Enemy,Player}
public abstract class CauseDamage : MonoBehaviour
{
    public float damage;
    public BulletOwner type;
    public abstract void OnTriggerEnter2D(Collider2D collision);
    //{
    //    //if (collision.gameObject.CompareTag("Player") && type != BulletOwner.Player)
    //    //{
    //    //    PlayerStateManager.Instance.GetHurt();
    //    //    //gameObject.SetActive(false);
    //    //}
    //    //if(collision.gameObject.CompareTag("Enemy") && type != BulletOwner.Enemy)
    //    //{
    //    //    collision.gameObject.GetComponent<EnemyStageControl>()?.GetHurt(damage);
    //    //    //gameObject.SetActive(false);
    //    //}
    //}
}
