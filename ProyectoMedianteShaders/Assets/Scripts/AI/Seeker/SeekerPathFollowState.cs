using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SeekerPathFollowState : State {

    float maxSpeed = 3;
    float minSpeed = 1;
    float threshold = 0.2f;
    float slowThreshold = 2.0f;
    float followSpeed = 0;
    bool increasing;

    public override void OnEnter(Agent a) {
        increasing = true;
        Debug.Log("PathFollow");
    }

    public override void Update(Agent a, float dt) {

        int currentTarget = a.GetComponent<Seeker>().currentTarget;

        Seeker seek = a.gameObject.GetComponent<Seeker>();
        a.GetComponent<Rigidbody2D>().gravityScale = 0;
        if (Vector2.Distance(seek.Path_Points[currentTarget].position, a.transform.position) > threshold) {
            //ATENUACIÓN SE SPEED CUANDO ESTA LLEGANDO
            if (Vector2.Distance(seek.Path_Points[currentTarget].position, a.transform.position) > slowThreshold) {
                followSpeed = Mathf.Clamp(followSpeed + Time.deltaTime, minSpeed, maxSpeed);
            }
            //RECUPERA LA SPEED NORMAL SI NO ESTA LLEGANDO
            else {
                followSpeed = maxSpeed - (2 - Vector2.Distance(seek.Path_Points[currentTarget].position, a.transform.position));
            }



            //SET VELOCITY A CADA FRAME
            a.gameObject.GetComponent<Rigidbody2D>().velocity = ((seek.Path_Points[currentTarget].position - a.transform.position).normalized * followSpeed);
        } else {
            if (increasing) {
                if (currentTarget < seek.Path_Points.Length - 1) {
                    currentTarget++;
                    a.GetComponent<Seeker>().currentTarget++;

                } else {
                    increasing = false;
                }
            } else {
                if (currentTarget > 0) {
                    currentTarget--;
                    a.GetComponent<Seeker>().currentTarget--;
                } else {
                    increasing = true;
                }
            }
        }

        if (seek.playerInVisionCone) {
            a.SwitchState(1, new SeekerChaseState());
        }


    }

    public override void OnExit(Agent a) {
        a.gameObject.GetComponent<Rigidbody2D>().velocity = new Vector2(0, 0);
    }
}
