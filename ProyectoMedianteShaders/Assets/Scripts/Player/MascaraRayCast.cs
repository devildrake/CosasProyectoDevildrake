using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MascaraRayCast : MonoBehaviour {
    public PlayerController playerController;
    //public RaycastHit2D hit2D;
    public RaycastHit hit;
    public bool wasHit;
    float duskRayDistance = 0.4f;
    float dawnRayDistance = 0.9f;
    // Use this for initialization
    void Start () {
        playerController = GetComponentInParent<PlayerController>();
        playerController.mascaraRayCast = this;
	}
	
	// Update is called once per frame
	void Update () {

        //hit2D = Physics2D.Raycast(transform.position, Vector2.right, 0.4f, LayerMask.GetMask("Platform"));
        //if (!hit2D) {
        //    hit2D = Physics2D.Raycast(transform.position, Vector2.right, 0.4f, LayerMask.GetMask("Ground"));
        //}

        if (playerController.worldAssignation == DoubleObject.world.DAWN && playerController.dawn) {
            if (playerController.facingRight) {
                wasHit = Physics.Raycast(transform.position, transform.position + Vector3.right, out hit, dawnRayDistance, LayerMask.GetMask("Platform"));
                if (!wasHit) {
                    wasHit = Physics.Raycast(transform.position, transform.position + Vector3.right, out hit, dawnRayDistance, LayerMask.GetMask("Ground"));
                }

                Debug.DrawLine(transform.position, transform.position + Vector3.right * dawnRayDistance);
            } else {
                wasHit = Physics.Raycast(transform.position, transform.position + Vector3.left, out hit, dawnRayDistance, LayerMask.GetMask("Platform"));
                if (wasHit) {
                    wasHit = Physics.Raycast(transform.position, transform.position + Vector3.left, out hit, dawnRayDistance, LayerMask.GetMask("Ground"));
                }
                Debug.DrawLine(transform.position, transform.position + Vector3.left * dawnRayDistance);
            }

        } else if(!playerController.dawn&&playerController.worldAssignation == DoubleObject.world.DUSK){
            if (playerController.facingRight) {
                wasHit = Physics.Raycast(transform.position, transform.position + Vector3.right, out hit, duskRayDistance, LayerMask.GetMask("Platform"));
                if (!wasHit) {
                    wasHit = Physics.Raycast(transform.position, transform.position + Vector3.right, out hit, duskRayDistance, LayerMask.GetMask("Ground"));
                }

                Debug.DrawLine(transform.position, transform.position + Vector3.right * duskRayDistance);
            } else {
                wasHit = Physics.Raycast(transform.position, transform.position + Vector3.left, out hit, duskRayDistance, LayerMask.GetMask("Platform"));
                if (wasHit) {
                    wasHit = Physics.Raycast(transform.position, transform.position + Vector3.left, out hit, duskRayDistance, LayerMask.GetMask("Ground"));
                }
                Debug.DrawLine(transform.position, transform.position + Vector3.left * duskRayDistance);
            }
        }



    }
}
