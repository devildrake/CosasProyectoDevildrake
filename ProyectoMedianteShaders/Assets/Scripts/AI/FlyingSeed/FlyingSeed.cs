﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlyingSeed : Agent {
    //static public int totalPathPoints;
    public Transform[] Path_Points = new Transform[0];
    public Vector3[] VectorPatrolPoints;
    public int currentTarget;
    public Vector3 orbitPos;
    //public bool stompedOn;
    public float timeOnTheGround=0;
    public bool rising;

    public float fallTimer;
    public float fallTime;
    public GameObject rabitoGiratorio;
    public GameObject grabbedObject;
    public Vector2 grabOffset;
    public GameObject detectStompObject;
    public float timeCastingBlowUp;

    FlyingSeed brotherScript;
    //Rigidbody2D rb;

    public void DropObject() {
        if (grabbedObject != null) {
            grabbedObject = null;
            grabOffset = new Vector2(0, 0);
            brotherScript.grabbedObject = null;
            brotherScript.grabOffset = new Vector2(0, 0);

        }
    }

    public void GrabObject(GameObject g){
        GetComponent<Rigidbody2D>().velocity = new Vector2(0, 0);
        grabbedObject = g;
        DoubleObject grabbedDoubleObject = g.GetComponent<DoubleObject>();

        grabOffset = g.transform.position - gameObject.transform.position;

        brotherScript.grabbedObject = grabbedDoubleObject.brotherObject;
        brotherScript.grabOffset = grabbedDoubleObject.brotherObject.transform.position - brotherObject.transform.position;

    }

    public void CheckForObjects() {
        LayerMask[] mascaras = new LayerMask[3];
        mascaras[0] = LayerMask.GetMask("Platform");
        mascaras[1] = LayerMask.GetMask("Enemy");
        mascaras[2] = LayerMask.GetMask("Platform");


        RaycastHit2D hit2D = PlayerUtilsStatic.RayCastArrayMask(transform.position - new Vector3(0, 0.5f, 0), Vector3.down, 0.05f, mascaras);
        if (hit2D) {
            Debug.Log("Trying to grab" + hit2D.collider.gameObject);
            GrabObject(hit2D.collider.gameObject);
            Debug.DrawRay(transform.position - new Vector3(0f, 0.5f, 0f), Vector2.down);
        } else {
        hit2D =  Physics2D.Raycast(transform.position - new Vector3(0, 0.5f, 0), Vector3.down, 0.05f, LayerMask.GetMask("Walkable"));
            if (hit2D) {
                print("Bruh");
                if (GetComponentInParent<CanBeGrabbed>() != null) {
                    GrabObject(hit2D.collider.gameObject);
                }
            }
        }

    }

    public void BlowUp() {
        GameObject ProjectilePrefab = Resources.Load<GameObject>("Prefabs/DoubleProjectile");


        Vector3 direction;

        if (!dawn)
            direction = ((GameLogic.instance.currentPlayer.gameObject.transform.position - gameObject.transform.position).normalized);
        else
            direction = ((GameLogic.instance.currentPlayer.gameObject.transform.position - brotherObject.transform.position).normalized);

        GameObject temp = Instantiate(ProjectilePrefab, transform.position + new Vector3(direction.x*1.2f,direction.y*1.2f,0), Quaternion.identity) as GameObject;

        DoubleProjectile[] projectiles = temp.GetComponentsInChildren<DoubleProjectile>();

        projectiles[0].gameObject.GetComponent<DoubleProjectile>().initialSpeed = direction * 2;// * Time.deltaTime;
        projectiles[1].gameObject.GetComponent<DoubleProjectile>().initialSpeed = direction * 2;// * Time.deltaTime;
        projectiles[0].gameObject.GetComponent<Rigidbody2D>().velocity = direction * 2;// * Time.deltaTime;
        projectiles[1].gameObject.GetComponent<Rigidbody2D>().velocity = direction * 2;// * Time.deltaTime;
        projectiles[0].gameObject.GetComponent<Rigidbody2D>().gravityScale = 0;
        projectiles[1].gameObject.GetComponent<Rigidbody2D>().gravityScale = 0;
    }


    void Start() {
        rb = GetComponent<Rigidbody2D>();
        brotherScript = brotherObject.GetComponent<FlyingSeed>();
        fallTime = 1.0f;

        if (Path_Points.Length != 0) {
            if (Path_Points[0] != null) {
                VectorPatrolPoints = new Vector3[Path_Points.Length];

                for (int i = 0; i < VectorPatrolPoints.Length; i++) {
                    VectorPatrolPoints[i] = new Vector3(Path_Points[i].position.x, Path_Points[i].position.y, Path_Points[i].position.z);
                    Destroy(Path_Points[i].gameObject);
                }
            }
        }


        stompedOn = false;
        currentTarget = 0;
        InitTransformable();

        isPunchable = false;
        isBreakable = false;
        interactuableBySmash = false;
        offset = GameLogic.instance.worldOffset;
        if (worldAssignation == world.DAWN) {
            rb.bodyType = RigidbodyType2D.Kinematic;
        } else {

        }

    }




    protected override void BrotherBehavior() {
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
        if (rb == null) {
            rb = GetComponent<Rigidbody2D>();
        }
        if (brotherScript.rb == null) {
            brotherScript.rb = brotherObject.GetComponent<Rigidbody2D>();
        }


        //El objeto que modifica a ambos haciendo de controlador es el que pertenece a Dawn
        if (worldAssignation == world.DAWN) {
            //Si antes del cambio estaba en dawn, pasara a hacerse kinematic y al otro dynamic, además de darle su velocidad
            if (dawn) {
                dawnState = new SeedIdleState();
                dominantVelocity = rb.velocity;
                brotherScript.dominantVelocity = rb.velocity;
                brotherScript.rb.bodyType = RigidbodyType2D.Dynamic;
                rb.bodyType = RigidbodyType2D.Kinematic;
                OnlyFreezeRotation();
                brotherScript.rb.velocity = dominantVelocity;
                rb.velocity = new Vector2(0.0f, 0.0f);
                if (duskState != null)
                    duskState.OnEnter(this);

            }
            //Si antes del cambio estaba en dusk, pasara a hacerse dynamic y al otro kinematic, además de darle su velocidad 
            else {
                brotherScript.SwitchState(1, new SeedPathFollowState());
                touchedByPlayer = false;
                dominantVelocity = brotherScript.rb.velocity;
                brotherScript.dominantVelocity = brotherScript.rb.velocity;
                rb.bodyType = RigidbodyType2D.Dynamic;
                brotherScript.rb.bodyType = RigidbodyType2D.Kinematic;
                brotherScript.rb.velocity = new Vector2(0.0f, 0.0f);
                rb.velocity = dominantVelocity;
                if (dawnState != null)
                    dawnState.OnEnter(this);

            }

            dawn = !dawn;
            brotherScript.dawn = !brotherScript.dawn;
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
        }
    }

    public void Spin(float angularSpeed) {
        if (rabitoGiratorio != null) {
            //Debug.Log("Spin");
            rabitoGiratorio.transform.Rotate(new Vector3(0, 0, 1), angularSpeed * Time.deltaTime);
        }
    }
    //SUAVIZAR SLIDE CON TIMER

    // Update is called once per frame
    void Update() {


        //if(duskState!=null)
        //print(duskState.ToString());

        AddToGameLogicList();
        BrotherBehavior();
        StartAI();


        if (worldAssignation == world.DAWN)
            DawnBehavior();
        else
            DuskBehavior();

        if(grabbedObject != null) {
            grabbedObject.transform.position = gameObject.transform.position + (Vector3)grabOffset;
        }

    }

    protected override void StartAI() {
        if (!startedAI && added) {
            startedAI = true;
            SwitchState(0, new SeedIdleState());
            SwitchState(1, new SeedPathFollowState());
            dawn = false;

        }
    }
}

