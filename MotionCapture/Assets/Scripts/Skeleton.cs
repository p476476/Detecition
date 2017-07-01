using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skeleton : MonoBehaviour {

    public List<Bone> bones = new List<Bone>();
    public List<Transform> joints = new List<Transform>();
    public List<DetectingPoint> detecting_points = new List<DetectingPoint>();

    private void Awake()
    {
        //取得所有的Bone存到Bones
        joints.AddRange(GetComponentsInChildren<Transform>());
        foreach (var b in joints)
        {
			Bone bone;
			if (b.name == "hips")
				bone = new Bone(b, b, this);
			else
            	bone = new Bone(b.parent, b, this);
			
            bones.Add(bone);


			bone.initDetectingPoints(detecting_points.Count);
			print (detecting_points.Count);
        }
    }

    // Use this for initialization
    void Start () {
        

        //畫出關節點
        showSkeleton();
    }
	
	// Update is called once per frame
	void Update () {
		foreach (var b in bones)
		{
			b.updateDetectingPoints ();
		}

    }

    void showSkeleton()
    {


        //將關節點以藍點標出
        foreach (var j in joints)
        {
            GameObject go = GameObject.CreatePrimitive(PrimitiveType.Sphere);
            go.GetComponent<Renderer>().material.color = Color.blue;
            go.transform.position = j.position;
            go.transform.localScale = new Vector3(0.1f, 0.1f, 0.1f);
            go.transform.parent = j;
            
        }
    }


}
