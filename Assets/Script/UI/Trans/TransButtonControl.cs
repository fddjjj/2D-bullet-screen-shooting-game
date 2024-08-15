using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TransButtonControl : MonoBehaviour
{
    Button button;
    public TransData transData;
    private void Awake()
    {
        button = GetComponent<Button>();
        button.onClick.AddListener(Submit);
    }

    public void Submit()
    {
        TransCanvasControl.Instance.currentTransData = transData;
        TransCanvasControl.Instance.Refresh();
    }
}
