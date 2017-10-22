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

    //BOOLEANO QUE GESTIONA SI EL TIEMPO ESTA REDUDCIDO O NO
    bool slowMotion;

    //ESTE INT ES -1 SI EL MOVIMIENTO PREVIO FUE HACIA LA IZQUIERDA Y ES 1 SI EL MOVIMIENTO PREVIO FUE HACIA LA DERECHA
    float prevHorizontalMov;

    //Booleano que comprueba si el personaje deberia estar ralentizado en el aire, se reinicia al estar en el suelo (Que se comprueba con colliders2D y tags)
    bool slowedInTheAir;

    //Se inicializan las cosas
    void Start() {
        groundMask = LayerMask.GetMask("Ground");
        rb = GetComponent<Rigidbody2D>();
        slowMotion = false;
        slowedInTheAir = false;
    }

    // Update is called once per frame
    void Update() {
        CheckNotGrounded();
        Move();
        CheckInputs();
        CheckSlowMotion();
    }

    //Método que comprueba si se ha pulsado la tecla control y cambia el booleano slowMotion además de modificar el timescale
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

    //Funcion que hace al personaje moverse hacia los lados a partir del input de las flechas, en caso de estar en el aire se mantiene siempre y cuando se cambie la dirección horizontal en el aire se reduce a la mitad la velocidad
    void Move() {
        bool changed = false;
        float mustSlow = 1;
        if (Input.GetAxisRaw("Horizontal") != (float)prevHorizontalMov) {
            changed = true;
            prevHorizontalMov = Input.GetAxisRaw("Horizontal");
        }

        if (grounded) {
            transform.Translate(Vector3.right * Input.GetAxis("Horizontal") * characterSpeed * Time.deltaTime);
            slowedInTheAir = false;
        }
        else if (changed) {

            slowedInTheAir = true;
            mustSlow = 0.5f;
            transform.Translate(Vector3.right * Input.GetAxis("Horizontal") * characterSpeed * mustSlow * Time.deltaTime);

        }

        else {
            if (slowedInTheAir) {
                mustSlow = 0.5f;
            }
            transform.Translate(Vector3.right * Input.GetAxis("Horizontal") * characterSpeed *mustSlow* Time.deltaTime);

        }

    }

    //Metodo que añade una fuerza al personaje para simular un salto
    void Jump() {
        GetComponent<Rigidbody2D>().AddForce(transform.up * jumpStrenght, ForceMode2D.Impulse);
    }

    //Método que comprueba si la velocidad y del personaje es 0 o aprox. y actualiza el booleano grounded en consecuencia
    void CheckNotGrounded() {
        if (rb.velocity.y < 0.1f && rb.velocity.y>=0.0f || rb.velocity.y > -0.1f && rb.velocity.y <=0.0f) {

        }
        else {
            grounded = false;
        }
    }


    //OnTriggerStay2D y OnTriggerEnter2D que comprueban si el tag del trigger es ground para actualizar el booleano grounded
    private void OnTriggerEnter2D(Collider2D collision) {
        if (collision.tag == "Ground") {
            grounded = true;
        }
    }

    private void OnTriggerStay2D(Collider2D collision) {
        if (collision.tag == "Ground") {
            grounded = true;
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
