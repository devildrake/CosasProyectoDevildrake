using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Transformable : MonoBehaviour{
    bool added;
    public bool dawn;


    public virtual void SetAddedFalse() {
        added = false;
    }

    public virtual void SetDawnTrue() {
        dawn = true;
    }

    public virtual void AddToGameLogicList() {
        if (!added) {
            if (GameLogic.instance != null) {
                added = true;
                GameLogic.instance.transformableObjects.Add(gameObject);
            }
        }
    }

    public virtual void Change() {}

}
