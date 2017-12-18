using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraOffset : MonoBehaviour {
    public GameObject DawnCamera;
    Vector3 positionWithOffset;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        positionWithOffset = transform.position;
        positionWithOffset.y += GameLogic.instance.worldOffset;
        DawnCamera.transform.position = positionWithOffset;
	}
}
