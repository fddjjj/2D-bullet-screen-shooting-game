using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ����������ű���
/// </summary>
public class LaserWeaponListener : MonoBehaviour
{
    #region ��Ա����
    [SerializeField][Tooltip("���ⷢ���ߵı�ǩ��")] public string brickTag = null;
    [SerializeField][Tooltip("���⹥��Ƶ�ʡ�")] public float attackRate = 0.1F;
    [SerializeField][Tooltip("���⹥����Χ��")] public float attackRange = 1F;
    [SerializeField][Tooltip("����prefab�����ʵ�ʳ��ȡ�")] public float length = 1F;

    private Vector3 ray = Vector3.zero;
    private Vector3 scale = Vector3.zero;
    private bool isAttacking = false;
    private float fadeOutRatio = 0.02F; // ���������󣬼����ȵ�����ϵ��
    private float countTime = 0;        // ����Ƶ�ʼ�ʱ��
    private LaserWeaponController laserWeaponController = null;
    #endregion

    #region ���Կ���
    /// <summary>
    /// �������
    /// </summary>
    public Vector3 Ray
    {
        get
        {
            return ray;
        }
        set
        {
            ray = value;
        }
    }

    /// <summary>
    /// �Ƿ��ڹ���״̬��
    /// </summary>
    public bool IsAttacking
    {
        get
        {
            return isAttacking;
        }
        set
        {
            isAttacking = value;
        }
    }

    /// <summary>
    /// ���ⷢ���߿�������
    /// </summary>
    public LaserWeaponController LaserWeaponController
    {
        get
        {
            return laserWeaponController;
        }
        set
        {
            laserWeaponController = value;
        }
    }
    #endregion

    #region ����˽�з���
    /// <summary>
    /// ��֡ˢ��ʱ������
    /// </summary>
    private void Update()
    {
        transform.position = laserWeaponController.gameObject.transform.position;	// �Ѽ��ⷢ��㶨�ڷ�����ԭ��
        if (isAttacking)
            Attacking();
        else
            DeAttacking();
        isAttacking = false;
    }

    /// <summary>
    /// ����������⵽����Ӵ�ʱ������
    /// </summary>
    /// <param name="collision">��ײ�������</param>
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (!CanCollide(collision.gameObject))
            return;
        if (Time.time > countTime)
            countTime = Time.time + attackRate;
        // ����ΪĿ������յ��˺��Ĵ��룬��
    }

    /// <summary>
    /// ������
    /// </summary>
    private void Attacking()
    {
        RaycastHit2D[] hits = Physics2D.RaycastAll(transform.position, ray, attackRange);
        List<RaycastHit2D> colliders = new List<RaycastHit2D>();
        for (int i = 0; i < hits.Length; i++)
            if (hits[i] && CanCollide(hits[i].transform.gameObject))
                colliders.Add(hits[i]);
        float distance = GetNearestHitDistance(colliders);
        scale = new Vector3(1, distance / length, 1);	// ѹ�������y�᳤��
        transform.localScale = scale;
    }

    /// <summary>
    /// ȥ������
    /// </summary>
    private void DeAttacking()
    {
        scale = new Vector3(scale.x - fadeOutRatio, scale.y, 1);
        transform.localScale = scale;
        if (scale.x <= 0)
            gameObject.SetActive(false);
    }

    /// <summary>
    /// �õ�������������ײ�Ķ����У���ײģ����С���߶γ��ȡ�
    /// </summary>
    /// <param name="hits">������ײ�����б�</param>
    /// <returns>��ײģ����С���߶γ��ȡ�</returns>
    private float GetNearestHitDistance(List<RaycastHit2D> hits)
    {
        float minDist = attackRange;
        for (int i = 0; i < hits.Count; i++)
            if (hits[i].distance < minDist)
                minDist = hits[i].distance;
        return minDist;
    }

    /// <summary>
    /// �жϵ�ǰ�����Ƿ��ܹ�����ײ�巢����ײ��
    /// </summary>
    /// <param name="collider">��ײ�����</param>
    /// <returns>true������ײ��false�򲻷�����</returns>
    private bool CanCollide(GameObject collider)
    {
        // ����ײ������Ǽ���ʱ
        if (collider.TryGetComponent(out LaserWeaponListener weaponListener))
        {
            // ͬԴ��������ײ
            if (brickTag == weaponListener.brickTag)
                return false;
        }
        else
        {
            // ��������ײ
            if (collider.tag == brickTag)
                return false;
        }
        return true;
    }
    #endregion
}

