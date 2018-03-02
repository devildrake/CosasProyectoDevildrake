using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerDetectionPlayer : MonoBehaviour {
    public bool playerInArea;
    private void OnTriggerEnter2D(Collider2D collision) {
        if (GetComponentInParent<Seeker>() != null) {
            if (collision.tag == "Player") {
                GameLogic.instance.KillPlayer();
            } else if (collision.tag != "Area") {
                if (GetComponentInParent<Seeker>() != null) {
                    GetComponentInParent<Seeker>().HideTentacles();
                }
            }
        } else {
            if (collision.tag == "Player") {
                playerInArea = true;
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision) {
        if (collision.tag == "Player") {
            playerInArea = false;
        }
    }

}
