using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PostProcessing;

public class AssignCameras : MonoBehaviour {
    public PostProcessingBehaviour[] ppp;
	// Use this for initialization
	void Start () {
        GameLogic.instance.ppp = ppp;
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
