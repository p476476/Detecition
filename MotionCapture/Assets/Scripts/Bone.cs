using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bone  {
    public Skeleton owner;
    public Transform head;
    public Transform tail;
    public float radius=0.1f;
    public DetectingPoint[] detect_points ;

	Vector3 interval;

    public Bone()
    {
    }
    public Bone(Transform BoneHead, Transform BoneTail,Skeleton owner)
    {
        this.owner = owner;
        head = BoneHead;
        tail = BoneTail;
    }

    public void initDetectingPoints()
    {
        //計算間隔
        int count = (int)(Vector3.Distance(head.position, tail.position) / radius);
        interval =  (tail.position- head.position)/count;

        detect_points = new DetectingPoint[count+2];
        
        for (int i = 0; i <count + 2; i++)
        {
            DetectingPoint dp = new DetectingPoint();
            dp.owner = this;
            dp.number = i;
            //設定偵測點位置
            //dp.position = new Vector3();
            dp.position = head.position+interval*i;
            //設定偵測點顏色
            detect_points[i] = dp;
            owner.detecting_points.Add(dp);
        }

    }

	public void updateDetectingPoints()
	{interval =  (tail.position- head.position)/detect_points.Length;
		for (int i = 0; i <detect_points.Length; i++)
		{
			
			//設定偵測點位置
			detect_points[i].position = head.position+interval*i;
			//設定偵測點顏色
		}
	}
}
