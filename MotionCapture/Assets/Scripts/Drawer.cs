using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Drawer : MonoBehaviour
{
	public bool draw_project_in_3d;
	public bool draw_project_on_texture;
	public bool draw_pixel_movement;

	public bool draw_dp_movement;
	public bool draw_dp_movement_3D;
	public bool draw_bone_movement;

	// Use this for initialization
	void Start ()
	{
	
	}
	
	// Update is called once per frame
	void Update ()
	{
	
	}

	private void OnDrawGizmos()
	{
		Data data = GetComponent<Data> ();

		RealCamera camera;
		Vector3 origin;

		if (draw_project_in_3d) {
			//空間中畫出投影點
			for (int camera_i = 0; camera_i < Main.instance.real_cameras.Length; camera_i++) { //each camera
				for (int i = 0; i < data.dp_normalized.GetLength (1); i++) {
					//print("draw");
					Gizmos.color = Color.yellow;
					Gizmos.DrawSphere (data.dp_on_project_plane[camera_i,i], data.dp_radius_on_plane [camera_i, i]);
				}

				camera = Main.instance.real_cameras [camera_i];
				Vector3 center = camera.transform.position + camera.transform.forward * camera.camera_distance;
				Vector3 p0 = center - 0.5f * camera.transform.right * camera.camera_width - 0.5f * camera.transform.up * camera.camera_height;
				Vector3 p1 = center - 0.5f * camera.transform.right * camera.camera_width + 0.5f * camera.transform.up * camera.camera_height;
				Vector3 p2 = center + 0.5f * camera.transform.right * camera.camera_width + 0.5f * camera.transform.up * camera.camera_height;
				Vector3 p3 = center + 0.5f * camera.transform.right * camera.camera_width - 0.5f * camera.transform.up * camera.camera_height;


				Debug.DrawLine(p0, p1, Color.black);
				Debug.DrawLine(p1, p2, Color.black);
				Debug.DrawLine(p2, p3, Color.black);
				Debug.DrawLine(p3, p0, Color.black);
			}


		}
			


		if (draw_project_on_texture) {
			//Plane上畫出投影點

			for (int camera_i = 0; camera_i < Main.instance.real_cameras.Length; camera_i++) { //each camera
				camera = Main.instance.real_cameras [camera_i];
				if (camera.show_project2D_vertice_on_plane && camera.plane != null) {

					//origin 為 plane 左下角的點
					origin = new Vector3 (camera.plane.position.x - (camera.plane.localScale.x * 5), camera.plane.position.y - (camera.plane.localScale.y * 5), camera.plane.position.z);
					for (int i = 0; i < data.dp_normalized.GetLength(1); i++) {
						Vector3 v = data.dp_normalized [camera_i, i];
						//若x,y在0~1之間 表示點在相機顯示範圍內 則畫出
						if (v.x >= 0 && v.x <= 1 && v.y >= 0 && v.y <= 1) {
							Vector3 v_onPlane = origin + new Vector3 (v.x * (camera.plane.localScale.x * 10), v.y * (camera.plane.localScale.y * 10), 0);
							Gizmos.color = Color.yellow;
							Gizmos.DrawSphere (v_onPlane, 0.1f);


						}
					}
				}
			}


		}


		if (draw_pixel_movement)
        {
			//畫出movement
            camera = Main.instance.real_cameras[0];
            //origin 為 plane 左下角的點

            origin = new Vector3(camera.plane.position.x - (camera.plane.localScale.x * 5), camera.plane.position.y - (camera.plane.localScale.y * 5), camera.plane.position.z);
            for (int i = 0; i < 640; i+=10)
            {
                for (int j = 0; j < 480; j+=10)
                {
                    Vector3 v = data.movement[0, i, j];
					/*
                    if (v.magnitude > 0)
                    {
                        Vector3 v_onPlane_0 = origin + new Vector3((i) / 640f * (camera.plane.localScale.x * 10), (j) / 480f * (camera.plane.localScale.y * 10), 0);
                        

                        Vector3 v_onPlane_1 = origin + new Vector3((i+v.x) / 640f * (camera.plane.localScale.x * 10), (j+v.y) / 480f * (camera.plane.localScale.y * 10), 0);

                        Debug.DrawLine(v_onPlane_0, v_onPlane_1, Color.red);
                        Gizmos.color = Color.blue;
                        Gizmos.DrawSphere(v_onPlane_0, 0.1f);




                    }*/
                    
                }
            }


   
        }

		if (draw_dp_movement) {

			for (int camera_i = 0; camera_i <1; camera_i++) { //each camera
				camera = Main.instance.real_cameras [camera_i];
				if (camera.show_project2D_vertice_on_plane && camera.plane != null) {

					//origin 為 plane 左下角的點
					origin = new Vector3 (camera.plane.position.x - (camera.plane.localScale.x * 5), camera.plane.position.y - (camera.plane.localScale.y * 5), camera.plane.position.z);
					for (int i = 0; i < data.dp_normalized.GetLength(1); i++) {
						Vector3 v = data.dp_normalized [camera_i, i];
						//若x,y在0~1之間 表示點在相機顯示範圍內 則畫出
						if (v.x >= 0 && v.x <= 1 && v.y >= 0 && v.y <= 1) {
							Vector3 v_onPlane = origin + new Vector3 (v.x * (camera.plane.localScale.x * 10), v.y * (camera.plane.localScale.y * 10), 0);
							Gizmos.color = Color.red;
							Debug.DrawLine(v_onPlane, v_onPlane-data.dp_movement[0,i], Color.red);

						}
					}
				}
			}


		}

		if (draw_dp_movement_3D) {

			for (int i = 0; i < data.dp_movement_3D.GetLength(0); i++) {
				
				//若x,y在0~1之間 表示點在相機顯示範圍內 則畫出
			
					
				Gizmos.DrawSphere(data.dp3D[i]+10*data.dp_movement_3D[i], 0.1f);
				Debug.DrawLine(data.dp3D[i],data.dp3D[i]+10*data.dp_movement_3D[i], Color.red);

			}
				
		}


		if (draw_bone_movement) {

			List<Bone> bones = Main.instance.skeleton.bones;
			for (int bone_i = 0; bone_i < data.bone_movement.Length; bone_i++) {
				Gizmos.color = Color.red;
				Debug.DrawLine(bones[bone_i].tail.position, bones[bone_i].tail.position+100*data.bone_movement[bone_i], Color.blue);
				//Debug.DrawLine(bones[bone_i].tail.position, bones[bone_i].tail.position+ bones[bone_i].tail.up, Color.red);


			}

		}

			/*
			for (int bone_i = 0; bone_i < data.bone_movement.Length; bone_i++) {
				
				Debug.DrawLine(bones[bone_i].tail.position, bones[bone_i].tail.position+data.bone_movement[bone_i], Color.red);
				Debug.DrawLine(bones[bone_i].tail.position, bones[bone_i].tail.position+ bones[bone_i].tail.up, Color.blue);
			}
			for (int i = 0; i < data.dp3D.Length; i++) {
				//List<DetectingPoint> dps = Main.instance.skeleton.detecting_points;
				Debug.DrawLine(data.dp3D[i], data.dp3D[i]+data.dp_movement_3D[i], Color.red);

			}*/
		

    }
}

