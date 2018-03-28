using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KillPlayerTrigger : MonoBehaviour {
    Rigidbody2D parentRb;
	// Use this for initialization
	void Start () {
        parentRb = GetComponentInParent<Rigidbody2D>();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    private void OnTriggerEnter2D(Collider2D collision) {
        if (collision.gameObject.tag == "Player") {
            if (parentRb.velocity.y <= 0) {
                GameLogic.instance.KillPlayer();
            }
        }
    }

    private void OnTriggerStay2D(Collider2D collision) {
        if (collision.gameObject.tag == "Player") {
            if (parentRb.velocity.y <= 0) {
                GameLogic.instance.KillPlayer();
            }
        }
    }
}
