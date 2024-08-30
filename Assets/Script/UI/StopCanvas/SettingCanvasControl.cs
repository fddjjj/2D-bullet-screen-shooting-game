using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SettingCanvasControl : SingleTon<SettingCanvasControl>
{
    protected override void Awake()
    {
        base.Awake();
        gameObject.SetActive(false);
    }
}
