using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class ExportMesh : MonoBehaviour
{

    [MenuItem("特效/ExportStar")]
    public static void ExportStar()
    {
        int num = 5;  //分割数
        float rad;   //弧度
        float r1 = 5, r2 = 2;//外半径，内半径

        VertexHelper vh = new VertexHelper();
        Mesh mesh = new Mesh();
        rad = 2 * Mathf.PI / num;

        vh.AddVert(new Vector3(0, 0, 0), Color.white, new Vector2(0.5f, 0.5f));
        for (int i = 0; i < num; i++)
        {
            //外顶点
            float x = Mathf.Sin(rad * i) * r1;
            float y = Mathf.Cos(rad * i) * r1;
            vh.AddVert(new Vector3(x, y, 0), Color.white, new Vector2((x + r1) / (2 * r1), (y + r1) / (2 * r1)));
            //内顶点
            float x2 = Mathf.Sin(rad * i + (rad / 2)) * r2;
            float y2 = Mathf.Cos(rad * i + (rad / 2)) * r2;
            vh.AddVert(new Vector3(x2, y2, 0), Color.white, new Vector2((x2 + r1) / (2 * r1), (y2 + r1) / (2 * r1)));
        }

        for (int i = 0; i < num * 2; i++)
        {
            if (i == 0)
            {
                vh.AddTriangle(0, num * 2, 1);
                vh.AddTriangle(0, 1, num * 2);
            }
            else
            {
                vh.AddTriangle(0, i, i + 1);
                vh.AddTriangle(0, i + 1, i);
            }
        }
        vh.FillMesh(mesh);
        //导出资源
        AssetDatabase.CreateAsset(mesh, "Assets/星星.asset");
        AssetDatabase.Refresh();
    }
}