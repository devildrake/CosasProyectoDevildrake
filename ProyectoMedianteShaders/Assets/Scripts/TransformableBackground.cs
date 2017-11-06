using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TransformableBackground : Transformable {

    private void Start() {
        InitTransformable();
    }

    private void Update() {
        AddToGameLogicList();

    }

    public override void Change() {
        if (dawn) {
            GetComponent<SpriteRenderer>().sprite = imagenDusk;
            dawn = false;
        }
        else {
            GetComponent<SpriteRenderer>().sprite = imagenDawn;

            dawn = true;
        }
    }

    protected override void LoadResources() {
        imagenDawn = Resources.Load<Sprite>("Background/Background1");
        imagenDusk = Resources.Load<Sprite>("Background/Background2");
    }

}
