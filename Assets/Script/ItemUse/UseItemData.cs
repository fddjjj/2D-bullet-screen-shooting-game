using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "UseItemData")]
public class UseItemData :ScriptableObject
{
    public int healthChange;
    public float baseDamageChange;
    public float baseDamageMulChange;
    public float baseSpeedChange;
        public void Use(UseItemData data)
    {
        if (data.healthChange != 0)
        {
            PlayerStateManager.Instance.playerMaxHealth += data.healthChange;
        }
        if (data.baseDamageChange != 0)
        {
            PlayerStateManager.Instance.playerBulletSpawner.equipmentDamage += data.baseDamageChange;
        }
    }
    public void UnUsed(UseItemData data)
    {
        if (data.healthChange != 0)
        {
            PlayerStateManager.Instance.playerMaxHealth -= data.healthChange;
        }
        if (data.baseDamageChange != 0)
        {
            PlayerStateManager.Instance.playerBulletSpawner.equipmentDamage -= data.baseDamageChange;
        }
    }
}
