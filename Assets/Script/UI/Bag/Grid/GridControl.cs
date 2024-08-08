using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
public class GridControl : MonoBehaviour,IPointerEnterHandler,IPointerExitHandler
{
    public Image chooseImage;

    public void OnPointerEnter(PointerEventData eventData)
    {
        chooseImage.gameObject.SetActive(true);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        chooseImage?.gameObject.SetActive(false);
    }
}
