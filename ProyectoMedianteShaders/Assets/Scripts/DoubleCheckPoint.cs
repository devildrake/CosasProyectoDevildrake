using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DoubleCheckPoint : DoubleObject
{
    public bool endLevel;
    bool interacted;
    float counterFeedback;
    float maxTimeFeedback = 5;
    public GameObject interactionSprite;
    public AudioClip interactSound;
    public ParticleSystem particulasInteraccion;
    // Use this for initialization
    void Start()
    {
        InitTransformable();
        offset = GameLogic.instance.worldOffset;
        GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezeAll;
        if(particulasInteraccion!=null)
        particulasInteraccion.Stop();
        if (worldAssignation == world.DAWN)
        {
            GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Kinematic;
        }
        if (interactionSprite != null)
            interactionSprite.SetActive(false);
    }

    protected override void BrotherBehavior()
    {
        Vector3 positionWithOffset;
        if (GetComponent<Rigidbody2D>().bodyType == RigidbodyType2D.Kinematic)
        {
            positionWithOffset = brotherObject.transform.position;

            if (worldAssignation == world.DAWN)
                positionWithOffset.y += offset;
            else
            {
                positionWithOffset.y -= offset;
            }

            transform.position = positionWithOffset;
            transform.rotation = brotherObject.transform.rotation;

        }

    }



    // Update is called once per frame
    void Update() {
        AddToGameLogicList();
        BrotherBehavior();
        if (interacted) {
            if (particulasInteraccion != null) {

                if (!particulasInteraccion.isPlaying)
                    particulasInteraccion.Play();

                counterFeedback += Time.deltaTime;
                if (counterFeedback > maxTimeFeedback) {
                    counterFeedback = 0;
                    interacted = false;
                    particulasInteraccion.Stop();

                }
            } else {

            }
        }
    }

    public override void Interact() {
        interacted = true;
        if (!endLevel) {
            if (worldAssignation == world.DAWN) {
                GameLogic.instance.SetSpawnPoint(brotherObject.gameObject.transform.position);
            } else {
                GameLogic.instance.SetSpawnPoint(gameObject.transform.position);
            }
        } else {
            //CODIGO DE ACABAR NIVEL
            //SceneManager.LoadScene(0);
            //GameLogic.instance.levelsData[GameLogic.instance.GetCurrentLevelIndex()].fragment = true;
            GameLogic.instance.additionalOffset = new Vector3(-2, 0, 3);

            InputManager.BlockInput();
            //GameLogic.instance.levelFinished = true;
            //Debug.Log(GameLogic.instance.levelFinished);
        }
        if (GetComponent<AudioSource>() != null) {
            GetComponent<AudioSource>().clip = interactSound;
            GetComponent<AudioSource>().volume = 0.1f;
            GetComponent<AudioSource>().Play();
        } else {
            //Debug.Log("No Audio Source");
        }
        
    }

    private void OnTriggerStay2D(Collider2D collision) {
        if (collision.gameObject.tag == "Player") {
            PlayerController localPlayer = collision.gameObject.GetComponent<PlayerController>();
            if (endLevel) {
                if (localPlayer.grounded) {
                    //collision.gameObject.GetComponent<PlayerController>().interactableObject = gameObject.GetComponent<DoubleObject>();
                    //if(interactionSprite!=null)
                    //interactionSprite.SetActive(true);
                    localPlayer.placeToGo = gameObject;
                    localPlayer.brotherScript.placeToGo = gameObject;
                    //Debug.Log(gameObject.tag);

                    localPlayer.mustEnd = true;
                    localPlayer.brotherScript.mustEnd = true;

                    if (!interacted)
                        Interact();
                } else {

                }
            } else {
                if (!interacted)
                    Interact();
            }

        }
    }

    public void OnTriggerEnter2D(Collider2D collision) {
        if (collision.gameObject.tag == "Player") {
            PlayerController localPlayer = collision.gameObject.GetComponent<PlayerController>();
            if (endLevel) {
                if (localPlayer.grounded) {
                    //collision.gameObject.GetComponent<PlayerController>().interactableObject = gameObject.GetComponent<DoubleObject>();
                    //if(interactionSprite!=null)
                    //interactionSprite.SetActive(true);
                    localPlayer.placeToGo = gameObject;
                    localPlayer.brotherScript.placeToGo = gameObject;
                    //Debug.Log(gameObject.tag);

                    localPlayer.mustEnd = true;
                    localPlayer.brotherScript.mustEnd = true;
                    if (!interacted)
                        Interact();
                }
            } else {
                if (!interacted)
                    Interact();
            }

        }
    }

    //public void OnTriggerStay2D(Collider2D collision) {
    //    if (collision.gameObject.tag == "Player") {
    //        //collision.gameObject.GetComponent<PlayerController>().interactableObject = gameObject.GetComponent<DoubleObject>();
    //        //if (interactionSprite != null)
    //        //   interactionSprite.SetActive(true);
    //        Interact();

    //    }
    //}

    public void OnTriggerExit2D(Collider2D collision) {
        //if (collision.gameObject.tag == "Player") {
        //    collision.gameObject.GetComponent<PlayerController>().interactableObject = null;
        //    if (interactionSprite != null)
        //        interactionSprite.SetActive(false);
        //}
    }

    protected override void LoadResources() {
        interactSound = Resources.Load<AudioClip>("Sounds/Interact");
    }
}

