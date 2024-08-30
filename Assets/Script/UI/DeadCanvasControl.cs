using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DeadCanvasControl : SingleTon<DeadCanvasControl>
{
    public TransData transData;
    public Button BackButton;
    public Button RefightButton;

    protected override void Awake()
    {
        base.Awake();
        gameObject.SetActive(false);
        RefightButton.onClick.AddListener(RefightClick);
        BackButton.onClick.AddListener(TransStopBoss);
    }
    public void RefightClick()
    {
        //PlayerStateManager.Instance.playerControl.StopPlayer();
        //TODO: 渐入渐出
        //更新人物位置和状态
        PlayerStateManager.Instance.player.GetComponent<Rigidbody2D>().position = MainSceneManager.Instance.playerStayPosition;
        PlayerStateManager.Instance.playHealth = PlayerStateManager.Instance.playerMaxHealth;
        PlayerStateManager.Instance.playerPower = PlayerStateManager.Instance.playerMaxPower;
        HealthCanvasControl.Instance.RefreshHealth();

        //更新boss位置
        EnemyManager.Instance.BossTransform.gameObject.GetComponent<Rigidbody2D>().position = EnemyManager.Instance.BossStartPosition;
        EnemyManager.Instance.BossStageControl.StopStage(EnemyManager.Instance.BossStageControl.currentStage);
        EnemyManager.Instance.BossStageControl.ChangeStage(Stage.FirstStage);

        PlayerStateManager.Instance.playerControl.inputControl.Player.Enable();
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
