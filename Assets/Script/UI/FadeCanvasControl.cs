using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FadeCanvasControl : SingleTon<FadeCanvasControl>
{
    public Image background;

    protected override void Awake()
    {
        base.Awake();
        gameObject.SetActive(false);
    }
}
