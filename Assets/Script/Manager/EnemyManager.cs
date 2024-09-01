using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : SingleTon<EnemyManager>
{
    //[SerializeField]
    public List<EnemyState> enemyStates = new List<EnemyState>();

    public Transform BossTransform;
    public Vector3 BossStartPosition;
    public EnemyStageControl BossStageControl;
    public bool HasBoss = false;
    public void AddEnemyState(EnemyState enemyState)
    {
        enemyStates.Add(enemyState);
    }
    public void removeEnemyState(EnemyState enemyState)
    {
        enemyStates.Remove(enemyState);
    }
    public Transform minDistanceOfEnemy(Transform bulletTransform)
    {
        float minDistance = 1000000f;
        Transform minTransform = null; 
        foreach (var state in enemyStates)
        {
            float distance = Vector3.Distance(state.enemyTransform.position, bulletTransform.position);
            if(distance < minDistance)
            {
                minDistance = distance;
                minTransform = state.enemyTransform;
            }  
        }
        return minTransform;
    }
}
