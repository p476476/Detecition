using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HumanTracker : MonoBehaviour {

    Transform head;

    //Body
    Transform[] body_position = new Transform[3];
    Vector3 body_forward;
    float shoulder_width;
    float body_width;

    //Hand
    Color32 hand_color;
    float upper_arm_length;
    float lower_arm_length;
    float upper_arm_width;
    float lower_arm_width;

    Transform[] left_hand_position = new Transform[3];
    Transform[] right_hand_position = new Transform[3];

    //Leg
    Color32 leg_color;
    float thigh_length;
    float calf_length;
    float thigh_width;
    float calf_width;

    Transform[] left_leg_position = new Transform[3];
    Transform[] right_leg_position = new Transform[3];

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
