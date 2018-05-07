using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MascaraRayCast : MonoBehaviour {
    public PlayerController playerController;
    public RaycastHit2D hit2D;

    // Use this for initialization
    void Start () {
        playerController = GetComponentInParent<PlayerController>();
        playerController.mascaraRayCast = this;
	}
	
	// Update is called once per frame
	void Update () {

        hit2D = Physics2D.Raycast(transform.position, Vector2.right, 0.4f, LayerMask.GetMask("Platform"));
        if (!hit2D) {
            hit2D = Physics2D.Raycast(transform.position, Vector2.right, 0.4f, LayerMask.GetMask("Ground"));
        }

    }
}
