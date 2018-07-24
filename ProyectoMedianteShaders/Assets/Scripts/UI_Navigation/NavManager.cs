using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class NavManager : MonoBehaviour {

    bool block = false;
    
	void Start () {

    }
	
	// Update is called once per frame
	void Update (){
        if (!block) {

        }
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
