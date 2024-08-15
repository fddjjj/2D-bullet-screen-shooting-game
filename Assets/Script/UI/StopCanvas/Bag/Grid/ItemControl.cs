using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemControl : MonoBehaviour
{
    public ItemTypeDefine itemData;
    public Image itemImage;

    private void Update()
    {
        if(itemData != null)
        {
            itemImage.gameObject.SetActive(true);
            itemImage.sprite = itemData.itemImage;
        }else
        {
            itemImage.gameObject.SetActive(false);
        }
    }
}
