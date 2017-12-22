using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoubleProjectileThrower : DoubleObject {
    // Use this for initialization
    Rigidbody2D rb;
    public LayerMask groundMask;
    float distanciaBordeSprite;
    float projectileGenerationTime = 4;
    float projectileTimer = 4;

    public float minDistance = 6;
    GameObject ProjectilePrefab;
    void Start() {
        GetComponent<Rigidbody2D>().gravityScale = 0;
        InitTransformable();
        isPunchable = false;
        isBreakable = false;
        interactuableBySmash = false;
        offset = GameLogic.instance.worldOffset;
        if (worldAssignation == world.DAWN) {
            GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Kinematic;
            GetComponent<SpriteRenderer>().sprite = imagenDawn;
        } else {
            GetComponent<SpriteRenderer>().sprite = imagenDusk;

        }


        rb = GetComponent<Rigidbody2D>();
        groundMask = LayerMask.GetMask("Ground");

        distanciaBordeSprite = 0.745f;
        rb.mass = 5000;
        ProjectilePrefab = Resources.Load<GameObject>("Prefabs/DoubleProjectile");
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
                //active = false;
                //brotherObject.GetComponent<DoubleProjectileThrower>().active = true;

            }
            //Si antes del cambio estaba en dusk, pasara a hacerse dynamic y al otro kinematic, además de darle su velocidad 
            else {
                dominantVelocity = brotherObject.GetComponent<Rigidbody2D>().velocity;
                brotherObject.GetComponent<DoubleObject>().dominantVelocity = brotherObject.GetComponent<Rigidbody2D>().velocity;
                GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Kinematic;
                brotherObject.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Dynamic;
                brotherObject.GetComponent<Rigidbody2D>().velocity = new Vector2(0.0f, 0.0f);
                GetComponent<Rigidbody2D>().velocity = dominantVelocity;
                //brotherObject.GetComponent<DoubleProjectileThrower>().active = false;
                //active = true;

            }

            dawn = !dawn;
            brotherObject.GetComponent<DoubleObject>().dawn = !brotherObject.GetComponent<DoubleObject>().dawn;
        }

    }

    public override void Activate() {
        base.Activate();
        //Change();
    }
    // Update is called once per frame
    void Update() {

        AddToGameLogicList();
        BrotherBehavior();

        if (activated && GameLogic.instance != null && GameLogic.instance.currentPlayer != null&&worldAssignation==world.DAWN) {
            //Debug.Log(Vector2.Distance(GameLogic.instance.currentPlayer.gameObject.transform.position, gameObject.transform.position));

            if ((Vector2.Distance(GameLogic.instance.currentPlayer.gameObject.transform.position, gameObject.transform.position) < minDistance)|| (Vector2.Distance(GameLogic.instance.currentPlayer.gameObject.transform.position, brotherObject.transform.position) < minDistance)) {
                ProjectileCreation();
            } else {

            }
        }

    }

    void CreateProjectile(Vector2 direction) {
        GameObject temp = Instantiate(ProjectilePrefab,brotherObject.transform.position + new Vector3(0,-1.1f,0),Quaternion.identity) as GameObject;


        DoubleProjectile [] projectiles = temp.GetComponentsInChildren<DoubleProjectile>();
        projectiles[0].gameObject.GetComponent<Rigidbody2D>().velocity = direction * 2;// * Time.deltaTime;
        projectiles[1].gameObject.GetComponent<Rigidbody2D>().velocity = direction * 2;// * Time.deltaTime;

        //Debug.Log(projectiles[0]);

    }

    void ProjectileCreation() {
            if (projectileTimer > 0) {
                projectileTimer -= Time.deltaTime;
            } else {
                projectileTimer = projectileGenerationTime;
            if(dawn)
                CreateProjectile((GameLogic.instance.currentPlayer.gameObject.transform.position - gameObject.transform.position).normalized);
            else
                CreateProjectile((GameLogic.instance.currentPlayer.gameObject.transform.position - brotherObject.transform.position).normalized);

        }
    }
    
}