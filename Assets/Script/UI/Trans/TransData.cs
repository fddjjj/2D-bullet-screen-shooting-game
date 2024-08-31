using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName ="TransData")]
public class TransData : ScriptableObject
{
    public string TargetLocation;
    public Vector3 TargetPosition;
    [TextArea] public string Description;
    public bool isPass = false;
    public int passTime_min;
    public int passTime_s;
    public int passTime_ms;

    public bool hasBoss;
    public bool needPlayerHealth;
}
