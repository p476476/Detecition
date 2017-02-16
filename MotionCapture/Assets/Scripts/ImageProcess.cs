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


}
