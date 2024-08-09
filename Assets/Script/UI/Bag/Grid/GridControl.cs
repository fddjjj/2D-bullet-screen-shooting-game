using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public enum GridType {Equiped,Unequiped}
public class GridControl : MonoBehaviour,IPointerEnterHandler,IPointerExitHandler
{
    public Image chooseImage;
    public ItemControl itemControl;
    public GridType gridType;
    private void Awake()
    {
        //itemControl = GetComponentInChildren<ItemControl>();
    }
    public void UseItem()
    {
        if(itemControl.itemData != null)
        {
            if(itemControl.itemData.type == ItemType.Weapon)
            {
                BagCanvasControl.Instance.characterControl.currentAttackType = itemControl.itemData.attackData.attackTypes;
            }else if(itemControl.itemData.type == ItemType.Rise)
            {

            }
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        chooseImage.gameObject.SetActive(true);
        if(itemControl.itemData != null)
        {
            BagCanvasControl.Instance.RefreshDescribe(itemControl.itemData.itemName, itemControl.itemData.functionDescribe, itemControl.itemData.storyDescribe);
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        chooseImage?.gameObject.SetActive(false);
        BagCanvasControl.Instance.ClearDescribe();
    }
}
