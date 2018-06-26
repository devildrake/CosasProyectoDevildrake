using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyWalker : DoubleObject {
    ///Objeto de la mesh
    public GameObject meshObject;

    ///Bool que regula si este es estático o patrulla
    public bool isStatic;

    Rigidbody rb;
    public LayerMask groundMask;
    public float bounceForce;
    public float velocity;
    float threshold = 0.2f;
    float maxSpeed = 2;
    bool once;

    public Transform[] PatrolPoints;
    public Vector3[] VectorPatrolPoints;
    bool goingA;
    float timeSinceStompedOn;

    //El Objeto que esta en DAWN pilla las posiciones de los patrolPoints y los destruye
    void Start() {
        if (worldAssignation == world.DAWN) {
            VectorPatrolPoints = new Vector3[2];
            VectorPatrolPoints[0] = new Vector3(PatrolPoints[0].position.x, PatrolPoints[0].position.y, PatrolPoints[0].position.z);
            VectorPatrolPoints[1] = new Vector3(PatrolPoints[1].position.x, PatrolPoints[1].position.y, PatrolPoints[1].position.z);
            Destroy(PatrolPoints[0].gameObject);
            Destroy(PatrolPoints[1].gameObject);
        }

        timeSinceStompedOn = 0.5f;
        bounceForce = 10.5f;
        velocity = 2.5f;
        InitTransformable();
        isPunchable = false;
        isBreakable = false;
        interactuableBySmash = false;
        offset = GameLogic.instance.worldOffset;
        if (worldAssignation == world.DAWN) {
            //GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Kinematic;
            rb.isKinematic = true;
        } 

        rb = GetComponent<Rigidbody>();
        groundMask = LayerMask.GetMask("Ground");

        rb.mass = 5000;
        rb.constraints = RigidbodyConstraints.FreezeAll;
    }

    protected override void BrotherBehavior() {
        Vector3 positionWithOffset;
        //if (GetComponent<Rigidbody2D>().bodyType == RigidbodyType2D.Kinematic) {
        if (rb.isKinematic) { 
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
                dominantVelocity = GetComponent<Rigidbody>().velocity;
                brotherObject.GetComponent<DoubleObject>().dominantVelocity = GetComponent<Rigidbody>().velocity;
                //brotherObject.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Dynamic;

                //GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Kinematic;

                if (rb == null) {
                    rb = GetComponent<Rigidbody>();
                }

                brotherObject.GetComponent<Rigidbody>().isKinematic = false;
                rb.isKinematic = true;

                OnlyFreezeRotation();
                brotherObject.GetComponent<Rigidbody>().velocity = dominantVelocity;
                GetComponent<Rigidbody>().velocity = new Vector3(0.0f,0.0f, 0.0f);
            }
            //Si antes del cambio estaba en dusk, pasara a hacerse dynamic y al otro kinematic, además de darle su velocidad 
            else {
                dominantVelocity = brotherObject.GetComponent<Rigidbody>().velocity;
                brotherObject.GetComponent<DoubleObject>().dominantVelocity = brotherObject.GetComponent<Rigidbody>().velocity;
                //GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Dynamic;
                rb.isKinematic = false;
                brotherObject.GetComponent<Rigidbody>().isKinematic = true;

                //brotherObject.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Kinematic;
                brotherObject.GetComponent<Rigidbody>().velocity = new Vector3(0.0f, 0.0f,0.0f);
                GetComponent<Rigidbody>().velocity = dominantVelocity;
            }

            dawn = !dawn;
            brotherObject.GetComponent<DoubleObject>().dawn = !brotherObject.GetComponent<DoubleObject>().dawn;
        }

    }

    //Comportamiento en dawn, castea un rayo hacia donde esta moviendose y si encuentra algo con layerMask Ground, cambia su dirección
    void DawnBehavior() {

        if (GetComponentInChildren<Animator>() != null) {
            GetComponentInChildren<Animator>().SetBool("isStatic", isStatic);
        }

        if (!isStatic) {

            RaycastHit2D hit2D;

            if (rb.velocity.x > 0) {
                hit2D = Physics2D.Raycast(transform.position+new Vector3(0,0.5f,0), Vector3.right, 1, LayerMask.GetMask("Platform"));
            } else {
                hit2D = Physics2D.Raycast(transform.position+ new Vector3(0, 0.5f, 0), Vector3.left, 1, LayerMask.GetMask("Platform"));
            }
            if (hit2D){
                goingA = !goingA;
            }



            if (goingA) {
                if (Mathf.Abs(VectorPatrolPoints[0].x - transform.position.x) > threshold) {
                    velocity = VectorPatrolPoints[0].x - transform.position.x;
                } else {
                    goingA = false;
                }
            } else {
                if (Mathf.Abs(VectorPatrolPoints[1].x - transform.position.x) > threshold) {
                    velocity = VectorPatrolPoints[1].x - transform.position.x;
                } else {
                    goingA = true;
                }
            }

            if (velocity > 0) {
                //rb.velocity = new Vector3(maxSpeed, 0,0);
                transform.Translate(new Vector3(maxSpeed * Time.deltaTime, 0, 0));
            } else {
                //rb.velocity = new Vector3(-maxSpeed, 0,0);
                transform.Translate(new Vector3(-maxSpeed * Time.deltaTime, 0, 0));
            }
        }
    }

    //Velocidad a 0 si es el de Dusk
    void DuskBehavior() {
        if (!dawn) {
            rb.velocity = new Vector3(0,0, 0);
                GetComponentInChildren<Animator>().SetBool("Jumped", timeSinceStompedOn < 0.4f);
        }
    }

    //Si colisiona en dawn el personaje, lo mata, si lo hace en dusk con velocidad y inferior o igual a 0, bota
    private void OnTriggerEnter(Collider other) {

        //Debug.Log(other.tag);

        if (dawn && worldAssignation == world.DAWN) {
            if (other.tag == "Player") {
                other.GetComponent<PlayerController>().Kill();
            }
        }else if (!dawn && worldAssignation == world.DUSK) {
            if (other.tag == "Player") {

                timeSinceStompedOn = 0;
                if (other.GetComponent<Rigidbody>().velocity.y <= 0) {
                    other.GetComponent<Rigidbody>().velocity = new Vector3(other.GetComponent<Rigidbody>().velocity.x, 0,0);
                    SoundManager.Instance.PlayEvent("event:/Enemies/Bouncer/Bounce", transform);
                    other.GetComponent<Rigidbody>().AddForce(new Vector3(0, 1 * bounceForce,0), ForceMode.Impulse);
                    other.GetComponent<PlayerController>().SetCanDash(true);
                    //Debug.Log(other.GetComponent<Rigidbody2D>().velocity);
                }
            } else if (other.GetComponent<DoubleObject>() != null) {
                if (other.GetComponent<DoubleObject>().canBounce) {
                    timeSinceStompedOn = 0;
                    SoundManager.Instance.PlayEvent("event:/Enemies/Bouncer/Bounce", transform);
                    other.GetComponent<Rigidbody>().velocity = new Vector3(other.GetComponent<Rigidbody>().velocity.x, 0,0);
                    other.GetComponent<Rigidbody>().AddForce(new Vector3(0, other.GetComponent<Rigidbody>().mass * bounceForce,0), ForceMode.Impulse);
                }
            }
        }
    }

    //Si colisiona en dawn el personaje, lo mata, si lo hace en dusk con velocidad y inferior o igual a 0, bota
    private void OnTriggerStay(Collider other) {
        if (dawn && worldAssignation == world.DAWN) {
            if (other.tag == "Player") {
                other.GetComponent<PlayerController>().Kill();
            }
        } else if (!dawn && worldAssignation == world.DUSK) {
            if (other.tag == "Player") {
                if (other.GetComponent<Rigidbody>().velocity.y <= 0&&!other.GetComponent<PlayerController>().grounded) {
                    timeSinceStompedOn = 0;
                    //Debug.Log(other.GetComponent<Rigidbody2D>());
                    other.GetComponent<Rigidbody>().velocity = new Vector3(other.GetComponent<Rigidbody>().velocity.x, 0,0.0f);
                    other.GetComponent<Rigidbody>().AddForce(new Vector3(0, 1 * bounceForce,0),ForceMode.Impulse);
                }
            } else if (other.GetComponent<DoubleObject>() != null) {
                if (other.GetComponent<DoubleObject>().canBounce) {
                    timeSinceStompedOn = 0;
                    SoundManager.Instance.PlayEvent("event:/Enemies/Bouncer/Bounce", transform);
                    other.GetComponent<Rigidbody>().velocity = new Vector3(other.GetComponent<Rigidbody>().velocity.x, 0, 0);
                    other.GetComponent<Rigidbody>().AddForce(new Vector3(0, other.GetComponent<Rigidbody>().mass * bounceForce, 0), ForceMode.Impulse);
                }
            }
        }
    }


    //MËTODO PARA ARREGLAR UNA TONTERIA POR LA QUE NO VA BIEN EL WALKER (SOLO SE LLAMA UNA VES)
    void Nen() {
        if (!once&&GameLogic.instance.transformableObjects[0]!=null) {
            if (GameLogic.instance.transformableObjects[0].GetComponent<DoubleObject>().dawn != dawn) {
                dawn = !dawn;
                once = true;
            }
        }
    }

    // Update is called once per frame
    void Update() {



        AddToGameLogicList();

        if (added) {
            if (timeSinceStompedOn < 0.5f) {
                timeSinceStompedOn += Time.deltaTime;
            }
            //Este método es una mierda que he tenido que meter, es una guarrada pero hace ByPass de un problemilla que solo está en este script
            Nen();

            //Se rota de forma que haga lo que deberia en funcion de la velociad que lleva
            if (meshObject != null) {
                if (rb.velocity.x > 0) {
                    meshObject.transform.rotation = Quaternion.identity;
                    meshObject.transform.rotation = Quaternion.identity * Quaternion.AngleAxis(90, new Vector3(0, 1, 0));
                } else if (rb.velocity.x < 0) {
                    meshObject.transform.rotation = Quaternion.identity * Quaternion.AngleAxis(270, new Vector3(0, 1, 0));
                }
            }

            BrotherBehavior();


            if (worldAssignation == world.DAWN)
                DawnBehavior();
            else
                DuskBehavior();

        }
    }
}
