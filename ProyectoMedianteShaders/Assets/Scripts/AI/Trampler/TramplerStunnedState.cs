using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TramplerStunnedState : State {

    public override void OnEnter(Agent a) {
        a.GetComponent<Trampler>().timeStunned = 0;
    }

    public override void Update(Agent a, float dt) {

        Trampler agentScript = a.GetComponent<Trampler>();


        agentScript.timeStunned += dt;
        if (agentScript.stunDuration < agentScript.timeStunned) {
            a.SwitchState(0, new TramplerIdleState());
        }
    }

    public override void OnExit(Agent a) {
        a.GetComponent<Trampler>().ResetPoints();

    }
}
