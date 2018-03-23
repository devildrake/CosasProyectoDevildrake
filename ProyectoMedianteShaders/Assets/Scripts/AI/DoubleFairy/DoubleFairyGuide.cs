using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoubleFairyGuide : DoubleObject {

    public List<FairySpot> fairySpotList;
    int targetIndex;
    public FairySpot currentSpot;
    float distanceFromPlayerThreshold;
    float max_Speed = 6.0f;
    // Use this for initialization
    Rigidbody2D rb;
    DoubleFairyGuide brotherScript;
    public GameObject spriteRendererObject;
    public GameObject fairyModel;
    public SpriteRenderer spriteRenderer;
    float myAlpha;
    float idleTimer=0;
    bool setIdle;
    public int currentIdlePattern = 0;

    bool NotDAWN(DoubleObject d) {
        return d.worldAssignation != world.DAWN;
    }

    void Start() {
        setIdle = false;
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        spriteRendererObject = spriteRenderer.gameObject;
        myAlpha = 0;

        distanceFromPlayerThreshold = 4.0f;
        brotherScript = brotherObject.GetComponent<DoubleFairyGuide>();
        targetIndex = 0;

        FairySpot[] fairySpotArray;

        fairySpotArray = GetComponentsInChildren<FairySpot>();

        if (worldAssignation == world.DAWN) {
            foreach (FairySpot f in fairySpotArray) {
                if (f.worldAssignation == world.DAWN) {
                    fairySpotList.Add(f);
                    f.transform.parent = transform.parent;
                } else {
                    f.transform.parent = transform.parent;
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
        spriteRendererObject.SetActive(true);
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

    bool isNotFromThisWorld(FairySpot f) {

        return worldAssignation != f.worldAssignation;
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
                brotherScript.fairyModel.transform.localPosition = fairyModel.transform.localPosition;
                brotherScript.idleTimer = idleTimer;
            }
            //Si antes del cambio estaba en dusk, pasara a hacerse dynamic y al otro kinematic, además de darle su velocidad 
            else {
                dominantVelocity = brotherScript.rb.velocity;
                brotherScript.dominantVelocity = brotherScript.rb.velocity;
                rb.bodyType = RigidbodyType2D.Dynamic;
                brotherScript.rb.bodyType = RigidbodyType2D.Kinematic;
                brotherScript.rb.velocity = new Vector2(0.0f, 0.0f);
                rb.velocity = dominantVelocity;
                fairyModel.transform.localPosition = brotherScript.fairyModel.transform.localPosition;
                idleTimer = brotherScript.idleTimer;

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
        if (targetIndex < fairySpotList.Count) {
            if (currentSpot == null) {
                fairyModel.transform.localPosition = new Vector3(0, 0, 0);
                idleTimer = 0;
                FadeOut();
                if (Vector2.Distance(transform.position, fairySpotList[targetIndex].transform.position) < 1.0f) {
                    currentSpot = fairySpotList[targetIndex];

                    brotherScript.currentSpot = fairySpotList[targetIndex].brotherScript;

                    if (spriteRendererObject != null) {
                        if (currentSpot.messageSprite != null) {
                            //spriteRendererObject.SetActive(false);
                            FadeOut();
                            spriteRenderer.sprite = currentSpot.messageSprite;
                            brotherScript.spriteRenderer.sprite = currentSpot.brotherScript.messageSprite;
                        } else {
                            spriteRenderer.sprite = null;
                            brotherScript.spriteRenderer.sprite = null;
                        }
                    }
                    //rb.velocity = new Vector2(0, 0);
                    Debug.Log(currentSpot);
                } else {
                    Vector2 DesiredVelocity = fairySpotList[targetIndex].transform.position - transform.position;
                    DesiredVelocity.Normalize();
                    DesiredVelocity *= max_Speed;
                    Vector2 SteeringForce = (DesiredVelocity - rb.velocity);
                    SteeringForce /= max_Speed;
                    Vector2 acceleration = SteeringForce*2;
                    rb.velocity += acceleration * Time.deltaTime;

                    //rb.velocity.Normalize();

                    //rb.velocity *= max_Speed;


                    //rb.velocity = rb.velocity * max_Speed;

                }
            } else {
                if (!currentSpot.mustStopHere) {
                    setIdle = false;
                    brotherScript.setIdle = false;
                    targetIndex++;
                    brotherScript.targetIndex++;
                    currentSpot = null;
                    brotherScript.currentSpot = null;
                    //spriteRendererObject.SetActive(false);
                    FadeOut();
                } else {

                    if (!setIdle) {
                        setIdle = true;
                        currentIdlePattern = Random.Range(0, 6);
                        brotherScript.setIdle = true;
                        brotherScript.currentIdlePattern = currentIdlePattern;

                        Debug.Log(currentIdlePattern);
                    }
                    rb.velocity = new Vector2(0, 0);

                    //////IDLE
                    float idleX = 0, idleY=0;


                    switch (currentIdlePattern) {
                        #region switchIdle
                        case 0:
                            idleTimer += Time.deltaTime;
                            idleX = Mathf.Cos(4 * idleTimer) - Mathf.Pow(Mathf.Cos(1 * idleTimer), 3);
                            idleY = Mathf.Sin(4 * idleTimer) - Mathf.Pow(Mathf.Sin(1 * idleTimer), 3);

                            fairyModel.transform.Translate(new Vector2(idleX * Time.deltaTime, idleY * Time.deltaTime));

                            break;
                        case 1:
                            idleTimer += Time.deltaTime * 2;


                            idleX = Mathf.Sin(-90 + idleTimer) * 1;
                            idleY = Mathf.Cos(90 + idleTimer) * 1;

                            fairyModel.transform.Translate(new Vector2(idleX * Time.deltaTime, idleY * Time.deltaTime));
                            break;
                        case 2:
                            idleTimer += Time.deltaTime;
                            idleX = Mathf.Cos(1 * idleTimer) - Mathf.Pow(Mathf.Cos(5 * idleTimer), 3);
                            idleY = Mathf.Sin(1 * idleTimer) - Mathf.Pow(Mathf.Sin(5 * idleTimer), 3);

                            fairyModel.transform.Translate(new Vector2(idleX * Time.deltaTime, idleY * Time.deltaTime));

                            break;
                        case 3:
                            idleTimer += Time.deltaTime;
                            idleX = Mathf.Cos(1 * idleTimer) - Mathf.Pow(Mathf.Cos(3 * idleTimer), 3);
                            idleY = Mathf.Sin(3 * idleTimer) - Mathf.Pow(Mathf.Sin(1 * idleTimer), 3);

                            fairyModel.transform.Translate(new Vector2(idleX * Time.deltaTime, idleY * Time.deltaTime));

                            break;
                        case 4:
                            idleTimer += Time.deltaTime;
                            idleX = Mathf.Cos(5 * idleTimer) - Mathf.Pow(Mathf.Cos(1 * idleTimer), 3);
                            idleY = Mathf.Sin(5 * idleTimer) - Mathf.Pow(Mathf.Sin(1 * idleTimer), 3);

                            fairyModel.transform.Translate(new Vector2(idleX * Time.deltaTime, idleY * Time.deltaTime));

                            break;
                        case 5:
                            idleTimer += Time.deltaTime;
                            idleX = Mathf.Cos(1 * idleTimer) - Mathf.Pow(Mathf.Cos(2 * idleTimer), 3);
                            idleY = Mathf.Sin(1 * idleTimer) - Mathf.Pow(Mathf.Sin(3 * idleTimer), 3);

                            fairyModel.transform.Translate(new Vector2(idleX * Time.deltaTime, idleY * Time.deltaTime));

                            break;
                    #endregion
                    }

                    //////IDLE

                    if (Vector2.Distance(GameLogic.instance.currentPlayer.transform.position, transform.position) < distanceFromPlayerThreshold) {
                        if (spriteRenderer != null) {
                            if (spriteRenderer.sprite != null) {
                                FadeIn();

                            } else {
                            }
                        } else {
                            Debug.Log("Null SpriteRenderers");

                        }
                    } else {
                        FadeOut();
                    }

                    if (currentSpot.messageSprite != null) {
                        if (GameLogic.instance.currentPlayer.transform.position.x > transform.position.x + distanceFromPlayerThreshold/2) {
                            currentSpot = null;
                            brotherScript.currentSpot = null;
                            setIdle = false;
                            brotherScript.setIdle = false;

                            targetIndex++;
                            brotherScript.targetIndex++;
                            //spriteRendererObject.SetActive(false);
                            FadeOut();

                        }
                    } else {
                        if (GameLogic.instance.currentPlayer.transform.position.x >= transform.position.x) {
                            currentSpot = null;
                            brotherScript.currentSpot = null;
                            setIdle = false;
                            brotherScript.setIdle = false;


                            targetIndex++;
                            brotherScript.targetIndex++;
                            //spriteRendererObject.SetActive(false);
                            FadeOut();

                        }
                    }



                }
            }
        } else {
            FadeOut();
        }
        
    }

    void FadeIn() {
        if (myAlpha < 0) {
            myAlpha = 0;
        }


        if (spriteRenderer.color.a < 0.75f) {
            myAlpha += Time.deltaTime;
            if (spriteRenderer.sprite != null) {
                spriteRenderer.color = new Color(spriteRenderer.color.r, spriteRenderer.color.g, spriteRenderer.color.b, myAlpha);
            }
        }
        brotherScript.myAlpha = myAlpha;

    }

    void FadeOut() {
        if (myAlpha < 0) {
            myAlpha = 0;
        }

        if (spriteRenderer.color.a > 0) {

            myAlpha -= Time.deltaTime;
            if (spriteRenderer.sprite != null) {
                spriteRenderer.color = new Color(spriteRenderer.color.r, spriteRenderer.color.g, spriteRenderer.color.b, myAlpha);
            }
        }
        brotherScript.myAlpha = myAlpha;

    }

    // Update is called once per frame
    void Update() {

        AddToGameLogicList();
        BrotherBehavior();
        if ((worldAssignation == world.DAWN && dawn) || (worldAssignation == world.DUSK && !dawn)) {
            FairyBehaviour();

        }

        if (added) {
            fairySpotList.RemoveAll(isNotFromThisWorld);
        }


    }
}
