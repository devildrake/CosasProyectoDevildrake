using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyWalker : DoubleObject {
    public bool isStatic;
    Rigidbody2D rb;
    public LayerMask groundMask;
    public float bounceForce;
    public float velocity;
    float threshold = 0.2f;
    float maxSpeed = 2;
    bool once;

    public Transform[] PatrolPoints;
    public Vector3[] VectorPatrolPoints;
    bool goingA;

    void Start() {
        if (worldAssignation == world.DAWN) {
            VectorPatrolPoints = new Vector3[2];
            VectorPatrolPoints[0] = new Vector3(PatrolPoints[0].position.x, PatrolPoints[0].position.y, PatrolPoints[0].position.z);
            VectorPatrolPoints[1] = new Vector3(PatrolPoints[1].position.x, PatrolPoints[1].position.y, PatrolPoints[1].position.z);
            Destroy(PatrolPoints[0].gameObject);
            Destroy(PatrolPoints[1].gameObject);
        }



        bounceForce = 10.5f;
        velocity = 2.5f;
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
        float randomVal = Random.Range(1, 4);
        //Debug.Log(randomVal);
        //GetComponentInChildren<MeshRenderer>().gameObject.transform.rotation *= Quaternion.AngleAxis(randomVal * 90, new Vector3(0, 0, 1));

        rb = GetComponent<Rigidbody2D>();
        groundMask = LayerMask.GetMask("Ground");

        rb.mass = 1;
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

    //Comportamiento en dawn, castea un rayo hacia donde esta moviendose y si encuentra algo con layerMask Ground, cambia su dirección
    void DawnBehavior() {

        if (GetComponentInChildren<Animator>() != null) {
            GetComponentInChildren<Animator>().SetBool("isStatic", isStatic);
        }

        if (!isStatic) {
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
                GetComponent<Rigidbody2D>().velocity = new Vector2(maxSpeed, 0);
            } else {
                GetComponent<Rigidbody2D>().velocity = new Vector2(-maxSpeed, 0);
            }
        }
    }
    //Velocidad a 0
    void DuskBehavior() {
        if (!dawn) {
            GetComponent<Rigidbody2D>().velocity = new Vector2(0, 0);
        }
    }

    //Si colisiona en dawn el personaje, lo mata, si lo hace en dusk con velocidad y inferior o igual a 0, bota
    private void OnTriggerEnter2D(Collider2D other) {

        //Debug.Log(other.tag);

        if (dawn && worldAssignation == world.DAWN) {
            if (other.tag == "Player") {
                other.GetComponent<PlayerController>().Kill();
            }
        }else if (!dawn && worldAssignation == world.DUSK) {
            if (other.tag == "Player") {

                if (other.GetComponent<Rigidbody2D>().velocity.y <= 0) {
                    other.GetComponent<Rigidbody2D>().velocity = new Vector2(other.GetComponent<Rigidbody2D>().velocity.x, 0);
                    other.GetComponent<Rigidbody2D>().AddForce(new Vector2(0, 1 * bounceForce), ForceMode2D.Impulse);
                    //Debug.Log(other.GetComponent<Rigidbody2D>().velocity);
                }
            }
        }
    }

    //Si colisiona en dawn el personaje, lo mata, si lo hace en dusk con velocidad y inferior o igual a 0, bota
    private void OnTriggerStay2D(Collider2D other) {
        if (dawn && worldAssignation == world.DAWN) {
            if (other.tag == "Player") {
                other.GetComponent<PlayerController>().Kill();
            }
        } else if (!dawn && worldAssignation == world.DUSK) {
            if (other.tag == "Player") {
                if (other.GetComponent<Rigidbody2D>().velocity.y <= 0&&!other.GetComponent<PlayerController>().grounded) {
                    Debug.Log(other.GetComponent<Rigidbody2D>());
                    other.GetComponent<Rigidbody2D>().velocity = new Vector2(other.GetComponent<Rigidbody2D>().velocity.x, 0);
                    other.GetComponent<Rigidbody2D>().AddForce(new Vector2(0, 1 * bounceForce),ForceMode2D.Impulse);
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

            Nen();

            if (GetComponent<Rigidbody2D>().velocity.x > 0) {
                transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);
            } else {
                transform.localScale = new Vector3(-1.0f, 1.0f, 1.0f);
            }

            BrotherBehavior();


            if (worldAssignation == world.DAWN)
                DawnBehavior();
            else
                DuskBehavior();


        }
    }
}
