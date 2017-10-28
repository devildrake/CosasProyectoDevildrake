using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : Transformable {

    //Booleano para modificar hacia donde mira el personaje
    bool faceRight;

    //Booleano privado que maneja que el personaje pueda utilizar el dash, se pone en true a la vez que el grounded y en false cuando se usa el Dash
    [SerializeField]
    private bool canDash;

    //Vector dirección para el dash/Hit
    private Vector2 direction;

    //Hijo asignado a mano por ahora, anchor de rotación
    public GameObject arrow;

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

    //ESTE INT ES -1 SI EL MOVIMIENTO PREVIO FUE HACIA LA IZQUIERDA Y ES 1 SI EL MOVIMIENTO PREVIO FUE HACIA LA DERECHA
    float prevHorizontalMov;

    [SerializeField]
    //Booleano que comprueba si el personaje deberia estar ralentizado en el aire, se reinicia al estar en el suelo (Que se comprueba con colliders2D y tags)
    bool slowedInTheAir;

    //Se inicializan las cosas
    void Start() {
        prevHorizontalMov = 1;
        faceRight = true;
        groundMask = LayerMask.GetMask("Ground");
        rb = GetComponent<Rigidbody2D>();
        slowedInTheAir = false;

        //Se hace esto para que UseDirectionCircle haga uso de su bool once y inicialize las cosas que le interesan
        direction = DirectionCircle.UseDirectionCircle(arrow, gameObject);


        //Start del transformable
        InitTransformable();
    }
    void Update() {
        //Add del transformable
        AddToGameLogicList();

        if (!GameLogic.instance.isPaused) {
            CheckNotGrounded();
            Move();
            CheckInputs();
        }else {


        }
        //CheckSlowMotion();
    }

    //Método setter para canDash
    void SetCanDash(bool a) {
        canDash = a;
    }

    //Método que cambia hacia donde mira el personaje, se le llama al cambiar el Input.GetAxis Horizontal de 1 a -1 o alreves
    void Flip(){
        Vector3 theScale = transform.localScale;
        theScale.x *= -1;
        transform.localScale = theScale;
    }

    //Funcion que hace al personaje moverse hacia los lados a partir del input de las flechas, en caso de estar en el aire se mantiene siempre y cuando se cambie la dirección horizontal en el aire se reduce a la mitad la velocidad
    void Move() {
        bool changed = false;
        float mustSlow = 1;
        if (Input.GetAxisRaw("Horizontal") != (float)prevHorizontalMov&&Input.GetAxisRaw("Horizontal")!=0.0f) {
            changed = true;
            prevHorizontalMov = Input.GetAxisRaw("Horizontal");
        }

        if (grounded) {
            GetComponent<Rigidbody2D>().AddForce(new Vector2(-GetComponent<Rigidbody2D>().velocity.x, 0), ForceMode2D.Impulse);
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
                rb.AddForce(new Vector2(-rb.velocity.x, 0), ForceMode2D.Impulse);
                mustSlow = 0.5f;
            }
            transform.Translate(Vector3.right * Input.GetAxis("Horizontal") * characterSpeed *mustSlow* Time.deltaTime);



        }

        if (changed) {
            Flip();
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
            SetCanDash(true);
            grounded = true;
        }
    }

    private void OnTriggerStay2D(Collider2D collision) {
        if (collision.tag == "Ground") {
            grounded = true;
            if (rb.velocity.y == 0) {
                SetCanDash(true);
            }
        }
    }


    //Método que comprueba los inputs y actua en consecuencia
    void CheckInputs() {
        //BARRA ESPACIADORA = SALTO
        if (Input.GetKeyDown(KeyCode.Space) && grounded) {
            Jump();
        }

 


        //Comportamiento de dawn
        if (dawn) {

            ChangeToDawn();

            if(canDash)       
                direction = DirectionCircle.UseDirectionCircle(arrow, gameObject);

            //Si se suelta el botón izquierdo del ratón y se puede dashear, se desactiva la slowMotion y se modifica el timeScale además de poner en false canDash
            if (Input.GetMouseButtonUp(0) && canDash) {
                GameLogic.instance.SetTimeScaleLocal(1.0f);
                Dash.DoDash(gameObject, direction, 10);
                SetCanDash(false);


                //Si se pulsa el botón izquierdo del ratón y se puede dashear, se activa la slowMotion y se modifica el timeScale de gameLogic
            }
            else if (Input.GetMouseButtonDown(0) && canDash) {
                GameLogic.instance.SetTimeScaleLocal(0.5f);
            }
        }

        //Comportamiento de dusk

        else {
            if (Input.GetMouseButtonUp(0)) {
                GetComponentInChildren<PunchArea>().Punch(direction,10);
                GameLogic.instance.SetTimeScaleLocal(1.0f);

            }
            else if (Input.GetMouseButtonDown(0) && canDash) {
                GameLogic.instance.SetTimeScaleLocal(0.5f);

            }

            ChangeToDusk();
            direction = DirectionCircle.UseDirectionCircle(arrow, gameObject);


        }
    }

    //Método que gestiona el momento inicial de cambiar a dawn si es que hay cambio
    void ChangeToDawn() {
        if (prevDawn != dawn) {
            changedWorld = true;
            prevDawn = dawn;
        }
        if (changedWorld) {
            changedWorld = false;
            DirectionCircle.SetOnce(false);
        }
    }

    //Método que gestiona el momento inicial de cambiar a dusk si es que hay cambio
    void ChangeToDusk() {
        if (prevDawn != dawn) {
            changedWorld = true;
            prevDawn = dawn;
        }
        if (changedWorld) {
            changedWorld = false;
            DirectionCircle.SetOnce(false);
            canDash = false;
            GameLogic.instance.SetTimeScaleLocal(1.0f);
        }

    }
    public override void Change() {
        if (dawn) {
            GetComponent<SpriteRenderer>().sprite = imagenDusk;
            dawn = false;
        }
        else {
            GetComponent<SpriteRenderer>().sprite = imagenDawn;

            dawn = true;
        }
    }

    protected override void LoadResources() {
        imagenDawn = Resources.Load<Sprite>("Sprites/bloom");
        imagenDusk = Resources.Load<Sprite>("Sprites/bloom");
    }
}
