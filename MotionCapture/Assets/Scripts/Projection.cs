using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projection{

	public Vector3[] projectVerticeTo2D(Vector3[] vertex3D,Transform camera_trans,float project_distance,float project_width,float project_height)
    {
        int vertex_num = vertex3D.Length;
        Vector3[] vertex2D = new Vector3[vertex_num];

        Vector3 camera_position = camera_trans.position;
        Vector3 camera_direction = camera_trans.forward.normalized;
        Vector3 cameraToPlane = camera_direction * project_distance;

        for(int i=0;i<vertex_num; i++)
        {
            Vector3 cameraToVertex = vertex3D[i] - camera_position;
            float t = Vector3.Dot(cameraToPlane, camera_direction) / Vector3.Dot(cameraToVertex, camera_direction);

            vertex2D[i] = camera_position + cameraToVertex * t;
        }

        

        return vertex2D;
    }
}
