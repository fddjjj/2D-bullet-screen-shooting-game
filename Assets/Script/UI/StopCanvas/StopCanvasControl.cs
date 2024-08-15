using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StopCanvasControl : SingleTon<StopCanvasControl>
{
    protected override void Awake()
    {
        base.Awake();
        gameObject.SetActive(false);
    }
    private void OnEnable()
    {
        BagCanvasControl.Instance.gameObject.SetActive(false);
    }
}
