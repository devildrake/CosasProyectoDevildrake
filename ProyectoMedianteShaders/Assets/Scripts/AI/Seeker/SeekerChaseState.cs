using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SeekerChaseState : State {
    public override void OnEnter(Agent a) {

        if (GameLogic.instance.currentPlayer != null)
            a.GetComponent<Seeker>().target = GameLogic.instance.currentPlayer.transform;
        a.GetComponent<Seeker>().timeOutOfSight = 0;

    }

    public override void Update(Agent a, float dt) {

        Transform target = a.GetComponent<Seeker>().target;
        Vector2 targetDir = target.position - a.transform.position;
        Vector2 whereTo = a.transform.right;
        if (a.GetComponent<Rigidbody2D>().velocity.x < 0) {
            whereTo *= -1;
        }

        float angle = Vector2.Angle(targetDir, whereTo);
        if (angle > a.GetComponent<Seeker>().coneAngle || Vector2.Distance(target.position, a.transform.position) > a.GetComponent<Seeker>().visionRange) {
            //Debug.Log(angle);
            a.GetComponent<Seeker>().timeOutOfSight += Time.deltaTime;
        } else {
            a.GetComponent<Seeker>().timeOutOfSight = 0;
            Debug.Log(Vector2.Distance(a.transform.position, target.position) + "Y " + a.GetComponent<Seeker>().visionRange);
            //Debug.Log(a.GetComponent<Seeker>().visionRange)
            //Debug.Log(Vector2.Distance(target.position, a.transform.position) > a.GetComponent<Seeker>().visionRange);
        }

        if (a.GetComponent<Seeker>().timeOutOfSight > 2.0f) {
            a.SwitchState(0, new SeekerPathFollowState());
        }

        a.GetComponent<Seeker>().lastPlayerPosSeen = target.transform.position;

        Vector3 direction = (a.GetComponent<Seeker>().lastPlayerPosSeen - a.transform.position).normalized;
        a.GetComponent<Rigidbody2D>().velocity = new Vector2(direction.x * a.GetComponent<Seeker>().chaseSpeed, direction.y * a.GetComponent<Seeker>().chaseSpeed);






    }

    public override void OnExit(Agent a) {
        a.GetComponent<Seeker>().timeOutOfSight = 0;
    }
}
