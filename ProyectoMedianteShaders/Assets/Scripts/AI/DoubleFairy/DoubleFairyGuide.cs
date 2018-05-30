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
    public GameObject bocadilloRendererObject;
    public GameObject feedBackRendererObject;
    public GameObject fairyModel;

    public SpriteRenderer feedBackRenderer;
    public SpriteRenderer bocadilloRenderer;

    float myAlpha;
    float idleTimer=0;
    bool setIdle;
    public int currentIdlePattern = 0;
    public bool hasAMessage;
    public bool messageSet;
    bool NotDAWN(DoubleObject d) {
        return d.worldAssignation != world.DAWN;
    }

    float dotAnimationTimer;

    Sprite spriteE;
    Sprite spriteB;

    [SerializeField]
    Sprite[] dotsAnimation;

    void Start() {
        PauseCanvas.textIndex = -1;
        PauseCanvas.lastIndex = -1;

        dotAnimationTimer = 0;
        spriteE = Resources.Load<Sprite>("Sprites/Fairy/EButton") as Sprite;
        spriteB = Resources.Load<Sprite>("Sprites/Fairy/BButton") as Sprite;
        dotsAnimation = new Sprite[3];
        dotsAnimation[0] = Resources.Load<Sprite>("Sprites/Fairy/dots/Puntos1") as Sprite;
        dotsAnimation[1] = Resources.Load<Sprite>("Sprites/Fairy/dots/Puntos2") as Sprite;
        dotsAnimation[2] = Resources.Load<Sprite>("Sprites/Fairy/dots/Puntos3") as Sprite;


        setIdle = false;
        //spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        feedBackRendererObject = feedBackRenderer.gameObject;
        bocadilloRendererObject = bocadilloRenderer.gameObject;
        myAlpha = 0;

        distanceFromPlayerThreshold = 1.5f;
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
        feedBackRendererObject.SetActive(true);
        bocadilloRendererObject.SetActive(true);
        dawn = false;
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
            brotherScript.dawn = dawn;
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

                if (GameLogic.instance.currentPlayer.interactableObject == this) {
                    GameLogic.instance.currentPlayer.interactableObject = null;
                    GameLogic.instance.eventState = GameLogic.EventState.NONE;
                    PauseCanvas.textIndex = -1;
                    PauseCanvas.lastIndex = -1;
                }

                if (Vector2.Distance(transform.position, fairySpotList[targetIndex].transform.position) < 1.0f) {
                    currentSpot = fairySpotList[targetIndex];

                    brotherScript.currentSpot = fairySpotList[targetIndex].brotherScript;

                    

                    ////////////////////////////////////////////////////////////////////////////////////////////////////////////
                    //if (spriteRendererObject != null) {
                    //    if (currentSpot.messageSprite != null) {
                    //        //spriteRendererObject.SetActive(false);
                    //        FadeOut();
                    //        spriteRenderer.sprite = currentSpot.messageSprite;
                    //        brotherScript.spriteRenderer.sprite = currentSpot.brotherScript.messageSprite;
                    //    } else {
                    //        spriteRenderer.sprite = null;
                    //        brotherScript.spriteRenderer.sprite = null;
                    //    }
                    //}
                    //rb.velocity = new Vector2(0, 0);
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

                    brotherScript.FadeOut();
                    if (GameLogic.instance.currentPlayer.interactableObject == this) {
                        GameLogic.instance.currentPlayer.interactableObject = null;
                        GameLogic.instance.eventState = GameLogic.EventState.NONE;
                        PauseCanvas.textIndex = -1;
                        PauseCanvas.lastIndex = -1;
                    }

            } else {

                    if (!setIdle) {
                        setIdle = true;
                        currentIdlePattern = Random.Range(0, 6);
                        brotherScript.setIdle = true;
                        brotherScript.currentIdlePattern = currentIdlePattern;

                        //Debug.Log(currentIdlePattern);
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
                        if (feedBackRenderer != null) {
                            //if (feedBackRenderer.sprite != null) {
                                if (GameLogic.instance.currentPlayer != null) {
                                    GameLogic.instance.currentPlayer.interactableObject = this;
                                }

                                if (!InputManager.gamePadConnected) {
                                    feedBackRenderer.sprite = spriteE;
                                    //spriteRenderer.transform.localScale = new Vector3(0.11f, 0.11f, 0.11f);
                                    //feedBackRenderer.transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);

                                } else {
                                    feedBackRenderer.sprite = spriteB;
                                   // feedBackRenderer.transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);
                                    //spriteRenderer.transform.localScale = new Vector3(0.11f, 0.11f, 0.11f);
                                }

                                FadeIn();

                            //} else {

                            //}
                        } else {
                            Debug.Log("Null SpriteRenderers");

                        }
                    } else {
                        if (GameLogic.instance.currentPlayer != null) {
                            if(GameLogic.instance.currentPlayer.interactableObject == this) {
                                    GameLogic.instance.currentPlayer.interactableObject = null;
                                    GameLogic.instance.eventState = GameLogic.EventState.NONE;
                                PauseCanvas.textIndex = -1;
                                PauseCanvas.lastIndex = -1;
                            }
                        }

                        if (!hasAMessage) {
                            FadeOut();
                            brotherScript.FadeOut();
                        } else{
                            int currentFrame=0;

                            if (dotAnimationTimer > 0.3f && dotAnimationTimer < 0.6f)
                                currentFrame = 1;
                            else if (dotAnimationTimer > 0.6f && dotAnimationTimer < 0.9f)
                                currentFrame = 2;
                            else if (dotAnimationTimer > 0.9f) {
                                dotAnimationTimer = 0;
                                currentFrame = 0;
                            }
                            feedBackRenderer.sprite = dotsAnimation[currentFrame];



                            dotAnimationTimer += Time.deltaTime;

                            FadeIn();
                            brotherScript.FadeIn();
                        }
                    }

                    if (currentSpot.idLastMessage-currentSpot.idFirstMessage>0) {
                        if (GameLogic.instance.currentPlayer.transform.position.x > transform.position.x + distanceFromPlayerThreshold*2.5) {
                            currentSpot = null;
                            messageSet = false;
                            brotherScript.currentSpot = null;
                            setIdle = false;
                            brotherScript.setIdle = false;

                            targetIndex++;
                            brotherScript.targetIndex++;
                            //spriteRendererObject.SetActive(false);
                            FadeOut();
                            brotherScript.FadeOut();
                        } else if(!messageSet){
                            hasAMessage = true;
                            brotherScript.messageSet = true;
                            brotherScript.hasAMessage = true;

                            messageSet = true;
                        }
                    } else {
                        if (GameLogic.instance.currentPlayer.transform.position.x >= transform.position.x) {
                            currentSpot = null;
                            brotherScript.currentSpot = null;
                            setIdle = false;
                            brotherScript.setIdle = false;
                            messageSet = false;

                            targetIndex++;
                            brotherScript.targetIndex++;
                            //spriteRendererObject.SetActive(false);
                            FadeOut();
                            hasAMessage = false;
                            brotherScript.hasAMessage = false;
                            brotherScript.FadeOut();
                        }
                    }



                }
            }
        } else {
            FadeOut();

            brotherScript.FadeOut();
        }
        
    }

    public override void Interact() {
        base.Interact();

        if (currentSpot != null) {
            if (PauseCanvas.lastIndex != currentSpot.idLastMessage) {
                PauseCanvas.lastIndex = currentSpot.idLastMessage;
                PauseCanvas.textIndex = currentSpot.idFirstMessage - 1;

                GameLogic.instance.eventState = GameLogic.EventState.TEXT;
            }

            PauseCanvas.textIndex++;

            if (PauseCanvas.textIndex > PauseCanvas.lastIndex) {
                GameLogic.instance.eventState = GameLogic.EventState.NONE;
                PauseCanvas.lastIndex = -1;
                hasAMessage = false;
                brotherScript.hasAMessage = false;
            }
        }

    }

    void FadeIn() {
        if (myAlpha < 0) {
            myAlpha = 0;
        }


        if (feedBackRenderer.color.a < 0.75f) {
            myAlpha += Time.deltaTime;
            //if (feedBackRenderer.sprite != null) {
                feedBackRenderer.color = new Color(feedBackRenderer.color.r, feedBackRenderer.color.g, feedBackRenderer.color.b, myAlpha);
            //}
            bocadilloRenderer.color = new Color(bocadilloRenderer.color.r, bocadilloRenderer.color.g, bocadilloRenderer.color.b, myAlpha);

        }
        brotherScript.myAlpha = myAlpha;

    }

    void FadeOut() {


        if (myAlpha < 0) {
            myAlpha = 0;
        }

        if (feedBackRenderer.color.a > 0) {

            myAlpha -= Time.deltaTime;
            //if (feedBackRenderer.sprite != null) {
                feedBackRenderer.color = new Color(feedBackRenderer.color.r, feedBackRenderer.color.g, feedBackRenderer.color.b, myAlpha);
            //}
            bocadilloRenderer.color = new Color(bocadilloRenderer.color.r, bocadilloRenderer.color.g, bocadilloRenderer.color.b, myAlpha);
        }
        brotherScript.myAlpha = myAlpha;

    }

    // Update is called once per frame
    void Update() {

        AddToGameLogicList();
        BrotherBehavior();
        if ((worldAssignation == world.DAWN && dawn) || (worldAssignation == world.DUSK && !dawn)) {
            if (GameLogic.instance != null && GameLogic.instance.currentPlayer != null) {
                FairyBehaviour();
            }

        } else {


        }



        if (added) {
            fairySpotList.RemoveAll(isNotFromThisWorld);
        }


    }
}
