using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoubleProjectile : DoubleObject {
    // Use this for initialization
    Rigidbody2D rb;
    public Vector2 initialSpeed;
    float angleToRot;
    float maxTimeAlive;
    float timeAlive;
    private void Awake()
    {
        maxTimeAlive = 15;
        InitTransformable();
        isPunchable = true;
        isBreakable = false;
        interactuableBySmash = false;
        if (worldAssignation == world.DAWN)
        {
            GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Kinematic;

            //GetComponent<SpriteRenderer>().sprite = imagenDawn;
        }
        else
        {
            GetComponent<Rigidbody2D>().velocity = initialSpeed;
            GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Dynamic;


            //GetComponent<SpriteRenderer>().sprite = imagenDusk;

        }
        rb = GetComponent<Rigidbody2D>();
    }

    void Start() {
        offset = GameLogic.instance.worldOffset;
        angleToRot = Vector3.Angle(Vector2.right, GetComponent<Rigidbody2D>().velocity);
        transform.Rotate(new Vector3(0, 0, 1), -angleToRot);
        Debug.Log(angleToRot);
    }

    public void BeDeflected() {
        GetComponent<Rigidbody2D>().gravityScale = 0;
        transform.rotation = Quaternion.identity;

        angleToRot = Vector3.Angle(Vector2.right, GetComponent<Rigidbody2D>().velocity);
        transform.Rotate(new Vector3(0, 0, 1), angleToRot);

    }

    protected override void BrotherBehavior() {

        Vector3 positionWithOffset;
        if (GetComponent<Rigidbody2D>().bodyType == RigidbodyType2D.Kinematic) {
            positionWithOffset = brotherObject.transform.position;

            if (worldAssignation == world.DAWN)
            {
                positionWithOffset.y += offset;
                GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Dynamic;
                brotherObject.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Kinematic;

            } else
            {
                positionWithOffset.y -= offset;
                GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Kinematic;
                brotherObject.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Dynamic;

            }

            transform.position = positionWithOffset;
            transform.rotation = brotherObject.transform.rotation;

        }

    }

    void BecomePunchable() {
        isPunchable = true;
    }

    protected override void LoadResources() {
        //if (worldAssignation == world.DAWN) {
        //    imagenDawn = Resources.Load<Sprite>("Presentacion/DawnSprites/DawnBox");
        //} else {
        //    imagenDusk = Resources.Load<Sprite>("Presentacion/DuskSprites/DuskBox");

        //}
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

    private void OnCollisionEnter2D(Collision2D collision) {
        if (collision.gameObject.tag == "Player") {
            Debug.Log("CollisionWithPlayer");
            collision.gameObject.GetComponent<PlayerController>().Kill();
        }
        if (collision.gameObject != brotherObject&&collision.gameObject.tag!="ProjectileThrow") {

            GameLogic.instance.SafelyDestroy(this);
        }
    }

    void ProjectileLifeSpan() {
        timeAlive += Time.deltaTime;
        if (timeAlive > maxTimeAlive) {
            GameLogic.instance.SafelyDestroy(this);
        }
    }

    // Update is called once per frame
    void Update() {
        AddToGameLogicList();
        BrotherBehavior();
        ProjectileLifeSpan();
        //if (!isPunchable) {
        //    Invoke("BecomePunchable", 0.5f);
        //}



    }
}
