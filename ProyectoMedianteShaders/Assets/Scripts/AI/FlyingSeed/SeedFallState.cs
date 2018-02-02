using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SeedFallState : State {
    override public void OnEnter(Agent a) {

        a.GetComponent<Rigidbody2D>().velocity = new Vector2(0, 0);
        a.GetComponent<Rigidbody2D>().gravityScale = 1;

    }

    override public void Update(Agent a,float dt) {
        RaycastHit2D hit2D;
        LayerMask groundMask = LayerMask.GetMask("Ground");

        hit2D = Physics2D.Raycast(a.transform.position, Vector2.down, 1.5f, groundMask);

        if (hit2D) {
            a.SwitchState(0, new SeedGoUpState());
        }
    }

    override public void OnExit(Agent a) {

    }
}
