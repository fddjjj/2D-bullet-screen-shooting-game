using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoxColliderControl : MonoBehaviour
{
    BoxCollider2D boxCollider2D;
    bool isTrriggered = false;
    public GameObject XXXCanvas;
    private void Awake()
    {
        boxCollider2D = GetComponent<BoxCollider2D>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.CompareTag("Player") && !isTrriggered)
        {
            isTrriggered = true;
            TeachCanvasControl.Instance.gameObject.SetActive(true);
            if(TeachCanvasControl.Instance.currentGO != null )
            {
                TeachCanvasControl.Instance.currentGO.SetActive(false);
            }
            TeachCanvasControl.Instance.currentGO = XXXCanvas;
            TeachCanvasControl.Instance.StopPlayerControl();
            //Debug.Log("stop move");

            XXXCanvas.SetActive(true);
        }
    }
}
