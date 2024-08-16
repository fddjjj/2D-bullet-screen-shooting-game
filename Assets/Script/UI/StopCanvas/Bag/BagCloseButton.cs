using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BagCloseButton : MonoBehaviour
{
    Button button;
    private void Awake()
    {
        button = GetComponent<Button>();
        button.onClick.AddListener(CloseBagCanvas);
    }

    public void CloseBagCanvas()
    {
        BagCanvasControl.Instance.gameObject.SetActive(false);
    }
}
