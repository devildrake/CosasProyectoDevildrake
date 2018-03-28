using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoubleBreakableBox : DoubleObject {
    // Use this for initialization
    Rigidbody2D rb;
    public LayerMask groundMask;
    bool broken;
    [SerializeField]
    AudioClip smashedClip;
    Animator myAnimator;
    DoubleBreakableBox brotherScript;
    void Start() {
        myAnimator = GetComponentInChildren<Animator>();
        offset = GameLogic.instance.worldOffset;
        LoadResources();
        if (worldAssignation == world.DAWN) {
            GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Kinematic;
        } else {
            transform.localPosition = new Vector3(0, 0, 0);
        }
        InitTransformable();

        rb = GetComponent<Rigidbody2D>();
        groundMask = LayerMask.GetMask("Ground");

        rb.mass = 5000;
        
        isPunchable = false;
        isBreakable = true;
        interactuableBySmash = false;
        broken = false;
        rb.gravityScale = 0;
        brotherScript = brotherObject.GetComponent<DoubleBreakableBox>();

    }

    protected override void BrotherBehavior() {
        Vector3 positionWithOffset;
        if (rb != null) {
            if (rb.bodyType == RigidbodyType2D.Kinematic) {
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

        Destroy(brotherScript.gameObject.GetComponent<Rigidbody2D>());
        Destroy(brotherScript.gameObject.GetComponent<BoxCollider2D>());

        //Destroy(brotherObject.gameObject);

        Invoke("DestroyCompletely", 1.0f);
        myAnimator.SetBool("broken", true);
        brotherScript.myAnimator.SetBool("broken", true);

        //   }
    }

    public void DestroyCompletely() {
        GameLogic.instance.SafelyDestroy(this);
    }

    // Update is called once per frame
    void Update() {

        if(transform.localPosition!=new Vector3(0,0,0)&&worldAssignation == world.DUSK) {
            transform.localPosition = new Vector3(0, 0, 0);
        }

        AddToGameLogicList();

        if (!broken) {
            BrotherBehavior();
        }
    }
}
