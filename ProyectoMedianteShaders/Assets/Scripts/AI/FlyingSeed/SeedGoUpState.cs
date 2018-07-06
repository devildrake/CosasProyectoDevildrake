using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SeedGoUpState : State {
    public float maxTimeOnTheGround = 3;
    float speedUp=0.2f;
    float stompTimer=0;
    float stompTime = 0.2f;
    override public void OnEnter(Agent a) {
        FlyingSeed agentScript = a.GetComponent<FlyingSeed>();

        agentScript.timeOnTheGround = 0;
        //a.GetComponent<Rigidbody2D>().gravityScale = 0;
        a.GetComponent<CustomGravity>().gravityScale = 0;
        agentScript.stompedOn = false;
        agentScript.detectStompObject.GetComponent<DetectStomp>().active = true;
    }



    override public void Update(Agent a, float dt) {
        FlyingSeed agentScript = a.GetComponent<FlyingSeed>();

        if (agentScript.grabbedObject==null)
            agentScript.CheckForObjects();

        if (stompTime>stompTimer)
        stompTimer += dt;

        //Lleva el tiempo establecido en el suelo, así que sube hasta alcanzar speed>1
        if (agentScript.timeOnTheGround > maxTimeOnTheGround) {
            if (a.GetComponent<Rigidbody>().velocity.y < 1) {
                a.GetComponent<Rigidbody>().velocity = new Vector2(0, a.GetComponent<Rigidbody>().velocity.y + speedUp * Time.deltaTime);
            } else {
                a.SwitchState(0, new SeedIdleState());
            }
        //Falta tiempo en el suelo
        } else {
            agentScript.timeOnTheGround += Time.deltaTime;
        }

        //Comprobación de cambio de estado
        if (agentScript.stompedOn&&stompTimer>stompTime&& agentScript.timeOnTheGround>maxTimeOnTheGround) {
            agentScript.SwitchState(0, new SeedFallState());
            stompTimer = 0;
        }
    }

    

    override public void OnExit(Agent a) {
        a.GetComponent<FlyingSeed>().detectStompObject.GetComponent<DetectStomp>().active = false;

    }
}
