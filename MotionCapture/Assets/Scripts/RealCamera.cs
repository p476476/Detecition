using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RealCamera : MonoBehaviour {

    public Transform texturePlane;

    public Vector3[] skeleton2D;//骨架投影到2D的點
    public Vector3[] skeleton2D_on_texture;//2D的點對應到Texture上

    float camera_width_in_meter;
    float camera_height_in_meter;

    WebCamTexture mCamera = null;//影像

    void startStreaming()
    {
            Debug.Log(gameObject.name+" start.");
            mCamera = new WebCamTexture();
            if(texturePlane != null)
            {
                texturePlane.getComponent<Renender>().material = mCamera;
            }
            mCamera.Play();
    }

    void stopStreaming()
    {
        Debug.Log(gameObject.name + " stop.");
        mCamera = new WebCamTexture();
        mCamera.Stop();
    }

    private void OnDrawGizmos()
    {

        //在場景中畫出2D投影點
        if (vec != null)
        {
            foreach (var vec in skeleton2D)
            {
                Gizmos.color = Color.yellow;
                Gizmos.DrawSphere(vec, 0.05f);
            }
        }

    }
}
