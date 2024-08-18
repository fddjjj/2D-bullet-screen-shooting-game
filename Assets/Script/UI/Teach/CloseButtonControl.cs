using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CloseButtonControl : MonoBehaviour
{
    Button button;
    private void Awake()
    {
        button = GetComponent<Button>();
        button.onClick.AddListener(CloseCanvas);
    }

    public void CloseCanvas()
    {
        TeachCanvasControl.Instance.currentGO.SetActive(false);
        TeachCanvasControl.Instance.StartPlayerControl();
        TeachCanvasControl.Instance.gameObject.SetActive(false);
    }
}
