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
        Debug.Log("PathFollow");
    }

    public override void Update(Agent a, float dt) {


            a.GetComponent<FlyingSeed>().Spin(1500.0f);
        
        for(int i = 0; i < a.GetComponent<FlyingSeed>().VectorPatrolPoints.Length; i++) {
            Debug.Log(a.GetComponent<FlyingSeed>().VectorPatrolPoints[i]);
        }

        int currentTarget = a.GetComponent<FlyingSeed>().currentTarget;
        FlyingSeed fs = a.gameObject.GetComponent<FlyingSeed>();
        a.GetComponent<Rigidbody2D>().gravityScale = 0;
        if (Vector2.Distance(fs.VectorPatrolPoints[currentTarget], a.transform.position) > threshold) {
            //ATENUACIÓN SE SPEED CUANDO ESTA LLEGANDO
            if(Vector2.Distance(fs.VectorPatrolPoints[currentTarget], a.transform.position) > slowThreshold) {
                followSpeed = Mathf.Clamp(followSpeed+Time.deltaTime,minSpeed,maxSpeed);
            } 
            //RECUPERA LA SPEED NORMAL SI NO ESTA LLEGANDO
            else {
                followSpeed = maxSpeed - (2 - Vector2.Distance(fs.VectorPatrolPoints[currentTarget], a.transform.position));
            }

            

            //SET VELOCITY A CADA FRAME
            a.gameObject.GetComponent<Rigidbody2D>().velocity = ((fs.VectorPatrolPoints[currentTarget]-a.transform.position).normalized * followSpeed);
        } else {
            if (increasing) {
                if (currentTarget < fs.VectorPatrolPoints.Length - 1) {
                    currentTarget++;
                    a.GetComponent<FlyingSeed>().currentTarget++;

                } else {
                    increasing = false;
                }
            } else {
                if (currentTarget > 0) {
                    currentTarget--;
                    a.GetComponent<FlyingSeed>().currentTarget--;
                } else {
                    increasing = true;
                }
            } 
        }

        if (a.touchedByPlayer) {
            a.SwitchState(1, new SeedBlowUpState());
        }


    }

    public override void OnExit(Agent a) {
        a.gameObject.GetComponent<Rigidbody2D>().velocity = new Vector2(0, 0);
    }
}
