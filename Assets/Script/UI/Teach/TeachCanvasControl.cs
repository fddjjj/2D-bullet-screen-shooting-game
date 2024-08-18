using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeachCanvasControl : SingleTon<TeachCanvasControl>
{
    public GameObject MoveCanvas;
    public GameObject SlideCanvas;
    public GameObject FlyCanvas;
    public GameObject ESCCanvas;
    public GameObject currentGO;
    public PlayerInputControl inputs;
    protected override void Awake()
    {
        base.Awake();
        gameObject.SetActive(false);
        inputs = PlayerStateManager.Instance.playerBulletSpawner.gameObject.GetComponent<CharacterControl>().inputControl;
    }
    public void StopPlayerControl()
    {
        inputs.Player.Disable();
    }

    public void StartPlayerControl()
    {
        inputs.Player.Enable();
    }
}
