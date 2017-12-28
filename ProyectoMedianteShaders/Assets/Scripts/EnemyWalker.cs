using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyWalker : DoubleObject {
    // Use this for initialization
    Rigidbody2D rb;
    public LayerMask groundMask;
    float distanciaBordeSprite;
    public float bounceForce;
    bool movingRight;
    public float velocity;
    void Start() {
        movingRight = true;
        bounceForce = 50;
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
        GetComponentInChildren<MeshRenderer>().gameObject.transform.rotation *= Quaternion.AngleAxis(randomVal * 90, new Vector3(0, 0, 1));

        rb = GetComponent<Rigidbody2D>();
        groundMask = LayerMask.GetMask("Ground");

        distanciaBordeSprite = 0.745f;
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

    void DawnBehavior() {
        if (dawn) {
            RaycastHit2D hit2D;

            if (movingRight) {
                hit2D = Physics2D.Raycast(gameObject.transform.position, Vector2.right+new Vector2(0,0.5f), 1f, groundMask);
                Debug.DrawRay(gameObject.transform.position+new Vector3(0,0.5f,0), Vector2.right);
                GetComponent<Rigidbody2D>().velocity = new Vector2(velocity, 0);
            } else {
                hit2D = Physics2D.Raycast(gameObject.transform.position, Vector2.left + new Vector2(0, 0.5f), 1f, groundMask);
                Debug.DrawRay(gameObject.transform.position + new Vector3(0, 0.5f, 0), Vector2.left);

                GetComponent<Rigidbody2D>().velocity = new Vector2(-velocity, 0);
            }
            if (hit2D) {
                Debug.Log(hit2D.collider.gameObject.tag);
                movingRight = !movingRight;
            }

        }
    }

    void DuskBehavior() {
        if (!dawn) {
            GetComponent<Rigidbody2D>().velocity = new Vector2(0, 0);
        }
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if (dawn && worldAssignation == world.DAWN) {
            if (other.tag == "Player") {
                other.GetComponent<PlayerController>().Kill();
            }
        }else if (!dawn && worldAssignation == world.DUSK) {
            if (other.tag == "Player") {
                if (other.GetComponent<Rigidbody2D>().velocity.y <= 0) {
                    other.GetComponent<Rigidbody2D>().AddForce(new Vector2(0, 1 * bounceForce), ForceMode2D.Impulse);
                }
            }
        }
    }

    private void OnTriggerStay2D(Collider2D other) {
        if (dawn && worldAssignation == world.DAWN) {
            if (other.tag == "Player") {
                other.GetComponent<PlayerController>().Kill();
            }
        } else if (!dawn && worldAssignation == world.DUSK) {
            if (other.tag == "Player") {
                if (other.GetComponent<Rigidbody2D>().velocity.y <= 0) {
                    Debug.Log(other.GetComponent<Rigidbody2D>());
                    other.GetComponent<Rigidbody2D>().AddForce(new Vector2(0, 1 * bounceForce),ForceMode2D.Impulse);
                }
            }
        }
    }

    // Update is called once per frame
    void Update() {
        AddToGameLogicList();
        BrotherBehavior();
        if (worldAssignation == world.DAWN)
            DawnBehavior();
        else
            DuskBehavior();

    }
}
