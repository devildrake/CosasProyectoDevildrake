using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoubleBox : DoubleObject {
    // Use this for initialization
    Rigidbody2D rb;
    DoubleBox brotherScript;

    public LayerMask groundMask;
    float timerToBecomePunchable;
    float timeToBecomePunchable;
	void Start () {
        brotherScript = brotherObject.GetComponent<DoubleBox>();
        canBounce = true;
        InitTransformable();
        isPunchable = true;
        isBreakable = false;
        interactuableBySmash = false;
        timeToBecomePunchable = 0.5f;
        offset = GameLogic.instance.worldOffset;
        if (worldAssignation == world.DAWN) {
            GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Kinematic;
            //GetComponent<SpriteRenderer>().sprite = imagenDawn;
        }
        else {
            //GetComponent<SpriteRenderer>().sprite = imagenDusk;

        }
        float randomVal = Random.Range(1, 4);
        //Debug.Log(randomVal);
        GetComponentInChildren<MeshRenderer>().gameObject.transform.rotation *= Quaternion.AngleAxis(randomVal * 90,new Vector3(0,0,1));

        rb = GetComponent<Rigidbody2D>();
        groundMask = LayerMask.GetMask("Ground");

        rb.mass = 5000;
	}

    protected override void BrotherBehavior() {
        Vector3 positionWithOffset;

        if(rb==null)
        rb = GetComponent<Rigidbody2D>();

        if (rb.bodyType == RigidbodyType2D.Kinematic) {
            positionWithOffset = brotherObject.transform.position;

            if(worldAssignation==world.DAWN)
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
        }
        else {
            imagenDusk = Resources.Load<Sprite>("Presentacion/DuskSprites/DuskBox");

        }
    }

    public override void Change() {
        if (rb == null)
            rb = GetComponent<Rigidbody2D>();
        if (brotherScript.rb == null)
            brotherScript.rb = GetComponent<Rigidbody2D>();

        //El objeto que modifica a ambos haciendo de controlador es el que pertenece a Dawn
        if (worldAssignation == world.DAWN) {
            //Si antes del cambio estaba en dawn, pasara a hacerse kinematic y al otro dynamic, además de darle su velocidad
            if (dawn) {
                dominantVelocity = rb.velocity;
                brotherScript.dominantVelocity = rb.velocity;
                brotherScript.rb.bodyType = RigidbodyType2D.Dynamic;
                rb.bodyType = RigidbodyType2D.Kinematic;
                OnlyFreezeRotation();
                brotherScript.rb.velocity = dominantVelocity;
                rb.velocity = new Vector2(0.0f, 0.0f);
            }
            //Si antes del cambio estaba en dusk, pasara a hacerse dynamic y al otro kinematic, además de darle su velocidad 
            else {
                dominantVelocity = brotherScript.rb.velocity;
                brotherScript.dominantVelocity = brotherScript.rb.velocity;
                rb.bodyType = RigidbodyType2D.Dynamic;
                brotherScript.rb.bodyType = RigidbodyType2D.Kinematic;
                brotherScript.rb.velocity = new Vector2(0.0f,0.0f);
                rb.velocity = dominantVelocity;
            }

            dawn = !dawn;
            brotherScript.dawn = !brotherScript.dawn;
        }

    }


    // Update is called once per frame
    void Update () {
        AddToGameLogicList();
        BrotherBehavior();

        //Caja puncheable, si no lo es en un momento dado, va sumando tiempo a un contador para volver a volverse punchable
        if (!isPunchable) {
            //Invoke("BecomePunchable", 0.5f);
            timerToBecomePunchable += Time.deltaTime;
            if (timerToBecomePunchable > timeToBecomePunchable) {
                timerToBecomePunchable = 0;
                isPunchable = true;
            }
        }


    }
}
