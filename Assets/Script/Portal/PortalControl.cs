using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PortalControl : MonoBehaviour
{
    public GameObject ePress;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            ePress.SetActive(true);
            PlayerStateManager.Instance.isCanTouch = true;
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            ePress.SetActive(false);
            PlayerStateManager.Instance.isCanTouch = false;
        }

    }
}
