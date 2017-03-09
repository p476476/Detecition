using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class RealCamera : MonoBehaviour {

    
    //用於顯示影像的平面
    public Transform plane;

	//bool
	public bool isStreaming = false;

    //draw 選項
    public bool show_project2D_vertice_on_plane = true;

    //Image
    public Texture2D last_frame;				
    public WebCamTexture mCamera = null;		
		

    //camera setting
	public int camera_num = 0;
    public float camera_width=1;
    public float camera_height =1;
    public float camera_distance = 1;
    public int pixel_width = 640;
    public int pixel_height = 480;
	public Vector3 project_plane_center;

    //project script
    Vertice3DTo2D v3to2;

    void OnStart()
    {
        last_frame = new Texture2D(pixel_width, pixel_height);

    }

    //開始擷取影像
	public void startStreaming(WebCamDevice wcd,int num)
    {
		camera_num = num;
        Debug.Log(gameObject.name+" start.");
        mCamera = new WebCamTexture(wcd.name);
        plane.GetComponent<Renderer>().material.SetTexture("_MainTex", mCamera);
        mCamera.Play();
		isStreaming = true;
    }

    //停止擷取影像
    public void stopStreaming()
    {
        Debug.Log(gameObject.name + " stop.");
        mCamera.Stop();
    }




}
