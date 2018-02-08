using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SeedGoUpState : State {
    public float maxTimeOnTheGround = 3;
    float speedUp=0.2f;
    float stompTimer=0;
    float stompTime = 0.2f;
    override public void OnEnter(Agent a) {
        a.GetComponent<FlyingSeed>().timeOnTheGround = 0;
        a.GetComponent<Rigidbody2D>().gravityScale = 0;
        a.GetComponent<FlyingSeed>().stompedOn = false;
        a.GetComponent<FlyingSeed>().detectStompObject.GetComponent<DetectStomp>().active = true;
    }



    override public void Update(Agent a, float dt) {

        if(a.GetComponent<FlyingSeed>().grabbedObject==null)
        a.GetComponent<FlyingSeed>().CheckForObjects();

        if (stompTime>stompTimer)
        stompTimer += dt;

        //Lleva el tiempo establecido en el suelo, así que sube hasta alcanzar speed>1
        if (a.GetComponent<FlyingSeed>().timeOnTheGround > maxTimeOnTheGround) {
            if (a.GetComponent<Rigidbody2D>().velocity.y < 1) {
                a.GetComponent<Rigidbody2D>().velocity = new Vector2(0, a.GetComponent<Rigidbody2D>().velocity.y + speedUp * Time.deltaTime);
            } else {
                a.SwitchState(0, new SeedIdleState());
            }
        //Falta tiempo en el suelo
        } else {
            a.GetComponent<FlyingSeed>().timeOnTheGround += Time.deltaTime;
        }

        //Comprobación de cambio de estado
        if (a.GetComponent<FlyingSeed>().stompedOn&&stompTimer>stompTime&&a.GetComponent<FlyingSeed>().timeOnTheGround>maxTimeOnTheGround) {
            a.GetComponent<FlyingSeed>().SwitchState(0, new SeedFallState());
            stompTimer = 0;
        }
    }

    

    override public void OnExit(Agent a) {
        a.GetComponent<FlyingSeed>().detectStompObject.GetComponent<DetectStomp>().active = false;

    }
}
