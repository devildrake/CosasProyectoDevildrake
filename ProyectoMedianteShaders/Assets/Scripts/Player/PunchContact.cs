using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PunchContact : MonoBehaviour {
    public Vector3 direction;
    public bool mustPunch=false;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        gameObject.transform.rotation = Quaternion.identity;	
	}

    void OnTriggerEnter(Collider col) {
        //Debug.Log("Nen");
        if (col.GetComponent<DoubleObject>()!= null) {
            if (col.GetComponent<DoubleObject>().isPunchable) {
                if (mustPunch) {
                    col.gameObject.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeRotation;
                    col.gameObject.GetComponent<Rigidbody>().AddForce(direction * 40000, ForceMode.Impulse);
                    col.gameObject.GetComponent<DoubleObject>().isPunchable = false;
                    mustPunch = false;
                }
            }
        }
    }
}
