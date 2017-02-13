using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skeleton : MonoBehaviour {

    public List<Bone> bones = new List<Bone>();
    

	// Use this for initialization
	void Start () {

        //取得所有的Bone存到Bones
        Transform[] bs;
        bs = GetComponentsInChildren<Transform>();
        foreach (var b in bs)
        {
            Bone bone = new Bone(b.parent,b);
            bones.Add(bone);
        }

        //畫出骨架
        showSkeleton();
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    void showSkeleton()
    {
        foreach (var bone in bones)
        {
            GameObject go = GameObject.CreatePrimitive(PrimitiveType.Sphere);
            go.GetComponent<Renderer>().material.color = Color.blue;
            go.transform.position = bone.tail.position;
            go.transform.localScale = new Vector3(0.1f, 0.1f, 0.1f);
            go.transform.parent = bone.tail;

            Debug.DrawLine(bone.head.position, bone.tail.position, Color.red);
        }
    }
}
