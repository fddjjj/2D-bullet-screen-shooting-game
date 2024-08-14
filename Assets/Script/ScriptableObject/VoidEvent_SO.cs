using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(menuName ="VoidEvent_SO")]
public class VoidEvent_SO : ScriptableObject
{
    public UnityAction Action;

    public void RaiseAction()
    {
        Action?.Invoke();
    }
}
