using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BushCollider : MonoBehaviour {
    Quaternion rotation;
    private void Awake() {
        rotation = transform.rotation;
    }

    private void LateUpdate() {
        transform.rotation = rotation;
    }

    private void OnTriggerEnter(Collider collision) {
        if (collision.tag == "Player") {
            GameLogic.instance.KillPlayer();
        }
    }

    private void OnCollisionEnter(Collision collision) {
        if (collision.gameObject.tag == "Player") {
            GameLogic.instance.KillPlayer();
        }
    }

}
