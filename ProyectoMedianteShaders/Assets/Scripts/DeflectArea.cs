using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeflectArea : MonoBehaviour {
    PlayerController padre;
    // Use this for initialization
    void Start() {
        padre = GetComponentInParent<PlayerController>();
    }

    // Update is called once per frame
    void Update() {

    }

    private void OnTriggerEnter2D(Collider2D c) {
        if (c.gameObject.tag == "Projectile") {
            if (!padre.objectsInDeflectArea.Contains(c.gameObject)) {
                padre.objectsInDeflectArea.Add(c.gameObject);
            }
        } else if (c.GetComponent<CanBeDeflected>() != null) {
            if (!padre.objectsInDeflectArea.Contains(c.gameObject)) {
                padre.objectsInDeflectArea.Add(c.gameObject);
                Debug.Log(c.gameObject);
            }
        }



        //c.GetComponent<CanBeDeflected>() != null;

    }

    private void OnTriggerStay2D(Collider2D c) {
            if (c.gameObject.tag == "Projectile") {
                if (!padre.objectsInDeflectArea.Contains(c.gameObject)) {
                    padre.objectsInDeflectArea.Add(c.gameObject);
                }
            } else if (c.GetComponent<CanBeDeflected>() != null) {

            if (!padre.objectsInDeflectArea.Contains(c.gameObject)) {
                Debug.Log("ADDED");

                padre.objectsInDeflectArea.Add(c.gameObject);
            }
        }
    }

    private void OnTriggerExit2D(Collider2D c) {
        if (c.gameObject.tag == "Projectile") {
            padre.objectsInDeflectArea.Remove(c.gameObject);
        } else if (c.GetComponent<CanBeDeflected>() != null) {
            //Debug.Log("Bye");
            if (padre.objectsInDeflectArea.Contains(c.gameObject)) {
                padre.objectsInDeflectArea.Remove(c.gameObject);
            }
        }
    }
}
