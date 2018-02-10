using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TramplerIdleState : State {

    public override void OnEnter(Agent a) {

    }

    public override void Update(Agent a, float dt) {

        RaycastHit2D hit2DA = Physics2D.Raycast(a.transform.position,Vector2.left, (a.GetComponent<Trampler>().pointA - a.transform.position).magnitude, LayerMask.GetMask("Player"));
        RaycastHit2D hit2DB = Physics2D.Raycast(a.transform.position, Vector2.right, (a.GetComponent<Trampler>().pointB - a.transform.position).magnitude, LayerMask.GetMask("Player"));

        //Debug.DrawRay(a.transform.position, (a.GetComponent<Trampler>().pointB - a.transform.position).magnitude*Vector2.left);
        //Debug.Log((a.GetComponent<Trampler>().pointB - a.transform.position).magnitude);
        if (hit2DA) {
            a.SwitchState(0, new TramplerChargeState());
            a.GetComponent<Trampler>().whereTo = 0;
        } else if(hit2DB){
            a.SwitchState(0, new TramplerChargeState());
            a.GetComponent<Trampler>().whereTo = 1;

        }


    }

    public override void OnExit(Agent a) {

    }

}
