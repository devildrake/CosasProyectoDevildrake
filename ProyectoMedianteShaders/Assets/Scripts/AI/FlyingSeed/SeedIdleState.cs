using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SeedIdleState : State {
    //bool rising = true;
    float idleThreshold = 0.2f;
    float idleVelocity = 0.1f;
    float idleOffset = 0.3f;

    override public void OnEnter(Agent a) {

        FlyingSeed agentScript = a.GetComponent<FlyingSeed>();

        agentScript.orbitPos = a.gameObject.transform.position;
        a.gameObject.GetComponent<Rigidbody>().useGravity = false;
        //Debug.Log("Changing orbit pos");
        a.GetComponent<Agent>().stompedOn = false;

        agentScript.detectStompObject.GetComponent<DetectStomp>().active = true;

    }

    override public void Update(Agent a, float dt) {
        FlyingSeed agentScript = a.GetComponent<FlyingSeed>();


        bool rising = agentScript.rising;
        Agent agent = a.gameObject.GetComponent<Agent>();

        Vector3 targetPos;
        if (rising) {
            targetPos = agentScript.orbitPos + new Vector3(0, idleOffset, 0);
        } else {
            targetPos = agentScript.orbitPos - new Vector3(0, idleOffset, 0);
        }

        if (Vector2.Distance(targetPos, agent.transform.position) > idleThreshold) {
            agent.GetComponent<Rigidbody>().velocity = (targetPos - agent.transform.position).normalized * idleVelocity;
        } else {
            rising = !rising;
        }

        if (agent.stompedOn) {
            agent.SwitchState(0,new SeedFallState());
        }


    }

    override public void OnExit(Agent a) {
        a.GetComponent<FlyingSeed>().detectStompObject.GetComponent<DetectStomp>().active = false;

    }
}



