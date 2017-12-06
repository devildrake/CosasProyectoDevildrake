using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ImpulsingAir : MonoBehaviour{
    public List<GameObject> inTriggerZoneObjects;
    public bool changed;
    public float windSpeed=-0.4f;
    bool rising;
    bool active;
// Use this for initialization
void Start () {
        if (GetComponentInParent<DoubleObject>().worldAssignation == DoubleObject.world.DAWN)
            active = true;

}

    // Update is called once per frame
    void Update() {
        if (active) {
            if (rising){
                if (windSpeed < -0.4f){
                    Debug.Log("Pasa a restar");
                    rising = false;
                }else{
                    windSpeed -= 0.5f * Time.deltaTime;
                }
            }
            else{
                if (windSpeed > 0.4f){
                    Debug.Log("Pasa a sumar");
                    rising = true;
                }
                else{
                    windSpeed += 0.7f * Time.deltaTime;
                }

            }


            if (changed){
                if (inTriggerZoneObjects.Count > 0) {
                    foreach (GameObject g in inTriggerZoneObjects){
                        g.GetComponent<Rigidbody2D>().gravityScale = 1;
                        if (g.tag == "Player"){
                            g.GetComponent<PlayerController>().onImpulsor = false;
                        }
                    }
                    inTriggerZoneObjects.Clear();
                    changed = false;
                }
            }
            if (inTriggerZoneObjects.Count != 0){
                GameObject closestItem = inTriggerZoneObjects[0];

                foreach (GameObject g in inTriggerZoneObjects){
                    if (Vector3.Distance(transform.position, g.transform.position) < Vector3.Distance(transform.position, closestItem.transform.position)){
                        closestItem.GetComponent<Rigidbody2D>().gravityScale = 1;
                        closestItem = g;
                    }
                }
                if(windSpeed>0&&!GameLogic.instance.isPaused)
                closestItem.GetComponent<Rigidbody2D>().AddForce(Vector2.up * windSpeed * closestItem.GetComponent<Rigidbody2D>().mass, ForceMode2D.Impulse);

            }
        }
    }

private void OnTriggerEnter2D(Collider2D collision)
{
    
        if (collision.gameObject.GetComponent<Rigidbody2D>() != null)
        {
            if (!inTriggerZoneObjects.Contains(collision.gameObject))
            {
                inTriggerZoneObjects.Add(collision.gameObject);
                if (collision.gameObject.tag == "Player")
                {
                    collision.gameObject.GetComponent<PlayerController>().onImpulsor = true;
                }
            
        }
    }
}

private void OnTriggerStay2D(Collider2D collision)
{
    
        if (collision.gameObject.GetComponent<Rigidbody2D>() != null)
        {
            if (!inTriggerZoneObjects.Contains(collision.gameObject))
            {
                inTriggerZoneObjects.Add(collision.gameObject);
                if (collision.gameObject.tag == "Player")
                {
                    collision.gameObject.GetComponent<PlayerController>().onImpulsor = true;
                }
            }
        
    }
}


private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.GetComponent<Rigidbody2D>() != null)
        {
            if (inTriggerZoneObjects.Contains(collision.gameObject))
            {
                collision.gameObject.GetComponent<Rigidbody2D>().gravityScale = 1;
                inTriggerZoneObjects.Remove(collision.gameObject);
                if (collision.gameObject.tag == "Player")
                {
                    collision.gameObject.GetComponent<PlayerController>().onImpulsor = false;
                }
            }
        
    }
}
}
