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
			float diff = (Mathf.Abs(secondPixels[i].r - firstPixels[i].r)
						 +Mathf.Abs(secondPixels[i].b - firstPixels[i].b)
				         +Mathf.Abs(secondPixels[i].g - firstPixels[i].g))/3.0f;
			difference[i].r = diff;
			difference[i].b = diff;
			difference[i].g = diff;
        }

        result.SetPixels(difference);
        return result;
    }

    
	//calculateMovement()
	public void calculateMovement(Vector3[,,] movement,Texture2D[] last_diff_frames,Texture2D[] current_diff_frames,int detect_range)
	{
        float thres = 0.3f;

		for (int frame_i = 0; frame_i < current_diff_frames.Length; frame_i++) {    //each frame
            Color[] c_frame  = current_diff_frames[frame_i].GetPixels ();
			Color[] l_frame = last_diff_frames[frame_i].GetPixels ();

			float temp_color_sum;

			for (int i = 0; i < c_frame.Length; i++) { //each pixel in a frame

				temp_color_sum = c_frame [i].r + c_frame [i].g + c_frame [i].b;

				if (temp_color_sum > thres) {
                    Vector3 move = new Vector3();
                    int count=0;
                    int temp_index;
                    // 8 direction 

                    //向右找
                    for (int j = 1; j < detect_range; j++)
                    {
                        temp_index = i + j;  //t = 偵測的pixel的index

                        if (temp_index % 640 == 0)  break; //超過邊界

                        temp_color_sum = l_frame[temp_index].r + l_frame[temp_index].g + l_frame[temp_index].b;
                        if (temp_color_sum > thres)
                        {
                            move.x += j;
                            count++;
                        }

                    }
                    
                    //向下
                    for (int j = 1; j < detect_range; j++)
                    {
                        temp_index = i + j * 640;

						if (temp_index >= 640 * 480)
							break;

                        temp_color_sum = l_frame[temp_index].r + l_frame[temp_index].g + l_frame[temp_index].b;
                        if (temp_color_sum > thres)
                        {
                            move.y += j;
                            count++;
                        }
                        
                    }

                    //向左
                    for (int j = 1; j < detect_range; j++)
                    {
                        temp_index = i - j;

                        if (temp_index % 640 == 639 || temp_index<0) break;
                       
                        temp_color_sum = l_frame[temp_index].r + l_frame[temp_index].g + l_frame[temp_index].b;
                        if (temp_color_sum > thres)
                        {
                            move.x -= j;
                            count++;
                        }
                       
                    }

                    //向上
                    for (int j = 1; j < detect_range; j++)
                    {
                        temp_index = i - j*640;

                        if (temp_index < 0) break;

                        temp_color_sum = l_frame[temp_index].r + l_frame[temp_index].g + l_frame[temp_index].b;
                        if (temp_color_sum > thres)
                        {
                            move.y -= j;
                            count++;
                        }
                    }

					if(count>0)
                   		movement[frame_i,i%640,i/640] = move / count;
					else
						movement[frame_i, i % 640, i / 640] = Vector3.zero;
				}
                else
                {
                    movement[frame_i, i % 640, i / 640] = Vector3.zero;
                }
			}
		}

	}



}
