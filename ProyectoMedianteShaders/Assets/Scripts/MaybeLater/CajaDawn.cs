using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CajaDawn : Transformable {
    public float offset;
    public GameObject cajaHermana;

    private void Start() {
        InitTransformable();
        //isPunchable = true;
    }



    private void Update() {
        AddToGameLogicList();
        BrotherBehavior();

    }

    void BrotherBehavior() {
        if (!dawn) {
            Vector3 posWithOffset = cajaHermana.transform.position;
            posWithOffset.y += offset;

            gameObject.transform.position = posWithOffset;
            gameObject.transform.rotation = cajaHermana.transform.rotation;

        }
        else {






        }
    }

    private void OnTriggerEnter2D(Collider2D collision) {
        if (collision.tag == "Ground") {
            //FreezeConstraints();
            Vector3 littleMov = transform.position;
            littleMov.y = transform.position.y - 0.03f;
            transform.position = littleMov;
        }
    }

    public override void Change() {
        if (dawn) {
            dawn = false;

            GetComponent<Rigidbody2D>().isKinematic = true;

        }
        else {
            dawn = true;

            GetComponent<Rigidbody2D>().isKinematic = false;
            gameObject.GetComponent<Rigidbody2D>().velocity = cajaHermana.GetComponent<Rigidbody2D>().velocity;

        }

    }

    protected override void LoadResources() {


    }

}
