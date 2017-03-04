using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Main : MonoBehaviour {

	public static Main instance;

    //script
    Vertice3DTo2D v3t2 = new Vertice3DTo2D();
    ImageProcess image_process = new ImageProcess();

    //object
    public Transform human;
    Skeleton skeleton;
	RealCameraManager real_camera_manager;
    public RealCamera[] real_cameras;

	//calculate data
	public Data data;

    //test
    public Transform tempPlane;


    // Use this for initialization
    void Start () {
        //取得3D骨架
        skeleton = human.GetComponentInChildren<Skeleton>();

		//取得所有camera
		real_camera_manager = GetComponent<RealCameraManager> ();
		real_cameras = real_camera_manager.getCameras ();

        //初始化計算資料
		data = new Data();
		data.init (real_camera_manager.camera_count, skeleton.detecting_points.Count,640,480);

        StartCoroutine(fun());
    }

    // Update is called once per frame
    void Update () {

        //骨架2D化(投影到投影面,normalize,轉換成textrue座標,紀錄2D骨架)

        Vector3[] dp3D = new Vector3[skeleton.detecting_points.Count];
        //取得偵測點array
        for (int i = 0; i < skeleton.detecting_points.Count; i++)
        {
            dp3D[i] = skeleton.detecting_points[i].position;
        }
		for(int i=0;i<real_cameras.Length;i++)
        {
			
            

            //投影到投影面
			Vector3[] dp1 = new Vector3[skeleton.detecting_points.Count];
			dp1 = v3t2.projectVertice3DToProjectPlane(dp3D, real_cameras[i]);
			for(int j=0;j<dp1.Length;j++)
			{
				data.dp_on_project_plane [i, j] = dp1 [j];
			}

            //normalize
			Vector3[] dp2 = new Vector3[skeleton.detecting_points.Count];
			dp2  = v3t2.normalizeVertice2D(dp1, real_cameras[i]);
			for(int j=0;j<dp2.Length;j++)
			{
				data.dp_normalized [i, j] = dp2 [j];
			}
				

            //轉換成textrue座標
            Vector3[] dp3 = new Vector3[skeleton.detecting_points.Count];
			dp3 = v3t2.transformNormalizeVerticeToTexture(dp2, real_cameras[i]);
			for(int j=0;j<dp2.Length;j++)
			{
				data.dp_on_texture [i, j] = dp3 [j];
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
                        //取得目前的影像
                        Texture2D current_frame = new Texture2D(camera.mCamera.width, camera.mCamera.height);
                        current_frame.SetPixels(camera.mCamera.GetPixels());

						current_frame.SetPixels (camera.mCamera.GetPixels ());
                        //與前一張影像作相減
						Texture2D diff_fram = image_process.differenceOfTwoImage(camera.last_frame,current_frame);
						diff_fram.Apply();

                        //顯示結果在temp frame
						tempPlane.GetComponent<Renderer>().material.SetTexture("_MainTex", diff_fram);

						//紀錄
						data.last_diff_frames[camera.camera_num] = diff_fram;
                        camera.last_frame = current_frame;

                    }
                }

                
               

            }

            yield return new WaitForSeconds(0.1f);
        }
    }



    
}
