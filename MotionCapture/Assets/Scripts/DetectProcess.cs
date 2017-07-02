using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;

public class DetectProcess
{
	public void calculateDetectPointDirection (Vector3[,,] movement, Vector3[,] dp_on_texture, Vector3[,] dp_movement)
	{
		for (int frame_i = 0; frame_i < dp_on_texture.GetLength(0); frame_i++) { //each camera
			for (int dp_i = 0; dp_i < dp_on_texture.GetLength (1); dp_i++) {    //each dp
				Vector3 total_move = new Vector3 ();
				int count = 0;

				float radius = 30;//dp_on_texture [frame_i, dp_i].z;
                

				for (int i = (int)(dp_on_texture [frame_i, dp_i].x - radius); i < (int)(dp_on_texture [frame_i, dp_i].x + radius); i+=5) {
					if (i >= 0 && i < 640) {
						for (int j = (int)(dp_on_texture [frame_i, dp_i].y - radius); j < (int)(dp_on_texture [frame_i, dp_i].y + radius); j+=5) {
							if (j >= 0 && j < 480) {
								total_move += movement [frame_i, i, j];
								count++;
							}
						}
					}
				}

				if (count == 0)
					dp_movement [frame_i, dp_i] = Vector3.zero;
				else
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

			for (int camera_i = 0; camera_i < 1; camera_i++) { //************1 should be modify



				Vector3 camera_position = cameras [camera_i].transform.position;
				Vector3 project_center = camera_position+ cameras [camera_i].transform.forward*cameras[camera_i].camera_distance;

				float offset_x = dp_on_texture [camera_i, dp_i].x + dp_movement [camera_i, dp_i].x - cameras [camera_i].pixel_width / 2;
				float offset_y = dp_on_texture [camera_i, dp_i].y + dp_movement [camera_i, dp_i].y - cameras [camera_i].pixel_height / 2;

				Vector3 p0  = cameras [camera_i].transform.position;
				Vector3 p1 = project_center + offset_x * cameras [camera_i].transform.right * cameras [camera_i].camera_width / 640f
				             + offset_y * cameras [camera_i].transform.up * cameras [camera_i].camera_height/480f; 

				Vector3 v0 = p1 - p0;
				Vector3 v1 = dp3D [dp_i] - p0;

	

				Vector3 temp = p0 + (Mathf.Abs (Vector3.Dot (v0, v1)) / Vector3.Dot (v0, v0)) * v0;

				Vector3 movement = temp - dp3D [dp_i];

				if(Mathf.Abs(movement.x)>Mathf.Abs(dp_movement_3D[dp_i].x))
					dp_movement_3D[dp_i].x = movement.x;
				
				if(Mathf.Abs(movement.y)>Mathf.Abs(dp_movement_3D[dp_i].y))
					dp_movement_3D[dp_i].y = movement.y;

				if (Mathf.Abs (movement.z) > Mathf.Abs (dp_movement_3D [dp_i].z))
					dp_movement_3D [dp_i].z = movement.z;

			}
		}
	}


	public void calculateBoneMovement (List<Bone> bones,Vector3[] dp_movement_3D,Vector3[] bone_movement)
	{
		for(int bone_i=0;bone_i<bones.Count;bone_i++)
		{
			bone_movement [bone_i] = Vector3.zero;
			Vector3 head_movement = new Vector3 ();
			Vector3 tail_movement = new Vector3();

			for (int i = bones[bone_i].dp_index; i < bones[bone_i].dp_index +  bones[bone_i].dp_count/2; i++) {
				head_movement += dp_movement_3D[i];
			}

			for (int i =  bones[bone_i].dp_index +  bones[bone_i].dp_count/2; i <  bones[bone_i].dp_index +  bones[bone_i].dp_count; i++) {
				tail_movement += dp_movement_3D[i];
			}

			Vector3 diff = tail_movement - head_movement;
			float angle = Vector3.Angle (head_movement, tail_movement);

			bone_movement [bone_i] = tail_movement-head_movement;
		
		}
		
	}

    public void moveDetectPoint(Vector3[] dp3D,Vector3[] dp_movement_3D)
    {
        for(int i=0;i<dp3D.Length;i++)
            dp3D[i] = dp3D[i] + dp_movement_3D[i];
    }

	public void rotateBones (List<Transform> joints,List<Bone> bones,Vector3[] bone_movement)
	{
		for(int i=5;i<bones.Count-1;i++)
		{
			joints [i ].LookAt (bones [i].tail.position+bone_movement[i]);
		}

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
