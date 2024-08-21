using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class StopCanvasControl : SingleTon<StopCanvasControl>
{
    public TransData transData;
    public Button TransButton;
    public Button RefightButton;
    public Button SettingButton;
    public Button QuitButton;
    protected override void Awake()
    {
        base.Awake();
        gameObject.SetActive(false);
        RefightButton.onClick.AddListener(RefightClick);
        TransButton.onClick.AddListener(TransStopBoss);
    }
    private void OnEnable()
    {
        BagCanvasControl.Instance.gameObject.SetActive(false);

    }

    public void RefightClick()
    {
        //PlayerStateManager.Instance.playerControl.StopPlayer();
        //TODO: ½¥Èë½¥³ö
        PlayerStateManager.Instance.player.GetComponent<Rigidbody2D>().position = MainSceneManager.Instance.playerStayPosition;
        PlayerStateManager.Instance.playHealth = PlayerStateManager.Instance.playerMaxHealth;
        PlayerStateManager.Instance.playerPower = PlayerStateManager.Instance.playerMaxPower;

        EnemyManager.Instance.BossTransform.gameObject.GetComponent<Rigidbody2D>().position = EnemyManager.Instance.BossStartPosition;

        EnemyManager.Instance.BossStageControl.StopStage(EnemyManager.Instance.BossStageControl.currentStage);
        //EnemyManager.Instance.BossStageControl.currentStage = Stage.FirstStage;
        //EnemyManager.Instance.BossStageControl.needRefresh = true;
        EnemyManager.Instance.BossStageControl.ChangeStage(Stage.FirstStage);
        PlayerStateManager.Instance.playerControl.inputControl.Player.Enable();
        //BagCanvasControl.Instance.gameObject.SetActive(false);
        gameObject.SetActive(false);
        Time.timeScale = 1f;
        PlayerStateManager.Instance.isstop = false;
    }
    public void TransStopBoss()
    {
        if (EnemyManager.Instance.HasBoss)
        {
            EnemyManager.Instance.BossStageControl.StopStage(EnemyManager.Instance.BossStageControl.currentStage);
            //ObjectPool.Instance.ClearAll();
        }
        EnemyManager.Instance.HasBoss = false;
        gameObject.SetActive(false);
        Time.timeScale = 1f;
        PlayerStateManager.Instance.isstop = false;
        MainSceneManager.Instance.NeedLoadScene.RaiseAction(transData.TargetLocation, transData.TargetPosition);
    }
}
