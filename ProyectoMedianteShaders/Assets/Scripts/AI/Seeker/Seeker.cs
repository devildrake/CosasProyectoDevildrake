using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Seeker : Agent {

    public Vector3 orbitPos;
    public bool rising;
    public GameObject tentacles;
    float tentacleHideTime;
    float tentacleHideTimer;
    public Transform[] Path_Points = new Transform[0];
    public int currentTarget;
    public bool playerInVisionCone;


    public void HideTentacles() {
        tentacleHideTimer = 0;
        if (worldAssignation == world.DUSK) {
            tentacles.SetActive(false);
        }
    }

    public void ShowTentacles() {
        if (worldAssignation == world.DUSK) {
            tentacles.SetActive(true);
        }
    }

    void Start() {
        if (worldAssignation == world.DUSK) {
            tentacleHideTimer = 0;
            tentacleHideTime = 3;
            tentacles = GetComponentInChildren<TriggerDetectionPlayer>().gameObject;
        }



        rising = true;
        stompedOn = false;
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
                //dawnState = new SeedIdleState();
                dominantVelocity = GetComponent<Rigidbody2D>().velocity;
                brotherObject.GetComponent<DoubleObject>().dominantVelocity = GetComponent<Rigidbody2D>().velocity;
                brotherObject.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Dynamic;
                GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Kinematic;
                OnlyFreezeRotation();
                brotherObject.GetComponent<Rigidbody2D>().velocity = dominantVelocity;
                GetComponent<Rigidbody2D>().velocity = new Vector2(0.0f, 0.0f);
                if (duskState != null)
                    duskState.OnEnter(this);

            }
            //Si antes del cambio estaba en dusk, pasara a hacerse dynamic y al otro kinematic, además de darle su velocidad 
            else {
                touchedByPlayer = false;
                dominantVelocity = brotherObject.GetComponent<Rigidbody2D>().velocity;
                brotherObject.GetComponent<DoubleObject>().dominantVelocity = brotherObject.GetComponent<Rigidbody2D>().velocity;
                GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Dynamic;
                brotherObject.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Kinematic;
                brotherObject.GetComponent<Rigidbody2D>().velocity = new Vector2(0.0f, 0.0f);
                GetComponent<Rigidbody2D>().velocity = dominantVelocity;
                if (dawnState != null)
                    dawnState.OnEnter(this);

            }

            dawn = !dawn;
            brotherObject.GetComponent<DoubleObject>().dawn = !brotherObject.GetComponent<DoubleObject>().dawn;
        }

    }

    void DawnBehavior() {
        if (dawn && dawnState != null) {
            dawnState.Update(this, Time.deltaTime);
        }
    }
    void DuskBehavior() {
        if (!dawn && duskState != null) {
            duskState.Update(this, Time.deltaTime);
            if (!tentacles.gameObject.activeInHierarchy) {
                tentacleHideTimer += Time.deltaTime;
                if (tentacleHideTimer > tentacleHideTime) {
                    tentacleHideTimer = 0;
                    ShowTentacles();
                }
            }
        }
    }



    // Update is called once per frame
    void Update() {
        //GetComponent<Rigidbody2D>().gravityScale = 1;



        AddToGameLogicList();
        BrotherBehavior();
        StartAI();


        if (worldAssignation == world.DAWN)
            DawnBehavior();
        else
            DuskBehavior();

    }

    protected override void StartAI() {
        if (!startedAI && added) {
            startedAI = true;
            SwitchState(0, new SeekerPathFollowState());
            SwitchState(1, new SeekerIdleState());
            dawn = false;

        }
    }
}
