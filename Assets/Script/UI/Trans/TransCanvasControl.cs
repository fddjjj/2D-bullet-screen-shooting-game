using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TransCanvasControl : SingleTon<TransCanvasControl>
{
    public TextMeshProUGUI describeText;
    public TextMeshProUGUI passStatText;
    public TextMeshProUGUI passTimeText;
    public TransData currentTransData;
    public Button TranstionButton;

    private void OnEnable()
    {
        describeText.text = string.Empty;
        passStatText.text = string.Empty;
        passTimeText.text = string.Empty;
        gameObject.SetActive(PlayerStateManager.Instance.isstop);
    }
    public void Refresh()
    {
        if(currentTransData != null)
        {
            describeText.text = currentTransData.Description;
            if (currentTransData.isPass)
                passStatText.text = "你过关";
            else 
                passStatText.text = "未通关";
            passTimeText.text = currentTransData.passTime_min.ToString() + ":" + currentTransData.passTime_s.ToString()+":"+currentTransData.passTime_ms.ToString();
            TranstionButton.gameObject.SetActive(true);
        }
    }

}
