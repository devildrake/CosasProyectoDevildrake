using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoubleKillerMist : DoubleObject {
    int localKillCount;
    Rigidbody2D rb;
    public Transform target;
    float currentSpeed;
    public float MAX_Speed;
    public float MIN_Speed;
    public float MAX_Distance;

    void Start() {
        InitTransformable();
        isPunchable = false;
        isBreakable = false;
        interactuableBySmash = false;
        offset = GameLogic.instance.worldOffset;
        if (worldAssignation == world.DAWN) {
            GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Kinematic;
        }

        rb = GetComponent<Rigidbody2D>();

        rb.mass = 5000;
        rb.gravityScale = 0;
        currentSpeed = 0;

        if (worldAssignation == world.DUSK) {
            originalPos = transform.position;
            brotherObject.GetComponent<DoubleKillerMist>().originalPos = transform.position + new Vector3(0,GameLogic.instance.worldOffset,0);
        }
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

    void BecomePunchable() {
        isPunchable = true;
    }

    public override void Change() {
        if (added) {
            if (GameLogic.instance.currentPlayer.worldAssignation == worldAssignation) {
                target = GameLogic.instance.currentPlayer.transform;
            } else {
                target = GameLogic.instance.currentPlayer.brotherObject.transform;
            }
        }


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
        }

    }

    protected override void AddToGameLogicList() {
        if (!added) {
            if (GameLogic.instance != null) {
                added = true;
                GameLogic.instance.transformableObjects.Add(gameObject);
                if (GameLogic.instance.currentPlayer != null)
                    target = GameLogic.instance.currentPlayer.transform;
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision) {
        if (collision.tag == "Player") {
            GameLogic.instance.KillPlayer();
        }
    }

    void ResetPos() {
            transform.position = originalPos;
            //brotherObject.transform.position = brotherObject.GetComponent<DoubleKillerMist>().transform.position;
    }

    // Update is called once per frame
    void Update() {
        AddToGameLogicList();
        BrotherBehavior();
        //Debug.Log(currentSpeed);
        if (target != null) { 
        float currentDistance = Vector2.Distance(target.position, transform.position);
        currentSpeed = Mathf.Clamp((currentDistance / MAX_Distance * MAX_Speed), MIN_Speed, MAX_Speed);
            GetComponent<Rigidbody2D>().velocity = (((target.position - transform.position).normalized) * currentSpeed);
            Debug.Log(GetComponent<Rigidbody2D>().velocity);
        } else if(GameLogic.instance!=null){
            Debug.Log("NullTarget");
            if (GameLogic.instance.currentPlayer != null) {
                if (GameLogic.instance.currentPlayer.worldAssignation == worldAssignation) {
                    target = GameLogic.instance.currentPlayer.transform;
                } else {
                    target = GameLogic.instance.currentPlayer.brotherObject.transform;
                }


            }
        }

        if (localKillCount < GameLogic.instance.timesDied) {
            ResetPos();
            localKillCount++;
        }


    }
}
