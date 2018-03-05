using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoubleHidableWall : DoubleObject {
    public GameObject wall;
    public GameObject trigger1;

    void Start() {
        canBounce = true;
        InitTransformable();
        isPunchable = true;
        isBreakable = false;
        interactuableBySmash = false;
        offset = GameLogic.instance.worldOffset;
        if (worldAssignation == world.DAWN) {
            //GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Kinematic;
            //GetComponent<SpriteRenderer>().sprite = imagenDawn;
        } else {
            //GetComponent<SpriteRenderer>().sprite = imagenDusk;

        }

    }

    protected override void BrotherBehavior() {
        if (worldAssignation == world.DAWN) {
            transform.position = new Vector3(brotherObject.transform.position.x, brotherObject.transform.position.y + GameLogic.instance.worldOffset, brotherObject.transform.position.z);
        }
    }

    protected override void LoadResources() {

    }

    public override void Change() {
            dawn = !dawn;

    }

    public void HideWall() {
        wall.SetActive(false);
        //wall.GetComponent<MeshRenderer>().material.color = new Color(wall.GetComponent<MeshRenderer>().material.color.r, wall.GetComponent<MeshRenderer>().material.color.g, wall.GetComponent<MeshRenderer>().material.color.b,0.5f);
    }

    public void ShowWall() {
        wall.SetActive(true);
        //wall.GetComponent<MeshRenderer>().material.color = new Color(wall.GetComponent<MeshRenderer>().material.color.r, wall.GetComponent<MeshRenderer>().material.color.g, wall.GetComponent<MeshRenderer>().material.color.b, 1);

    }

    // Update is called once per frame
    void Update() {
        AddToGameLogicList();

        BrotherBehavior();

        if (trigger1.GetComponent<TriggerDetectionPlayer>().playerInArea || brotherObject.GetComponent<DoubleHidableWall>().trigger1.GetComponent<TriggerDetectionPlayer>().playerInArea) {
            HideWall();
            brotherObject.GetComponent<DoubleHidableWall>().HideWall();
        } else {
            brotherObject.GetComponent<DoubleHidableWall>().ShowWall();
            ShowWall();
        }

    }
}
