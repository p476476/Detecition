using UnityEngine;
using System.Collections;

[System.Serializable]
public class Data :MonoBehaviour
{

	//image result data
	public Texture2D[] last_diff_frames;			//[camera No.]
	public Texture2D[] current_diff_frames;		//[camera No.]
	public Vector3[,,] movement;	//[camera No. , x , y]  (pixel movement)


	//detect point position data
	public Vector3[] dp3D;
	public Vector3[,] dp_on_project_plane;		//[camera No. , detection point No.];
	public Vector3[,] dp_normalized;			//[camera No. , detection point No.];
	public Vector3[,] dp_on_texture;			//[camera No. , detection point No.];

	public float[] dp_radius;
	public float[,] dp_radius_on_plane;
	public int[,] dp_radius_on_texture;

	// modified detect point		(use it to find next pose)
	public Vector3[] modified_dp3D;
	public Vector3[,] modified_dp_on_project_plane;		//[camera No. , detection point No.];
	public Vector3[,] modified_dp_normalized;			//[camera No. , detection point No.];
	public Vector3[,] modified_dp_on_texture;			//[camera No. , detection point No.];


	//detect point data
	public DetectingPoint2D [,] dp_2D;			//[camera No. , detection point No.];
	public Vector3[,] dp_movement;				//[camera No. , detection point No.];
	public Vector3[] dp_movement_3D;			//[detection point No.];



	//bone calculate data
	public Vector3[] bone_movement;

	public void init(int camera_count,int dp_count,int bone_count,int camera_width_pixel,int camera_height_pixel)
	{
		last_diff_frames = new Texture2D[camera_count];
		current_diff_frames= new Texture2D[camera_count];

		movement = new Vector3[camera_count, camera_width_pixel, camera_height_pixel];
        for (int i = 0; i < movement.GetLength(0); i++)
        {
            for (int j = 0; j < movement.GetLength(1); j++)
            {
                for (int k = 0; k < movement.GetLength(2); k++)
                {
                    movement[i, j, k] = new Vector3();
                }
            }
        }


		dp3D = new Vector3[dp_count];
		for (int i=0;i<dp3D.GetLength(0);i++)
		{
			dp3D[i] = new Vector3();
		}

        dp_on_project_plane = new Vector3[camera_count,dp_count];
        for (int i=0;i<dp_on_project_plane.GetLength(0);i++)
        {
            for (int j = 0; j < dp_on_project_plane.GetLength(1); j++)
            {
                dp_on_project_plane[i, j] = new Vector3();
            }
        }

       
		dp_normalized = new Vector3[camera_count,dp_count];
        for (int i = 0; i < dp_normalized.GetLength(0); i++)
        {
            for (int j = 0; j < dp_normalized.GetLength(1); j++)
            {
                dp_normalized[i, j] = new Vector3();
            }
        }

        dp_on_texture = new Vector3[camera_count,dp_count];
        for (int i = 0; i < dp_on_texture.GetLength(0); i++)
        {
            for (int j = 0; j < dp_on_texture.GetLength(1); j++)
            {
                dp_on_texture[i, j] = new Vector3();
            }
        }

		//======================
		dp_radius = new float[dp_count];
		dp_radius_on_plane = new float[camera_count,dp_count];
		dp_radius_on_texture = new int[camera_count,dp_count];

		//======================

        dp_2D = new DetectingPoint2D[camera_count,dp_count];
        for (int i = 0; i < dp_2D.GetLength(0); i++)
        {
            for (int j = 0; j < dp_2D.GetLength(1); j++)
            {
                dp_2D[i, j] = new DetectingPoint2D();
            }
        }

        dp_movement = new Vector3[camera_count, dp_count];
        for (int i = 0; i < dp_movement.GetLength(0); i++)
        {
            for (int j = 0; j < dp_movement.GetLength(1); j++)
            {
                dp_movement[i, j] = new Vector3();
            }
        }

		dp_movement_3D = new Vector3[dp_count];
		for (int i = 0; i < dp_movement.GetLength(0); i++)
		{
			dp_movement_3D[i] = new Vector3();
		}

		//=========================
		bone_movement= new Vector3[bone_count];
		for (int i = 0; i < dp_movement.GetLength(0); i++)
		{
			dp_movement_3D[i] = new Vector3();
		}
    }

}

