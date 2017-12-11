using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoubleProjectileSwitch : DoubleObject
{
    static public int size;
    public DoubleObject[] objectsToTrigger = new DoubleObject[size];
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
        GetComponent<Rigidbody2D>().gravityScale = 0;
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

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Projectile"&&!activated)
        {
            Activate();
            Debug.Log("ACTIVATESTUFF");

            foreach (DoubleObject g in objectsToTrigger)
            {
                Debug.Log("ACTIVATE");
                g.Activate();
                brotherObject.GetComponent<DoubleObject>().Activate();
            }
        }

    }
}

