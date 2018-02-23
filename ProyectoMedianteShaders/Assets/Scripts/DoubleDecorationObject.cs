using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoubleDecorationObject : MonoBehaviour {
    public GameObject brotherObject;
    public DoubleObject.world worldAssignation;
    

    // Use this for initialization
    void Start () {
        if (worldAssignation == DoubleObject.world.DAWN) {
            transform.position = new Vector3(brotherObject.transform.position.x, brotherObject.transform.position.y+GameLogic.instance.worldOffset, brotherObject.transform.position.z);
        }
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
