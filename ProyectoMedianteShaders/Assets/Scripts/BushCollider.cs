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

    private void OnTriggerEnter2D(Collider2D collision) {
        if (collision.tag == "Player") {
            GameLogic.instance.KillPlayer();
        }
    }

    private void OnCollisionEnter2D(Collision2D collision) {
        if (collision.gameObject.tag == "Player") {
            GameLogic.instance.KillPlayer();
        }
    }

}
