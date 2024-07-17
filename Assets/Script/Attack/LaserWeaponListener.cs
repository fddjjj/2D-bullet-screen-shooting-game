using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 激光监听器脚本。
/// </summary>
public class LaserWeaponListener : MonoBehaviour
{
    #region 成员变量
    [SerializeField][Tooltip("激光发射者的标签。")] public string brickTag = null;
    [SerializeField][Tooltip("激光攻击频率。")] public float attackRate = 0.1F;
    [SerializeField][Tooltip("激光攻击范围。")] public float attackRange = 1F;
    [SerializeField][Tooltip("激光prefab对象的实际长度。")] public float length = 1F;

    private Vector3 ray = Vector3.zero;
    private Vector3 scale = Vector3.zero;
    private bool isAttacking = false;
    private float fadeOutRatio = 0.02F; // 攻击结束后，激光宽度的缩减系数
    private float countTime = 0;        // 攻击频率计时器
    private LaserWeaponController laserWeaponController = null;
    #endregion

    #region 属性控制
    /// <summary>
    /// 射击方向。
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
    /// 是否处于攻击状态。
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
    /// 激光发射者控制器。
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

    #region 基础私有方法
    /// <summary>
    /// 在帧刷新时触发。
    /// </summary>
    private void Update()
    {
        transform.position = laserWeaponController.gameObject.transform.position;	// 把激光发射点定在发射者原点
        if (isAttacking)
            Attacking();
        else
            DeAttacking();
        isAttacking = false;
    }

    /// <summary>
    /// 当触发器检测到物理接触时触发。
    /// </summary>
    /// <param name="collision">碰撞物体对象。</param>
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (!CanCollide(collision.gameObject))
            return;
        if (Time.time > countTime)
            countTime = Time.time + attackRate;
        // 以下为目标对象收到伤害的代码，略
    }

    /// <summary>
    /// 攻击。
    /// </summary>
    private void Attacking()
    {
        RaycastHit2D[] hits = Physics2D.RaycastAll(transform.position, ray, attackRange);
        List<RaycastHit2D> colliders = new List<RaycastHit2D>();
        for (int i = 0; i < hits.Length; i++)
            if (hits[i] && CanCollide(hits[i].transform.gameObject))
                colliders.Add(hits[i]);
        float distance = GetNearestHitDistance(colliders);
        scale = new Vector3(1, distance / length, 1);	// 压缩激光的y轴长度
        transform.localScale = scale;
    }

    /// <summary>
    /// 去攻击。
    /// </summary>
    private void DeAttacking()
    {
        scale = new Vector3(scale.x - fadeOutRatio, scale.y, 1);
        transform.localScale = scale;
        if (scale.x <= 0)
            gameObject.SetActive(false);
    }

    /// <summary>
    /// 得到所有与射线碰撞的对象中，碰撞模长最小的线段长度。
    /// </summary>
    /// <param name="hits">射线碰撞对象列表。</param>
    /// <returns>碰撞模长最小的线段长度。</returns>
    private float GetNearestHitDistance(List<RaycastHit2D> hits)
    {
        float minDist = attackRange;
        for (int i = 0; i < hits.Count; i++)
            if (hits[i].distance < minDist)
                minDist = hits[i].distance;
        return minDist;
    }

    /// <summary>
    /// 判断当前对象是否能够与碰撞体发生碰撞。
    /// </summary>
    /// <param name="collider">碰撞体对象。</param>
    /// <returns>true则发生碰撞，false则不发生。</returns>
    private bool CanCollide(GameObject collider)
    {
        // 当碰撞体对象是激光时
        if (collider.TryGetComponent(out LaserWeaponListener weaponListener))
        {
            // 同源武器不碰撞
            if (brickTag == weaponListener.brickTag)
                return false;
        }
        else
        {
            // 己方不碰撞
            if (collider.tag == brickTag)
                return false;
        }
        return true;
    }
    #endregion
}

