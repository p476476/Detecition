using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Vertice3DTo2D
{

    //將3D骨架投影到投影面
    public Vector3[] projectVertice3DToProjectPlane(Vector3[] vertex3D, RealCamera camera)
    {
        int vertex_num = vertex3D.Length;
        Vector3[] vertex2D = new Vector3[vertex_num];

        Vector3 camera_position = camera.transform.position;
        Vector3 camera_direction = camera.transform.forward.normalized;
        Vector3 cameraToPlane = camera_direction * camera.camera_distance;

        for (int i = 0; i < vertex_num; i++)
        {
            //計算投影後位置
            Vector3 cameraToVertex = vertex3D[i] - camera_position;
            float t = Vector3.Dot(cameraToPlane, camera_direction) / Vector3.Dot(cameraToVertex, camera_direction);
            vertex2D[i] = camera_position + cameraToVertex * t;

            //計算投影後大小
        }



        return vertex2D;
    }

    /*
    * 將投影面上的點
    * 以左下角為原點
    * 投影面的長和寬為1單位
    * 做normalize
    * (0,1,0)_____________(1,1,0)
    *        |           |
    *        |   投影面   |
    * (0,0,0)|___________|(1,0,0)
    * 
    */
    public Vector3[] normalizeVertice2D(Vector3[] input_vertice, RealCamera camera)
    {
        Vector3[] result = new Vector3[input_vertice.Length];

        //origin 為 相機成像 左下角的點
        Vector3 origin = camera.transform.position + camera.transform.forward * camera.camera_distance
                                                   - camera.transform.right * camera.camera_width / 2
                                                   - camera.transform.up * camera.camera_height / 2;


        for (int i = 0; i < input_vertice.Length; i++)
        {
            Vector3 normalized_pos;
            Vector3 diff = input_vertice[i] - origin;
            normalized_pos.x = Vector3.Project(diff, camera.transform.right).magnitude / camera.camera_width * (Vector3.Dot(Vector3.Project(diff, camera.transform.right).normalized, camera.transform.right));
            normalized_pos.y = Vector3.Project(diff, camera.transform.up).magnitude / camera.camera_height * (Vector3.Dot(Vector3.Project(diff, camera.transform.up).normalized, camera.transform.up));
            normalized_pos.z = 0;
            result[i] = normalized_pos;        }

        return result;
    }

    /*
     * 將normalized的點
     * 轉換成pixel座標
     * Input : normalize 後的點
     */
    public Vector3[] transformNormalizeVerticeToTexture(Vector3[] input_vertice, RealCamera camera)
    {
        Vector3[] result = new Vector3[input_vertice.Length];
        for (int i = 0; i < input_vertice.Length; i++)
        {
            Vector3 pos_on_texture;
            pos_on_texture.x = input_vertice[i].x * camera.pixel_width;
            pos_on_texture.y = input_vertice[i].y * camera.pixel_height;
            pos_on_texture.z = 0;

            result[i] = pos_on_texture;
        }

        return result;

    }
}
