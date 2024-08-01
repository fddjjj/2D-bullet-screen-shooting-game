using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Stage { FirstStage, SecondStage, ThirdStage, FourthStage, FifthStage, SixthStage, SeventhStage }
public class EnemyStageControl : MonoBehaviour
{
    public float EnemyHealth;
    public Stage currentStage;
    public FirstStageSkill firstStageSkill;
    public SecondStageSkill secondStageSkill;
    public ThirdStageSkill thirdStageSkill;
    public FourthStageSkill fourthStageSkill;
    public FifthStageSkill fifthStageSkill;
    public bool needRefresh = false;
    public bool isInvincible = false;
    public float InvincibleTime;
    private void Awake()
    {
        //currentStage = Stage.FirstStage;
        firstStageSkill = GetComponent<FirstStageSkill>();
        secondStageSkill = GetComponent<SecondStageSkill>();
        thirdStageSkill = GetComponent<ThirdStageSkill>();
        fourthStageSkill = GetComponent<FourthStageSkill>();
        fifthStageSkill = GetComponent<FifthStageSkill>();
        needRefresh = true;
    }
    void Update()
    {
        if(needRefresh)
        {
            switch (currentStage)
            {
                case Stage.FirstStage:
                    EnemyHealth = firstStageSkill.Maxhealth;
                    firstStageSkill.OnEnter();
                    break;
                case Stage.SecondStage:
                    EnemyHealth = secondStageSkill.Maxhealth;
                    secondStageSkill.OnEnter();
                    break;
                case Stage.ThirdStage:
                    EnemyHealth = thirdStageSkill.Maxhealth;
                    thirdStageSkill.OnEnter();
                    break;
                case Stage.FourthStage:
                    EnemyHealth = fourthStageSkill.Maxhealth;
                    fourthStageSkill.OnEnter();
                    break;
                case Stage.FifthStage:
                    EnemyHealth = fifthStageSkill.Maxhealth;
                    fifthStageSkill.OnEnter();
                    break;
                case Stage.SixthStage:
                    Debug.Log("Win");
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
        needRefresh = true;

    }

    public void GetHurt(float damage)
    {
        if (!isInvincible)
        {
            isInvincible = true;
            StartCoroutine(StopInvincible());
            EnemyHealth -= damage;
            Debug.Log("Hurt Enemy");
            //TODO:动画叠加虚化效果
        }
    }
    IEnumerator StopInvincible()
    {
        yield return new WaitForSeconds(InvincibleTime);
        isInvincible = false;
    }
}
