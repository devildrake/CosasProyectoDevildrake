using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelEntrance : DoubleObject {
    // Use this for initialization
    Rigidbody2D rb;
    public int levelToLoad;
    public int sequenceLength;
    public GameObject interactionSprite;
    public float timeForSequence;
    public bool tryMovePlayer;
    public GameObject spriteTime;
    public GameObject spriteFragments;
    public GameObject spriteLevelNumb;
    public GameObject portalEffect;
    public LevelEntrance brotherScript;
    bool interacted;
    public bool finalBeta;
    public ParticleSystem[] particleSystems;
    public bool[] particleMustDo;
    bool waitAFrame;
    bool waitASecondFrame;

    //Temporizador para byPassear posible problema de Z al entrar en portales
    float timer =0;
    void Start() {

        if (GameLogic.instance != null) {
            if (!GameLogic.instance.playedTutorial) {
                GameLogic.instance.FinishTutorial();
            }
        }

        particleMustDo = new bool[3];

        interacted = false;
        brotherScript = brotherObject.GetComponent<LevelEntrance>();
        if(levelToLoad-1==2)
        GameLogic.instance.levelsData[2].completed = true;

        timeForSequence = 500;

        spriteTime.SetActive(false);
        spriteLevelNumb.SetActive(true);
        ///////////////




        interactionSprite.SetActive(false);
        InitTransformable();
        isPunchable = false;
        isBreakable = false;
        interactuableBySmash = false;
        offset = GameLogic.instance.worldOffset;
        if (worldAssignation == world.DAWN) {
            GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Kinematic;
            //GetComponent<SpriteRenderer>().sprite = imagenDawn;
        } else {
            //GetComponent<SpriteRenderer>().sprite = imagenDusk;

        }
        //float randomVal = Random.Range(1, 4);
        //Debug.Log(randomVal);
        //GetComponentInChildren<MeshRenderer>().gameObject.transform.rotation *= Quaternion.AngleAxis(randomVal * 90, new Vector3(0, 0, 1));

        if (GameLogic.instance.levelsData[levelToLoad-1].completed) {


            //Debug.Log(GameLogic.instance.completedLevels);


            activated = true;
            //Debug.Log("Activating Door");

        } else {
            //Debug.Log("DoorShouldBeDisabled");
        }

        rb = GetComponent<Rigidbody2D>();

        rb.mass = 5000;
        rb.gravityScale = 0;

        for(int i = 0; i < particleSystems.Length; i++) {
            if (particleSystems[i] != null) {
                particleSystems[i].Stop();
            }
        }



    }

    void FragmentsCheck() {
        bool temp = true;
        //string levels="";
        //string bools = "";

        for (int i = levelToLoad; i < (levelToLoad + sequenceLength); i++) {
           // levels += i.ToString();
          //  bools += GameLogic.instance.levelsData[i].fragment.ToString();
            if (!GameLogic.instance.levelsData[i].fragment) {
                temp = false;
            }
            //Debug.Log(GameLogic.instance.levelsData[i].fragment);
        }
            if (temp) {
                if (!GameLogic.instance.levelsData[levelToLoad].fragmentFeedBack) {
                    if (particleSystems[0] != null) {

                        if (!particleSystems[0].isEmitting||particleSystems[0].isStopped) {
                            particleSystems[0].Play();
                        particleMustDo[0] = true;

                    }
                    particleSystems[0].gameObject.transform.position = Vector3.MoveTowards(particleSystems[0].gameObject.transform.position, spriteFragments.transform.position, 10 * Time.deltaTime);
                        if ((particleSystems[0].gameObject.transform.position - spriteFragments.transform.position).magnitude < 0.1) {
                            particleSystems[0].startSpeed += 1;
                            particleSystems[0].Emit(200);
                            particleSystems[0].startSpeed -= 1;
                            spriteFragments.SetActive(true);
                            GameLogic.instance.levelsData[levelToLoad].fragmentFeedBack = false;
                        particleMustDo[0] = false;

                        particleSystems[0].Emit(1000);
                        //Destroy(particleSystems[0].gameObject);
                        particleSystems[0].Stop();

                        //ENTRA AQUI; HAY QUE METER AQUI PARTICLEDESU
                    }
                } else {
                        spriteFragments.SetActive(true);
                    }
                } else {
                    spriteFragments.SetActive(true);
                }
        } else {
                spriteFragments.SetActive(false);
            }

       // Debug.Log("My index is " + levelToLoad + " and I am checking fragments " + levels + " and the results are " + bools);

    }

    void ActivatedCheck() {
        //spriteLevelNumb.SetActive(GameLogic.instance.levelsData[levelToLoad - 1].completed);
        //portalEffect.SetActive(GameLogic.instance.levelsData[levelToLoad - 1].completed);
        //if (spriteLevelNumb.activeInHierarchy) {
        //    //Debug.Log("AHAHSDHASDS " + GameLogic.instance.levelsData[levelToLoad].completed);
        //} else {
        //    //Debug.Log(levelToLoad - 1 + "NO ESTA COMPLETADO -> " + GameLogic.instance.levelsData[levelToLoad - 1].completed);
        //}
        portalEffect.SetActive(false);

        if (GameLogic.instance.levelsData[levelToLoad - 1].completed) {
            if (!GameLogic.instance.levelsData[levelToLoad - 1].completedFeedBack) {
                if (particleSystems.Length > 2) {
                    if (particleSystems[1] != null) {
                        if (!particleSystems[1].isEmitting||particleSystems[1].isStopped) {
                            particleSystems[1].Play();
                            particleMustDo[1] = true;

                        }

                        particleSystems[1].gameObject.transform.position = Vector3.MoveTowards(particleSystems[1].gameObject.transform.position, spriteLevelNumb.transform.position, 10 * Time.deltaTime);
                        if ((particleSystems[1].gameObject.transform.position - spriteLevelNumb.transform.position).magnitude < 0.1) {
                            particleSystems[1].startSpeed += 1;
                            particleSystems[1].Emit(200);
                            particleSystems[1].startSpeed -= 1;
                            spriteFragments.SetActive(true);
                            Destroy(particleSystems[1].gameObject);
                            particleMustDo[1] = false;

                            GameLogic.instance.levelsData[levelToLoad - 1].completedFeedBack = true;
                            //ENTRA AQUI; HAY QUE METER AQUI PARTICLEDESU
                        }

                    } else {
                        spriteLevelNumb.SetActive(true);
                        portalEffect.SetActive(true);
                    }
                }
            } else {
                spriteLevelNumb.SetActive(true);
                portalEffect.SetActive(true);
            }
        } else {
            spriteLevelNumb.SetActive(false);
            portalEffect.SetActive(false);
        }
    }

    void TimeCheck() {
        if ((GameLogic.instance.levelsData[levelToLoad].completed)) {
            float total = 0.0f;
            for (int i = levelToLoad; i < levelToLoad + sequenceLength - 1; i++) {
                total += GameLogic.instance.levelsData[i].timeLapse;
            }
           // Debug.Log("Total = " + total);
           if (timeForSequence > total) {
                if (!GameLogic.instance.levelsData[levelToLoad].timeLapseFeedBack) {
                    if (particleSystems[2] != null) {

                        if (!particleSystems[2].isEmitting||particleSystems[2].isStopped) {
                            particleSystems[2].Play();
                            particleMustDo[2] = true;

                        }
                        particleSystems[2].gameObject.transform.position = Vector3.MoveTowards(particleSystems[2].gameObject.transform.position, spriteTime.transform.position, 10 * Time.deltaTime);
                        if ((particleSystems[2].gameObject.transform.position - spriteTime.transform.position).magnitude < 0.1) {
                            particleSystems[2].startSpeed += 1;
                            particleSystems[2].Emit(200);
                            particleSystems[2].startSpeed -= 1;
                            particleMustDo[2] = true;

                            spriteFragments.SetActive(true);
                            Destroy(particleSystems[2].gameObject);
                            GameLogic.instance.levelsData[levelToLoad].timeLapseFeedBack = true;
                            //ENTRA AQUI; HAY QUE METER AQUI PARTICLEDESU
                        }
                    } else {
                        spriteTime.SetActive(true);
                    }
                } else {
                    spriteTime.SetActive(true);
                }
            } else {
                spriteTime.SetActive(false);
            }


        }
    }

    protected override void AddToGameLogicList() {
        if (!added) {
            if (GameLogic.instance != null) {
                added = true;
                GameLogic.instance.transformableObjects.Add(gameObject);

                FragmentsCheck();
                ActivatedCheck();
                TimeCheck();
                if (spriteFragments.activeInHierarchy && spriteLevelNumb.activeInHierarchy && spriteTime.activeInHierarchy) {
                    spriteLevelNumb.GetComponent<SpriteRenderer>().color = Color.yellow;
                }



            }
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

    protected override void LoadResources() {
        if (worldAssignation == world.DAWN) {
            imagenDawn = Resources.Load<Sprite>("Presentacion/DawnSprites/DawnBox");
        } else {
            imagenDusk = Resources.Load<Sprite>("Presentacion/DuskSprites/DuskBox");

        }
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
        }

    }

    public override void Interact() {

        if (activated) {
            //GameLogic.instance.lastEntranceIndex = levelToLoad;
            //GameLogic.instance.LoadScene(levelToLoad);
            if (GameLogic.instance != null) {
                GameLogic.instance.pauseCanvas.nextSceneIndex = levelToLoad;
                GameLogic.instance.levelToLoad = levelToLoad;
                GameLogic.instance.lastEntranceIndex = levelToLoad;
                InputManager.BlockInput();
                GameLogic.instance.currentPlayer.placeToGo = gameObject;
                GameLogic.instance.currentPlayer.brotherScript.placeToGo = brotherObject;
                interacted = true;
            }
        }
    }

    public void OnTriggerEnter2D(Collider2D collision) {
        if (activated) {
            if (!InputManager.GetBlocked()) {
                if (collision.gameObject.tag == "Player") {
                    collision.gameObject.GetComponent<PlayerController>().interactableObject = gameObject.GetComponent<DoubleObject>();
                    if (interactionSprite != null)
                        interactionSprite.SetActive(true);
                }
            }
        }
    }
    
    public void OnTriggerExit2D(Collider2D collision) {
        if (collision.gameObject.tag == "Player") {
            collision.gameObject.GetComponent<PlayerController>().interactableObject = null;
            if (interactionSprite != null)
                interactionSprite.SetActive(false);
        }
    }

    void ParticleBusiness(int i) {

        switch (i) {
            case 0:
                spriteFragments.SetActive(false);
            particleSystems[0].gameObject.transform.position = Vector3.MoveTowards(particleSystems[0].gameObject.transform.position, spriteFragments.transform.position,  Time.deltaTime);
            if ((particleSystems[0].gameObject.transform.position - spriteFragments.transform.position).magnitude < 0.1) {
                particleSystems[0].startSpeed += 1;
                particleSystems[0].Emit(200);
                particleSystems[0].startSpeed -= 1;
                spriteFragments.SetActive(true);
                    GameLogic.instance.levelsData[levelToLoad].fragmentFeedBack = true;
                    Invoke("Save", 1.0f);


                    ParticleSystem.MainModule module = particleSystems[0].main;
                    ParticleSystem.ShapeModule shapeModule = particleSystems[0].shape;
                    module.maxParticles = 1000;
                    module.startLifetime = 0.4f;
                    module.startSpeed = 30;
                    shapeModule.radius = 0.01f;


                    particleSystems[0].Emit(1000);
                    //Destroy(particleSystems[0].gameObject);
                    particleSystems[0].Stop();
                    particleMustDo[0] = false;

                    //ENTRA AQUI; HAY QUE METER AQUI PARTICLEDESU
                }
                break;
            case 1:
                spriteLevelNumb.SetActive(false);
                particleSystems[1].gameObject.transform.position = Vector3.MoveTowards(particleSystems[1].gameObject.transform.position, spriteLevelNumb.transform.position, Time.deltaTime);
                if ((particleSystems[1].gameObject.transform.position - spriteLevelNumb.transform.position).magnitude < 0.1) {
                    particleSystems[1].startSpeed += 1;
                    particleSystems[1].Emit(200);
                    particleSystems[1].startSpeed -= 1;
                    spriteLevelNumb.SetActive(true);
                    portalEffect.SetActive(true);
                    GameLogic.instance.levelsData[levelToLoad - 1].completedFeedBack = true;
                    //ENTRA AQUI; HAY QUE METER AQUI PARTICLEDESU
                    Invoke("Save", 1.0f);



                    ParticleSystem.MainModule module = particleSystems[1].main;
                    ParticleSystem.ShapeModule shapeModule = particleSystems[1].shape;
                    module.maxParticles = 1000;
                    module.startLifetime = 0.4f;
                    module.startSpeed = 30;
                    shapeModule.radius = 0.01f;


                    particleSystems[1].Emit(1000);
                    //Destroy(particleSystems[0].gameObject);
                    particleSystems[1].Stop();
                    particleMustDo[1] = false;

                }
                break;
            case 2:
                spriteTime.SetActive(false);
                particleSystems[2].gameObject.transform.position = Vector3.MoveTowards(particleSystems[2].gameObject.transform.position, spriteTime.transform.position, Time.deltaTime);
                if ((particleSystems[2].gameObject.transform.position - spriteTime.transform.position).magnitude < 0.1) {
                    particleSystems[2].startSpeed += 1;
                    particleSystems[2].Emit(200);
                    particleSystems[2].startSpeed -= 1;
                    spriteTime.SetActive(true);
                    GameLogic.instance.levelsData[levelToLoad].timeLapseFeedBack = true;
                    Invoke("Save", 1.0f);



                    ParticleSystem.MainModule module = particleSystems[2].main;
                    ParticleSystem.ShapeModule shapeModule = particleSystems[2].shape;
                    module.maxParticles = 1000;
                    module.startLifetime = 0.4f;
                    module.startSpeed = 30;
                    shapeModule.radius = 0.01f;


                    particleSystems[2].Emit(1000);
                    //Destroy(particleSystems[0].gameObject);
                    particleSystems[2].Stop();
                    particleMustDo[2] = false;

                    //ENTRA AQUI; HAY QUE METER AQUI PARTICLEDESU
                }
                break;
        }
    }

    void Save() {
        GameLogic.instance.Save();
    }

    // Update is called once per frame
    void Update() {
        AddToGameLogicList();

        if (!waitAFrame) {
            waitAFrame = true;
        }else if (!waitASecondFrame) {
            waitASecondFrame = true;
        }

        if (added&&waitASecondFrame) {
            if (!finalBeta) {
                if (GameLogic.instance.currentPlayer != null && !interacted) {
                    if (!tryMovePlayer && levelToLoad == GameLogic.instance.lastEntranceIndex) {
                        GameLogic.instance.gameState = GameLogic.GameState.LEVEL;
                        if (worldAssignation == world.DAWN) {
                            if (!GameLogic.instance.setSpawnPoint) {
                                if (GameLogic.instance.currentPlayer.worldAssignation == world.DAWN) {
                                    GameLogic.instance.currentPlayer.transform.position = gameObject.transform.position + new Vector3(0, 0.55f, 2);
                                    GameLogic.instance.currentPlayer.brotherObject.transform.position = brotherObject.transform.position + new Vector3(0, 0.55f, 2);
                                    GameLogic.instance.SetSpawnPoint(new Vector3(gameObject.transform.position.x, gameObject.transform.position.y + 0.55f, -0.1085899f));
                                    GameLogic.instance.cameraTransition = true;

                                }
                                else {
                                    GameLogic.instance.currentPlayer.transform.position = brotherObject.transform.position + new Vector3(0, 0.55f, 2);
                                    GameLogic.instance.currentPlayer.brotherObject.transform.position = transform.position + new Vector3(0, 0.55f, 2);
                                    GameLogic.instance.SetSpawnPoint(new Vector3(brotherObject.transform.position.x, brotherObject.transform.position.y + 0.55f, -0.1085899f));
                                    GameLogic.instance.cameraTransition = true;

                                }
                                GameLogic.instance.timeElapsed = 0;
                                GameLogic.instance.pickedFragments = 0;
                                tryMovePlayer = true;
                                brotherScript.tryMovePlayer = true;
                                GameLogic.instance.setSpawnPoint = true;
                            }
                            else {
                                GameLogic.instance.timeElapsed = 0;
                                GameLogic.instance.pickedFragments = 0;
                                Debug.Log("GameLogic se adelaantó");
                                if (GameLogic.instance.currentPlayer.worldAssignation == world.DAWN) {
                                    GameLogic.instance.currentPlayer.transform.position = gameObject.transform.position + new Vector3(0, 0.55f, 2);
                                    GameLogic.instance.currentPlayer.brotherObject.transform.position = brotherObject.transform.position + new Vector3(0, 0.55f, 2);
                                    GameLogic.instance.SetSpawnPoint(new Vector3(gameObject.transform.position.x, gameObject.transform.position.y + 0.55f, -0.1085899f));
                                    GameLogic.instance.cameraTransition = true;

                                }
                                else {
                                    GameLogic.instance.currentPlayer.transform.position = brotherObject.transform.position + new Vector3(0, 0.55f, 2);
                                    GameLogic.instance.currentPlayer.brotherObject.transform.position = transform.position + new Vector3(0, 0.55f, 2);
                                    GameLogic.instance.SetSpawnPoint(brotherObject.transform.position + new Vector3(0, 0.55f, 0));
                                    GameLogic.instance.cameraTransition = true;

                                }
                                tryMovePlayer = true;
                                brotherScript.tryMovePlayer = true;

                            }
                        }
                        //Debug.Log(transform.position);
                        //Debug.Log(GameLogic.instance.currentPlayer.transform.position);
                        tryMovePlayer = true;
                        //brotherScript.tryMovePlayer = true;
                    } else if(!tryMovePlayer){
                        tryMovePlayer = true;
                    }





                }
                else if (interacted) {
                    timer += Time.deltaTime;
                    if (GameLogic.instance.currentPlayer.transform.position.z > 4||timer>2.0f) {
                        GameLogic.instance.LoadScene(levelToLoad);
                    }
                }
                for(int i = 0; i < particleSystems.Length; i++) {
                    if (particleSystems[i] != null) {
                        if (particleMustDo[i]) {
                            ParticleBusiness(i);
                        }
                    }
                }


            }
            else { 

            }
            BrotherBehavior();
        }
    }
}


