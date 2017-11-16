using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoubleBush : DoubleObject {

	// Use this for initialization
	void Start () {
        InitTransformable();
        offset = GameLogic.instance.worldOffset;
        GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezeAll;

        if (worldAssignation == world.DAWN) {
            GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Kinematic;
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

    private void Kill(GameObject obj) {
        obj.GetComponent<Transformable>().Kill();
    }

    // Update is called once per frame
    void Update () {
        AddToGameLogicList();
        BrotherBehavior();
    }

    public void OnTriggerEnter2D(Collider2D collision) {
        if (collision.tag == "Player") {
            Kill(collision.gameObject);
            Debug.Log("kill");
        }
    }
}
