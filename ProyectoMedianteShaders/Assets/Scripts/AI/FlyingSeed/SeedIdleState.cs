using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SeedIdleState : State {
    bool rising = true;
    float idleThreshold = 0.2f;
    float idleVelocity = 0.1f;
    float idleOffset = 0.3f;
    override public void OnEnter(Agent a) {
        a.gameObject.GetComponent<FlyingSeed>().orbitPos = a.gameObject.transform.position;
        a.gameObject.GetComponent<Rigidbody2D>().gravityScale = 0;
        Debug.Log("Changing orbit pos");
    }

    override public void Update(Agent a, float dt) {
        FlyingSeed agent = a.gameObject.GetComponent<FlyingSeed>();

        Vector3 targetPos;
        if (rising) {
            targetPos = agent.orbitPos + new Vector3(0, idleOffset, 0);
        } else {
            targetPos = agent.orbitPos - new Vector3(0, idleOffset, 0);
        }

        if (Vector2.Distance(targetPos, agent.transform.position) > idleThreshold) {
            agent.GetComponent<Rigidbody2D>().velocity = (targetPos - agent.transform.position).normalized * idleVelocity;
        } else {
            rising = !rising;
        }

        if (agent.stompedOn) {
            agent.SwitchState(0,new SeedFallState());
        }


    }

    override public void OnExit(Agent a) {

    }
}
