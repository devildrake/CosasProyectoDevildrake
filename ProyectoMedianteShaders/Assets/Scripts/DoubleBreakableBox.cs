using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoubleBreakableBox : DoubleObject {
    // Use this for initialization
    Rigidbody2D rb;
    public LayerMask groundMask;
    float distanciaBordeSprite;
    bool broken;
    [SerializeField]
    AudioClip smashedClip;

    void Start() {
        InitTransformable();
        offset = GameLogic.instance.worldOffset;
        if (worldAssignation == world.DAWN) {
            GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Kinematic;
            GetComponent<SpriteRenderer>().sprite = imagenDawn;
        }else {
            GetComponent<SpriteRenderer>().sprite = imagenDusk;

        }
        rb = GetComponent<Rigidbody2D>();
        groundMask = LayerMask.GetMask("Ground");

        distanciaBordeSprite = 0.745f;
        rb.mass = 5000;
        
        isPunchable = false;
        isBreakable = true;
        interactuableBySmash = false;
        broken = false;
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
        smashedClip = Resources.Load<AudioClip>("Sounds/BeSmashed");
        if(worldAssignation == world.DAWN) {
            imagenDawn = Resources.Load<Sprite>("Presentacion/DawnSprites/DawnBreakableBox");
        }
        else {
            imagenDusk = Resources.Load<Sprite>("Presentacion/DuskSprites/DuskBreakableBox");
        }

    }

    public override void Change() {

    }

    public override void GetBroken() {
        broken = true;
        //if (!dawn) {
            if (GetComponent<AudioSource>() != null) {
                GetComponent<AudioSource>().clip = smashedClip;
                GetComponent<AudioSource>().Play();


            }
            else {
                Debug.Log("No audio source to play to");
            }

        Destroy(gameObject.GetComponent<Rigidbody2D>());
        Destroy(gameObject.GetComponent<BoxCollider2D>());
        Destroy(gameObject.GetComponent<SpriteRenderer>());
        Destroy(brotherObject.gameObject);

        Invoke("DestroyCompletely", 0.5f);



        //   }
    }

    public void DestroyCompletely() {
        Destroy(gameObject);
    }

    // Update is called once per frame
    void Update() {
        AddToGameLogicList();

        if(!broken)
        BrotherBehavior();
    }
}
