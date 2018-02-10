using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TramplerDraggableState : State {

    public override void OnEnter(Agent a) {
        a.isMovable = true;
    }

    public override void Update(Agent a, float dt) {

    }

    public override void OnExit(Agent a) {
        a.isMovable = false;
    }
}
