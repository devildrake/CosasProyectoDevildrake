using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TramplerStunnedState : State {

    public override void OnEnter(Agent a) {
        a.GetComponent<Trampler>().timeStunned = 0;
    }

    public override void Update(Agent a, float dt) {
        a.GetComponent<Trampler>().timeStunned += dt;
        if (a.GetComponent<Trampler>().stunDuration < a.GetComponent<Trampler>().timeStunned) {
            a.SwitchState(0, new TramplerIdleState());
        }
    }

    public override void OnExit(Agent a) {
        a.GetComponent<Trampler>().ResetPoints();

    }
}
