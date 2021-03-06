﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DetectTouch : MonoBehaviour {
    private void OnCollisionEnter2D(Collision2D collision) {
        if (collision.collider.tag == "Player") {
            if (gameObject.GetComponentInParent<Agent>() != null) {
                gameObject.GetComponentInParent<Agent>().touchedByPlayer = true;
            } else {
                gameObject.GetComponent<Agent>().touchedByPlayer = true;
            }
        } 
    }

    //private void OnCollisionStay2D(Collision2D collision) {
    //    if (collision.collider.tag == "Player" && isActive) {
    //        gameObject.GetComponentInParent<Agent>().touchedByPlayer = true;
    //    }
    //}
}
