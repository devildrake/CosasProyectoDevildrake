using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Change_Scale : MonoBehaviour {

    [HideInInspector]public bool change;
    public float velocity = 50;
    public float maxScale = 25;

    void Start() {
        transform.localScale = new Vector3(0, 0, 0);
    }

    void Update() {
        //if (Input.GetKeyDown(KeyCode.LeftShift)) {
        //    change = !change;
        //}
        if (change) {
            if (transform.localScale.x < maxScale) {
                transform.localScale += new Vector3(1.0f, 1.0f, 1.0f) * velocity * Time.deltaTime;
            }
        } else {
            if (transform.localScale.x > 1) {
                transform.localScale -= new Vector3(1.0f, 1.0f, 1.0f) * velocity * Time.deltaTime;
            } else {
                transform.localScale = new Vector3(0, 0, 0);
            }
        }
    }
}
