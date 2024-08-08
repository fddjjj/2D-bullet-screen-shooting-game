using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum ItemType {Weapon,Rise}
[CreateAssetMenu(menuName = "ItemData")]
public class ItemTypeDefine :ScriptableObject
{
    public Sprite itemImage;
    public int cost;
    public ItemType type;
    public ItemUse itemUse;
}
