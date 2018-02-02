using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SeedGoUpState : State {
    public float maxTimeOnTheGround = 3;
    float speedUp=0.2f;
    override public void OnEnter(Agent a) {
        a.GetComponent<FlyingSeed>().timeOnTheGround = 0;
        a.GetComponent<Rigidbody2D>().gravityScale = 0;
    }

    override public void Update(Agent a, float dt) {
        Debug.Log("a");
        if (a.GetComponent<FlyingSeed>().timeOnTheGround > maxTimeOnTheGround) {
            Debug.Log("b");

            if (a.GetComponent<Rigidbody2D>().velocity.y < 1) {
                a.GetComponent<Rigidbody2D>().velocity = new Vector2(0, a.GetComponent<Rigidbody2D>().velocity.y + speedUp * Time.deltaTime);
            } else {
                a.SwitchState(0, new SeedIdleState());
            }

        } else {
            Debug.Log("c");

            a.GetComponent<FlyingSeed>().timeOnTheGround += Time.deltaTime;
        }
    }

    override public void OnExit(Agent a) {

    }
}
