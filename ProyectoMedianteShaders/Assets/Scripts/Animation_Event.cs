using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Animation_Event : MonoBehaviour {
    public int isDown = 0;  //0--> false
                            //1--> true

    public void AnimationState(int down) {
        print("animantion finished");
        isDown = down;
    }
}
