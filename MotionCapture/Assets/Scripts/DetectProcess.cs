using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DetectProcess  {
    public void calculateDetectPointDirection(Vector3[,,] movement, Vector3[,] dp_on_texture, DetectingPoint2D[,] dp_2D, Vector3[,] dp_movement)
    {
        for(int frame_i = 0; frame_i < dp_2D.GetLength(0); frame_i++)
        {
            for(int dp_i = 0; dp_i < dp_2D.GetLength(1); dp_i++)
            {
                Vector3 total_move = new Vector3();
                int count = 0;

                float redius = dp_on_texture[frame_i,dp_i].z;
                

                for (int i = (int)(dp_on_texture[frame_i, dp_i].x- redius); i < (int)(dp_on_texture[frame_i, dp_i].x + redius); i++)
                {
                    if(i>=0 && i < 640)
                    {
                        for (int j = (int)(dp_on_texture[frame_i, dp_i].y - redius); j < (int)(dp_on_texture[frame_i, dp_i].y + redius); j++)
                        {
                            if(j>=0 && j < 480)
                            {
                                total_move += movement[frame_i,i,j];
                                count++;
                            }
                        }
                    }
                }

                dp_movement[frame_i, dp_i] = total_move / count;


            }
        }
    }
}
