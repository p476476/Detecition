using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bone  {
    public Transform head;
    public Transform tail;

    public Bone(Transform BoneHead,Transform BoneTail)
    {
        head = BoneHead;
        tail = BoneTail;
    }
}
