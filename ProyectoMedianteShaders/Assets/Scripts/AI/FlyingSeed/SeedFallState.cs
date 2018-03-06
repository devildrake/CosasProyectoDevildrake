using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SeedFallState : State {
    override public void OnEnter(Agent a) {
        //a.GetComponent<Rigidbody2D>().velocity = new Vector2(0, -0.01f);
        a.GetComponent<Rigidbody2D>().velocity = new Vector2(0, -0.01f);

    }

    override public void Update(Agent a,float dt) {

        if (a.GetComponent<FlyingSeed>().fallTimer > a.GetComponent<FlyingSeed>().fallTime) {

            a.GetComponent<FlyingSeed>().Spin(-1500.0f);

            a.GetComponent<FlyingSeed>().DropObject();

            a.GetComponent<Rigidbody2D>().gravityScale = 0.5f;
            a.GetComponent<FlyingSeed>().DropObject();


            if (a.GetComponent<Rigidbody2D>().velocity.y == 0) {
                a.SwitchState(0, new SeedGoUpState());
                a.GetComponent<FlyingSeed>().rabitoGiratorio.transform.rotation = Quaternion.identity;
                a.GetComponent<FlyingSeed>().rabitoGiratorio.transform.Rotate(new Vector3(1, 0, 0), -90);

                a.GetComponent<FlyingSeed>().fallTimer = 0;
            }
        } else {
            a.GetComponent<FlyingSeed>().fallTimer += Time.deltaTime;
        }
    }

    override public void OnExit(Agent a) {
        Debug.Log("Exit");

    }
}
