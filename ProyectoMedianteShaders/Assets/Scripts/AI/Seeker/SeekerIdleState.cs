using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SeekerIdleState : State {
    float idleThreshold = 0.2f;
    float idleVelocity = 0.1f;
    float idleOffset = 0.2f;

    override public void OnEnter(Agent a) {
        a.GetComponent<Seeker>().rising = true;
        a.gameObject.GetComponent<Seeker>().orbitPos = a.gameObject.transform.position;
        a.gameObject.GetComponent<Rigidbody2D>().gravityScale = 0;
    }

    override public void Update(Agent a, float dt) {
        bool rising = a.GetComponent<Seeker>().rising;

        Vector3 targetPos;
        if (rising) {
            targetPos = a.GetComponent<Seeker>().orbitPos + new Vector3(0, idleOffset, 0);
        } else {
            targetPos = a.GetComponent<Seeker>().orbitPos - new Vector3(0, idleOffset, 0);
        }

        if (Vector2.Distance(targetPos, a.transform.position) > idleThreshold) {
            a.GetComponent<Rigidbody2D>().velocity = (targetPos - a.transform.position).normalized * idleVelocity;

            //Debug.Log(Vector2.Distance(targetPos, a.transform.position));
        } else {
            rising = !rising;


        }

    }

    override public void OnExit(Agent a) {

    }
}
