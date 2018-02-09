using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SeedFallState : State {
    override public void OnEnter(Agent a) {
        a.GetComponent<FlyingSeed>().DropObject();
        a.GetComponent<Rigidbody2D>().velocity = new Vector2(0, -0.01f);
        a.GetComponent<Rigidbody2D>().gravityScale = 0.5f;
        a.GetComponent<FlyingSeed>().DropObject();
    }

    override public void Update(Agent a,float dt) {

        if (a.GetComponent<Rigidbody2D>().velocity.y==0) {
            a.SwitchState(0, new SeedGoUpState());
        }
    }

    override public void OnExit(Agent a) {

    }
}
