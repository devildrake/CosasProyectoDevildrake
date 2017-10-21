using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {
    float characterSpeed = 5;
    float jumpStrenght = 5;
    [SerializeField]
    bool grounded = false;
    public LayerMask groundMask;
    Rigidbody2D rb;
    void Start() {
        groundMask = LayerMask.GetMask("Ground");
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update() {
        Move();
        CheckGrounded();
        CheckInputs();
    }


    void Move() {
        transform.Translate(Vector3.right * Input.GetAxis("Horizontal") * characterSpeed * Time.deltaTime);
    }

    void Jump() {
        GetComponent<Rigidbody2D>().AddForce(transform.up * jumpStrenght, ForceMode2D.Impulse);
    }

    void CheckGrounded() {
        if (rb.velocity.y < 0.1f && rb.velocity.y>=0.0f || rb.velocity.y > -0.1f && rb.velocity.y <=0.0f) {
            grounded = true;
        }
        else {
            grounded = false;
        }
    }

    void CheckInputs() {
        if (Input.GetKeyDown(KeyCode.Space) && grounded) {
            Jump();
        }
    }

}
