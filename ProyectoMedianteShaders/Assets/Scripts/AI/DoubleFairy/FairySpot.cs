using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FairySpot : DoubleObject {
    public GameObject spriteObject;

    protected override void LoadResources() {
        //ASIGNAR DAWNSPRITE Y DUSKSPRITE ESTARIA BIEN

    }

    public FairySpot brotherScript;

    public bool mustStopHere;
    public bool hasMessage;
    Sprite messageSprite;

    void Start() {
        if (spriteObject != null) {
            spriteObject.SetActive(false);
        }

        brotherScript = brotherObject.GetComponent<FairySpot>();
        canBounce = false;
        InitTransformable();
        isPunchable = false;
        isBreakable = false;
        interactuableBySmash = false;
        offset = GameLogic.instance.worldOffset;

    }

    protected override void BrotherBehavior() {
        Vector3 positionWithOffset;

        positionWithOffset = brotherObject.transform.position;

        if (worldAssignation == world.DAWN) {
            if (!dawn) {
                positionWithOffset.y += offset;
                transform.position = positionWithOffset;
                transform.rotation = brotherObject.transform.rotation;
            }
        } else if (worldAssignation == world.DUSK) {
            if (dawn) {
                positionWithOffset.y -= offset;
                transform.position = positionWithOffset;
                transform.rotation = brotherObject.transform.rotation;
            }
        }



    }

    public override void Change() {
        dawn = !dawn;
    }

    protected override void AddToGameLogicList() {
        if (!added) {
            if (GameLogic.instance != null) {
                added = true;
                GameLogic.instance.transformableObjects.Add(gameObject);
            }
        }
    }


    // Update is called once per frame
    void Update() {
        AddToGameLogicList();
        BrotherBehavior();
    }
}
