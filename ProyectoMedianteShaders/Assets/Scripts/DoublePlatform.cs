using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoublePlatform : DoubleObject {

	// Use this for initialization
	void Start () {
        InitTransformable();
        offset = GameLogic.instance.worldOffset;
        isPunchable = false;

        if (worldAssignation == world.DAWN) {
            GetComponent<SpriteRenderer>().sprite = imagenDawn;
            Vector2 positionWithOffset = brotherObject.transform.position;
            positionWithOffset.y += offset;
            transform.position = positionWithOffset;
            transform.rotation = brotherObject.transform.rotation;
        }
        else {
            GetComponent<SpriteRenderer>().sprite = imagenDusk;
        }
    }

    protected override void LoadResources() {
        Debug.Log("Loading Resources");
        imagenDawn = Resources.Load<Sprite>("Sprites/Platfroms/Hearthstone1");
        imagenDusk = Resources.Load<Sprite>("Sprites/Platfroms/Hearthstone2");

    }

    // Update is called once per frame
    void Update () {
		
	}
}
