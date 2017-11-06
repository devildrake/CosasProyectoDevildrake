using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoubleBreakableBox : DoubleObject {
    // Use this for initialization
    Rigidbody2D rb;
    public LayerMask groundMask;
    float distanciaBordeSprite;

    void Start() {
        InitTransformable();
        offset = GameLogic.instance.worldOffset;
        if (worldAssignation == world.DAWN)
            GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Kinematic;

        rb = GetComponent<Rigidbody2D>();
        groundMask = LayerMask.GetMask("Ground");

        distanciaBordeSprite = 0.745f;
        rb.mass = 5000;
        
        isPunchable = false;
        isBreakable = true;
        interactuableBySmash = false;
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

    }

    public override void Change() {

    }

    public override void GetBroken() {
        Destroy(brotherObject.gameObject);
        Destroy(gameObject);
    }

    // Update is called once per frame
    void Update() {
        AddToGameLogicList();
        BrotherBehavior();
    }
}
