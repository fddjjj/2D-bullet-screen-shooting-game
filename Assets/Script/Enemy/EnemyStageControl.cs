using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Stage { FirstStage, SecondStage, ThirdStage, FourthStage, FifthStage, SixthStage, SeventhStage }
public class EnemyStageControl : MonoBehaviour
{
    public Stage currentStage;
    public FirstStageSkill firstStageSkill;

    public bool needRefresh = false;
    private void Awake()
    {
        currentStage = Stage.FirstStage;
        firstStageSkill = GetComponent<FirstStageSkill>();
    }
    void Update()
    {
        if(needRefresh)
        {
            switch (currentStage)
            {
                case Stage.FirstStage:
                    firstStageSkill.OnEnter();
                    break;
                case Stage.SecondStage:
                    break;
                case Stage.ThirdStage:
                    break;
                case Stage.FourthStage: 
                    break;
                case Stage.FifthStage: 
                    break;
                case Stage.SixthStage: 
                    break;
                case Stage.SeventhStage: 
                    break;
                default:
                    break;     
            }
            needRefresh = false;
        }
       
    }


    public void ChangeStage(Stage stage)
    {
        currentStage = stage;
    }

}
