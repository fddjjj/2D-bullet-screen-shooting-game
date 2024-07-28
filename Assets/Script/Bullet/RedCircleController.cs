using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RedCircleController : MonoBehaviour
{
    bool isChange = false;
    public float checkRadius;
    Rigidbody2D rb;
    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }
    void Update()
    {
        if(Vector3.Distance(PlayerStateManager.Instance.playerTransform.position,transform.position) <= checkRadius && !isChange)
        {
            rb.velocity = rb.velocity * 0.1f;
        }

    }
}
