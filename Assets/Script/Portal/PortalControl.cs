using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PortalControl : MonoBehaviour
{
    public GameObject ePress;
    public bool isCanTouch = false;
    public PlayerInputControl inputControl;
    private void Awake()
    {
        inputControl = new PlayerInputControl();
        inputControl.UI.E.started += Trans;
    }

    private void Trans(InputAction.CallbackContext context)
    {
        if (isCanTouch)
        {
            //UI Open
        }
    }

    private void Update()
    {
        
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        ePress.SetActive(true);
        isCanTouch = true;
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        ePress.SetActive(false);
        isCanTouch = false;
    }
}
