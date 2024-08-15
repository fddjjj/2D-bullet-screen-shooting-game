using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class TranstionButtonControl : MonoBehaviour
{
    Button transtionButton;
    private void Awake()
    {
        transtionButton = GetComponent<Button>();
        transtionButton.onClick.AddListener(Transmission);
    }

    public void Transmission()
    {
        TransCanvasControl.Instance.gameObject.SetActive(false);
        Time.timeScale = 1f;
        PlayerStateManager.Instance.isstop = false;
        MainSceneManager.Instance.NeedLoadScene.RaiseAction(TransCanvasControl.Instance.currentTransData.TargetLocation, TransCanvasControl.Instance.currentTransData.TargetPosition);
    }
}
