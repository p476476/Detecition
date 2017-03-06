using UnityEngine;
using System.Collections;

public class Drawer : MonoBehaviour
{
    public bool de;

	// Use this for initialization
	void Start ()
	{
	
	}
	
	// Update is called once per frame
	void Update ()
	{
	
	}

	private void OnDrawGizmos()
	{
        Data data = Main.instance.data;


		//空間中畫出投影點
		foreach(Vector3 v in data.dp_on_project_plane)
		{
            //print("draw");
			Gizmos.color = Color.yellow;
			Gizmos.DrawSphere(v, 0.05f);
		}
        RealCamera camera;
        Vector3 origin;
		//Plane上畫出投影點
        for (int camera_i=0; camera_i < Main.instance.real_cameras.Length; camera_i++)
        {
            camera = Main.instance.real_cameras[camera_i];
            if (camera.show_project2D_vertice_on_plane && camera.plane != null)
            {

                //origin 為 plane 左下角的點
                origin = new Vector3(camera.plane.position.x - (camera.plane.localScale.x * 5), camera.plane.position.y - (camera.plane.localScale.y * 5), camera.plane.position.z);
                for (int i=0;i<100;i++)
                {
                    Vector3 v = data.dp_normalized[camera_i, i];
                    //若x,y在0~1之間 表示點在相機顯示範圍內 則畫出
                    if (v.x >= 0 && v.x <= 1 && v.y >= 0 && v.y <= 1)
                    {
                        Vector3 v_onPlane = origin + new Vector3(v.x * (camera.plane.localScale.x * 10), v.y * (camera.plane.localScale.y * 10), 0);
                        Gizmos.color = Color.yellow;
                        Gizmos.DrawSphere(v_onPlane, 0.5f);
                    }
                }
            }
        }

        if (de)
        {
            camera = Main.instance.real_cameras[0];
            //origin 為 plane 左下角的點

            origin = new Vector3(camera.plane.position.x - (camera.plane.localScale.x * 5), camera.plane.position.y - (camera.plane.localScale.y * 5), camera.plane.position.z);
            for (int i = 0; i < 640; i+=10)
            {
                for (int j = 0; j < 480; j+=10)
                {
                    Vector3 v = data.movement[0, i, j];

                    if (v.magnitude > 0)
                    {
                        Vector3 v_onPlane_0 = origin + new Vector3((640-i) / 640f * (camera.plane.localScale.x * 10), (480-j) / 480f * (camera.plane.localScale.y * 10), 0);
                        

                        Vector3 v_onPlane_1 = origin + new Vector3((640-i-v.x) / 640f * (camera.plane.localScale.x * 10), (480-j-v.y) / 480f * (camera.plane.localScale.y * 10), 0);

                        Debug.DrawLine(v_onPlane_0, v_onPlane_1, Color.red);
                        Gizmos.color = Color.blue;
                        Gizmos.DrawSphere(v_onPlane_0, 0.1f);
                    }
                    
                }
            }
   
        }
       

    }
}

