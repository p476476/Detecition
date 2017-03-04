using UnityEngine;
using System.Collections;

public class Drawer : MonoBehaviour
{

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

		//空間中畫出投影點
		foreach(Vector3 v in Main.instance.data.dp_on_project_plane)
		{
			Gizmos.color = Color.yellow;
			Gizmos.DrawSphere(v, 0.05f);
		}
		/*
		//Plane上畫出投影點
		if (show_project2D_vertice_on_plane && plane != null)
		{

			//origin 為 plane 左下角的點
			Vector3 origin =  new Vector3(plane.position.x - (plane.localScale.x * 5), plane.position.y - (plane.localScale.y * 5), plane.position.z);
			foreach (var v in skeleton2D_normalize)
			{
				//若x,y在0~1之間 表示點在相機顯示範圍內 則畫出
				if (v.x >= 0 && v.x <= 1 && v.y >= 0 && v.y <= 1)
				{
					Vector3 v_onPlane = origin + new Vector3(v.x * (plane.localScale.x * 10), v.y * (plane.localScale.y * 10), 0);
					Gizmos.color = Color.yellow;
					Gizmos.DrawSphere(v_onPlane, 0.5f);
				}
			}
		}*/
	}
}

