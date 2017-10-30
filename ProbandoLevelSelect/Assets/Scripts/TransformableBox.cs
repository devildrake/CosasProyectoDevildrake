using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TransformableBox : Transformable {

    private void Start() {
        InitTransformable();
        isPunchable = true;
    }

    private void Update() {
        AddToGameLogicList();

    }

    public override void Change() {
        if (dawn) {
            GetComponent<SpriteRenderer>().sprite = imagenDusk;
            GetComponent<SpriteRenderer>().color = Color.black;
            dawn = false;
        }
        else {
            GetComponent<SpriteRenderer>().sprite = imagenDawn;
            GetComponent<SpriteRenderer>().color = Color.yellow;

            dawn = true;
        }
    }

    protected override void LoadResources() {
        imagenDawn = Resources.Load<Sprite>("Background/Background1");
        imagenDusk = Resources.Load<Sprite>("Background/Background2");
    }

}
