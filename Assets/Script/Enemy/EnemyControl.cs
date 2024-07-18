using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyControl : MonoBehaviour
{
    public EnemyState selfState = new EnemyState();
    public Transform selfTransform;
    public string selfName;
    private void Awake()
    {
        selfTransform = GetComponent<Transform>();
        selfName = this.gameObject.name;
        //EnemyState State = new EnemyState();
        //selfState = State;
        Debug.Log(selfName);
    }



    private void OnEnable()
    {
        selfState.enemyName = selfName;
        selfState.enemyTransform = selfTransform;
        EnemyManager.Instance.AddEnemyState(selfState);
    }
    public void OnDisable()
    {
        EnemyManager.Instance.removeEnemyState(selfState);
    }
}
