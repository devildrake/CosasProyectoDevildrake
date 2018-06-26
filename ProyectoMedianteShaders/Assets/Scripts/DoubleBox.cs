using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoubleBox : DoubleObject {
    // Use this for initialization
    Rigidbody rb;
    DoubleBox brotherScript;

    public LayerMask groundMask;
    float timerToBecomePunchable;
    float timeToBecomePunchable;
    bool grounded=false;
	void Start () {
        brotherScript = brotherObject.GetComponent<DoubleBox>();
        canBounce = true;
        InitTransformable();
        isPunchable = true;
        isBreakable = false;
        interactuableBySmash = false;
        timeToBecomePunchable = 0.5f;
        offset = GameLogic.instance.worldOffset;
        rb = GetComponent<Rigidbody>();

        if (worldAssignation == world.DAWN) {
            //GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Kinematic;
            //GetComponent<SpriteRenderer>().sprite = imagenDawn;
            //GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Kinematic;
            rb.isKinematic = true;
            transform.position += new Vector3(0,GameLogic.instance.worldOffset);

        } else {
            //GetComponent<SpriteRenderer>().sprite = imagenDusk;
            // GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Kinematic;
            //GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Dynamic;
            rb.isKinematic = false;

        }
        float randomVal = Random.Range(1, 4);
        //Debug.Log(randomVal);
        GetComponentInChildren<MeshRenderer>().gameObject.transform.rotation *= Quaternion.AngleAxis(randomVal * 90,new Vector3(0,0,1));

        groundMask = LayerMask.GetMask("Ground");

        rb.mass = 5000;
	}

    protected override void BrotherBehavior() {
        Vector3 positionWithOffset;

        if (rb == null)
            rb = GetComponent<Rigidbody>();

        //  if (rb.bodyType == RigidbodyType2D.Kinematic) {
        if (rb.isKinematic) { 
        //Debug.Log(worldAssignation);


            positionWithOffset = brotherObject.transform.position;

            if (worldAssignation == world.DAWN)
                positionWithOffset.y += offset;
            else {
                positionWithOffset.y -= offset;
            }

            transform.position = positionWithOffset;
            transform.rotation = brotherObject.transform.rotation;

        }

        //if (rb.bodyType == RigidbodyType2D.Kinematic) {
        //    if (worldAssignation == world.DAWN) {
        //        transform.position = brotherObject.transform.position + new Vector3(0, GameLogic.instance.worldOffset);
        //    } else {
        //        transform.position = brotherObject.transform.position - new Vector3(0, GameLogic.instance.worldOffset);
        //    }
        //}



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
            rb = GetComponent<Rigidbody>();
        if (brotherScript.rb == null)
            brotherScript.rb = GetComponent<Rigidbody>();

        //El objeto que modifica a ambos haciendo de controlador es el que pertenece a Dawn
        if (worldAssignation == world.DAWN) {
            //Si antes del cambio estaba en dawn, pasara a hacerse kinematic y al otro dynamic, además de darle su velocidad
            if (dawn) {
                dominantVelocity = rb.velocity;
                brotherScript.dominantVelocity = rb.velocity;
                //brotherScript.rb.bodyType = RigidbodyType2D.Dynamic;
                //rb.bodyType = RigidbodyType2D.Kinematic;
                brotherScript.rb.isKinematic = false;
                rb.isKinematic = true;

                OnlyFreezeRotation();
                brotherScript.rb.velocity = dominantVelocity;
                rb.velocity = new Vector2(0.0f, 0.0f);
            }
            //Si antes del cambio estaba en dusk, pasara a hacerse dynamic y al otro kinematic, además de darle su velocidad 
            else {
                dominantVelocity = brotherScript.rb.velocity;
                brotherScript.dominantVelocity = brotherScript.rb.velocity;
                //rb.bodyType = RigidbodyType2D.Dynamic;
                //brotherScript.rb.bodyType = RigidbodyType2D.Kinematic;
                brotherScript.rb.isKinematic = true;
                rb.isKinematic = false;
                brotherScript.rb.velocity = new Vector2(0.0f,0.0f);
                rb.velocity = dominantVelocity;
            }

            dawn = !dawn;
            brotherScript.dawn = !brotherScript.dawn;
        }

    }

    protected override void AddToGameLogicList() {
        if (!added) {
            if (GameLogic.instance != null) {
                added = true;
                GameLogic.instance.transformableObjects.Add(gameObject);
                if (worldAssignation == world.DUSK) {
                    rb.isKinematic = false;
                }
            }
        }
    }


    // Update is called once per frame
    void Update () {

        if (rb.velocity.y < -0.1f || rb.velocity.y > 0.1f) {
            grounded = false;
        } else {
            if (!grounded) {
                SoundManager.Instance.PlayEvent("event:/Enemies/Walker/Steps", transform);
                SoundManager.Instance.PlayEvent("event:/Enemies/Walker/Steps", transform);
                SoundManager.Instance.PlayEvent("event:/Enemies/Walker/Steps", transform);
                SoundManager.Instance.PlayEvent("event:/Enemies/Walker/Steps", transform);
                SoundManager.Instance.PlayEvent("event:/Enemies/Walker/Steps", transform);
                SoundManager.Instance.PlayEvent("event:/Enemies/Walker/Steps", transform);
                grounded = true;
            }
        }

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
