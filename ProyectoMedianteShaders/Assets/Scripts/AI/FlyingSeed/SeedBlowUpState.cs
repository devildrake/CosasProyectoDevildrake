using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SeedBlowUpState : State {
     
    float timeToExplode = 2;

    public override void OnEnter(Agent a) {
        Debug.Log("BlowUp");
        a.GetComponent<FlyingSeed>().timeCastingBlowUp = 0;
        a.GetComponent<Rigidbody2D>().velocity = new Vector2(0, 0);
        a.touchedByPlayer = false;
    }

    public override void Update(Agent a, float dt) {
        FlyingSeed agentScript = a.GetComponent<FlyingSeed>();

        agentScript.Spin(500.0f);

        //Esta casteando explosión
        if (agentScript.timeCastingBlowUp < timeToExplode) {
            agentScript.timeCastingBlowUp += dt;
        }


        //Explota
        else {
            agentScript.BlowUp();
            a.SwitchState(1, new SeedPathFollowState());
        }
    }

    public override void OnExit(Agent a) {
        a.touchedByPlayer = false;
    }

}
