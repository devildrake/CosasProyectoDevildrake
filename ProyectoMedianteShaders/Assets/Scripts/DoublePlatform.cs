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
            //GetComponent<SpriteRenderer>().sprite = imagenDawn;
            Vector2 positionWithOffset = brotherObject.transform.position;
            positionWithOffset.y += offset;
            //print("offset " + offset);
            transform.position = positionWithOffset;
            transform.rotation = brotherObject.transform.rotation;
        }
        else {
            //GetComponent<SpriteRenderer>().sprite = imagenDusk;
        }
    }

    protected override void LoadResources() {
        
        //Debug.Log("Loading Resources");
        if (gameObject.tag == "Slide") {
            imagenDawn = Resources.Load<Sprite>("Presentacion/DawnSprites/DawnPlatformSlide");
            imagenDusk = Resources.Load<Sprite>("Presentacion/DuskSprites/DuskPlatformSlide");
        }
        else {
            imagenDawn = Resources.Load<Sprite>("Presentacion/DawnSprites/DawnPlatform");
            imagenDusk = Resources.Load<Sprite>("Presentacion/DuskSprites/DuskPlatform");
        }
    }

    // Update is called once per frame
    void Update () {
		
	}
}
