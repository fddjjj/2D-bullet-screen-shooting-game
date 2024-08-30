using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(menuName = "SoundValueChangeEvent")]
public class SoundValueChange_SO : ScriptableObject
{
    public string mixer_GroupName;
    public UnityAction<string,float> action;

    public void RaiseAction(float amount)
    {
        action.Invoke(mixer_GroupName,amount);
    }
}
