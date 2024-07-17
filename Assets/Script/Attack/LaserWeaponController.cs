using UnityEngine;

/// <summary>
/// 激光控制脚本类。
/// </summary>
public class LaserWeaponController : MonoBehaviour
{
    #region 成员变量
    [SerializeField][Tooltip("Prefab模板对象。")] public GameObject prefab = null;

    private GameObject laserObject = null;                      // 实际激光对象
   // private ObjectPool objectPool = ObjectPool.GetInstance();   // 此处使用了对象池来控制物体的生成
    #endregion

    #region 公有方法
    /// <summary>
    /// 向对应方向发射激光。
    /// </summary>
    /// <param name="ray">射击方向向量。</param>
    public void Attack(Vector3 ray)
    {
        // 激光武器只会实例化唯一的对象
        if (laserObject == null)
           // laserObject = objectPool.GetObject(prefab.name, transform.position, transform.rotation);
        laserObject.SetActive(true);
        // 初始化射击方向
        if (ray.y < 0)
            laserObject.transform.localEulerAngles = new Vector3(0, 0, Vector2.Angle(new Vector2(-1, 0), ray) + 90);
        else
            laserObject.transform.localEulerAngles = new Vector3(0, 0, Vector2.Angle(new Vector2(1, 0), ray) - 90);
        // 初始化监听器属性
        laserObject.GetComponent<LaserWeaponListener>().brickTag = gameObject.tag;
        laserObject.GetComponent<LaserWeaponListener>().Ray = ray;
        laserObject.GetComponent<LaserWeaponListener>().IsAttacking = true;
        laserObject.GetComponent<LaserWeaponListener>().LaserWeaponController = this;
    }
    #endregion

    #region 私有方法
    /// <summary>
    /// 在第一帧前触发
    /// </summary>
    private void Start()
    {
        // 此处采用了先构造对象模板，再通过对象池控制物体的生成和销毁。
        // 这种方法好处是，如果场景中存在多个角色发射激光，就不会反复的调用Destroy()方法，减少了计算开支
        prefab = Resources.Load("LASER") as GameObject;
    }

    /// <summary>
    /// 在帧刷新时触发。
    /// </summary>
    private void Update()
    {
        if (Input.GetMouseButton(0))
        {
            Vector3 ray = Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position;
            Attack(ray);
        }
    }

    /// <summary>
    /// 当脚本失效时触发。
    /// </summary>
    private void OnDisable()
    {
        try
        {
          //  objectPool.ReleaseObject(laserObject);
        }
        catch
        {
            Debug.Log("Object is already been destroyed!");
        }
    }
    #endregion

    #region 静态方法
    /// <summary>
    /// 获取归一化后的方向向量。
    /// </summary>
    /// <param name="ray">射线方向。</param>
    /// <returns>归一化后的二维向量。</returns>
    private static Vector2 NormalDirection(Vector2 ray)
    {
        float dist = Mathf.Sqrt(ray.x * ray.x + ray.y * ray.y);
        if (dist == 0)
            return Vector2.zero;
        return new Vector2(ray.x / dist, ray.y / dist);
    }
    #endregion
}

