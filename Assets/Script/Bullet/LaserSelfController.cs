using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserSelfController : MonoBehaviour
{
    [SerializeField][Tooltip("激光发射者的标签。")] public string brickTag = null;
    [SerializeField][Tooltip("激光攻击频率。")] public float attackRate = 0.1F;
    [SerializeField][Tooltip("激光攻击范围。")] public float attackRange ;
    [SerializeField][Tooltip("激光prefab对象的实际长度。")] public float length ;

    public Vector3 ray = Vector3.zero;
    public Vector3 scale = Vector3.zero;
    public bool isAttacking = false;
   // private float fadeOutRatio = 0.02F; // 攻击结束后，激光宽度的缩减系数
    public float countTime = 0;        // 攻击频率计时器
   // public Transform shootPoint;

    void Update()
    {
        //transform.position = shootPoint.position;
        if (isAttacking)
            Attacking();
        else
            gameObject.SetActive(false);
    }
    private void Attacking()
    {
        RaycastHit2D[] hits = Physics2D.RaycastAll(transform.position, ray, attackRange);
        List<RaycastHit2D> colliders = new List<RaycastHit2D>();
        //Debug.Log(hits.Length);
        for (int i = 0; i < hits.Length; i++)
        {

            if (hits[i].collider.CompareTag("Enemy") )
            {

                colliders.Add(hits[i]);

                //Debug.Log("find");
            }
        }
        float distance = GetNearestHitDistance(colliders);
        scale = new Vector3(1, distance / length, 1);
        // 压缩激光的y轴长度
        transform.localScale = scale;
    }
    private float GetNearestHitDistance(List<RaycastHit2D> hits)
    {
        float minDist = attackRange;
        for (int i = 0; i < hits.Count; i++)
            if (hits[i].distance < minDist)
                minDist = hits[i].distance;
        return minDist;
    }
}
