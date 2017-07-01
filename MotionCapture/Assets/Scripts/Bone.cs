using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bone  {
    public Skeleton owner;
    public Transform head;
    public Transform tail;
    public float radius=0.1f;
	public int dp_index;
	public int dp_count;

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

	public void initDetectingPoints(int start_index)
    {
		dp_index = start_index;

        //計算間隔
		dp_count = (int)(Vector3.Distance(head.position, tail.position) / radius)+1;
		interval =  (tail.position- head.position)/(dp_count);

        
		for (int i = 0; i <dp_count; i++)
        {
            DetectingPoint dp = new DetectingPoint();
            dp.owner = this;
			dp.number = dp_index+i;
            //設定偵測點位置
            //dp.position = new Vector3();
            dp.position = head.position+interval*i;
            //設定偵測點顏色
			//dp.color
			dp.radius = this.radius;
            owner.detecting_points.Add(dp);
        }
    }

	public void updateDetectingPoints()
	{
		for (int i = 0; i <dp_count; i++)
		{
			
			//設定偵測點位置
			owner.detecting_points[dp_index+i].position = head.position+interval*i;
			//設定偵測點顏色
		}
	}
}
