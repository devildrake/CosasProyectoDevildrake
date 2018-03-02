using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoubleHidableWall : DoubleObject {
    public GameObject wall;
    public GameObject trigger1;
    public GameObject trigger2;


    void Start() {
        canBounce = true;
        InitTransformable();
        isPunchable = true;
        isBreakable = false;
        interactuableBySmash = false;
        offset = GameLogic.instance.worldOffset;
        if (worldAssignation == world.DAWN) {
            GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Kinematic;
            //GetComponent<SpriteRenderer>().sprite = imagenDawn;
        } else {
            //GetComponent<SpriteRenderer>().sprite = imagenDusk;

        }

    }

    protected override void BrotherBehavior() {
        Vector3 positionWithOffset;
        positionWithOffset = brotherObject.transform.position;

        if (worldAssignation == world.DAWN)
            positionWithOffset.y += offset;
        else {
            positionWithOffset.y -= offset;
        }

        transform.position = positionWithOffset;
        transform.rotation = brotherObject.transform.rotation;
    }

    protected override void LoadResources() {

    }

    public override void Change() {
            dawn = !dawn;

    }


    // Update is called once per frame
    void Update() {
        AddToGameLogicList();
        BrotherBehavior();

        //if()

    }
}
