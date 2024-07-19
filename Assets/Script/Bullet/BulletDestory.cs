using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletDestory : MonoBehaviour
{

    private void OnBecameInvisible()
    {
        Destroy(gameObject);
    }

}
