﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : Transformable {

    AudioClip jumpClip;
    AudioClip smashClip;
    AudioClip dashClip;

    public Vector2 originalSizeCollider;
    public Vector2 originalOffsetCollider;


    bool leftPressed;
    public List<GameObject> NearbyObjects;

    bool crawling;
    bool moving;
    bool smashing;

    public float distanciaBordeSprite = 0.745f;

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
    float jumpStrenght = 7;

    //Booleano que gestiona si el personaje esta en el suelo (Para saber si puede saltar o no)
    [SerializeField]
    bool grounded = false;

    bool facingRight;

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
        originalOffsetCollider = GetComponent<BoxCollider2D>().offset;
        originalSizeCollider = GetComponent<BoxCollider2D>().size;

        leftPressed = false;
        prevHorizontalMov = 1;
        facingRight = true;
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

        if (arrow == null) {
            arrow = GameObject.FindGameObjectWithTag("Arrow");
        }

        //Comportamiento sin pausar
        if (!GameLogic.instance.isPaused) {
            CheckGrounded();
            CheckObjectsInFront();
            Move();
            CheckInputs();
            Smashing();
        }



        //Comportamiento de pausado
        else {


        }
    }

    void Smashing() {
        if (smashing) {
            if (grounded) {
                DoSmash();
                smashing = false;
            }
        }
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
        facingRight = !facingRight;
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
            if (Input.GetAxis("Horizontal") != 0) {
                moving = true;
            }else {
                moving = false;
            }
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

    void CheckObjectsInFront() {
        NearbyObjects.Clear();
        if (!moving) {
            RaycastHit2D hit2D;

            if (facingRight)
                hit2D = Physics2D.Raycast(rb.position, Vector2.right, 1.5f, groundMask);
            else {
                hit2D = Physics2D.Raycast(rb.position, Vector2.left, 1.5f, groundMask);
            }

            if (hit2D) {
                if (!NearbyObjects.Contains(hit2D.transform.gameObject))
                    NearbyObjects.Add(hit2D.transform.gameObject);
            }
        }
    }

    //Metodo que añade una fuerza al personaje para simular un salto
    void Jump() {
        GetComponent<Rigidbody2D>().AddForce(transform.up * jumpStrenght, ForceMode2D.Impulse);
        GetComponent<AudioSource>().clip = jumpClip;
        GetComponent<AudioSource>().Play();
    }

    //Método que comprueba si la velocidad y del personaje es 0 o aprox. y actualiza el booleano grounded en consecuencia
    void CheckGrounded() {
        if (rb.velocity.y < 0.1f && rb.velocity.y>=0.0f || rb.velocity.y > -0.1f && rb.velocity.y <=0.0f) {
            RaycastHit2D hit2D = Physics2D.Raycast(rb.position - new Vector2(0f, 0.5f), Vector2.down, 0.2f, groundMask);
            RaycastHit2D hit2DLeft = Physics2D.Raycast(rb.position - new Vector2(0f, 0.5f) + new Vector2(-distanciaBordeSprite, 0), Vector2.down, 0.2f, groundMask);
            RaycastHit2D hit2DRight = Physics2D.Raycast(rb.position - new Vector2(0f, 0.5f) + new Vector2(distanciaBordeSprite, 0), Vector2.down, 0.2f, groundMask);
            // If the raycast hit something
            if (hit2D||hit2DLeft||hit2DRight) {
                grounded = true;
                if (dawn) {
                    SetCanDash(true);
                }
            }
        }
        else {
            grounded = false;
        }
    }

    void DawnBehavior() {
        if (canDash)
            direction = DirectionCircle.UseDirectionCircle(arrow, gameObject);

        //Si se suelta el botón izquierdo del ratón y se puede dashear, se desactiva la slowMotion y se modifica el timeScale además de poner en false canDash
        if (Input.GetMouseButtonUp(0) && canDash) {
            if (leftPressed) {
                GameLogic.instance.SetTimeScaleLocal(1.0f);

                //En caso de ser la dirección hacia la derecha se comprueba si la prevHorizontalMov no era 1, en caso de no serlo, significa que el personaje antes estaba 
                //mirando a la izquierda, por tanto se hace un flip y se cambia prevHorizontalMov al que corresponde.
                if (direction.x > 0) {
                    if (prevHorizontalMov != 1.0f) {
                        Flip();
                    }
                    prevHorizontalMov = 1.0f;

                }

                //En este else se hace lo mismo pero hacia la izquierda
                else {
                    if (prevHorizontalMov != -1.0f) {
                        Flip();
                    }
                    prevHorizontalMov = -1.0f;
                }

                GetComponent<AudioSource>().clip = dashClip;
                GetComponent<AudioSource>().Play();
                Dash.DoDash(gameObject, direction, 10);
                SetCanDash(false);
                leftPressed = false;
            }

            //Si se pulsa el botón izquierdo del ratón y se puede dashear, se activa la slowMotion y se modifica el timeScale de gameLogic
        }
        else if (Input.GetMouseButtonDown(0) && canDash) {
            leftPressed = true;
            GameLogic.instance.SetTimeScaleLocal(0.5f);
        }

        if (Input.GetKeyDown(KeyCode.LeftControl)) {
            crawling = true;
            Vector2 newSize = GetComponent<BoxCollider2D>().size;
            newSize.y /= 2;
            Vector2 newOffset = GetComponent<BoxCollider2D>().offset;
            newOffset.y = newSize.y / 2;

            GetComponent<BoxCollider2D>().size = newSize;
            GetComponent<BoxCollider2D>().offset -= newOffset;


        }
        else if (Input.GetKeyUp(KeyCode.LeftControl)) {
            GetComponent<BoxCollider2D>().size = originalSizeCollider;
            GetComponent<BoxCollider2D>().offset = originalOffsetCollider;

            crawling = false;


        }

    }

        //Método para aglotinar comportamiento de Dusk 
        void DuskBehavior() {

        //Si se suelta el botón izquierdo del ratón y se habia pulsado previamente, el tiempo pasa a cero
        //En caso de estar grounded Se hace una llamada a Punch
        if (Input.GetMouseButtonUp(0)) {
            if (leftPressed) {
                GameLogic.instance.SetTimeScaleLocal(1.0f);
                if (grounded) {
                    Punch(direction, 40000);
                }

                leftPressed = false;
            }
        }

        //En caso de pulsar el botón izquierdo del ratón y estar grounded, se pone el timepo a 0.5
        else if (Input.GetMouseButtonDown(0) && grounded) {
            GameLogic.instance.SetTimeScaleLocal(0.5f);
            leftPressed = true;
        }

        //Si el personaje no esta en el suelo el tiempo pasa a ser 1 en Dusk
        if (!grounded) 
            GameLogic.instance.SetTimeScaleLocal(1.0f);

        //Se renderizan las flechas en caso de clicar solo si esta en el suelo
        else
            direction = DirectionCircle.UseDirectionCircle(arrow, gameObject);

        if (Input.GetKeyDown(KeyCode.LeftControl)&&!grounded) {
            Smash();
        }

    }

    void Smash() {
        if (!smashing) {

            GetComponent<AudioSource>().clip = smashClip;
            GetComponent<AudioSource>().Play();
            rb.AddForce(new Vector2(0, -500));
            smashing = true;
        }
    }

    void DoSmash() {
        RaycastHit2D hit2D = Physics2D.Raycast(rb.position - new Vector2(0f, 0.5f), Vector2.down, 0.2f, groundMask);
        if (hit2D) {
            Debug.Log("HittingStuff");
            if (hit2D.transform.gameObject.GetComponent<DoubleObject>().isBreakable) {
                hit2D.transform.gameObject.GetComponent<DoubleObject>().GetBroken();
            }
        }
    }

    //Método que comprueba los inputs y actua en consecuencia
    void CheckInputs() {
        //BARRA ESPACIADORA = SALTO
        if (Input.GetKeyDown(KeyCode.Space) && grounded&&!crawling) {
            Jump();
        }
        //Comportamiento de dawn
        if (dawn) {
            DawnBehavior();
        }

        //Comportamiento de dusk
        else {
            DuskBehavior();
        }
    }

    public void Punch(Vector2 direction, float MAX_FORCE) {
        foreach (GameObject g in NearbyObjects) {
            if (g.GetComponent<DoubleObject>().isPunchable) {
                g.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezeRotation;
                g.GetComponent<Rigidbody2D>().AddForce(direction * MAX_FORCE, ForceMode2D.Impulse);
                g.GetComponent<DoubleObject>().isPunchable = false;
            }

        }

    }

    //Método de cambio, varia con respecto a los demás ya que además modifica variables especiales
    public override void Change() {
        Vector2 newPosition;

        DirectionCircle.SetOnce(false);
        if (dawn) {
            GetComponent<SpriteRenderer>().sprite = imagenDusk;
            dawn = false;
            SetCanDash(false);
            GameLogic.instance.SetTimeScaleLocal(1.0f);

            newPosition = transform.position;
            newPosition.y -= GameLogic.instance.worldOffset;
            transform.position = newPosition;
            crawling = false;
        }
        else {
            GetComponent<SpriteRenderer>().sprite = imagenDawn;
            dawn = true;
            newPosition = transform.position;
            newPosition.y += GameLogic.instance.worldOffset;
            transform.position = newPosition;
        }
    }

    protected override void LoadResources() {
        imagenDawn = Resources.Load<Sprite>("Sprites/bloom");
        imagenDusk = Resources.Load<Sprite>("Sprites/bloom");
        dashClip = Resources.Load<AudioClip>("Sounds/Dash");
        jumpClip = Resources.Load<AudioClip>("Sounds/Jump");
        smashClip = Resources.Load<AudioClip>("Sounds/Smash");
    }
}
