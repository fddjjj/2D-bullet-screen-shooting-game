using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(menuName = "FloatEvent")]
public class FloatEvent_SO : ScriptableObject
{
    public UnityAction<float> action;
    public void RaiseAction(float amount)
    {
        action.Invoke(amount);
    }
}
