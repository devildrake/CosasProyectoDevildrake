using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DoubleCheckPoint : DoubleObject
{
    public bool endLevel;
    // Use this for initialization
    void Start()
    {
        InitTransformable();
        offset = GameLogic.instance.worldOffset;
        GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezeAll;

        if (worldAssignation == world.DAWN)
        {
            GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Kinematic;
        }
    }

    protected override void BrotherBehavior()
    {
        Vector3 positionWithOffset;
        if (GetComponent<Rigidbody2D>().bodyType == RigidbodyType2D.Kinematic)
        {
            positionWithOffset = brotherObject.transform.position;

            if (worldAssignation == world.DAWN)
                positionWithOffset.y += offset;
            else
            {
                positionWithOffset.y -= offset;
            }

            transform.position = positionWithOffset;
            transform.rotation = brotherObject.transform.rotation;

        }

    }



    // Update is called once per frame
    void Update()
    {
        AddToGameLogicList();
        BrotherBehavior();
    }

    public override void Interact() {
        if (!endLevel) {
            if (worldAssignation == world.DAWN) {
                GameLogic.instance.SetSpawnPoint(brotherObject.gameObject.transform.position);
            } else {
                GameLogic.instance.SetSpawnPoint(gameObject.transform.position);
            }
        } else {
            //CODIGO DE ACABAR NIVEL
            SceneManager.LoadScene(0);
        }
    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player") {
            collision.gameObject.GetComponent<PlayerController>().interactableObject = gameObject.GetComponent<DoubleObject>();
        }
    }

    public void OnTriggerStay2D(Collider2D collision) {
        if (collision.gameObject.tag == "Player") {
            collision.gameObject.GetComponent<PlayerController>().interactableObject = gameObject.GetComponent<DoubleObject>();
        }
    }

    public void OnTriggerExit2D(Collider2D collision) {
        if (collision.gameObject.tag == "Player") {
            collision.gameObject.GetComponent<PlayerController>().interactableObject = null;
        }
    }

}

