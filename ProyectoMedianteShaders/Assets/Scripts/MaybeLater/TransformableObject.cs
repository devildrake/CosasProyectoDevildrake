using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TransformableObject : Transformable {
    Vector2 speed;
    Vector2 position;
    public GameObject objectDawn;
    public GameObject objectDusk;
    float offset = 20;
    // Use this for initialization
    void Start () {
        isPunchable = true;
	}
	
	// Update is called once per frame
	void Update () {
        
        if (dawn) {
            speed = objectDawn.GetComponent<Rigidbody2D>().velocity;
            position = objectDawn.transform.position;
            objectDusk.transform.position = new Vector2(position.x, position.y+ offset);

        }else {
            speed = objectDusk.GetComponent<Rigidbody2D>().velocity;
            position = objectDusk.transform.position;
            objectDawn.transform.position = new Vector2(position.x, position.y + offset);
        }

	}

    public override void Change() {
        Debug.Log("CambiandoCaja");
        if (dawn) {
            objectDusk.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Dynamic;
            objectDusk.GetComponent<Rigidbody2D>().velocity = speed;
            objectDawn.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Kinematic;

            dawn = false;
        }else {
            objectDawn.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Dynamic;
            objectDawn.GetComponent<Rigidbody2D>().velocity = speed;
            objectDusk.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Kinematic;
            dawn = true;
        }
    }
}
