using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerDetectionPlayer : MonoBehaviour {

    private void OnTriggerEnter2D(Collider2D collision) {
        if (collision.tag == "Player") {
            GameLogic.instance.KillPlayer();
        } else if(collision.tag!="Area"){
            if (GetComponentInParent<Seeker>() != null) {
                GetComponentInParent<Seeker>().HideTentacles();
            }
        }
    }

}
