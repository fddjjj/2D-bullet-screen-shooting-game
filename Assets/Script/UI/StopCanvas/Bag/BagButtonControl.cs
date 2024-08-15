using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BagButtonControl : MonoBehaviour
{
    Button button;
    private void Awake()
    {
        button = GetComponent<Button>();
        button.onClick.AddListener(OpenBag);
    }

    public void OpenBag()
    {
        BagCanvasControl.Instance.gameObject.SetActive(true);
    }
}
