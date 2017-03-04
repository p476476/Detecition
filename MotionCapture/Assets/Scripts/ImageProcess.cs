using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ImageProcess {
    public Texture2D differenceOfTwoImage(Texture2D firstImg,Texture2D secondImg)
    {
        
        int width = firstImg.width;
        int height = firstImg.height;
        Texture2D result = new Texture2D(width,height);

        Color[] difference = new Color[width * height];
        Color[] firstPixels = firstImg.GetPixels();
        Color[] secondPixels = secondImg.GetPixels();

        for (int i=0;i<width*height;i++)
        {
            difference[i].r = Mathf.Abs(secondPixels[i].r- firstPixels[i].r);
            difference[i].b = Mathf.Abs(secondPixels[i].b - firstPixels[i].b);
            difference[i].g = Mathf.Abs(secondPixels[i].g - firstPixels[i].g);
        }

        result.SetPixels(difference);
        return result;
    }

	public void calculateMovement(Vector3[] eight_direction_movement,Texture2D[] last_diff_frames,Texture2D[] current_diff_frames,int detect_range)
	{
		for (int frame_i = 0; frame_count < current_diff_frames.Length; frame_count++) {
			Color[] c_frame = current_diff_frames[frame_i].GetPixels ();
			Color[] l_frame = last_diff_frames[frame_i].GetPixels ();

			float temp_color_sum;

			for (int i = 0; i < c_frame.Length; i++) { //i = 一個pixel 在 frame 中的 index

				temp_color_sum = c_frame [i].r + c_frame [i].g + c_frame [i].b;

				if (temp_color_sum > 0.1) {
					// 8 direction 

					//向右找
					for (int j = 1; j < detect_range; j++) {


						int temp_index = i + j;  //t = 偵測的pixel的index

						if (temp_index % 640 == 0) //超過邊界
							break;

						temp_color_sum = l_frame [temp_index].r + l_frame [temp_index].g + l_frame [temp_index].b;
						if (temp_color_sum > 0.1) {
							eight_direction_movement[frame_i,
						}
					}


				}
			}
		}
					





			}

		}

	}

	int trans_2D_to_1D(int x,int y,int width,int height){
		return y*width+x;
	}
		
	


}
