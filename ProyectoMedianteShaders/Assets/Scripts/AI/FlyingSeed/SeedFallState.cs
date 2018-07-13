using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SeedFallState : State {
    override public void OnEnter(Agent a) {
        //a.GetComponent<Rigidbody2D>().velocity = new Vector2(0, -0.01f);
        a.GetComponent<Rigidbody>().velocity = new Vector2(0, -0.02f);
        a.GetComponent<CustomGravity>().gravityScale = 0;
        FlyingSeed f = a.GetComponent<FlyingSeed>();
        f.fallTimer = 0;
    }

    override public void Update(Agent a,float dt) {
        FlyingSeed agentScript = a.GetComponent<FlyingSeed>();
        if (agentScript.rb.velocity.y == 0 && agentScript.fallTimer < agentScript.fallTime) {
            a.GetComponent<Rigidbody>().velocity = new Vector2(0, -0.02f);
        }

        if (agentScript.fallTimer > agentScript.fallTime) {

            agentScript.Spin(-1500.0f);

            agentScript.DropObject();

            //a.GetComponent<Rigidbody2D>().gravityScale = 0.5f;
            a.GetComponent<CustomGravity>().gravityScale = 0.5f;
            agentScript.DropObject();


            if (a.GetComponent<Rigidbody>().velocity.y > -0.01) {
                a.SwitchState(0, new SeedGoUpState());
                agentScript.rabitoGiratorio.transform.rotation = Quaternion.identity;
                agentScript.rabitoGiratorio.transform.Rotate(new Vector3(1, 0, 0), -90);

                agentScript.fallTimer = 0;
            } else {
            }
        } else {
            agentScript.fallTimer += Time.deltaTime;
            //Debug.Log(agentScript.fallTimer);
        }
    }

    override public void OnExit(Agent a) {
        a.GetComponent<CustomGravity>().gravityScale = 1;
        Debug.Log("Exit");

    }
}
