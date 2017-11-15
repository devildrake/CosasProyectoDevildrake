using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlideZone : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    private void OnTriggerStay2D(Collider2D collision) {
        if (collision.tag == "Player") {
            if (collision.gameObject.GetComponent<PlayerController>().grounded) {
                collision.gameObject.GetComponent<PlayerController>().sliding = true;
            }
        }
    }


}
