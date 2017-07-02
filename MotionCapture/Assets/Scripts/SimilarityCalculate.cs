using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimilarityCalculate  {

	float color_similarity_threshold = 0.3f;
	int sample_rate = 4;

	//int similarity_count=0;
	//int max_similarity_count=10;


	public float Similarity(Texture2D[] current_diff_frames,DetectingPoint2D [,] dp_2D,Vector3[,] dp_on_texture,int[,] dp_radius_on_texture){

		int total_diff_count=0;
		int cover_count=0;

		for (int i = 0; i < current_diff_frames.Length; i+=sample_rate) { 		//each frame

			Color[] pixels = current_diff_frames [i].GetPixels ();	//get pixels

			for (int j = 0; j < pixels.Length; j++) {				//each pixel

				total_diff_count++;

				if (pixels [i].r > color_similarity_threshold) {	//diff 
					
					for (int k = 0; k < dp_on_texture.GetLength (0); k++) {	//each dp
						
						if (dp_2D [i, k].color.r - pixels [j].r < 0.1	 //if same color and distance(pixel to dp) < radius
							&& (Vector3.Distance (dp_on_texture [i, k], new Vector3 (j / 640, j % 640)) < (float)dp_radius_on_texture [i,k])) {
							cover_count++;
							break;
						}


					}
				}
			}
		}

		if (total_diff_count == 0) {
			Debug.Log ("total_diff_count = 0 in SimilarityCalculate");
			return 1f;
		}


		return (float)cover_count / total_diff_count;

	}

}
