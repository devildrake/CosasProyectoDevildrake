using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : DoubleObject {

    AudioClip jumpClip;
    AudioClip smashClip;
    AudioClip dashClip;
    AudioClip crawlClip;
    AudioClip walkClip;
    AudioClip deflectClip;
    AudioClip grabClip;
    AudioClip slideClip;
    AudioClip dieClip;
    AudioClip punchClip;

    public Vector2 originalSizeCollider;
    public Vector2 originalOffsetCollider;

    public List<GameObject> objectsInDeflectArea;
    public GameObject deflectArea;

    float slowMotionTimeScale;
    float dashForce;

    float timeToRest;
    float timeToDrag;
    float timeCountToDrag;
    float maxSpeedY;

    bool leftPressed;
    public List<GameObject> NearbyObjects;

    public bool crawling; //iiiiin my craaaaaawl
    bool moving;
    public bool smashing;
    bool dashing;
    bool grabbing;
    public bool onImpulsor;
    public bool canJumpOnImpulsor;
    public bool calledImpuslorBool;
    public bool sliding;
    Vector3 distanceToGrabbedObject;

    public float distanciaBordeSprite = 0.745f;

    //Booleano privado que maneja que el personaje pueda utilizar el dash, se pone en true a la vez que el grounded y en false cuando se usa el Dash
    [SerializeField]
    private bool canDash;

    //Vector dirección para el dash/Hit
    private Vector2 direction;

    //Vector dirección para el deflect
    private Vector2 deflectDirection;

    //Hijo asignado a mano por ahora, anchor de rotación
    public GameObject arrow;

    //Velocidad hacia los laterales base
    float characterSpeed = 5;

    //Fuerza del salto
    float jumpStrenght = 7;

    //Booleano que gestiona si el personaje esta en el suelo (Para saber si puede saltar o no)
    [SerializeField]
    public bool grounded = false;

    public bool facingRight;

    //Mascara de suelo (Mas tarde podria ser util, ahora esta aqui por que para gestionar grounded hice pruebas varias)
    public LayerMask groundMask;
    public LayerMask slideMask;

    //Referencia al RigidBody2D del personaje, se inicializa en el start.
    Rigidbody2D rb;

    //ESTE INT ES -1 SI EL MOVIMIENTO PREVIO FUE HACIA LA IZQUIERDA Y ES 1 SI EL MOVIMIENTO PREVIO FUE HACIA LA DERECHA
    float prevHorizontalMov;

    [SerializeField]
    //Booleano que comprueba si el personaje deberia estar ralentizado en el aire, se reinicia al estar en el suelo (Que se comprueba con colliders2D y tags)
    bool slowedInTheAir;

    //Referencia al script que controla la mascara alfa que se utiliza para el shader de mundos.
    //Queremos acceder a este script para cambiar un flag y activar la transición de un mundo a otro.
    public Change_Scale maskObjectScript; 

    //Se inicializan las cosas
    void Start() {
        maxSpeedY = 8;
        originalOffsetCollider = GetComponent<BoxCollider2D>().offset;
        originalSizeCollider = GetComponent<BoxCollider2D>().size;

        leftPressed = false;
        prevHorizontalMov = 1;
        facingRight = true;
        groundMask = LayerMask.GetMask("Ground");
        slideMask = LayerMask.GetMask("Slide");
        offset = GameLogic.instance.worldOffset;

        rb = GetComponent<Rigidbody2D>();
        slowedInTheAir = false;
        slowMotionTimeScale = 0.3f;
        dashForce = 8;
        //Se hace esto para que UseDirectionCircle haga uso de su bool once y inicialize las cosas que le interesan
        direction = DirectionCircle.UseDirectionCircle(arrow, gameObject,0);


        //Start del transformable
        added = false;
        dawn = false;
        LoadResources();
        if (worldAssignation == world.DAWN)
            GetComponent<SpriteRenderer>().sprite = imagenDawn;

        else
            GetComponent<SpriteRenderer>().sprite = imagenDusk;

        originalPos = GameLogic.instance.spawnPoint;
        deflectArea.SetActive(false);
        timeToDrag = 0.8f;
        timeToRest = 0.2f;
        timeCountToDrag = 0;

    }
    void Update() {

        //Add del transformable
        AddToGameLogicList();

        if (arrow == null) {
            arrow = GameObject.FindGameObjectWithTag("Arrow");
        }

        //Comportamiento sin pausar
        if (!GameLogic.instance.isPaused) {
            if ((worldAssignation == world.DAWN && dawn) ||(worldAssignation == world.DUSK&&!dawn)) {
                CheckGrounded();
                CheckObjectsInFront();
                Move();
                CheckInputs();
                Smashing();
                ClampSpeed();
            }
            BrotherBehavior();
        }



        //Comportamiento de pausado
        else {


        }
    }

    void ClampSpeed() {
        //Debug.Log(GetComponent<Rigidbody2D>().velocity.y);
        if (GetComponent<Rigidbody2D>().velocity.y > maxSpeedY) {
            GetComponent<Rigidbody2D>().velocity = new Vector2(GetComponent<Rigidbody2D>().velocity.x,maxSpeedY);
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

    protected override void BrotherBehavior()
    {
        Vector3 positionWithOffset;
        if (GetComponent<Rigidbody2D>().bodyType == RigidbodyType2D.Kinematic)
        {
            positionWithOffset = brotherObject.transform.position;

            if (worldAssignation == world.DAWN)
                positionWithOffset.y += offset;
            else
            {
                positionWithOffset.y -= offset;
            }

            transform.position = positionWithOffset;
            transform.rotation = brotherObject.transform.rotation;

        }

    }

    //Método que cambia hacia donde mira el personaje, se le llama al cambiar el Input.GetAxis Horizontal de 1 a -1 o alreves
    void Flip() {
        //Debug.Log("Flip");
        if (!grabbing) {
            Vector3 theScale = transform.localScale;
            theScale.x *= -1;
            transform.localScale = theScale;
            facingRight = !facingRight;

            if (brotherObject.GetComponent<PlayerController>().facingRight != facingRight)
            {
                brotherObject.GetComponent<PlayerController>().Flip();
                brotherObject.GetComponent<PlayerController>().prevHorizontalMov = prevHorizontalMov;
            }
        }

    }

    //Funcion que hace al personaje moverse hacia los lados a partir del input de las flechas, en caso de estar en el aire se mantiene siempre y cuando se cambie la dirección horizontal en el aire se reduce a la mitad la velocidad
    void Move() {
        if (!sliding) {
            if (!grabbing) {
                timeCountToDrag = 0;
                bool changed = false;
                float mustSlow = 1;
                if (Input.GetAxisRaw("Horizontal") != (float)prevHorizontalMov && Input.GetAxisRaw("Horizontal") != 0.0f) {
                    changed = true;
                    prevHorizontalMov = Input.GetAxisRaw("Horizontal");
                }

                if (grounded) {
                    GetComponent<Rigidbody2D>().AddForce(new Vector2(-GetComponent<Rigidbody2D>().velocity.x, 0), ForceMode2D.Impulse);
                    transform.Translate(Vector3.right * Input.GetAxis("Horizontal") * characterSpeed * Time.deltaTime);
                    slowedInTheAir = false;
                    if (Input.GetAxis("Horizontal") != 0) {
                        moving = true;
                        if (!GetComponent<AudioSource>().isPlaying&&!crawling) {
                            GetComponent<AudioSource>().pitch = 0.3f;
                            GetComponent<AudioSource>().clip = walkClip;
                            GetComponent<AudioSource>().Play();
                        }
                        else if(crawling){
                            GetComponent<AudioSource>().pitch = 0.3f;
                            GetComponent<AudioSource>().clip = crawlClip;
                            GetComponent<AudioSource>().Play();
                        }
                    }
                    else {
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
                    transform.Translate(Vector3.right * Input.GetAxis("Horizontal") * characterSpeed * mustSlow * Time.deltaTime);
                }

                if (changed) {
                    Flip();
                }

            }
            else {
                if (grounded) {
                    if (NearbyObjects[0].GetComponent<DoubleObject>().isMovable) {
                        timeCountToDrag += Time.deltaTime;

                        if (timeCountToDrag < timeToRest) {

                        }
                        else if (timeCountToDrag < timeToRest + timeToDrag) {
                            transform.Translate(Vector3.right * Input.GetAxis("Horizontal") * characterSpeed / 2 * Time.deltaTime);
                            NearbyObjects[0].transform.position = transform.position - distanceToGrabbedObject;
                            Debug.Log(distanceToGrabbedObject);
                            if (!GetComponent<AudioSource>().isPlaying) {
                                GetComponent<AudioSource>().clip = grabClip;
                                GetComponent<AudioSource>().Play();
                            }
                        }else {
                            timeCountToDrag = 0;
                        }
                    }
                    else {
                        grabbing = false;
                    }
                }
                else {

                }

            }
        }else {
            if (rb.velocity.y > 0) {
                sliding = false;
            }
            if (!GetComponent<AudioSource>().isPlaying) {
                GetComponent<AudioSource>().clip = slideClip;
                GetComponent<AudioSource>().Play();
            }

        }
    }

    void CheckObjectsInFront() {
        bool temp = false;
        NearbyObjects.Clear();
        if (!moving) {
            RaycastHit2D hit2D;

            if (facingRight)
                hit2D = Physics2D.Raycast(rb.position, Vector2.right, 1.5f, groundMask);
            else {
                hit2D = Physics2D.Raycast(rb.position, Vector2.left, 1.5f, groundMask);
            }

            if (hit2D) {
                if (!NearbyObjects.Contains(hit2D.transform.gameObject)) {
                    NearbyObjects.Add(hit2D.transform.gameObject);
                    temp = true;
                }
            }
        }
        if (!temp) {
            grabbing = false;
        }
    }

    //Metodo que añade una fuerza al personaje para simular un salto
    void Jump() {
        GetComponent<Rigidbody2D>().velocity= new Vector2(0,0);
        GetComponent<AudioSource>().pitch = 1.0f;

        GetComponent<Rigidbody2D>().AddForce(transform.up * jumpStrenght, ForceMode2D.Impulse);
        GetComponent<AudioSource>().clip = jumpClip;
        GetComponent<AudioSource>().Play();
        sliding = false;
    }

    //Método que comprueba si la velocidad y del personaje es 0 o aprox. y actualiza el booleano grounded en consecuencia
    void CheckGrounded() {

            RaycastHit2D hit2D = Physics2D.Raycast(rb.position - new Vector2(0f, 0.5f), Vector2.down, 0.1f, groundMask);
            RaycastHit2D hit2DLeft = Physics2D.Raycast(rb.position - new Vector2(0f, 0.5f) + new Vector2(-distanciaBordeSprite, 0), Vector2.down, 0.1f, groundMask);
            RaycastHit2D hit2DRight = Physics2D.Raycast(rb.position - new Vector2(0f, 0.5f) + new Vector2(distanciaBordeSprite, 0), Vector2.down, 0.1f, groundMask);
            // If the raycast hit something
            if (hit2D || hit2DLeft || hit2DRight) {
                grounded = true;
                dashing = false;
                SetCanDash(true);
            }
        
        else {
            grounded = false;
        }
        RaycastHit2D hit2DLeftO = Physics2D.Raycast(rb.position - new Vector2(0f, 0.5f) + new Vector2(-distanciaBordeSprite, 0), Vector2.down, 0.1f, groundMask);
        RaycastHit2D hit2DRightO = Physics2D.Raycast(rb.position - new Vector2(0f, 0.5f) + new Vector2(distanciaBordeSprite, 0), Vector2.down, 0.1f, groundMask);

        if (!hit2DLeftO && !hit2DRightO) {
            bool right = true;

            //SLIDE PA LA DERECHA
            hit2DRightO = Physics2D.Raycast(rb.position - new Vector2(0f, 0.5f) + new Vector2(distanciaBordeSprite, 0), Vector2.down, 3.0f, slideMask);
            hit2DLeftO = Physics2D.Raycast(rb.position - new Vector2(0f, 0.5f) + new Vector2(-distanciaBordeSprite, 0), Vector2.down, 0.2f, slideMask);


            if (!(hit2DLeftO && hit2DRightO)){
                right = false;
                //SLIDE PA LA IZQUIERDA
                hit2DRightO = Physics2D.Raycast(rb.position - new Vector2(0f, 0.5f) + new Vector2(distanciaBordeSprite, 0), Vector2.down, 0.2f, slideMask);
                hit2DLeftO = Physics2D.Raycast(rb.position - new Vector2(0f, 0.5f) + new Vector2(-distanciaBordeSprite, 0), Vector2.down, 3.0f, slideMask);
            }
            if (hit2DLeftO && hit2DRightO) {
                if (!facingRight&& right) {
                    Flip();
                    prevHorizontalMov = 1.0f;

                }
                else if (facingRight && !right) {
                    Flip();
                    prevHorizontalMov = -1.0f;

                }
                sliding = true;
                rb.velocity = new Vector2(rb.velocity.x, -10);

            }
        }
        else {
            sliding = false;
        }
    }

    void DawnBehavior() {
        if (canDash)
            direction = PlayerUtilsStatic.UseDirectionCircle(arrow, gameObject,0);

        if (objectsInDeflectArea.Count != 0) {
            deflectDirection = PlayerUtilsStatic.UseDirectionCircle(arrow, gameObject, 1,-10,60);
            deflectArea.SetActive(true);
        }
        else {
            deflectArea.SetActive(false);
        }

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
                GetComponent<AudioSource>().pitch = 1.0f;

                GetComponent<AudioSource>().clip = dashClip;
                GetComponent<AudioSource>().Play();
                canJumpOnImpulsor = false;
                Dash.DoDash(gameObject, direction, dashForce);
                dashing = true;
                SetCanDash(false);
                leftPressed = false;
            }

            //Si se pulsa el botón izquierdo del ratón y se puede dashear, se activa la slowMotion y se modifica el timeScale de gameLogic
        }
        else if (Input.GetMouseButtonDown(0) && canDash) {
            leftPressed = true;
            GameLogic.instance.SetTimeScaleLocal(slowMotionTimeScale);
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
        if (Input.GetMouseButtonDown(1)&&objectsInDeflectArea.Count!=0) {
            GameLogic.instance.SetTimeScaleLocal(slowMotionTimeScale);
        }else if (Input.GetMouseButtonUp(1)&& objectsInDeflectArea.Count!=0) {
            foreach(GameObject g in objectsInDeflectArea) {
                Dash.DoDash(g, deflectDirection, 20);
                GetComponent<AudioSource>().clip = deflectClip;
                GetComponent<AudioSource>().Play();
                g.GetComponent<Rigidbody2D>().gravityScale = 0;
            }
            GameLogic.instance.SetTimeScaleLocal(1f);
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
                    GetComponent<AudioSource>().clip = punchClip;
                    GetComponent<AudioSource>().Play();
                }

                leftPressed = false;
            }
        }

        //En caso de pulsar el botón izquierdo del ratón y estar grounded, se pone el timepo a 0.5
        else if (Input.GetMouseButtonDown(0) && grounded) {
            GameLogic.instance.SetTimeScaleLocal(slowMotionTimeScale);
            leftPressed = true;
        }

        //Si el personaje no esta en el suelo el tiempo pasa a ser 1 en Dusk
        if (!grounded) {
            GameLogic.instance.SetTimeScaleLocal(1.0f);
        }
        //Se renderizan las flechas en caso de clicar solo si esta en el suelo
        else {
            direction = PlayerUtilsStatic.UseDirectionCircle(arrow, gameObject,0,0,60);
            if (Input.GetMouseButtonDown(1)) {
                foreach (GameObject g in NearbyObjects) {
                    if (g.GetComponent<DoubleObject>().isMovable) {
                        distanceToGrabbedObject = transform.position - NearbyObjects[0].transform.position;
                        grabbing = true;

                    }
                }
            }else if(Input.GetMouseButtonUp(1)){
                grabbing = false;
            }
        }
        if (Input.GetKeyDown(KeyCode.LeftControl)&&!grounded) {
            Smash();
            GetComponent<AudioSource>().pitch = 1.0f;

            GetComponent<AudioSource>().clip = GetComponent<PlayerController>().smashClip;
            GetComponent<AudioSource>().Play();
        }

    }

    void Smash() {
        if (!smashing) {
            rb.AddForce(new Vector2(-rb.velocity.x,0),ForceMode2D.Impulse);
            GetComponent<AudioSource>().clip = smashClip;
            GetComponent<AudioSource>().pitch = 1.0f;

            GetComponent<AudioSource>().Play();
            rb.velocity = new Vector2(0, 0);
            rb.AddForce(new Vector2(0, -700));
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

    void ImpulsorBool()
    {
        calledImpuslorBool = false;
        canJumpOnImpulsor = true;
    }
    //Método que comprueba los inputs y actua en consecuencia
    void CheckInputs() {
        bool someRayCastChecks = false;

 
            RaycastHit2D hit2D = Physics2D.Raycast(rb.position - new Vector2(0f, 0.5f), Vector2.down, 0.1f, groundMask);
            RaycastHit2D hit2DLeft = Physics2D.Raycast(rb.position - new Vector2(0f, 0.5f) + new Vector2(-distanciaBordeSprite, 0), Vector2.down, 0.1f, groundMask);
            RaycastHit2D hit2DRight = Physics2D.Raycast(rb.position - new Vector2(0f, 0.5f) + new Vector2(distanciaBordeSprite, 0), Vector2.down, 0.1f, groundMask);

            if (hit2D || hit2DLeft || hit2DRight)
            {
                someRayCastChecks = true;
            }
            if (!calledImpuslorBool)
            {
                Invoke("ImpulsorBool", 0.4f);
                calledImpuslorBool = true;
            }
            

        //BARRA ESPACIADORA = SALTO
        if (Input.GetKeyDown(KeyCode.Space) && grounded&&!crawling||Input.GetKeyDown(KeyCode.Space) && sliding) {
            Jump();
        }

        if (onImpulsor) {
            //GetComponent<Rigidbody2D>().gravityScale = 0;

            if (someRayCastChecks && Input.GetKeyDown(KeyCode.Space)) {
                if (!calledImpuslorBool) {
                    Invoke("ImpulsorBool", 0.2f);
                    calledImpuslorBool = true;
                }
                canJumpOnImpulsor = false;
                Debug.Log("JUMP");
                Jump();
            }
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

        //Vector2 newPosition;

        DirectionCircle.SetOnce(false);


        //Si antes del cambio estaba en dusk, pasara a hacerse dynamic y al otro kinematic, además de darle su velocidad 


        if (dawn) {
            //GetComponent<SpriteRenderer>().sprite = imagenDusk;
            dawn = false;
            GameLogic.instance.SetTimeScaleLocal(1.0f);

            //newPosition = transform.position;
            //newPosition.y -= GameLogic.instance.worldOffset;
            //transform.position = newPosition;
            crawling = false;

            //activar el shader
            if (worldAssignation == world.DAWN){
                dominantVelocity = GetComponent<Rigidbody2D>().velocity;
                brotherObject.GetComponent<DoubleObject>().dominantVelocity = GetComponent<Rigidbody2D>().velocity;
                brotherObject.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Dynamic;
                GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Kinematic;
                OnlyFreezeRotation();
                brotherObject.GetComponent<Rigidbody2D>().velocity = dominantVelocity;
                GetComponent<Rigidbody2D>().velocity = new Vector2(0.0f, 0.0f);
            }
            maskObjectScript.change = false;
        }
        else {
            //GetComponent<SpriteRenderer>().sprite = imagenDawn;
            dawn = true;
            //newPosition = transform.position;
            //newPosition.y += GameLogic.instance.worldOffset;
            //transform.position = newPosition;
            if (worldAssignation == world.DAWN){
                dominantVelocity = brotherObject.GetComponent<Rigidbody2D>().velocity;
                brotherObject.GetComponent<DoubleObject>().dominantVelocity = brotherObject.GetComponent<Rigidbody2D>().velocity;
                GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Dynamic;
                brotherObject.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Kinematic;
                brotherObject.GetComponent<Rigidbody2D>().velocity = new Vector2(0.0f, 0.0f);
                GetComponent<Rigidbody2D>().velocity = dominantVelocity;
            }

            //activar el shader
            maskObjectScript.change = true;
        }
    }

    protected override void LoadResources() {
        imagenDawn = Resources.Load<Sprite>("Presentacion/DawnSprites/Dawn");
        imagenDusk = Resources.Load<Sprite>("Presentacion/DuskSprites/Dusk"); ;
        dashClip = Resources.Load<AudioClip>("Sounds/Dash");
        jumpClip = Resources.Load<AudioClip>("Sounds/Jump");
        smashClip = Resources.Load<AudioClip>("Sounds/Smash");
        crawlClip = Resources.Load<AudioClip>("Sounds/CrawlWalk");
        walkClip = Resources.Load<AudioClip>("Sounds/WalkLoop");
        deflectClip = Resources.Load<AudioClip>("Sounds/Deflect");
        grabClip = Resources.Load<AudioClip>("Sounds/Grab");
        slideClip = Resources.Load<AudioClip>("Sounds/Slide");
        dieClip = Resources.Load<AudioClip>("Sounds/Die");
        punchClip = Resources.Load<AudioClip>("Sounds/Punch");
    }
    public override void Kill() {
        
        transform.position = GameLogic.instance.spawnPoint;
        if (GetComponent<Rigidbody2D>() != null) {
            GetComponent<Rigidbody2D>().velocity = new Vector2(0, 0);
        }
        GetComponent<AudioSource>().clip = dieClip;
        GetComponent<AudioSource>().Play();
    }

}
