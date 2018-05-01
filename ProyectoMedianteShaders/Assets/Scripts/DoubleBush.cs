using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoubleBush : DoubleObject {
    // Use this for initialization
    Rigidbody2D rb;
    DoubleBush brotherScript;
    bool spawnSound = false;
	void Start () {
        rb = GetComponent<Rigidbody2D>();
        InitTransformable();
        offset = GameLogic.instance.worldOffset;
        rb.constraints = RigidbodyConstraints2D.FreezeAll;

        if (worldAssignation == world.DAWN) {
           rb.bodyType = RigidbodyType2D.Kinematic;
        }
        brotherScript = brotherObject.GetComponent<DoubleBush>();


    }

    protected override void BrotherBehavior() {
        Vector3 positionWithOffset;
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

    private void Kill(GameObject obj) {
        
        obj.GetComponent<PlayerController>().Kill();
    }

    void DuskBehavior() {

        if (!spawnSound) {
            PlayDuskCrunch();
            spawnSound = true;
        }
        //if (colliderSubida.transform.localPosition.y < 0) {
        //    colliderSubida.GetComponent<Rigidbody2D>().velocity = new Vector2(0, upVelocity);
        //    colliderSubida.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezeRotation;

        //} else {
        //    colliderSubida.GetComponent<Rigidbody2D>().constraints=RigidbodyConstraints2D.FreezeAll;
        //    colliderSubida.GetComponent<Rigidbody2D>().velocity = new Vector2(0, 0);
        //}

    }

    void DawnBehavior() {

        //if (brotherObject.GetComponent<DoubleBush>().colliderSubida.transform.localPosition.y > -3.29) {
        //    brotherObject.GetComponent<DoubleBush>().colliderSubida.transform.localPosition = new Vector2(brotherObject.GetComponent<DoubleBush>().colliderSubida.transform.localPosition.x, -3.29f);

        //}

        //Comentado el codigo para que baje poco a poco el collider 
        //if (brotherObject.GetComponent<DoubleBush>().colliderSubida.transform.localPosition.y > -3.29) {
        //    brotherObject.GetComponent<DoubleBush>().colliderSubida.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezeRotation;
        //    brotherObject.GetComponent<DoubleBush>().colliderSubida.GetComponent<Rigidbody2D>().velocity = new Vector2(0, -brotherObject.GetComponent<DoubleBush>().upVelocity);
        //} else {
        //    brotherObject.GetComponent<DoubleBush>().colliderSubida.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezeAll;
        //    brotherObject.GetComponent<DoubleBush>().colliderSubida.GetComponent<Rigidbody2D>().velocity = new Vector2(0, 0);

    }




    //public override void Change() {
    //    if (worldAssignation == world.DUSK) {
    //        colliderSubida.transform.position = initialColliderPos;
    //    }
    //    base.Change();

    //}

    public override void Change() {
        if (rb == null)
            rb = GetComponent<Rigidbody2D>();

            if (worldAssignation == world.DAWN) {
            brotherObject.SetActive(false);

            if (dawn) {
                brotherObject.SetActive(true);
                rb.isKinematic = true;
                brotherObject.GetComponent<Rigidbody2D>().isKinematic = false;
                if (brotherScript != null) {
                    brotherScript.PlayDuskCrunch();
                }
            } else {
                rb.isKinematic = false;
                brotherObject.GetComponent<Rigidbody2D>().isKinematic = true;

            }
        }
        dawn = !dawn;
    }

    public void PlayDuskCrunch() {
        SoundManager.Instance.PlayOneShotSound("event:/Props/DuskBushCrunch",transform);
    }

    // Update is called once per frame
    void Update () {
        AddToGameLogicList();
        BrotherBehavior();
        if (!dawn && worldAssignation == world.DUSK) {
            DuskBehavior();
        }else if (dawn && worldAssignation == world.DAWN) {
            DawnBehavior();
        }

    }



    public void OnTriggerEnter2D(Collider2D collision) {
        if (collision.tag == "Player") {
            if (!dawn) {
                {
                    Kill(collision.gameObject);
                }
            } else {
                collision.GetComponent<PlayerController>().behindBush = true;
            }
        }
    }
    

    public void OnTriggerStay2D(Collider2D collision) {
        if (collision.tag == "Player") {
            if (!dawn) {
                {
                    Kill(collision.gameObject);
                }
            } else {
                collision.GetComponent<PlayerController>().behindBush = true;
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision) {
        if (collision.tag == "Player") {
            if (dawn) {
                collision.GetComponent<PlayerController>().behindBush = false;
            }
        }
    }
}
