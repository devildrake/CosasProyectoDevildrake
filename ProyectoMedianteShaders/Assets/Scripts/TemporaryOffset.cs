using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TemporaryOffset : DoubleObject {
    //int localKillCount;
    public Vector3 additionalOffset;
    Rigidbody2D rb;
    GameObject player;
    public float temporalCameraAttenuation;
    TemporaryOffset brotherScript;

    void Start() {
        //localKillCount = 0;
        InitTransformable();
        offset = GameLogic.instance.worldOffset;
        rb = GetComponent<Rigidbody2D>();
        rb.mass = 5000;
        rb.gravityScale = 0;
        brotherScript = brotherObject.GetComponent<TemporaryOffset>();



        if (worldAssignation == world.DAWN) {
            rb.bodyType = RigidbodyType2D.Kinematic;
        }

    }

    protected override void BrotherBehavior() {
        if (rb == null)
            rb = GetComponent<Rigidbody2D>();

        if (brotherScript == null)
            brotherScript = brotherObject.GetComponent<TemporaryOffset>();

        Vector3 positionWithOffset;
        if (rb.bodyType == RigidbodyType2D.Kinematic) {
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
        if (rb == null)
            rb = GetComponent<Rigidbody2D>();

        if(brotherScript==null)
        brotherScript = brotherObject.GetComponent<TemporaryOffset>();

        if (brotherScript.rb == null)
            brotherScript.rb = GetComponent<Rigidbody2D>();

        //El objeto que modifica a ambos haciendo de controlador es el que pertenece a Dawn
        if (worldAssignation == world.DAWN) {
            //Si antes del cambio estaba en dawn, pasara a hacerse kinematic y al otro dynamic, además de darle su velocidad
            if (dawn) {
                dominantVelocity = rb.velocity;
                brotherScript.dominantVelocity = rb.velocity;
                brotherScript.rb.bodyType = RigidbodyType2D.Dynamic;
                rb.bodyType = RigidbodyType2D.Kinematic;
                OnlyFreezeRotation();
                brotherScript.rb.velocity = dominantVelocity;
                rb.velocity = new Vector2(0.0f, 0.0f);
            }
            //Si antes del cambio estaba en dusk, pasara a hacerse dynamic y al otro kinematic, además de darle su velocidad 
            else {
                dominantVelocity = brotherScript.rb.velocity;
                brotherScript.dominantVelocity = brotherScript.rb.velocity;
                rb.bodyType = RigidbodyType2D.Dynamic;
                brotherScript.rb.bodyType = RigidbodyType2D.Kinematic;
                brotherScript.rb.velocity = new Vector2(0.0f, 0.0f);
                rb.velocity = dominantVelocity;
            }

            dawn = !dawn;
            brotherScript.dawn = !brotherScript.dawn;
            if (player != null) {
                brotherScript.player = player;
            }else if (brotherScript.player != null) {
                player = brotherScript.player;
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
