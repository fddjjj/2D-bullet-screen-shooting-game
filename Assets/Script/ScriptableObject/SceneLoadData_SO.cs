using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(menuName ="SceneLoadData_SO")]
public class SceneLoadData_SO : ScriptableObject
{
    public UnityAction<string, Vector3,TransData> Action;
    public void RaiseAction(string sceneName,Vector3 playerStayPosition,TransData transData)
    {
        Action?.Invoke(sceneName, playerStayPosition,transData);
    }

}
