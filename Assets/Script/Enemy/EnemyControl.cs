using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;


public class EnemyControl : MonoBehaviour
{
    public EnemyState selfState = new EnemyState();
    public Transform selfTransform;
    public string selfName;
    bool isSameDir = false;
    bool isLookAtRight = true;
    private void Awake()
    {
        selfTransform = GetComponent<Transform>();
        selfName = this.gameObject.name;
        //EnemyState State = new EnemyState();
        //selfState = State;

        //Debug.Log(selfName);
    }



    private void OnEnable()
    {
        selfState.enemyName = selfName;
        selfState.enemyTransform = selfTransform;
        EnemyManager.Instance.AddEnemyState(selfState);
        

    }
    public void OnDisable()
    {
        //Debug.Log(EnemyManager.Instance.enemyStates.Count);
        //EnemyManager.Instance.removeEnemyState(selfState);
        //Debug.Log(EnemyManager.Instance.enemyStates.Count);
    }
    private void Update()
    {
        if (selfTransform.position.x >= PlayerStateManager.Instance.playerTransform.position.x && isLookAtRight || selfTransform.position.x <= PlayerStateManager.Instance.playerTransform.position.x && !isLookAtRight)
            isSameDir = true;

        if (isSameDir)
        {
            selfTransform.localScale = new Vector3(-selfTransform.localScale.x, selfTransform.localScale.y, selfTransform.localScale.z);
            isSameDir = false;
            isLookAtRight = !isLookAtRight;
        }
            
    }

}
