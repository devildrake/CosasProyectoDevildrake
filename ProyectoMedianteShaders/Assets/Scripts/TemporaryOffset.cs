using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TemporaryOffset : DoubleObject {
    int localKillCount;
    public Vector3 additionalOffset;
    Rigidbody2D rb;
    GameObject player;
    public float temporalCameraAttenuation;

    void Start() {
        localKillCount = 0;
        InitTransformable();
        offset = GameLogic.instance.worldOffset;
        if (worldAssignation == world.DAWN) {
            GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Kinematic;
        }
        rb = GetComponent<Rigidbody2D>();
        rb.mass = 5000;
        rb.gravityScale = 0;
    }

    protected override void BrotherBehavior() {
        Vector3 positionWithOffset;
        if (GetComponent<Rigidbody2D>().bodyType == RigidbodyType2D.Kinematic) {
            positionWithOffset = brotherObject.transform.position;

            if (worldAssignation == world.DAWN)
                positionWithOffset.y += offset;
            else {
                positionWithOffset.y -= offset;
            }

            transform.position = positionWithOffset;
            transform.rotation = brotherObject.transform.rotation;

        }

    }

    protected override void LoadResources() {

    }

    public override void Change() {


        //El objeto que modifica a ambos haciendo de controlador es el que pertenece a Dawn
        if (worldAssignation == world.DAWN) {
            //Si antes del cambio estaba en dawn, pasara a hacerse kinematic y al otro dynamic, además de darle su velocidad
            if (dawn) {
                dominantVelocity = GetComponent<Rigidbody2D>().velocity;
                brotherObject.GetComponent<DoubleObject>().dominantVelocity = GetComponent<Rigidbody2D>().velocity;
                brotherObject.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Dynamic;
                GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Kinematic;
                OnlyFreezeRotation();
                brotherObject.GetComponent<Rigidbody2D>().velocity = dominantVelocity;
                GetComponent<Rigidbody2D>().velocity = new Vector2(0.0f, 0.0f);
            }
            //Si antes del cambio estaba en dusk, pasara a hacerse dynamic y al otro kinematic, además de darle su velocidad 
            else {
                dominantVelocity = brotherObject.GetComponent<Rigidbody2D>().velocity;
                brotherObject.GetComponent<DoubleObject>().dominantVelocity = brotherObject.GetComponent<Rigidbody2D>().velocity;
                GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Dynamic;
                brotherObject.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Kinematic;
                brotherObject.GetComponent<Rigidbody2D>().velocity = new Vector2(0.0f, 0.0f);
                GetComponent<Rigidbody2D>().velocity = dominantVelocity;
            }

            dawn = !dawn;
            brotherObject.GetComponent<DoubleObject>().dawn = !brotherObject.GetComponent<DoubleObject>().dawn;
            if (player != null) {
                brotherObject.GetComponent<TemporaryOffset>().player = player;
            }else if (brotherObject.GetComponent<TemporaryOffset>().player != null) {
                player = brotherObject.GetComponent<TemporaryOffset>().player;
            }

        }

    }


    void Update() {
        AddToGameLogicList();
        BrotherBehavior();
    }

    private void OnTriggerEnter2D(Collider2D collision) {
        if (collision.tag == "Player") {
            GameLogic.instance.additionalOffset = additionalOffset;
            player = collision.gameObject;
            if (temporalCameraAttenuation != 0) {
                GameLogic.instance.cameraAttenuation = temporalCameraAttenuation;
            } else {
                GameLogic.instance.cameraAttenuation = 1;
            }

        }   
    }


    private void OnTriggerStay2D(Collider2D collision) {
        if (player == null) {
            if (collision.tag == "Player") {
                GameLogic.instance.additionalOffset = additionalOffset;
                player = collision.gameObject;
                if (temporalCameraAttenuation != 0) {
                    GameLogic.instance.cameraAttenuation = temporalCameraAttenuation;
                } else {
                    GameLogic.instance.cameraAttenuation = 1;
                }
            }
        }
    }

    public void ResetOffset() {
        GameLogic.instance.additionalOffset = new Vector3(0, 0, 0);
    }

    //private void OnTriggerExit2D(Collider2D collision) {
    //    if (collision.tag == "Player") {
    //        ResetOffset();
    //        player = null;
    //    }
    //}

}
