using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bone  {
    public Skeleton owner;
    public Transform head;
    public Transform tail;
    public float radius=0.1f;
    public DetectingPoint[] detect_points ;

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
        Debug.Log(head.name + " " + tail.name + " " + Vector3.Distance(tail.position, head.position));
        //計算間隔
        int count = (int)(Vector3.Distance(head.position, tail.position) / radius);
        Vector3 interval =  (tail.position- head.position)/count;

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
}
