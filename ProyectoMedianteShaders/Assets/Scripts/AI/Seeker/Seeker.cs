using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Seeker : Agent {
    public float visionRange = 10.0f;
    public float coneAngle = 45.0f;
    public Vector3 lastPlayerPosSeen;
    public float chaseSpeed = 2;
    public Vector3 orbitPos;
    public bool rising;
    public GameObject tentacles;
    float tentacleHideTime;
    float tentacleHideTimer;
    //[Tooltip]

    [Header("Hay que setear esto con objetos ajenos (no hijos de este)")]
    public Transform[] Path_Points = new Transform[0];
    public int currentTarget;
    public Transform target;
    public float timeOutOfSight;
    public bool increasing;

    [Header("Esto se setea por codigo")]
    public Vector3[] Path_Positions;
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
        timeOutOfSight = 0;
        visionRange = 10;
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

        if (Path_Points.Length > 0) {
            Path_Positions = new Vector3[Path_Points.Length];

            for (int i = 0; i < Path_Points.Length; i++) {

                //Path_Points[i].position = new Vector3(Path_Points[i].position.x, Path_Points[i].position.y+GameLogic.instance.worldOffset, Path_Points[i].position.z);

                Path_Positions[i] = new Vector3(Path_Points[i].position.x, Path_Points[i].position.y+GameLogic.instance.worldOffset, Path_Points[i].position.z);

            }
        }

        //Debug.Log(Path_Points[0].position);


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

    private void OnCollisionEnter2D(Collision2D collision) {
        if (dawn && worldAssignation == world.DAWN) {
            if (collision.collider.tag == "Player") {
                GameLogic.instance.KillPlayer();
            }
        }
    }

    public override void Change() {
        //if (GameLogic.instance.currentPlayer != null) 
        //target = GameLogic.instance.currentPlayer.transform;


        currentTarget = 0;

        //El objeto que modifica a ambos haciendo de controlador es el que pertenece a Dawn
        if (worldAssignation == world.DAWN) {
            SwitchState(0, new SeekerPathFollowState());
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

        if (GetComponent<Rigidbody2D>().velocity.x > 0) {
            transform.localScale = new Vector3(1.0f,1.0f,1.0f);
        } else {
            transform.localScale = new Vector3(-1.0f, 1.0f, 1.0f);
        }

        AddToGameLogicList();
        BrotherBehavior();
        StartAI();


        if (worldAssignation == world.DAWN)
            DawnBehavior();
        else
            DuskBehavior();

    }

    public void ResetOrbit() {
        orbitPos = gameObject.transform.position;
        brotherObject.GetComponent<Seeker>().orbitPos = brotherObject.transform.position;

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
