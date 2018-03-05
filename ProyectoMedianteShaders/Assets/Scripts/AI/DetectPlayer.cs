using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DetectPlayer : MonoBehaviour {
    [Tooltip("Esta variable existe para hacer al trigger perder de vista al jugador cuando este muera, aprovechando que GameLogic se guarda las veces que ha muerto")]
    float recordedTimesDied;
    private void OnTriggerEnter2D(Collider2D collision) {
        if (collision.tag == "Player") {
            if (GetComponentInParent<Agent>() != null) {
                GetComponentInParent<Agent>().playerInArea = true;
            }
        }
    }

    private void OnTriggerStay2D(Collider2D collision) {
        if (collision.tag == "Player") {
            if (GetComponentInParent<Agent>() != null) {
                GetComponentInParent<Agent>().playerInArea = true;
            }
        }
    }

    private void OnTriggerExit2D(Collider2D other) {
        if (GetComponentInParent<Agent>() != null) {
            if (other.tag == "Player") {
                GetComponentInParent<Agent>().playerInArea = false;
            }
        }
    }

    private void Start() {
        recordedTimesDied = 0;
    }

    private void Update() {

        if (recordedTimesDied < GameLogic.instance.timesDied) {
            recordedTimesDied++;
            if (GetComponentInParent<Agent>() != null) {
                GetComponentInParent<Agent>().playerInArea = false;
            }
        }
    }

}
