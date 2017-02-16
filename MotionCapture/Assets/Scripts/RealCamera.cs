using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class RealCamera : MonoBehaviour {

    
    //用於顯示影像的平面
    public Transform plane;

    //draw 選項
    public bool draw_project2D_vertice_in_space = true;
    public bool show_project2D_vertice_on_plane = true;

    //Image
    public Texture2D last_frame;
    public WebCamTexture mCamera = null;

    //camera setting
    public float camera_width=1;
    public float camera_height =1;
    public float camera_distance = 1;
    public int pixel_width = 640;
    public int pixel_height = 480;

    //project script
    Vertice3DTo2D v3to2;

    void OnStart()
    {
        last_frame = new Texture2D(pixel_width, pixel_height);
    }

    //開始擷取影像
    public void startStreaming(WebCamDevice wcd)
    {
        Debug.Log(gameObject.name+" start.");
        mCamera = new WebCamTexture(wcd.name);
        plane.GetComponent<Renderer>().material.SetTexture("_MainTex", mCamera);
        mCamera.Play();
    }

    //停止擷取影像
    public void stopStreaming()
    {
        Debug.Log(gameObject.name + " stop.");
        mCamera.Stop();
    }

    

    private void OnDrawGizmos()
    {
        //相機成像後的座標
        Vector3[] skeleton2D = this.GetComponent<Skeleton2D>().dp_on_project_plane;
        //skeleton2D 相機成像長和寬 做normalize
        Vector3[] skeleton2D_normalize = this.GetComponent<Skeleton2D>().dp_normalized;

        //空間中畫出投影點
        if(draw_project2D_vertice_in_space)
        {
            foreach (var v in skeleton2D)
            {
                Gizmos.color = Color.yellow;
                Gizmos.DrawSphere(v, 0.05f);
            }
        }

        //Plane上畫出投影點
        if (show_project2D_vertice_on_plane && plane != null)
        {
            //orgin 為

            //origin 為 plane 左下角的點
            Vector3 origin =  new Vector3(plane.position.x - (plane.localScale.x * 5), plane.position.y - (plane.localScale.y * 5), plane.position.z);
            foreach (var v in skeleton2D_normalize)
            {
                //若x,y在0~1之間 表示點在相機顯示範圍內 則畫出
                if (v.x >= 0 && v.x <= 1 && v.y >= 0 && v.y <= 1)
                {
                    Vector3 v_onPlane = origin + new Vector3(v.x * (plane.localScale.x * 10), v.y * (plane.localScale.y * 10), 0);
                    Gizmos.color = Color.yellow;
                    Gizmos.DrawSphere(v_onPlane, 0.5f);
                }
            }
        }
    }
}
