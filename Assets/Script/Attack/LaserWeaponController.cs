using UnityEngine;

/// <summary>
/// ������ƽű��ࡣ
/// </summary>
public class LaserWeaponController : MonoBehaviour
{
    #region ��Ա����
    [SerializeField][Tooltip("Prefabģ�����")] public GameObject prefab = null;

    private GameObject laserObject = null;                      // ʵ�ʼ������
   // private ObjectPool objectPool = ObjectPool.GetInstance();   // �˴�ʹ���˶�������������������
    #endregion

    #region ���з���
    /// <summary>
    /// ���Ӧ�����伤�⡣
    /// </summary>
    /// <param name="ray">�������������</param>
    public void Attack(Vector3 ray)
    {
        // ��������ֻ��ʵ����Ψһ�Ķ���
        if (laserObject == null)
           // laserObject = objectPool.GetObject(prefab.name, transform.position, transform.rotation);
        laserObject.SetActive(true);
        // ��ʼ���������
        if (ray.y < 0)
            laserObject.transform.localEulerAngles = new Vector3(0, 0, Vector2.Angle(new Vector2(-1, 0), ray) + 90);
        else
            laserObject.transform.localEulerAngles = new Vector3(0, 0, Vector2.Angle(new Vector2(1, 0), ray) - 90);
        // ��ʼ������������
        laserObject.GetComponent<LaserWeaponListener>().brickTag = gameObject.tag;
        laserObject.GetComponent<LaserWeaponListener>().Ray = ray;
        laserObject.GetComponent<LaserWeaponListener>().IsAttacking = true;
        laserObject.GetComponent<LaserWeaponListener>().LaserWeaponController = this;
    }
    #endregion

    #region ˽�з���
    /// <summary>
    /// �ڵ�һ֡ǰ����
    /// </summary>
    private void Start()
    {
        // �˴��������ȹ������ģ�壬��ͨ������ؿ�����������ɺ����١�
        // ���ַ����ô��ǣ���������д��ڶ����ɫ���伤�⣬�Ͳ��ᷴ���ĵ���Destroy()�����������˼��㿪֧
        prefab = Resources.Load("LASER") as GameObject;
    }

    /// <summary>
    /// ��֡ˢ��ʱ������
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
    /// ���ű�ʧЧʱ������
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

    #region ��̬����
    /// <summary>
    /// ��ȡ��һ����ķ���������
    /// </summary>
    /// <param name="ray">���߷���</param>
    /// <returns>��һ����Ķ�ά������</returns>
    private static Vector2 NormalDirection(Vector2 ray)
    {
        float dist = Mathf.Sqrt(ray.x * ray.x + ray.y * ray.y);
        if (dist == 0)
            return Vector2.zero;
        return new Vector2(ray.x / dist, ray.y / dist);
    }
    #endregion
}

