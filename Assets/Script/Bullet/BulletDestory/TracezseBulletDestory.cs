using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TracezseBulletDestory : BulletDestory
{
    public override void OnBecameInvisible()
    {
        base.OnBecameInvisible();
        DestorySelf();
    }
    public void DestorySelf()
    {
        ObjectPool.Instance.CollectObject(gameObject);
    }
}
