using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FairySpot : DoubleObject {

    protected override void LoadResources() {
        //ASIGNAR DAWNSPRITE Y DUSKSPRITE ESTARIA BIEN

    }

    public FairySpot brotherScript;

    public bool mustStopHere;
    public bool hasMessage;
    public Sprite messageSprite;

    public DoubleFairyGuide parentFairy;

    void Start() {
        brotherScript = brotherObject.GetComponent<FairySpot>();
        canBounce = false;
        InitTransformable();
        isPunchable = false;
        isBreakable = false;
        interactuableBySmash = false;
        offset = GameLogic.instance.worldOffset;

    }

    protected override void BrotherBehavior() {
        if (worldAssignation == world.DAWN && dawn) {
            transform.position = new Vector3(brotherObject.transform.position.x, brotherObject.transform.position.y + GameLogic.instance.worldOffset, brotherObject.transform.position.z);
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

        if (added) {
            if(parentFairy == null) {
                parentFairy = GetComponentInParent<DoubleFairyGuide>();
            } else {
                if (!parentFairy.fairySpotList.Contains(this)) {
                    parentFairy.fairySpotList.Add(this);
                } else {
                    for(int i = 0; i < parentFairy.fairySpotList.Count; i++) {
                        if (parentFairy.fairySpotList[i] == this) {
                            //parentFairy.fairySpotPositionList[i] = transform.position;
                            parentFairy.fairySpotList[i].transform.position = transform.position;


                        }
                    }


                }
            }
        }

    }
}
