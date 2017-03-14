using System.Collections;
using System;
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
	public void calculateDetectPointMovementIn3D (Vector3[] dp3D,Vector3[,] dp_movement, Vector3[,] dp_on_texture, Vector3[] dp_movement_3D)
	{

		RealCamera[] cameras = Main.instance.real_cameras;
		
		for (int dp_i = 0; dp_i < dp_movement.GetLength (1); dp_i++) {
			dp_movement_3D [dp_i] = Vector3.zero;

			for (int camera_i = 0; camera_i < dp_movement.GetLength (0); dp_i++) {



				Vector3 camera_position = cameras [camera_i].transform.position;
				Vector3 project_center = Vector3.zero;


				float offset_x = dp_on_texture [camera_i, dp_i].x + dp_movement [camera_i, dp_i].x - cameras [camera_i].pixel_width / 2;
				float offset_y = dp_on_texture [camera_i, dp_i].y + dp_movement [camera_i, dp_i].y - cameras [camera_i].pixel_height / 2;

				Vector3 p0  = cameras [camera_i].transform.position;
				Vector3 p1 = project_center + offset_x * cameras [camera_i].transform.right / cameras [camera_i].camera_width
				             + offset_y * cameras [camera_i].transform.up / cameras [camera_i].camera_height; 

				Vector3 v0 = p1 - p0;
				Vector3 v1 = dp3D [dp_i] - p0;

				Vector3 temp = p0 + (Mathf.Abs (Vector3.Dot (v0, v1)) / Vector3.Dot (v0, v0)) * v0;
				Vector3 movement = temp - dp3D [dp_i];

				if(Mathf.Abs(movement.x)>Mathf.Abs(dp_movement_3D[dp_i].x))
					dp_movement_3D[dp_i].x = movement.x;
				
				if(Mathf.Abs(movement.y)>Mathf.Abs(dp_movement_3D[dp_i].y))
					dp_movement_3D[dp_i].y = movement.y;

				if(Mathf.Abs(movement.z)>Mathf.Abs(dp_movement_3D[dp_i].z))
					dp_movement_3D[dp_i].z = movement.z;

			}
		}
	}


	public void calculateBoneMovement ()
	{
		
	}

	public void sortDetectPoint(Vector3[] dp_3D,RealCamera camera,int[] sort_index){
		for (int i = 0; i < sort_index.Length; i++) {
			sort_index [i] = i;
		}

		float[] distance = new float[dp_3D.Length];

		for (int i = 0; i < dp_3D.Length; i++) {
			distance [i] = Vector3.Distance (dp_3D [i], camera.transform.position);
		}

		for (int i = 0; i < dp_3D.Length; i++) {
			for (int j = i+1; j< dp_3D.Length; j++) {
				if (distance [i] > distance [j]) {
					int temp;
					temp = sort_index [i];
					sort_index [i] = sort_index [j];
					sort_index[j] = temp;

					float tempf;
					tempf = distance [i];
					distance [i] = distance [j];
					distance [j] = tempf;
				}
			}
		}

		Array.Clear(distance,0,distance.Length);

	}


}
