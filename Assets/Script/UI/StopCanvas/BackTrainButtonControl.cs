using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BackTrainButtonControl : MonoBehaviour
{
    Button button;
    public TransData transData;
    private void Awake()
    {
        button = GetComponent<Button>();
        button.onClick.AddListener(BackTrain);
    }

    public void BackTrain()
    {
        StopCanvasControl.Instance.gameObject.SetActive(false);
        Time.timeScale = 1f;
        PlayerStateManager.Instance.isstop = false;
        MainSceneManager.Instance.NeedLoadScene.RaiseAction(transData.TargetLocation, transData.TargetPosition, transData);
    }
}
