using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoublePlatform : DoubleObject {
    public bool isSlider;
	// Use this for initialization
	void Start () {
        if (gameObject.tag == "Slide") {
            isSlider = true;
        }
        InitTransformable();
        offset = GameLogic.instance.worldOffset;
        isPunchable = false;

        if (worldAssignation == world.DAWN) {
            Vector2 positionWithOffset = brotherObject.transform.position;
            positionWithOffset.y += offset;
            transform.position = positionWithOffset;
            transform.rotation = brotherObject.transform.rotation;
        }



    }

    protected override void AddToGameLogicList() {
        if (!added) {
            if (GameLogic.instance != null) {
                added = true;
                GameLogic.instance.transformableObjects.Add(gameObject);
                //COMPROVACIÓN DE QUE NO SEA EL CAMPO DE PRUEBAS, LAS MESH SE HABILITAN EN ESTE CAMPO DE PRUEBAS POR DEBUGGING REASONS
                if (GetComponentInChildren<MeshRenderer>() != null && GameLogic.instance.GetCurrentLevel() != "CampoDePruebas") {
                    GetComponentInChildren<MeshRenderer>().enabled = false;
                }
            }
        }

    }

    protected override void LoadResources() {
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
