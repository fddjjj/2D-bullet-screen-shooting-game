using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// �����(�ظ�����/ʹ�õ���Ϸ����)  �ӵ������ܣ�����������
/// </summary>
public class ObjectPool : SingleTon<ObjectPool>
{
    //�ֶ�  �� ����Ԥ����(��Ϊһ�����ܿ����ж��Ԥ�Ƽ�) ���ܵĸ�����
    private Dictionary<string, List<GameObject>> cache = new Dictionary<string, List<GameObject>>();
    //int i = 0; //��� 0


    /// <summary>
    /// ������ʾ����
    /// </summary>
    /// <returns>The object.</returns>
    /// <param name="key">��������</param>
    /// <param name="go">�����Ԥ�Ƽ�</param>
    /// <param name="position">�������λ��</param>
    /// <param name="quaternion">����ĽǶ�</param>
    public GameObject CreateObject(string key, GameObject go, Vector3 position, Quaternion quaternion)
    {
        GameObject tempgo = cache.ContainsKey(key) ? cache[key].Find(p => !p.activeSelf) : null; //���س���δ����Ķ������ж�������ͷ��ؿգ���ֵ����ʱ����
        if (tempgo != null)                                                                      //�����ʱ����Ϊ��
        {
            tempgo.transform.position = position;   //����λ��
            tempgo.transform.rotation = quaternion; //��ת��Ϣ
        }
        else //���򣬾��ǿ��ˡ���Ҳ����û�ܴӳ�����ȡ������
        {
            tempgo = Instantiate(go, position, quaternion); //�Ǿ͸��ݴ����Ԥ�������һ��������
            //print("ʵ��������������" + i++);
            if (!cache.ContainsKey(key)) //����û�м�
            {
                cache.Add(key, new List<GameObject>()); //�½�һ�� �б�
            }

            cache[key].Add(tempgo); //���ֵ��е��б����/add ��ʱ���壬����м���ֱ�������
        }

        tempgo.SetActive(true); //��������ʱ����
        return tempgo;          //����
    }


    /// <summary>
    /// ֱ�ӻ���
    /// </summary>
    /// <param name="go">Go.</param>
    public void CollectObject(GameObject go)
    {
        go.SetActive(false);
    }


    /// <summary>
    /// �ӳٻ���
    /// </summary>
    /// <param name="go">Go.</param>
    /// <param name="delay">Delay.</param>
    public void CollectObject(GameObject go, float delay)
    {
        StartCoroutine(Collect(go, delay));
    }


    private IEnumerator Collect(GameObject go, float delay)
    {
        yield return new WaitForSeconds(delay);
        CollectObject(go);
    }


    /// <summary>
    /// �ͷ���Դ
    /// </summary>
    /// <returns>The clear.</returns>
    /// <param name="key">Key.</param>
    public void Clear(string key)
    {
        if (cache.ContainsKey(key))
        {
            //Destroy�������еĶ���
            for (int i = 0; i < cache[key].Count; i++)
            {
                Destroy(cache[key][i]);
            }

            //��������е�����ֵ
            //cache[key].Clear();
            //������������ֵһ�������
            cache.Remove(key);
        }
    }
    public void SetFalse(string key)
    {
        if (cache.ContainsKey(key))
        {
            //Destroy�������еĶ���
            for (int i = 0; i < cache[key].Count; i++)
            {
                cache[key][i].SetActive(false);
            }
        }
    }


    /// <summary>
    /// �ͷ����ж����
    /// </summary>
    public void ClearAll()
    {
        var list = new List<string>(cache.Keys);
        for (int i = 0; i < list.Count; i++)
        {
            Clear(list[i]);
        }
    }
}