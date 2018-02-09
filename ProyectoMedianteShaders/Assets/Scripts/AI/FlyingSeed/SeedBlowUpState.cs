using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SeedBlowUpState : State {
     
    float timeToExplode = 2;

    public override void OnEnter(Agent a) {
        a.GetComponent<FlyingSeed>().timeCastingBlowUp = 0;
        a.GetComponent<Rigidbody2D>().velocity = new Vector2(0, 0);
        a.touchedByPlayer = false;
    }

    public override void Update(Agent a, float dt) {
        //Esta casteando explosión
        if (a.GetComponent<FlyingSeed>().timeCastingBlowUp < timeToExplode) {
            a.GetComponent<FlyingSeed>().timeCastingBlowUp += dt;
        }
        //Explota
        else {



            a.GetComponent<FlyingSeed>().BlowUp();
            a.SwitchState(1, new SeedPathFollowState());
        }
    }

    public override void OnExit(Agent a) {
        a.touchedByPlayer = false;
    }

}
