using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SeekerIdleState : State {
    float idleThreshold = 0.2f;
    float idleVelocity = 0.1f;
    float idleOffset = 0.3f;

    override public void OnEnter(Agent a) {
        a.GetComponent<Seeker>().rising = true;
        a.gameObject.GetComponent<Seeker>().orbitPos = a.gameObject.transform.position;
        a.gameObject.GetComponent<Rigidbody2D>().gravityScale = 0;
    }

    override public void Update(Agent a, float dt) {
        Agent agent = a.gameObject.GetComponent<Agent>();


        Vector3 targetPos;
        if (a.GetComponent<Seeker>().rising) {
            targetPos = agent.GetComponent<Seeker>().orbitPos + new Vector3(0, idleOffset, 0);
        } else {
            targetPos = agent.GetComponent<Seeker>().orbitPos - new Vector3(0, idleOffset, 0);
        }

        if (Vector2.Distance(targetPos, agent.transform.position) > idleThreshold) {
            agent.GetComponent<Rigidbody2D>().velocity = (targetPos - agent.transform.position).normalized * idleVelocity;
            Debug.Log(Vector2.Distance(targetPos, agent.transform.position));
        } else {
            a.GetComponent<Seeker>().rising = !a.GetComponent<Seeker>().rising;


        }


    }

    override public void OnExit(Agent a) {

    }
}
