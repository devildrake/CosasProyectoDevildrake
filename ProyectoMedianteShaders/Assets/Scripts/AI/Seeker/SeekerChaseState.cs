using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SeekerChaseState : State {
    public override void OnEnter(Agent a) {
        Seeker agentScript = a.GetComponent<Seeker>();
        if (GameLogic.instance.currentPlayer != null)
            agentScript.target = GameLogic.instance.currentPlayer.transform;

        agentScript.timeOutOfSight = 0;
    }

    public override void Update(Agent a, float dt) {
        Seeker agentScript = a.GetComponent<Seeker>();

        Transform target = agentScript.target;
        Vector2 targetDir = target.position - a.transform.position;
        Vector2 whereTo = a.transform.right;
        if (a.GetComponent<Rigidbody2D>().velocity.x < 0) {
            whereTo *= -1;
        }

        float angle = Vector2.Angle(targetDir, whereTo);
        if (angle > agentScript.coneAngle || Vector2.Distance(target.position, a.transform.position) > agentScript.visionRange) {
            //Debug.Log(angle);
            agentScript.timeOutOfSight += Time.deltaTime;
        } else {
            agentScript.timeOutOfSight = 0;
            //Debug.Log(Vector2.Distance(a.transform.position, target.position) + "Y " + a.GetComponent<Seeker>().visionRange);
            //Debug.Log(a.GetComponent<Seeker>().visionRange)
            //Debug.Log(Vector2.Distance(target.position, a.transform.position) > a.GetComponent<Seeker>().visionRange);
        }

        if (target.GetComponent<PlayerController>().behindBush&&target.GetComponent<PlayerController>().crawling) {
            agentScript.timeOutOfSight = 2.01f;
        }

        if (agentScript.timeOutOfSight > 2.0f) {
            a.SwitchState(0, new SeekerPathFollowState());
        }

        agentScript.lastPlayerPosSeen = target.transform.position;

        Vector3 direction = (agentScript.lastPlayerPosSeen - a.transform.position).normalized;
        a.GetComponent<Rigidbody2D>().velocity = new Vector2(direction.x * agentScript.chaseSpeed, direction.y * agentScript.chaseSpeed);

    }

    public override void OnExit(Agent a) {
        a.GetComponent<Seeker>().timeOutOfSight = 0;
    }
}
