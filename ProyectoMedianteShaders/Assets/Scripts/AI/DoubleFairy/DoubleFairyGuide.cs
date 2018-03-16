using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoubleFairyGuide : DoubleObject {

    public List<FairySpot> fairySpotList;

    int targetIndex;
    public FairySpot currentSpot;
    float distanceFromPlayerThreshold;
    float max_Speed = 2.0f;
    // Use this for initialization
    Rigidbody2D rb;
    DoubleFairyGuide brotherScript;


    bool NotDAWN(DoubleObject d) {
        return d.worldAssignation != world.DAWN;
    }

    void Start() {
        distanceFromPlayerThreshold = 4.0f;
        brotherScript = brotherObject.GetComponent<DoubleFairyGuide>();
        targetIndex = 0;

        FairySpot[] fairySpotArray;

        fairySpotArray = GetComponentsInChildren<FairySpot>();

        if (worldAssignation == world.DAWN) {
            foreach (FairySpot f in fairySpotArray) {
                if (f.worldAssignation == world.DAWN) {
                    fairySpotList.Add(f);
                } else if (f.worldAssignation == world.DUSK) {
                    brotherScript.fairySpotList.Add(f);
                }
            }
        }


        canBounce = false;
        InitTransformable();
        isPunchable = false;
        isBreakable = false;
        interactuableBySmash = false;
        offset = GameLogic.instance.worldOffset;
        if (worldAssignation == world.DAWN) {
            GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Kinematic;
            transform.position += new Vector3(0, GameLogic.instance.worldOffset);
        } else {
            GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Dynamic;
        }
        rb = GetComponent<Rigidbody2D>();

        rb.mass = 5000;
        rb.gravityScale = 0;
    }

    protected override void BrotherBehavior() {
        Vector3 positionWithOffset;

        if (rb == null)
            rb = GetComponent<Rigidbody2D>();

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

    public override void Change() {
        if (rb == null)
            rb = GetComponent<Rigidbody2D>();
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
        }

    }

    protected override void AddToGameLogicList() {
        if (!added) {
            if (GameLogic.instance != null) {
                added = true;
                GameLogic.instance.transformableObjects.Add(gameObject);
                if (worldAssignation == world.DUSK) {
                    rb.isKinematic = false;
                }
            }
        }
    }


    void FairyBehaviour() {
        if (currentSpot == null) {
            if (Vector2.Distance(transform.position, fairySpotList[targetIndex].transform.position) < 1.0f) {
                currentSpot = fairySpotList[targetIndex];
                brotherScript.currentSpot = fairySpotList[targetIndex].brotherScript;
            } else {
                Debug.Log("Hace Cosas");
                Vector2 DesiredVelocity = fairySpotList[targetIndex].transform.position - transform.position;
                DesiredVelocity.Normalize();
                DesiredVelocity *= max_Speed;
                Vector2 SteeringForce = (DesiredVelocity - rb.velocity);
                SteeringForce /= max_Speed;
                Vector2 acceleration = SteeringForce;
                rb.velocity += acceleration * Time.deltaTime;
                rb.velocity.Normalize();
                rb.velocity *= max_Speed;
            }
        } else {
            if (!currentSpot.mustStopHere) {
                targetIndex++;
                brotherScript.targetIndex++;
            } else {
                if (Vector2.Distance(GameLogic.instance.currentPlayer.transform.position, transform.position) < distanceFromPlayerThreshold) {
                    currentSpot.spriteObject.SetActive(true);
                } else {
                    currentSpot.spriteObject.SetActive(false);
                    if (GameLogic.instance.currentPlayer.transform.position.x > transform.position.x + (distanceFromPlayerThreshold * 1.2f)) {
                        targetIndex++;
                    }


                }

            }


        }
    }

    // Update is called once per frame
    void Update() {
        AddToGameLogicList();
        BrotherBehavior();
        if ((worldAssignation == world.DAWN && dawn) || (worldAssignation == world.DUSK && !dawn)) {
            FairyBehaviour();

        }



    }
}
