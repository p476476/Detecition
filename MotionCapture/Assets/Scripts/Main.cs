using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Main : MonoBehaviour {

    Projection projection = new Projection();
    public Transform human;
    public Transform[] real_cameras;

    Vector3[] vec3D;
    Skeleton skeleton;

    // Use this for initialization
    void Start () {
        skeleton = human.GetComponentInChildren<Skeleton>();
        //public WebCamTexture mCamera = null;
        

    }

    // Update is called once per frame
    void Update () {
        vec3D = new Vector3[skeleton.bones.Count];
        for (int i = 0; i < skeleton.bones.Count; i++)
        {
            vec3D[i] = skeleton.bones[i].tail.position;
        }

        foreach (var camera in real_cameras)
        {
            camera.GetComponent<RealCamera>().skeleton2D = projection.projectVerticeTo2D(vec3D, camera, 1f, 1f, 1f);
        }


    }

   
}
