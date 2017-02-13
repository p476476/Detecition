using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RealCamera : MonoBehaviour {

    public Vector3[] skeleton2D;
    public WebCamTexture mCamera = null;

    void startStreaming()
    {
            Debug.Log(gameObject.name+" start.");
            mCamera = new WebCamTexture();
            mCamera.Play();
    }

    void stopStreaming()
    {
        Debug.Log(gameObject.name + " stop.");
        mCamera = new WebCamTexture();
        mCamera.Stop();
    }
}
