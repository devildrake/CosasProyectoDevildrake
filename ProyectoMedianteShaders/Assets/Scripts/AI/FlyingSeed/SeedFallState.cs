using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SeedFallState : State {
    override public void OnEnter(Agent a) {
        //a.GetComponent<Rigidbody2D>().velocity = new Vector2(0, -0.01f);
        a.GetComponent<Rigidbody2D>().velocity = new Vector2(0, -0.01f);

    }

    override public void Update(Agent a,float dt) {
        FlyingSeed agentScript = a.GetComponent<FlyingSeed>();


        if (agentScript.fallTimer > a.GetComponent<FlyingSeed>().fallTime) {

            agentScript.Spin(-1500.0f);

            agentScript.DropObject();

            a.GetComponent<Rigidbody2D>().gravityScale = 0.5f;
            agentScript.DropObject();


            if (a.GetComponent<Rigidbody2D>().velocity.y == 0) {
                a.SwitchState(0, new SeedGoUpState());
                agentScript.rabitoGiratorio.transform.rotation = Quaternion.identity;
                agentScript.rabitoGiratorio.transform.Rotate(new Vector3(1, 0, 0), -90);

                agentScript.fallTimer = 0;
            }
        } else {
            agentScript.fallTimer += Time.deltaTime;
        }
    }

    override public void OnExit(Agent a) {
        Debug.Log("Exit");

    }
}
