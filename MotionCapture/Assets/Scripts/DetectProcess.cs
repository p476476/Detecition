using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DetectProcess
{
	public void calculateDetectPointDirection (Vector3[,,] movement, Vector3[,] dp_on_texture, DetectingPoint2D[,] dp_2D, Vector3[,] dp_movement)
	{
		for (int frame_i = 0; frame_i < dp_2D.GetLength (0); frame_i++) {
			for (int dp_i = 0; dp_i < dp_2D.GetLength (1); dp_i++) {
				Vector3 total_move = new Vector3 ();
				int count = 0;

				float redius = dp_on_texture [frame_i, dp_i].z;
                

				for (int i = (int)(dp_on_texture [frame_i, dp_i].x - redius); i < (int)(dp_on_texture [frame_i, dp_i].x + redius); i++) {
					if (i >= 0 && i < 640) {
						for (int j = (int)(dp_on_texture [frame_i, dp_i].y - redius); j < (int)(dp_on_texture [frame_i, dp_i].y + redius); j++) {
							if (j >= 0 && j < 480) {
								total_move += movement [frame_i, i, j];
								count++;
							}
						}
					}
				}

				dp_movement [frame_i, dp_i] = total_move / count;


			}
		}
	}

	//calculate Detect Point Movement In 3D
	public void calculateDetectPointMovementIn3D (Vector3[,] dp_movement, Vector3[,] dp_on_texture, Vector3[] dp_movement_3D)
	{

		RealCamera[] cameras = Main.instance.real_cameras;
		
		for (int dp_i = 0; dp_i < dp_movement.GetLength (1); dp_i++) {
			dp_movement_3D [dp_i] = Vector3.zero;

			Vector3[] vertice = new Vector3[20];

			Vector3[] vectors = new Vector3[20];
			Vector3[] p0 = new Vector3[20];


			for (int camera_i = 0; camera_i < dp_movement.GetLength (0); camera_i++) {


				if (cameras [camera_i].isStreaming == true) {//and if 沒被遮擋
					Vector3 camera_position = cameras [camera_i].transform.position;
					Vector3 project_center = Vector3.zero;


					float offset_x = dp_on_texture [camera_i, dp_i].x + dp_movement [camera_i, dp_i].x - cameras [camera_i].pixel_width / 2;
					float offset_y = dp_on_texture [camera_i, dp_i].y + dp_movement [camera_i, dp_i].y - cameras [camera_i].pixel_height / 2;

					p0 [camera_i] = cameras [camera_i].transform.position;
					Vector3 p1 = project_center + offset_x * cameras [camera_i].transform.right / cameras [camera_i].camera_width
					             + offset_y * cameras [camera_i].transform.up / cameras [camera_i].camera_height; 

					vectors [camera_i] = p1 - p0 [camera_i];






				}

				

			}
		}
		

	}


	public void calculateBoneMovement ()
	{
		
	}


}
