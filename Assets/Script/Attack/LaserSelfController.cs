using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserSelfController : MonoBehaviour
{
    [SerializeField][Tooltip("���ⷢ���ߵı�ǩ��")] public string brickTag = null;
    [SerializeField][Tooltip("���⹥��Ƶ�ʡ�")] public float attackRate = 0.1F;
    [SerializeField][Tooltip("���⹥����Χ��")] public float attackRange = 1F;
    [SerializeField][Tooltip("����prefab�����ʵ�ʳ��ȡ�")] public float length = 1F;

    private Vector3 ray = Vector3.zero;
    private Vector3 scale = Vector3.zero;
    public bool isAttacking = false;
    private float fadeOutRatio = 0.02F; // ���������󣬼����ȵ�����ϵ��
    private float countTime = 0;        // ����Ƶ�ʼ�ʱ��
    public Transform shootPoint;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = shootPoint.position;
        if (isAttacking)
            Attacking();
        else
            DeAttacking();
    }
    private void Attacking()
    {
        RaycastHit2D[] hits = Physics2D.RaycastAll(transform.position, ray, attackRange);
        List<RaycastHit2D> colliders = new List<RaycastHit2D>();
        for (int i = 0; i < hits.Length; i++)
            if (hits[i] )
                colliders.Add(hits[i]);
        float distance = GetNearestHitDistance(colliders);
        scale = new Vector3(1, distance / length, 1);	// ѹ�������y�᳤��
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
    private void DeAttacking()
    {
        scale = new Vector3(scale.x - fadeOutRatio, scale.y, 1);
        transform.localScale = scale;
        if (scale.x <= 0)
            gameObject.SetActive(false);
    }
}
