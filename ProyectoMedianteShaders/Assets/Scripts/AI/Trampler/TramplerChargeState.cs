using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TramplerChargeState : State {
    float threshold = 0.2f;
    float accelerationRate = 4.5f;

    public override void OnEnter(Agent a) {
        a.touchedByPlayer = false;
        a.GetComponent<Trampler>().mustStop = false;

    }

    public override void Update(Agent a, float dt) {
        RaycastHit2D hitWallRight = PlayerUtilsStatic.RayCastArrayMask(a.transform.position+ new Vector3(0.5f, 0, 0), Vector2.right,.1f,a.GetComponent<Trampler>().masks);
        RaycastHit2D hitWallLeft = PlayerUtilsStatic.RayCastArrayMask(a.transform.position+new Vector3(-0.5f,0,0), Vector2.left, .1f, a.GetComponent<Trampler>().masks);

        //Debug.DrawRay(a.transform.position + new Vector3(0.5f, 0, 0), Vector2.right* .1f);

        if ((hitWallRight &&a.GetComponent<Trampler>().currentSpeed>0)|| (hitWallLeft&& a.GetComponent<Trampler>().currentSpeed < 0)) {
            a.SwitchState(0, new TramplerStunnedState());
            Debug.Log("HITWALL");
        }

        if (a.touchedByPlayer) {
            a.touchedByPlayer = false;
            GameLogic.instance.KillPlayer();
        }

        if (a.GetComponent<Trampler>().whereTo == 0) {
            //La distancia entre el punto A y el objeto (en X) es superior a Threshold, por lo que debe seguir moviendose
            //Debug.Log(Mathf.Abs((a.transform.position.x - a.GetComponent<Trampler>().pointA.x)));
            if (Mathf.Abs((a.transform.position.x - a.GetComponent<Trampler>().pointA.x)) > threshold && a.GetComponent<Trampler>().mustStop == false) {
                // a.GetComponent<Rigidbody2D>().velocity = new Vector2(Mathf.Clamp(a.GetComponent<Rigidbody2D>().velocity.x - accelerationRate * Time.deltaTime, -a.GetComponent<Trampler>().maxSpeed, 0),0);

                a.GetComponent<Trampler>().currentSpeed -= (accelerationRate * Time.deltaTime);
                a.GetComponent<Trampler>().currentSpeed = Mathf.Clamp(a.GetComponent<Trampler>().currentSpeed, -a.GetComponent<Trampler>().maxSpeed, 0);

                a.GetComponent<Rigidbody2D>().velocity = new Vector2(a.GetComponent<Trampler>().currentSpeed, 0);//new Vector2(a.GetComponent<Rigidbody2D>().velocity.x - accelerationRate * Time.deltaTime, 0);

                //Vector2 velocity = a.GetComponent<Rigidbody2D>().velocity;
                //Debug.Log(accelerationRate * Time.deltaTime);


            } else {
                a.GetComponent<Trampler>().mustStop = true;

                //Debug.Log("arrived");
                //Llega a A, se cansa 
                a.GetComponent<Trampler>().currentSpeed -= a.GetComponent<Trampler>().currentSpeed * Time.deltaTime*1.5f;

                a.GetComponent<Rigidbody2D>().velocity = new Vector2(a.GetComponent<Trampler>().currentSpeed,0);


                if (a.GetComponent<Rigidbody2D>().velocity.x>=-0.2f) {
                    a.SwitchState(0, new TramplerStunnedState());
                }

            }

        }else if (a.GetComponent<Trampler>().whereTo == 1){

            //La distancia entre el punto B y el objeto (en X) es superior a Threshold, por lo que debe seguir moviendose
            if (Mathf.Abs((a.transform.position.x - a.GetComponent<Trampler>().pointB.x)) > threshold&& a.GetComponent<Trampler>().mustStop == false) {

                a.GetComponent<Trampler>().currentSpeed += (accelerationRate * Time.deltaTime);
                a.GetComponent<Trampler>().currentSpeed = Mathf.Clamp(a.GetComponent<Trampler>().currentSpeed, 0, a.GetComponent<Trampler>().maxSpeed);

                a.GetComponent<Rigidbody2D>().velocity = new Vector2(a.GetComponent<Trampler>().currentSpeed, 0);//new Vector2(a.GetComponent<Rigidbody2D>().velocity.x - accelerationRate * Time.deltaTime, 0);
            } else {
                a.GetComponent<Trampler>().mustStop = true;

                //Llega a B, se cansa 

                a.GetComponent<Trampler>().currentSpeed -= a.GetComponent<Trampler>().currentSpeed * Time.deltaTime*1.5f;

                a.GetComponent<Rigidbody2D>().velocity = new Vector2(a.GetComponent<Trampler>().currentSpeed, 0);

                if (a.GetComponent<Rigidbody2D>().velocity.x <= 0.2f) {
                    a.SwitchState(0, new TramplerStunnedState());
                }

            }
        } else {
            //a.SwitchState(0, new TramplerIdleState());
        }


    }

    public override void OnExit(Agent a) {
        a.GetComponent<Trampler>().currentSpeed = 0;
        a.GetComponent<Trampler>().ResetPoints();
        a.GetComponent<Trampler>().whereTo = 2;
    }
}
