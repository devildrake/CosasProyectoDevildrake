using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class NavManager : MonoBehaviour {


	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    private void MouseOff() {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = false;
    }

    private void MouseOn() {
        Cursor.visible = true;
    }
}
