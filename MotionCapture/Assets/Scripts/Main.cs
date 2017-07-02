using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Main : MonoBehaviour {

	public static Main instance;

    //script
    Vertice3DTo2D v3t2 = new Vertice3DTo2D();
	DetectProcess detect_process = new DetectProcess();
    ImageProcess image_process = new ImageProcess();
	SimilarityCalculate similarity_calculate = new SimilarityCalculate();

    //object
    public Transform human;
    public Skeleton skeleton;
	RealCameraManager real_camera_manager;
    public RealCamera[] real_cameras;

	//calculate data
	public Data data;

    //test
    public Transform tempPlane;
    public bool start_detect = false;

    
    Vector3[] dp1;
    Vector3[] dp2;
    Vector3[] dp3;

    private void Awake()
    {
        instance = this;
    }

    // Use this for initialization
    void Start () {
        //取得3D骨架
        skeleton = human.GetComponentInChildren<Skeleton>();

		//取得所有camera
		real_camera_manager = GetComponent<RealCameraManager> ();
		real_cameras = real_camera_manager.getCameras ();

        //初始化計算資料
        data = GetComponent<Data>();
		data.init (real_camera_manager.camera_count, skeleton.detecting_points.Count,skeleton.bones.Count,640,480);


        dp1 = new Vector3[skeleton.detecting_points.Count];
        dp2 = new Vector3[skeleton.detecting_points.Count];
        dp3 = new Vector3[skeleton.detecting_points.Count];
        StartCoroutine(fun());
    }

    // Update is called once per frame
    void Update () {

        //骨架2D化(投影到投影面,normalize,轉換成textrue座標,紀錄2D骨架)
        //取得偵測點array
        for (int i = 0; i < skeleton.detecting_points.Count; i++)
        {
            data.dp3D[i] = skeleton.detecting_points[i].position;
			data.dp_radius[i] = skeleton.detecting_points [i].radius;
        }

        //Detect_Point投影到Texture (存到modified_dp用於計算，存到dp用於顯示)
        Detect_Point_Project(data.dp3D,data.dp_on_project_plane, data.dp_normalized, data.dp_on_texture);

        //Detect_Point半徑 投影到Texture 
        v3t2.scaleVertice3DRadius(data.dp_radius, data.dp3D, real_cameras, data.dp_radius_on_plane, data.dp_radius_on_texture);


    }

	//detecting
     IEnumerator fun()
    {
        yield return new WaitForSeconds(1f);
        Debug.Log("start detecting.");
        while (true)
        {

            foreach (RealCamera camera in real_cameras)
            {
                //影像相減
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
						Texture2D diff_frame = image_process.differenceOfTwoImage(camera.last_frame,current_frame);
						diff_frame.Apply();

                        //顯示結果在temp frame
						tempPlane.GetComponent<Renderer>().material.SetTexture("_MainTex", diff_frame);

						//紀錄
						data.last_diff_frames[camera.camera_num] = data.current_diff_frames[camera.camera_num];
                        data.current_diff_frames[camera.camera_num] = diff_frame;

						//camera.last_frame = current_frame;

						camera.last_frame.SetPixels (current_frame.GetPixels());
						DestroyImmediate (current_frame);
    
                    }               
                }
            }

            if (start_detect)
            {

                //計算 pixel movement
                image_process.calculateMovement(data.movement, data.last_diff_frames, data.current_diff_frames, 10);

                float similarity_value = 0f;            //相似值
                float vaild_similarity_value = 0.7f;    //當相似值>vaild_similarity_value，完成追蹤
                int calculate_times = 0;                //計算次數
                int max_calculate_times = 10;           //最大計算次數

                //Detect_Point投影到Texture (存到modified_dp用於計算，存到dp用於顯示)
                Detect_Point_Project(data.dp3D, data.modified_dp_on_project_plane, data.modified_dp_normalized, data.modified_dp_on_texture);

                do
                {
                    calculate_times++;

                    

                    //計算 dp movement on Texture
                    //結果存到 dp_movement
                    detect_process.calculateDetectPointDirection(data.movement, data.modified_dp_on_texture, data.dp_movement);

                    //結合所有dp 在各 texture 的 2d movement 來計算 dp 3d movement
                    //結果存到dp_movement_3D
                    detect_process.calculateDetectPointMovementIn3D(data.modified_dp3D, data.dp_movement, data.modified_dp_on_texture, data.dp_movement_3D);

                    //移動dp
                    detect_process.moveDetectPoint(data.modified_dp3D, data.dp_movement_3D);

                    //Detect_Point投影到Texture (存到modified_dp用於計算，存到dp用於顯示)
                    Detect_Point_Project(data.modified_dp3D, data.modified_dp_on_project_plane, data.modified_dp_normalized, data.modified_dp_on_texture);

                    //相似度計算
                    similarity_value = similarity_calculate.Similarity();
                } while (similarity_value < vaild_similarity_value && calculate_times < max_calculate_times);

                //將modified_dp的結果更新到骨架上


                detect_process.calculateBoneMovement(skeleton.bones, data.dp_movement_3D, data.bone_movement);

				//
				//
				//detect_process.rotateBones (skeleton.joints, skeleton.bones, data.bone_movement);

            }
			GC.Collect();

            yield return new WaitForSeconds(0.05f);
        }
    }

    //Detect_Point投影到Texture
    void Detect_Point_Project(Vector3[]dp3D, Vector3[,] dp_on_project_plane, Vector3[,] dp_normalized, Vector3[,] dp_on_texture)
    {
        for (int i = 0; i < real_cameras.Length; i++)
        {
            //投影到投影面
            dp1 = v3t2.projectVertice3DToProjectPlane(dp3D, real_cameras[i]);
            for (int j = 0; j < dp1.Length; j++)
            {
                dp_on_project_plane[i, j] = dp1[j];
            }

            //normalize
            dp2 = v3t2.normalizeVertice2D(dp1, real_cameras[i]);
            for (int j = 0; j < dp2.Length; j++)
            {
                dp_normalized[i, j] = dp2[j];
            }


            //轉換成textrue座標
            dp3 = v3t2.transformNormalizeVerticeToTexture(dp2, real_cameras[i]);
            for (int j = 0; j < dp2.Length; j++)
            {
                dp_on_texture[i, j] = dp3[j];
            }

        }
    }




}
