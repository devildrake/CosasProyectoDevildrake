using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DetectStomp : MonoBehaviour {
    public bool active;

    private void OnTriggerEnter2D(Collider2D collision) {
        if (collision.tag == "Player") {
            GetComponentInParent<Agent>().stompedOn = true;
        }
    }

    private void OnTriggerStay2D(Collider2D collision) {
        if (collision.tag == "Player") {
            GetComponentInParent<Agent>().stompedOn = true;
        }
    }
}
