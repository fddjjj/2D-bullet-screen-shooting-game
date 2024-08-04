using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStateManager : SingleTon<PlayerStateManager>
{
    public float playHealth;
    public float playerMaxHealth;
    public Transform playerTransform;
    public float playerPower;
    public float playerMaxPower;
    public bool isInvincible = false;
    public float InvincibleTime;
    public bool isRecoverPower = true;
    public float powerRecoverSpeed;
    private void OnEnable()
    {
        playHealth = playerMaxHealth;
        playerPower = playerMaxPower;
    }
    private void Update()
    {
        if (isRecoverPower)
        {
            if(playerPower < playerMaxPower)
            {
                playerPower += powerRecoverSpeed * Time.deltaTime;
            }
        }
    }
    public void GetHurt()
    {
        if (!isInvincible)
        {
            isInvincible = true;
            StartCoroutine(StopInvincible());
            playHealth --;
            HealthCanvasControl.Instance.RefreshHealth();
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

