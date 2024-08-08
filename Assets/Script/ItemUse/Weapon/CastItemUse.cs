using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CastItemUse : ItemUse
{
    CharacterControl characterControl;

    public override void RiseUse()
    {
    }

    public override void WeaponUse()
    {
        GameObject obj =  GameObject.Find("StudentCharacter");
        characterControl = obj.GetComponent<CharacterControl>();
        if(characterControl != null )
        {
            characterControl.currentAttackType = AttackTypes.cast;
            Debug.Log("成功改变攻击类型");
        }
    }
}
