using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class EnemyHealthCanvasControl : SingleTon<EnemyHealthCanvasControl>
{
    public Image healthSlider;
    public TextMeshProUGUI enemyName;
    public Image enemyHead;
    public TextMeshProUGUI enemyStage;

    public void ChangeHealthSlider(float num)
    {
        healthSlider.fillAmount = num;
    }
    public void ChangeEnemyStage(int currentStage,int totalStage,string stageName)
    {
        enemyStage.text = "Stage   " + currentStage.ToString() + "/" + totalStage.ToString() + "   "+stageName;
    }
    public void ChangeEnemyHead(Sprite head)
    {
        enemyHead.sprite = head;
    }
}
