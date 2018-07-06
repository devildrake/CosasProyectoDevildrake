using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DetectStomp : MonoBehaviour {
    public bool active;

    private void OnTriggerEnter(Collider collision) {
        if (collision.tag == "Player") {
            GetComponentInParent<Agent>().stompedOn = true;
        }
    }

    private void OnTriggerStay(Collider collision) {
        if (collision.tag == "Player") {
            GetComponentInParent<Agent>().stompedOn = true;
        }
    }
}
