using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoubleBush : DoubleObject {
    public GameObject colliderSubida;
    float upVelocity;
    Vector3 initialColliderPos;
	// Use this for initialization
	void Start () {
        upVelocity = 2;
        InitTransformable();
        offset = GameLogic.instance.worldOffset;
        GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezeAll;

        if (worldAssignation == world.DAWN) {
            GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Kinematic;
        }

        else
            initialColliderPos = colliderSubida.transform.position;


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

    private void Kill(GameObject obj) {
        
        obj.GetComponent<PlayerController>().Kill();
    }

    void DuskBehavior() {
        if (colliderSubida.transform.localPosition.y < 0) {
            colliderSubida.GetComponent<Rigidbody2D>().velocity = new Vector2(0, upVelocity);
        } else {
            colliderSubida.GetComponent<Rigidbody2D>().constraints=RigidbodyConstraints2D.FreezeAll;
            colliderSubida.GetComponent<Rigidbody2D>().velocity = new Vector2(0, 0);
        }

    }

    //public override void Change() {
    //    if (worldAssignation == world.DUSK) {
    //        colliderSubida.transform.position = initialColliderPos;
    //    }
    //    base.Change();

    //}

    // Update is called once per frame
    void Update () {
        AddToGameLogicList();
        BrotherBehavior();
        if (!dawn && worldAssignation == world.DUSK) {
            DuskBehavior();
        }

    }



    public void OnTriggerEnter2D(Collider2D collision) {
        if (!dawn)
        {
            if (collision.tag == "Player")
            {
                Kill(collision.gameObject);
            }
        }
    }

    public void OnTriggerStay2D(Collider2D collision)
    {
        if (!dawn)
        {
            if (collision.tag == "Player")
            {
                Kill(collision.gameObject);
            }
        }
    }
}
