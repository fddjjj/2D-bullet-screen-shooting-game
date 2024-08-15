using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(menuName ="SceneLoadData_SO")]
public class SceneLoadData_SO : ScriptableObject
{
    public UnityAction<string, Vector3> Action;
    public void RaiseAction(string sceneName,Vector3 playerStayPosition)
    {
        Action?.Invoke(sceneName, playerStayPosition);
    }

}
