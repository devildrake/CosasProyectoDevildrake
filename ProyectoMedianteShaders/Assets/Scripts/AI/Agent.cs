using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Agent : DoubleObject {
    Rigidbody2D rb;
    protected State dawnState;
    protected State duskState;
    protected bool startedAI;
    void Start() {
        startedAI = false;
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

        rb.mass = 1;
    }

    /// <summary>
    ///The integer represents if the state you want to change is dawn (0) or dusk(1), it already checks for nullreferences
    /// </summary>
    public void SwitchState(int i,State newState) {
        if (i == 0) {
            if (dawnState != null) {
                dawnState.OnExit(this);
                dawnState = newState;
                dawnState.OnEnter(this);
            } else {
                dawnState = newState;
                dawnState.OnEnter(this);
            }
        }
        else{
            if (duskState != null) {
                duskState.OnExit(this);
                duskState = newState;
                duskState.OnEnter(this);
            } else{
                duskState = newState;
                duskState.OnEnter(this);
            }
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
            if(dawnState!=null)
            dawnState.Update(this, Time.deltaTime);
        }
    }
    //Velocidad a 0
    void DuskBehavior() {
        if (!dawn) {
            if (duskState != null)
                duskState.Update(this, Time.deltaTime);
        }
    }

    protected virtual void StartAI() {

    }

}


