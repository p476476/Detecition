using UnityEngine;
using System.Collections;

public class Data
{

	//image result data
	public Texture2D[] last_diff_frames;			//[camera No.]
	public Texture2D[] current_diff_frames;		//[camera No.]
	public float[,,,] eight_direction_movement;	//[camera No. , x , y , direction]


	//detect point position data
	public Vector3[,] dp_on_project_plane;		//[camera No. , detection point No.];
	public Vector3[,] dp_normalized;			//[camera No. , detection point No.];
	public Vector3[,] dp_on_texture;			//[camera No. , detection point No.];

	//detect point data
	DetectingPoint2D [,] dp_2D;					//[camera No. , detection point No.];
	public float[,,] dp_eight_direction_vaule;	//[camera No. , detection point No., direction];

	public void init(int camera_count,int dp_count,int camera_width_pixel,int camera_height_pixel)
	{
		last_diff_frames = new Texture2D[camera_count];
		current_diff_frames= new Texture2D[camera_count];
		eight_direction_movement = new float[camera_count, camera_width_pixel, camera_height_pixel, 8];

		dp_on_project_plane = new Vector3[camera_count,dp_count];
		dp_normalized = new Vector3[camera_count,dp_count];
		dp_on_texture = new Vector3[camera_count,dp_count];

		dp_2D = new DetectingPoint2D[camera_count,dp_count];
		dp_eight_direction_vaule = new float[camera_count, dp_count, 8];
	}

}

