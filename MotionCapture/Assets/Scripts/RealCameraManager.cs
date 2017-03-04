using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RealCameraManager : MonoBehaviour {

    public Transform real_camera_catalog;
    public RealCamera[] real_cameras ;
	public int camera_count=0;

    // Use this for initialization
    void Start () {
        //攝影機設定
        WebCamDevice[] wcd = WebCamTexture.devices;
		camera_count = wcd.Length;
        if (wcd != null)
        {
            for (int i = 0; i < wcd.Length; i++)
            {
                real_cameras[i].startStreaming(wcd[i],i);
            }
        }
    }

	public RealCamera[] getCameras()
	{
		return real_cameras;
	}
}
