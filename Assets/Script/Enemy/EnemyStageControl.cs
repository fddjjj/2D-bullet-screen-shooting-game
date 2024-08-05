using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Stage { FirstStage, SecondStage, ThirdStage, FourthStage, FifthStage, SixthStage, SeventhStage }
public class EnemyStageControl : MonoBehaviour
{
    public int totalStage;
    public float currentStageEnemyMaxHealth;
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
                    currentStageEnemyMaxHealth = EnemyHealth;
                    EnemyHealthCanvasControl.Instance.ChangeHealthSlider(EnemyHealth/currentStageEnemyMaxHealth);
                    EnemyHealthCanvasControl.Instance.ChangeEnemyStage(1, totalStage, "Skill1");
                    firstStageSkill.OnEnter();
                    break;
                case Stage.SecondStage:
                    EnemyHealth = secondStageSkill.Maxhealth;
                    currentStageEnemyMaxHealth = EnemyHealth;
                    EnemyHealthCanvasControl.Instance.ChangeHealthSlider(EnemyHealth/currentStageEnemyMaxHealth);
                    EnemyHealthCanvasControl.Instance.ChangeEnemyStage(2, totalStage, "Skill2");
                    secondStageSkill.OnEnter();
                    break;
                case Stage.ThirdStage:
                    EnemyHealth = thirdStageSkill.Maxhealth;
                    currentStageEnemyMaxHealth = EnemyHealth;
                    EnemyHealthCanvasControl.Instance.ChangeHealthSlider(EnemyHealth/currentStageEnemyMaxHealth);
                    EnemyHealthCanvasControl.Instance.ChangeEnemyStage(3, totalStage, "Skill3");
                    thirdStageSkill.OnEnter();
                    break;
                case Stage.FourthStage:
                    EnemyHealth = fourthStageSkill.Maxhealth;
                    currentStageEnemyMaxHealth = EnemyHealth;
                    EnemyHealthCanvasControl.Instance.ChangeHealthSlider(EnemyHealth/currentStageEnemyMaxHealth);
                    EnemyHealthCanvasControl.Instance.ChangeEnemyStage(4, totalStage, "Skill4");
                    fourthStageSkill.OnEnter();
                    break;
                case Stage.FifthStage:
                    EnemyHealth = fifthStageSkill.Maxhealth;
                    currentStageEnemyMaxHealth = EnemyHealth;
                    EnemyHealthCanvasControl.Instance.ChangeHealthSlider(EnemyHealth/currentStageEnemyMaxHealth);
                    EnemyHealthCanvasControl.Instance.ChangeEnemyStage(5, totalStage, "Skill5");
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
            EnemyHealthCanvasControl.Instance.ChangeHealthSlider(EnemyHealth/currentStageEnemyMaxHealth);
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
