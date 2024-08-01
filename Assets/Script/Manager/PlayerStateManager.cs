using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStateManager : SingleTon<PlayerStateManager>
{
    public float playHealth;
    public Transform playerTransform;
    public float playerPower;
    public bool isInvincible = false;
    public float InvincibleTime;
    public void GetHurt()
    {
        if (!isInvincible)
        {
            isInvincible = true;
            StartCoroutine(StopInvincible());
            playHealth --;
            //TODO:动画叠加虚化效果
            Debug.Log("PlayerHurt");
            if(playHealth < 0 )
            {
                //TODO:结束
            }
        }
    }

    IEnumerator StopInvincible()
    {
        yield return new WaitForSeconds(InvincibleTime);
        isInvincible = false;
    }
}

