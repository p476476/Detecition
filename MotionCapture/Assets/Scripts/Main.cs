using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Main : MonoBehaviour {

    //script
    Vertice3DTo2D v3t2 = new Vertice3DTo2D();
    ImageProcess image_process = new ImageProcess();

    //object
    public Transform human;
    Skeleton skeleton;
    public RealCamera[] real_cameras;
    Skeleton2D[] skeletons2D;
    //test
    public Transform tempPlane;


    // Use this for initialization
    void Start () {
        //取得3D骨架
        skeleton = human.GetComponentInChildren<Skeleton>();

        //取得所有camera
        real_cameras = GetComponent<RealCameraManager>().real_cameras;

        //取得與camera對應的2D骨架
        skeletons2D = new Skeleton2D[real_cameras.Length];
        for (int i=0;i<real_cameras.Length;i++)
        {
            skeletons2D[i] = real_cameras[i].GetComponent<Skeleton2D>();
        }

        StartCoroutine(fun());
    }

    // Update is called once per frame
    void Update () {


        /*
        //更新影像
        foreach (var camera in real_cameras)
        {
            if (camera.mCamera == null)
            {
                //Debug.Log("mCamera is null.");
            }
            else
            {
                if (camera.last_frame == null)
                {
                    //第一張影像
                    camera.last_frame = new Texture2D(camera.mCamera.width, camera.mCamera.height);
                    camera.last_frame.SetPixels(camera.mCamera.GetPixels());
                }
                else
                {
                    //取地目前的影像
                    camera.last_frame.SetPixels(camera.mCamera.GetPixels());
                }
            }

        }*/
        //骨架2D化(投影到投影面,normalize,轉換成textrue座標,紀錄2D骨架)

        Vector3[] dp3D = new Vector3[skeleton.detecting_points.Count];

        //取得偵測點array
        for (int i = 0; i < skeleton.detecting_points.Count; i++)
        {
            dp3D[i] = skeleton.detecting_points[i].position;
        }
        foreach (RealCamera camera in real_cameras)
        {

            Debug.Log(camera.transform.name);  
            Skeleton2D sk2D = camera.GetComponent<Skeleton2D>();

            //投影到投影面
            sk2D.dp_on_project_plane = v3t2.projectVertice3DToProjectPlane(dp3D, camera);

            //normalize
            sk2D.dp_normalized = v3t2.normalizeVertice2D(sk2D.dp_on_project_plane, camera);

            //轉換成textrue座標
            Vector3[] dp2D_on_texture = new Vector3[skeleton.detecting_points.Count];
            dp2D_on_texture = v3t2.transformNormalizeVerticeToTexture(sk2D.dp_normalized, camera);

            //紀錄2D骨架
            sk2D.detecting_points_2D = new DetectingPoint2D[skeleton.detecting_points.Count];
            for (int i = 0; i < skeleton.detecting_points.Count; i++)
            {
                DetectingPoint2D dp = new DetectingPoint2D();
                dp.position = dp2D_on_texture[i];
                sk2D.detecting_points_2D[i] = dp;
            }
        }


    }

     IEnumerator fun()
    {
        yield return new WaitForSeconds(3f);
        Debug.Log("start detecting.");
        while (true)
        {

            foreach (RealCamera camera in real_cameras)
            {
                //做影像相減
                if (camera.mCamera == null)
                {
                    //Debug.Log("mCamera is null.");
                }
                else
                {
                    if (camera.last_frame == null)
                    {
                        //第一張影像
                        camera.last_frame = new Texture2D(camera.mCamera.width, camera.mCamera.height);
                        camera.last_frame.SetPixels(camera.mCamera.GetPixels());
                    }
                    else
                    {
                        //取地目前的影像
                        Texture2D current_frame = new Texture2D(camera.mCamera.width, camera.mCamera.height);
                        current_frame.SetPixels(camera.mCamera.GetPixels());

                        //與前一張影像作相減
                        Texture2D result = image_process.differenceOfTwoImage(camera.last_frame, current_frame);
                        result.Apply();

                        //顯示結果在temp frame
                        tempPlane.GetComponent<Renderer>().material.SetTexture("_MainTex", result);

                        camera.last_frame = current_frame;

                    }
                }

                
               

            }

            yield return new WaitForSeconds(0.1f);
        }
    }
    
}
