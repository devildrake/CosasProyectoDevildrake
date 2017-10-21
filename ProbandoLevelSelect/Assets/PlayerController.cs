using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {

    //Velocidad hacia los laterales base
    float characterSpeed = 5;

    //Fuerza del salto
    float jumpStrenght = 5;

    //Booleano que gestiona si el personaje esta en el suelo (Para saber si puede saltar o no)
    [SerializeField]
    bool grounded = false;

    //Mascara de suelo (Mas tarde podria ser util, ahora esta aqui por que para gestionar grounded hice pruebas varias)
    public LayerMask groundMask;

    //Referencia al RigidBody2D del personaje, se inicializa en el start.
    Rigidbody2D rb;

    bool slowMotion;

    //Se inicializan las cosas
    void Start() {
        groundMask = LayerMask.GetMask("Ground");
        rb = GetComponent<Rigidbody2D>();
        slowMotion = false;
    }

    // Update is called once per frame
    void Update() {
        CheckGrounded();
        Move();
        CheckInputs();
        CheckSlowMotion();
    }

    void CheckSlowMotion() {
        if ((Input.GetKeyDown(KeyCode.LeftControl)) && !slowMotion) {
            Time.timeScale = 0.5f;
            slowMotion = true;
        }
        else if ((Input.GetKeyDown(KeyCode.LeftControl)) && slowMotion) {
            Time.timeScale = 1.0f;
            slowMotion = false;
        }
    }

    //Funcion que hace al personaje moverse hacia los lados a partir del input de las flechas, en caso de estar en el aire se mantiene siempre y cuando no se pulsen de nuevo las teclas A o D lo que hace que se reduzca a la mitad la velocidad de movimiento en x 
    void Move() {


        if (grounded)
            transform.Translate(Vector3.right * Input.GetAxis("Horizontal") * characterSpeed * Time.deltaTime);
        else if (Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.A)) {
            transform.Translate((Vector3.right * Input.GetAxis("Horizontal") * characterSpeed) / 2 * Time.deltaTime);
        }

        else {
            transform.Translate(Vector3.right * Input.GetAxis("Horizontal") * characterSpeed * Time.deltaTime);
        }
    }

    //Metodo que añade una fuerza al personaje para simular un salto
    void Jump() {
        GetComponent<Rigidbody2D>().AddForce(transform.up * jumpStrenght, ForceMode2D.Impulse);
    }

    //Método que comprueba si la velocidad y del personaje es 0 o aprox. y actualiza el booleano grounded en consecuencia
    void CheckGrounded() {
        if (rb.velocity.y < 0.1f && rb.velocity.y>=0.0f || rb.velocity.y > -0.1f && rb.velocity.y <=0.0f) {
            grounded = true;
        }
        else {
            grounded = false;
        }
    }

    //Método que comprueba los inputs y actua en consecuencia
    void CheckInputs() {

        //BARRA ESPACIADORA = SALTO
        if (Input.GetKeyDown(KeyCode.Space) && grounded) {
            Jump();
        }
    }

}
