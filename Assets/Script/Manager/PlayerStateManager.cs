using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStateManager : SingleTon<PlayerStateManager>
{
    [Header("组件")]
    public Transform playerTransform;
    public BulletSpawner playerBulletSpawner;
    public float playHealth;
    public float playerMaxHealth;
    public GameObject player;
    [Header("参数")]
    public float playerPower;
    public float playerMaxPower;
    public bool isInvincible = false;
    public float InvincibleTime;
    public bool isRecoverPower = true;
    public float powerRecoverSpeed;
    public float InvincibleTimer = 0f;
    public bool needRefresh = false;
    public bool isstop = false;
    public bool isCanTouch = false;
    private void OnEnable()
    {
        playHealth = playerMaxHealth;
        playerPower = playerMaxPower;
    }
    private void Update()
    {
        InvincibleTimer -= Time.deltaTime;
        if (InvincibleTimer < 0f && needRefresh)
            isInvincible = false;
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
            ResetInvincibleTimer(InvincibleTime);
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
    public void ResetInvincibleTimer(float t)
    {
        needRefresh = true;
        InvincibleTimer = t;
    }
}

