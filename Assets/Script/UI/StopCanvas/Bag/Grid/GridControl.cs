using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public enum GridType {Equiped,Unequiped}
public class GridControl : MonoBehaviour,IPointerEnterHandler,IPointerExitHandler, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    public Image chooseImage;
    public ItemControl itemControl;
    public GridType gridType;
    public GameObject itemPrefab;
    public GameObject tmpItemDragObject;
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

    public void OnBeginDrag(PointerEventData eventData)
    {
        if (itemControl.itemData != null)
        {
            tmpItemDragObject = Instantiate(itemPrefab,BagCanvasControl.Instance.dragCanvas.transform);
            ItemControl tmpItemControl = tmpItemDragObject.GetComponent<ItemControl>();
            tmpItemControl.itemData = itemControl.itemData;
            BagCanvasControl.Instance.currentGridType = gridType;
        }
    }

    public void OnDrag(PointerEventData eventData)
    {
        if(tmpItemDragObject != null)
            tmpItemDragObject.transform.position = eventData.position;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (tmpItemDragObject == null)
            return;
        if (EventSystem.current.IsPointerOverGameObject())
        {
            if(BagCanvasControl.Instance.CheckInEquipedProperty(eventData.position) || BagCanvasControl.Instance.CheckInShootProperty(eventData.position)|| BagCanvasControl.Instance.CheckInSimplyProperty(eventData.position))
            {
                BagCanvasControl.Instance.targetGridType = eventData.pointerEnter.gameObject.GetComponent<GridControl>().gridType;
                if(BagCanvasControl.Instance.currentGridType == GridType.Unequiped && BagCanvasControl.Instance.targetGridType == GridType.Unequiped)
                {
                    Destroy(tmpItemDragObject);
                }else if (BagCanvasControl.Instance.currentGridType == GridType.Unequiped && BagCanvasControl.Instance.targetGridType == GridType.Equiped)
                {
                    ItemControl targetItemControl = eventData.pointerEnter.gameObject.GetComponent<GridControl>().itemControl;
                    targetItemControl.itemData = tmpItemDragObject.GetComponent<ItemControl>().itemData;
                    ItemTypeDefine  itemData = targetItemControl.itemData;
                    if (itemData.type == ItemType.Weapon)
                    {
                        BagCanvasControl.Instance.characterControl.currentAttackType = itemData.attackData.attackTypes;
                    }else if (itemData.type == ItemType.Rise)
                    {
                        itemData.useData.Use(itemData.useData);
                        HealthCanvasControl.Instance.RefreshHealth();
                    }
                    Destroy(tmpItemDragObject);
                }else if(BagCanvasControl.Instance.currentGridType == GridType.Equiped && BagCanvasControl.Instance.targetGridType == GridType.Unequiped)
                {
                    ItemTypeDefine itemData = itemControl.itemData;
                    //清除当前装备内容
                    if (itemData.type == ItemType.Weapon)
                    {
                        //BagCanvasControl.Instance.characterControl.currentAttackType = itemData.attackData.attackTypes;
                    }
                    else if (itemData.type == ItemType.Rise)
                    {
                        itemData.useData.UnUsed(itemData.useData);
                    }
                    itemControl.itemData = null;
                    Destroy(tmpItemDragObject);
                }else if(BagCanvasControl.Instance.currentGridType == GridType.Equiped && BagCanvasControl.Instance.targetGridType == GridType.Equiped)
                {
                    ItemControl targetItemControl = eventData.pointerEnter.gameObject.GetComponent<GridControl>().itemControl;
                    ItemTypeDefine tmpItemData = targetItemControl.itemData;
                    targetItemControl.itemData = tmpItemDragObject.GetComponent<ItemControl>().itemData;
                    itemControl.itemData = tmpItemData;
                    Destroy(tmpItemDragObject);
                }
            }
            else
            {
                Destroy(tmpItemDragObject);
            }
        }
        else
        {
            Destroy(tmpItemDragObject);
        }
    }
}
