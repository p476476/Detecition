using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RealCameraManager : MonoBehaviour {

    public Transform real_camera_catalog;
    public RealCamera[] real_cameras ;

    // Use this for initialization
    void Start () {

        //real_cameras = real_camera_catalog.GetComponentsInChildren<RealCamera>();
        //攝影機設定
        WebCamDevice[] wcd = WebCamTexture.devices;
        if (wcd != null)
        {
            for (int i = 0; i < wcd.Length; i++)
            {
                real_cameras[i].startStreaming(wcd[i]);
            }
        }
    }

    public RealCamera[] getCameras()
    {
        return real_cameras;
    }
}
