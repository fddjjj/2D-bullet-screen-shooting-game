using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "AttackData")]
public class AttackData_SO :ScriptableObject
{
    //[System.Serializable]
    public float attackCooldown;
    public float attackDamage;
    public float attackMultiply;
    public float attackAddDamage;
    public float bullerSpeed;

}
