using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SeekerPathFollowState : State {

    float maxSpeed = 3;
    float minSpeed = 1;
    float threshold = 0.2f;
    float slowThreshold = 2.0f;
    float followSpeed = 0;


    public override void OnEnter(Agent a) {
 
    }

    public override void Update(Agent a, float dt) {

        Seeker agentScript = a.GetComponent<Seeker>();


        if (GameLogic.instance.currentPlayer != null)
            agentScript.target = GameLogic.instance.currentPlayer.transform;

        int currentTarget = agentScript.currentTarget;

        //Debug.Log(currentTarget);
        //Debug.Log(Vector2.Distance(seek.Path_Points[currentTarget].position, a.transform.position));

        a.GetComponent<Rigidbody2D>().gravityScale = 0;
        if (Vector2.Distance(agentScript.Path_Positions[currentTarget], a.transform.position) > threshold) {
            //ATENUACIÓN SE SPEED CUANDO ESTA LLEGANDO
            if (Vector2.Distance(agentScript.Path_Positions[currentTarget], a.transform.position) > slowThreshold) {
                followSpeed = Mathf.Clamp(followSpeed + Time.deltaTime, minSpeed, maxSpeed);
            }
            //RECUPERA LA SPEED NORMAL SI NO ESTA LLEGANDO
            else {
                followSpeed = maxSpeed - (2 - Vector2.Distance(agentScript.Path_Positions[currentTarget], a.transform.position));
            }

            //SET VELOCITY A CADA FRAME
            a.gameObject.GetComponent<Rigidbody2D>().velocity = ((agentScript.Path_Positions[currentTarget] - a.transform.position).normalized * followSpeed);
            //Debug.Log((seek.Path_Points[currentTarget].position - a.transform.position).normalized * followSpeed);
            //Debug.Log(a.gameObject.GetComponent<Rigidbody2D>().velocity);

        } else {
            if (a.GetComponent<Seeker>().increasing) {
                if (currentTarget < agentScript.Path_Positions.Length - 1) {
                    currentTarget++;
                    agentScript.currentTarget++;

                } else {
                    agentScript.increasing = false;
                }
            } else {
                if (currentTarget > 0) {
                    currentTarget--;
                    agentScript.currentTarget--;
                } else {
                    agentScript.increasing = true;
                }
            }
        }
        Transform target = a.GetComponent<Seeker>().target;
            Vector2 targetDir = target.position - a.transform.position;
            Vector2 whereTo = a.transform.right;
        if (a.GetComponent<Rigidbody2D>().velocity.x < 0) {
            whereTo *= -1;
        }

        float angle = Vector2.Angle(targetDir, whereTo);
        if (angle < agentScript.coneAngle && Vector2.Distance(target.position,a.transform.position) < agentScript.visionRange) {
            //Debug.Log(angle);
            a.SwitchState(0, new SeekerChaseState());
        }

        //Debug.DrawLine(a.transform.position, a.transform.position);
        //Debug.Log(angle);



    }

    public override void OnExit(Agent a) {
        a.gameObject.GetComponent<Rigidbody2D>().velocity = new Vector2(0, 0);
    }
}
