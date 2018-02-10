using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trampler : Agent {

    public float stunDuration;
    public float timeStunned;
    public Vector3 pointA;
    public Vector3 pointB;
    public GameObject objectA;
    public GameObject objectB;
    public float maxSpeed;
    public float currentSpeed;
    //Int para definir hacia donde debe moverse el trampler al cargar, 0 es hacia A, 1 es hacia B, 2 o cualquier otra cosa es aun no se sae
    public int whereTo;
    public LayerMask[] masks;
    public bool mustStop;
    public void ResetPoints() {
        if (worldAssignation == world.DAWN) {
            pointA = new Vector3(objectA.transform.position.x, objectA.transform.position.y, objectA.transform.position.z);
            pointB = new Vector3(objectB.transform.position.x, objectB.transform.position.y, objectB.transform.position.z);
        }
    }

    void Start() {
        masks = new LayerMask[2];

        masks[0] = LayerMask.GetMask("Ground");
        masks[1] = LayerMask.GetMask("Platform");

        ResetPoints();

        stunDuration = 2;
        timeStunned = 0;
        maxSpeed = 5;

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
                //brotherObject.GetComponent<FlyingSeed>().SwitchState(1, new SeedPathFollowState());
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
        }
    }



    // Update is called once per frame
    void Update() {
        GetComponent<Rigidbody2D>().gravityScale = 1;
        //if(dawnState!=null&&dawn&&worldAssignation==world.DAWN)
        //Debug.Log(dawnState.ToString());


        //if (duskState != null)
        //    print(duskState.ToString());

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
            SwitchState(0, new TramplerIdleState());
            SwitchState(1, new TramplerDraggableState());
            dawn = false;

        }
    }
}
