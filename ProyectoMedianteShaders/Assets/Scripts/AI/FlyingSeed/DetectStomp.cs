using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DetectStomp : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    private void OnTriggerEnter2D(Collider2D collision) {
        if (collision.tag == "Player") {
            GetComponentInParent<FlyingSeed>().stompedOn = true;
        }
    }

    private void OnTriggerStay2D(Collider2D collision) {
        if (collision.tag == "Player") {
            GetComponentInParent<FlyingSeed>().stompedOn = true;
        }
    }
}
