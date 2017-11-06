using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PunchArea : MonoBehaviour {
    public List<GameObject> NearbyObjects;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
    private void OnTriggerEnter2D(Collider2D collision) {
        if (!NearbyObjects.Contains(collision.gameObject)){
            NearbyObjects.Add(collision.gameObject);
        }
    }

    private void OnTriggerExit2D(Collider2D collision) {
        if (NearbyObjects.Contains(collision.gameObject)) {
            NearbyObjects.Remove(collision.gameObject);
        }
    }

    public void Punch(Vector2 direction,float MAX_FORCE) {
        foreach(GameObject g in NearbyObjects) {
            if (g.GetComponent<Transformable>().isPunchable) {
                g.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.None;
                g.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezeRotation;
                g.GetComponent<Rigidbody2D>().AddForce(direction * MAX_FORCE, ForceMode2D.Impulse);
            }

        }

    }
}
