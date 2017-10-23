using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TransformablePlatform : Transformable {

    public Sprite imagenDusk;
    public Sprite imagenDawn;

    private void Start() {
        SetAddedFalse();
        SetDawnTrue();
        Change();
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


}
