using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SeedPathFollowState : State {
    float maxSpeed=3;
    float minSpeed=1;
    float threshold = 0.2f;
    float slowThreshold = 2.0f;
    float followSpeed = 0;
    bool increasing;
    
    public override void OnEnter(Agent a) {
        increasing = true;
        //Debug.Log("PathFollow");
    }

    public override void Update(Agent a, float dt) {

        FlyingSeed agentScript = a.GetComponent<FlyingSeed>();
        agentScript.Spin(1500.0f);
        


        int currentTarget = agentScript.currentTarget;
        a.GetComponent<Rigidbody>().useGravity = false;

        if (Vector2.Distance(agentScript.VectorPatrolPoints[currentTarget], a.transform.position) > threshold) {
            //ATENUACIÓN SE SPEED CUANDO ESTA LLEGANDO
            if(Vector2.Distance(agentScript.VectorPatrolPoints[currentTarget], a.transform.position) > slowThreshold) {
                followSpeed = Mathf.Clamp(followSpeed+Time.deltaTime,minSpeed,maxSpeed);
            } 
            //RECUPERA LA SPEED NORMAL SI NO ESTA LLEGANDO
            else {
                followSpeed = maxSpeed - (2 - Vector2.Distance(agentScript.VectorPatrolPoints[currentTarget], a.transform.position));
            }

            

            //SET VELOCITY A CADA FRAME
            a.gameObject.GetComponent<Rigidbody>().velocity = ((agentScript.VectorPatrolPoints[currentTarget]-a.transform.position).normalized * followSpeed);
        } else {
            if (increasing) {
                if (currentTarget < agentScript.VectorPatrolPoints.Length - 1) {
                    currentTarget++;
                    agentScript.currentTarget++;

                } else {
                    increasing = false;
                }
            } else {
                if (currentTarget > 0) {
                    currentTarget--;
                    agentScript.currentTarget--;
                } else {
                    increasing = true;
                }
            } 
        }

        if (a.touchedByPlayer) {
            Debug.Log("Touch");
            a.SwitchState(1, new SeedBlowUpState());
        }


    }

    public override void OnExit(Agent a) {
        a.gameObject.GetComponent<Rigidbody>().velocity = new Vector2(0, 0);
    }
}
