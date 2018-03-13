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

    void Start() {
        brotherScript = brotherObject.GetComponent<LevelEntrance>();
        if(levelToLoad-1==2)
        GameLogic.instance.levelsData[2].completed = true;

        timeForSequence = 500;

        spriteTime.SetActive(false);
        spriteLevelNumb.SetActive(true);
        ///////////////




        interactionSprite.SetActive(false);
        InitTransformable();
        isPunchable = true;
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
    }

    void FragmentsCheck() {
        for (int i = levelToLoad; i < (levelToLoad + sequenceLength); i++) {
            bool temp = true;
            if (!GameLogic.instance.levelsData[i].fragment) {
                temp = false;
            }

            spriteFragments.SetActive(temp);
            //Debug.Log(GameLogic.instance.levelsData[i].fragment);

        }


    }

    void ActivatedCheck() {
        spriteLevelNumb.SetActive(GameLogic.instance.levelsData[levelToLoad - 1].completed);
        portalEffect.SetActive(GameLogic.instance.levelsData[levelToLoad - 1].completed);
        if (spriteLevelNumb.activeInHierarchy) {
            //Debug.Log("AHAHSDHASDS " + GameLogic.instance.levelsData[levelToLoad].completed);
        } else {
            //Debug.Log(levelToLoad - 1 + "NO ESTA COMPLETADO -> " + GameLogic.instance.levelsData[levelToLoad - 1].completed);
        }


    }

    void TimeCheck() {
        if ((GameLogic.instance.levelsData[levelToLoad].completed)) {
            float total = 0.0f;
            for (int i = levelToLoad; i < levelToLoad + sequenceLength - 1; i++) {
                total += GameLogic.instance.levelsData[i].timeLapse;
            }

            spriteTime.SetActive(timeForSequence > total);

            Debug.Log("Total = " + total);

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
            GameLogic.instance.lastEntranceIndex = levelToLoad;
            GameLogic.instance.LoadScene(levelToLoad);
        }
    }

    public void OnTriggerEnter2D(Collider2D collision) {
        if (activated) {
            if (collision.gameObject.tag == "Player") {
                collision.gameObject.GetComponent<PlayerController>().interactableObject = gameObject.GetComponent<DoubleObject>();
                if (interactionSprite != null)
                    interactionSprite.SetActive(true);
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

    // Update is called once per frame
    void Update() {
        AddToGameLogicList();

        if (added) {
            if (!tryMovePlayer&&levelToLoad==GameLogic.instance.lastEntranceIndex) {
                if (worldAssignation == world.DAWN) {
                        if (GameLogic.instance.currentPlayer.worldAssignation == world.DAWN) {
                            GameLogic.instance.currentPlayer.transform.position = gameObject.transform.position+new Vector3(0, 0.55f, 2);
                            GameLogic.instance.currentPlayer.brotherObject.transform.position = brotherObject.transform.position + new Vector3(0, 0.55f, 2);
                            GameLogic.instance.SetSpawnPoint(gameObject.transform.position);

                        } else {
                        GameLogic.instance.currentPlayer.transform.position = brotherObject.transform.position + new Vector3(0, 0.55f, 2);
                        GameLogic.instance.currentPlayer.brotherObject.transform.position = transform.position + new Vector3(0, 0.55f, 2);
                        GameLogic.instance.SetSpawnPoint(brotherObject.transform.position);

                        }
                        tryMovePlayer = true;
                        brotherScript.tryMovePlayer = true;


                }
                //Debug.Log(transform.position);
                //Debug.Log(GameLogic.instance.currentPlayer.transform.position);
                tryMovePlayer = true;
                //brotherScript.tryMovePlayer = true;
            }


        }

        BrotherBehavior();
    }
}


