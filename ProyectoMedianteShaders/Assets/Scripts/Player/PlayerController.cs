using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class PlayerController : DoubleObject {

    //Clips de audio diferentes
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

    public bool hasACrystal;
    public bool savedFragment;

    //Lista de objetos en el area de Deflect
    public List<GameObject> objectsInDeflectArea;

    //COOLDOWNS
    private float punchCoolDown;
    private float punchTimer;
    private float dashCoolDown;

    //Float que indica si el personaje ha de ir hacia la derecha o hacia la izquierda en su transición inicial, de todas formas
    //Se ha de setear a 0 para que haga la comprovación en un principio.
    public float whichX;
    //Objeto con el transform de la posición a la que queremos que llegue el pj
    public GameObject placeToGo;

    private float dashTimer;

    private float deflectCoolDown;
    private float deflectTimer;


    float timeNotMoving;

    float slowMotionTimeScale;
    float dashForce;

    float timeToRest;
    float timeToDrag;
    float timeCountToDrag;
    float maxSpeedY;
    float timeMoving;

    bool leftPressed;
    public List<GameObject> NearbyObjects;

    public bool crawling; 
    bool moving;
    public bool smashing;
    bool grabbing;
    GameObject capaObject;

    //Bool para determinar si esta detrás de un bush
    public bool behindBush;

    //Bool para determinar si esta dentro del area de un impulsor
    public bool onImpulsor;
    public bool canJumpOnImpulsor;
    public bool calledImpuslorBool;
    public bool sliding;
    public DoubleObject interactableObject;
    Vector3 distanceToGrabbedObject;

    public float distanciaBordeSprite = 0.745f;

    //Booleano privado que maneja que el personaje pueda utilizar el dash, se pone en true a la vez que el grounded y en false cuando se usa el Dash
    [SerializeField]
    public bool canDash;

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
    public LayerMask[] grabbableMask;

    //Referencia al RigidBody2D del personaje, se inicializa en el start.
    public Rigidbody2D rb;

    //ESTE INT ES -1 SI EL MOVIMIENTO PREVIO FUE HACIA LA IZQUIERDA Y ES 1 SI EL MOVIMIENTO PREVIO FUE HACIA LA DERECHA
    public float prevHorizontalMov;

    [SerializeField]
    //Booleano que comprueba si el personaje deberia estar ralentizado en el aire, se reinicia al estar en el suelo (Que se comprueba con colliders2D y tags)
    bool slowedInTheAir;

    //Referencia al script que controla la mascara alfa que se utiliza para el shader de mundos.
    //Queremos acceder a este script para cambiar un flag y activar la transición de un mundo a otro.
    public Change_Scale maskObjectScript;

    //Referencia al sistema de particulas de dawn para el deflect.
    public ParticleSystem PSdawnDeflectCast, PSdawnDeflectRelease, PSdawnDash1, PSdawnDash2, PSdawnDeflectFeedback;
    private ParticleSystem.EmissionModule emissionModuleDash1, emissionModuleDash2;

    AudioSource audioSource;
    GroundCheck groundCheck;
    PlayerController brotherScript;
    Animator myAnimator;
    Animator brotherAnimator;
    BoxCollider2D myBoxCollider;

    //Se inicializan las cosas
    void Start() {
        
        //El sistema de particulas de Dawn del deflect se inicia desactivado
        if (PSdawnDeflectCast != null) {
            PSdawnDeflectCast.Stop();
        }
        if(PSdawnDeflectRelease != null) {
            PSdawnDeflectRelease.Stop();
        }
        if (PSdawnDash1 != null) {
            emissionModuleDash1 = PSdawnDash1.emission;
            emissionModuleDash1.enabled = false;
        }
        if(PSdawnDash2 != null) {
            emissionModuleDash2 = PSdawnDash2.emission;
            emissionModuleDash2.enabled = false;
        }
        if(PSdawnDeflectFeedback != null) {
            PSdawnDeflectFeedback.Stop();
        }
        audioSource = GetComponent<AudioSource>();
        groundCheck = GetComponentInChildren<GroundCheck>();
        brotherScript = brotherObject.GetComponent<PlayerController>();
        myBoxCollider = GetComponent<BoxCollider2D>(); 
        myAnimator = GetComponentInChildren<Animator>();
        brotherAnimator = brotherObject.GetComponentInChildren<Animator>();

        timeNotMoving = 0;
        dashCoolDown = 0.5f;
        punchCoolDown = 0.5f;
        deflectCoolDown = 0.5f;

        grabbableMask = new LayerMask[3];

        grabbableMask[0] = LayerMask.GetMask("Ground");
        grabbableMask[2] = LayerMask.GetMask("Enemy");
        grabbableMask[1] = LayerMask.GetMask("Platform");


        maxSpeedY = 20;
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
        dashForce = 7;
        //Se hace esto para que UseDirectionCircle haga uso de su bool once y inicialize las cosas que le interesan
        direction = DirectionCircle.UseDirectionCircle(arrow, gameObject,0);


        //Start del transformable
        added = false;
        dawn = false;
        LoadResources();
        if (worldAssignation == world.DAWN)
            GetComponent<SpriteRenderer>().sprite = imagenDawn;

        else
            //GetComponent<SpriteRenderer>().sprite = imagenDusk;

        originalPos = GameLogic.instance.spawnPoint;
        timeToDrag = 0.8f;
        timeToRest = 0.2f;
        timeCountToDrag = 0;

    }

    protected override void AddToGameLogicList() {
        if (!added) {
            if (GameLogic.instance != null) {
                added = true;
                GameLogic.instance.transformableObjects.Add(gameObject);
                if (worldAssignation == world.DUSK)
                    GameLogic.instance.currentPlayer = this;

                if(GetComponentInChildren<Cloth>()!=null)
                capaObject = GetComponentInChildren<Cloth>().gameObject;

                //Debug.Log(capaObject);

            }
        }

    }

    bool isNotAlive(GameObject g) {
        return g == null;
    }

    void CoolDowns() {
        if (punchCoolDown > punchTimer) {
            punchTimer += Time.deltaTime;
        }

        if (dashCoolDown > dashTimer) {
            dashTimer += Time.deltaTime;
        }

        if (deflectCoolDown > deflectTimer) {
            deflectTimer += Time.deltaTime;
        }

        objectsInDeflectArea.RemoveAll(isNotAlive);


    }

    void MoveEvents() {
        if (InputManager.instance != null) {
            if (InputManager.GetBlocked() && !GameLogic.instance.cameraTransition) {
                if (placeToGo != null) {
                    if (placeToGo.transform.position.x != transform.position.x) {
                        if (whichX == 0) {
                            if (placeToGo.transform.position.x - transform.position.x > 0) {
                                whichX = 1.0f;
                            } else {
                                whichX = -1.0f;
                            }
                        } else {
                            if (whichX > 0) {
                                if (transform.position.x > placeToGo.transform.position.x) {
                                    transform.position = new Vector3(placeToGo.transform.position.x, transform.position.y, transform.position.z);

                                } else {
                                    transform.Translate(Vector3.right * 2.0f * Time.deltaTime);
                                    myAnimator.SetBool("grounded", true);
                                    myAnimator.SetBool("moving", true);
                                    myAnimator.SetFloat("timeMoving", 1.0f);

                                }

                            } else {
                                if (transform.position.x < placeToGo.transform.position.x) {
                                    transform.position = new Vector3(placeToGo.transform.position.x, transform.position.y, transform.position.z);
                                } else {
                                    transform.Translate(Vector3.left * 2.0f * Time.deltaTime);
                                    myAnimator.SetBool("grounded", true);
                                    myAnimator.SetBool("moving", true);
                                    myAnimator.SetFloat("timeMoving", 1.0f);
                                }
                            }
                        }
                    } else {
                        InputManager.UnBlockInput();
                        if(placeToGo!=null)
                        Destroy(placeToGo.gameObject);
                    }
                } else {
                    InputManager.UnBlockInput();
                    Debug.Log("UNBLOCKBRUH");
                }
            }
        }
    }

    void Update() {

        if (rb.velocity.y < 0) {
            rb.gravityScale = 1.5f;
        } else {
            rb.gravityScale = 1;
        }

        AddToGameLogicList();

        if (added) {

            MoveEvents();


            if (!GameLogic.instance.levelFinished) {
                //Debug.Log(grounded);
                //Add del transformable

                if (arrow == null) {
                    arrow = GameObject.FindGameObjectWithTag("Arrow");
                }

                //Comportamiento sin pausar
                if (!GameLogic.instance.isPaused) {
                    if ((worldAssignation == world.DAWN && dawn) || (worldAssignation == world.DUSK && !dawn)) {

                        if (grounded && worldAssignation == world.DUSK) {
                            brotherScript.SetCanDash(true);
                        }

                        //CheckGrounded();
                        groundCheck.CheckGrounded(this);
                        CheckObjectsInFront();
                        if (!GameLogic.instance.cameraTransition) {
                            CheckInputs();
                            Move();
                        }
                        Smashing();
                        ClampSpeed();
                    }
                    //BrotherBehavior();
                    CoolDowns();
                }

                //Comportamiento de pausado
                else {
                }

                SetAnimValues();

            } else if(!GameLogic.instance.saved){
                if (hasACrystal||brotherScript.hasACrystal) {
                    if (!savedFragment) {
                        Debug.Log("SavingFragment?");
                        //crystalFragment.picked = true;
                        GameLogic.instance.SaveFragment(true);
                        savedFragment = true;
                        brotherScript.savedFragment = true;

                        for (int i = 1; i < 30; i++) {
                            if(GameLogic.instance.levelsData[i].fragment)
                            Debug.Log(i + " --- " + GameLogic.instance.levelsData[i].fragment);
                        }

                    }
                }
                GameLogic.instance.Save();
            }
        }
        BrotherBehavior();



    }

    void SetAnimValues() {
        if (myAnimator != null&&!InputManager.GetBlocked()) {

            if (worldAssignation == world.DUSK&&!dawn) {
                myAnimator.SetBool("moving", moving);
                myAnimator.SetBool("grounded", grounded);
                myAnimator.SetFloat("speedY", rb.velocity.y);
                myAnimator.SetBool("sliding", sliding);
                myAnimator.SetFloat("timeMoving", timeMoving);

                brotherAnimator.SetBool("moving", moving);
                brotherAnimator.SetBool("grounded", grounded);
                brotherAnimator.SetFloat("speedY", rb.velocity.y);
                brotherAnimator.SetBool("sliding", sliding);
                brotherAnimator.SetFloat("timeMoving", timeMoving);

                myAnimator.SetBool("punching", punchTimer < punchCoolDown);
                brotherAnimator.SetBool("dashing", dashTimer < dashCoolDown);
                brotherAnimator.SetBool("crawling", crawling);
                brotherAnimator.SetBool("deflecting", deflectTimer < deflectCoolDown);

            } else if(dawn&&worldAssignation==world.DAWN){

                myAnimator.SetBool("moving", moving);
                myAnimator.SetBool("grounded", grounded);
                myAnimator.SetFloat("speedY", rb.velocity.y);
                myAnimator.SetBool("sliding", sliding);
                myAnimator.SetFloat("timeMoving", timeMoving);

                brotherAnimator.SetBool("moving", moving);
                brotherAnimator.SetBool("grounded", grounded);
                brotherAnimator.SetFloat("speedY", rb.velocity.y);
                brotherAnimator.SetBool("sliding", sliding);
                brotherAnimator.SetFloat("timeMoving", timeMoving);

                myAnimator.SetBool("dashing", dashTimer < dashCoolDown);
                myAnimator.SetBool("crawling", crawling);
                myAnimator.SetBool("dash_press", InputManager.instance.dashButton&&dashTimer>=dashCoolDown&&canDash&&!grounded);
                myAnimator.SetBool("deflecting", deflectTimer < deflectCoolDown);
                myAnimator.SetBool("deflect_press", InputManager.instance.deflectButton && deflectTimer >= deflectCoolDown&&objectsInDeflectArea.Count>0);
                brotherAnimator.SetBool("punching", punchTimer < punchCoolDown);

            }

        } else {
            Debug.Log("NullAnimator or Block in progress");
        }
    }

    //Clampeo de la velocidad del personaje
    void ClampSpeed() {
        if (rb != null) {
            if (rb.velocity.y > maxSpeedY) {
                rb.velocity = new Vector2(rb.velocity.x, maxSpeedY);
            }
        } else {
            rb = GetComponent<Rigidbody2D>();
        }
    }

    //Método que comprueba si el personaje está cargando contra el suelo, en caso afirmativo, hace el Smash
    void Smashing() {
        if (smashing) {
            if (grounded) {
                DoSmash();
                smashing = false;
            }
        }
    }

    //Método setter para canDash
    public void SetCanDash(bool a) {
        canDash = a;
        brotherScript.canDash = true;
    }

    //Método virtual que modifica la posicio del objeto dominado con la del dominante
    protected override void BrotherBehavior()
    {
        Vector3 positionWithOffset;
        if (rb.bodyType == RigidbodyType2D.Kinematic)
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

        } else {


            InputManager.instance.UpdatePreviousPlayer();
        }

    }

    //Método que cambia hacia donde mira el personaje, se le llama al cambiar el Input.GetAxis Horizontal de 1 a -1 o alreves
    public void Flip() {
        if (!grabbing) {
            Vector3 theScale = transform.localScale;
            //theScale.x *= -1;
            //transform.localScale = theScale;

            transform.Rotate(new Vector3(0, 1, 0), 180);


            facingRight = !facingRight;

            if (brotherScript.facingRight != facingRight)
            {
                brotherScript.Flip();
                brotherScript.prevHorizontalMov = prevHorizontalMov;
            }
        }

        if (capaObject != null) {
            capaObject.SetActive(false);
            capaObject.SetActive(true);
        }

    }

    //Funcion que hace al personaje moverse hacia los lados a partir del input de las flechas, en caso de estar en el aire se mantiene siempre y cuando se cambie la dirección horizontal en el aire se reduce a la mitad la velocidad
    void Move() {
        if (!sliding) {
            if (!grabbing) {
                timeCountToDrag = 0;
                bool changed = false;
                float mustSlow = 1;
                if (Input.GetAxisRaw("Horizontal") != (float)prevHorizontalMov && Input.GetAxisRaw("Horizontal") != 0.0f&&!InputManager.GetBlocked()) {
                    changed = true;
                    //Debug.Log("CHANGE");
                    prevHorizontalMov = Input.GetAxisRaw("Horizontal");
                }


                if (grounded) {
                    if (dawn&&worldAssignation==world.DAWN&& InputManager.instance.dashButton) {
                        PlayerUtilsStatic.ResetDirectionCircle(arrow);
                    }

                    rb.velocity = new Vector2(0, rb.velocity.y);
                    //Debug.Log("Se para");
                    if (facingRight)
                        transform.Translate(Vector3.right * InputManager.instance.horizontalAxis * characterSpeed * Time.deltaTime);
                    else
                        transform.Translate(Vector3.left * InputManager.instance.horizontalAxis * characterSpeed * Time.deltaTime);

                    slowedInTheAir = false;
                    if (InputManager.instance.horizontalAxis != 0) {
                        timeMoving += Time.deltaTime;
                        timeNotMoving = 0;
                        moving = true;
                        if (!audioSource.isPlaying&&!crawling) {
                            audioSource.pitch = 0.3f;
                            audioSource.clip = walkClip;
                            audioSource.Play();
                        }
                        else if(crawling){
                            audioSource.pitch = 0.3f;
                            audioSource.clip = crawlClip;
                            audioSource.Play();
                        }
                    }
                    else {
                        timeMoving = 0;
                        if (timeNotMoving > 0.1f) {
                            moving = false;
                        } else {
                            timeNotMoving += Time.deltaTime;
                        }


                    }


                }
                //Not Grounded and changed
                else if (changed) {
                    slowedInTheAir = true;
                    mustSlow = 0.5f;

                    if(facingRight)
                    transform.Translate(Vector3.right * InputManager.instance.horizontalAxis * characterSpeed*0.75f * mustSlow * Time.deltaTime);
                    else
                    transform.Translate(Vector3.left * InputManager.instance.horizontalAxis * characterSpeed *0.75f*mustSlow* Time.deltaTime);


                    //Velocidad X a 0
                    rb.AddForce(new Vector2(-rb.velocity.x, 0), ForceMode2D.Impulse);

                }
                //Not Grounded and not changed
                else {
                    if (slowedInTheAir) {
                        //rb.AddForce(new Vector2(-rb.velocity.x, 0), ForceMode2D.Impulse);
                        mustSlow = 0.5f;
                    }

                    if(facingRight)
                    transform.Translate(Vector3.right * InputManager.instance.horizontalAxis * characterSpeed*0.75f * mustSlow * Time.deltaTime);

                    else
                        transform.Translate(Vector3.left * InputManager.instance.horizontalAxis * characterSpeed * 0.75f * mustSlow * Time.deltaTime);

                }

                if (changed) {
                    Flip();
                }

            }
            //Grabbing es true
            else {
                if (grounded) {
                    if (NearbyObjects[0].GetComponent<DoubleObject>().isMovable) {
                        timeCountToDrag += Time.deltaTime;

                        if (timeCountToDrag < timeToRest) {

                        }
                        else if (timeCountToDrag < timeToRest + timeToDrag) {

                            if (facingRight)
                                transform.Translate(Vector3.right * InputManager.instance.horizontalAxis * characterSpeed / 2 * Time.deltaTime);

                            else
                                transform.Translate(Vector3.left * InputManager.instance.horizontalAxis * characterSpeed / 2 * Time.deltaTime);


                            NearbyObjects[0].transform.position = transform.position - distanceToGrabbedObject;
                            Debug.Log(distanceToGrabbedObject);
                            if (!audioSource.isPlaying) {
                                audioSource.clip = grabClip;
                                audioSource.Play();
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
                    grabbing = false;
                }

            }
        }else {//Sliding es true
            if (rb.velocity.y > 0) {
                sliding = false;
            }
            if (!audioSource.isPlaying) {
                audioSource.clip = slideClip;
                audioSource.Play();
            }

        }

    }

    //Método que comprueba si hay algun objeto en la dirección en la que mira el personaje antes lo hacía solo quieto, pero 
    //Lo cambié para que lo pudiera hace ren movimiento.
    void CheckObjectsInFront() {
        bool temp = false;
        NearbyObjects.Clear();
       // if (!moving) {
            RaycastHit2D hit2D;

        if (facingRight) {
            //ESTO FUNCIONA hit2D = Physics2D.Raycast(rb.position, Vector2.right, 1.5f, LayerMask.GetMask("Platform"));
            hit2D = Physics2D.Raycast(rb.position, Vector2.right, 1.5f, LayerMask.GetMask("Platform"));
            if (!hit2D) {
                hit2D = Physics2D.Raycast(rb.position, Vector2.right, 1.5f, LayerMask.GetMask("Enemy"));
            }
            if (!hit2D) {
                hit2D = Physics2D.Raycast(rb.position, Vector2.right, 1.5f, LayerMask.GetMask("Ground"));
            }

            //hit2D = Physics2D.Raycast(rb.position, Vector2.right, 1.5f, groundMask);
            //hit2D = PlayerUtilsStatic.RayCastArrayMask(rb.position, Vector2.right, 1.5f, grabbableMask);
            Debug.DrawRay(rb.position, Vector2.right * 1.5f);
        } else {
            //hit2D = Physics2D.Raycast(rb.position, Vector2.left, 1.5f, groundMask);
            //hit2D = PlayerUtilsStatic.RayCastArrayMask(rb.position, Vector2.left, 1.5f, grabbableMask);
            hit2D = Physics2D.Raycast(rb.position, Vector2.left, 1.5f, LayerMask.GetMask("Platform"));
            if (!hit2D) {
                hit2D = Physics2D.Raycast(rb.position, Vector2.left, 1.5f, LayerMask.GetMask("Enemy"));
            }
            if (!hit2D) {
                hit2D = Physics2D.Raycast(rb.position, Vector2.left, 1.5f, LayerMask.GetMask("Ground"));
            }
        }

        if (hit2D) {
                if (!NearbyObjects.Contains(hit2D.transform.gameObject)) {
                    NearbyObjects.Add(hit2D.transform.gameObject);
                    temp = true;
                }
            }
       // }
        if (!temp) {
            grabbing = false;
        }
    }

    //Metodo que añade una fuerza al personaje para simular un salto
    void Jump() {
        rb.velocity= new Vector2(0,0);
        audioSource.pitch = 1.0f;

        rb.AddForce(transform.up * jumpStrenght, ForceMode2D.Impulse);
        audioSource.clip = jumpClip;
        audioSource.Play();
        sliding = false;
    }

    void DawnBehavior() {

        if (!grounded) {
            if (canDash && dashTimer > dashCoolDown) {
                direction = PlayerUtilsStatic.UseDirectionCircle(arrow, gameObject, 0);
                if (PSdawnDash1 != null) {
                    ParticleSystem.MainModule main = PSdawnDash1.main;
                    main.simulationSpeed = 1;
                    emissionModuleDash1.enabled = true;
                }
                if (PSdawnDash2 != null) {
                    ParticleSystem.MainModule main = PSdawnDash2.main;
                    main.simulationSpeed = 1;
                    emissionModuleDash2.enabled = true;
                }

            }
        } else {
            if (PSdawnDash1 != null) {
                emissionModuleDash1.enabled = false;
                ParticleSystem.MainModule main = PSdawnDash1.main;
                main.simulationSpeed = 8;
            }
            if (PSdawnDash2 != null) {
                emissionModuleDash2.enabled = false;
                ParticleSystem.MainModule main = PSdawnDash2.main;
                main.simulationSpeed = 8;
            }
            if (leftPressed) {
                GameLogic.instance.SetTimeScaleLocal(1);
                leftPressed = false;
            }
        }

        if (objectsInDeflectArea.Count != 0) {
            deflectDirection = PlayerUtilsStatic.UseDirectionCircle(arrow, gameObject, 1,-10,60);
            if (PSdawnDeflectFeedback != null) {
                PSdawnDeflectFeedback.Play();
            }
        }
        else {
            if (PSdawnDeflectFeedback != null) {
                PSdawnDeflectFeedback.Stop();
            }
        }

        //Si se suelta el botón izquierdo del ratón y se puede dashear, se desactiva la slowMotion y se modifica el timeScale además de poner en false canDash
        if (/*Input.GetMouseButtonUp(0)*/!InputManager.instance.dashButton&& InputManager.instance.prevDashButton && canDash && dashTimer > dashCoolDown) {
            if (leftPressed&&!grounded) {
                GameLogic.instance.SetTimeScaleLocal(1.0f);

                //En caso de ser la dirección hacia la derecha se comprueba si la prevHorizontalMov no era 1, en caso de no serlo, significa que el personaje antes estaba 
                //mirando a la izquierda, por tanto se hace un flip y se cambia prevHorizontalMov al que corresponde.
                if (direction.x > 0) {
                    if (prevHorizontalMov != 1.0f) {
                        prevHorizontalMov = 1.0f;
                        Flip();
                    }
                    //Debug.Log("Cambio de Prev");
                }

                //En este else se hace lo mismo pero hacia la izquierda
                else if(direction.x<0){
                    if (prevHorizontalMov != -1.0f) {
                        prevHorizontalMov = -1.0f;
                        Flip();
                    }

                }
                audioSource.pitch = 1.0f;

                audioSource.clip = dashClip;
                audioSource.Play();
                canJumpOnImpulsor = false;
                rb.velocity = new Vector2(0, 0);
                PlayerUtilsStatic.DoDash(gameObject, direction, dashForce,true);
                dashTimer = 0;
                SetCanDash(false);
                leftPressed = false;
            }

            //Si se pulsa el botón izquierdo del ratón y se puede dashear, se activa la slowMotion y se modifica el timeScale de gameLogic
        }
        else if (/*Input.GetMouseButtonDown(0)*/InputManager.instance.dashButton && !InputManager.instance.prevDashButton && canDash&& !grounded && dashTimer > dashCoolDown) {
            leftPressed = true;
            GameLogic.instance.SetTimeScaleLocal(slowMotionTimeScale);
        }

        if (/*Input.GetKeyDown(KeyCode.LeftControl)*/InputManager.instance.crawlButton&&!InputManager.instance.prevCrawlButton) {
            crawling = true;
            Vector2 newSize = myBoxCollider.size;
            newSize.y /= 2;
            Vector2 newOffset = myBoxCollider.offset;
            newOffset.y = newSize.y / 2;




            GetComponent<BoxCollider2D>().size = newSize;
            GetComponent<BoxCollider2D>().offset -= newOffset;
        }
        else if (/*Input.GetKeyUp(KeyCode.LeftControl)*/!InputManager.instance.crawlButton && InputManager.instance.prevCrawlButton) {
            GetComponent<BoxCollider2D>().size = originalSizeCollider;
            GetComponent<BoxCollider2D>().offset = originalOffsetCollider;

            crawling = false;
        }
        //Empieza el deflect
        if (/*Input.GetMouseButtonDown(1)*/InputManager.instance.deflectButton && !InputManager.instance.prevDeflectButton && objectsInDeflectArea.Count!=0) {
            if (PSdawnDeflectCast != null) {
                PSdawnDeflectCast.Play(); //Se inicia el sistema de particulas del deflect
            }
            GameLogic.instance.SetTimeScaleLocal(slowMotionTimeScale);
        }else if (/*Input.GetMouseButtonUp(1)*/!InputManager.instance.deflectButton && InputManager.instance.prevDeflectButton && objectsInDeflectArea.Count!=0) {
            if (PSdawnDeflectCast != null) {
                PSdawnDeflectCast.Stop(); //Se detiene el sistema de particulas del deflect
            }
            if(PSdawnDeflectRelease != null) {
                PSdawnDeflectRelease.Play();
            }
            foreach(GameObject g in objectsInDeflectArea) {

                if (g.GetComponent<Trampler>() != null) {
                    g.GetComponent<Trampler>().SwitchState(0, new TramplerStunnedState());
                }

                g.GetComponent<Rigidbody2D>().velocity = new Vector2(0, 0);
                PlayerUtilsStatic.DoDash(g, deflectDirection, 20*g.GetComponent<Rigidbody2D>().mass/2,false);
                audioSource.clip = deflectClip;
                audioSource.Play();

                if (g.tag == "Projectile") {
                    g.GetComponent<DoubleProjectile>().BeDeflected();
                }

            }
            GameLogic.instance.SetTimeScaleLocal(1f);
        }

    }

        //Método para aglotinar comportamiento de Dusk 
        void DuskBehavior() {

        //Si se suelta el botón izquierdo del ratón y se habia pulsado previamente, el tiempo pasa a cero
        //En caso de estar grounded Se hace una llamada a Punch
        if (/*Input.GetMouseButtonUp(0)*/!InputManager.instance.dashButton && InputManager.instance.prevDashButton && punchTimer > punchCoolDown) {
            if (leftPressed) {
                GameLogic.instance.SetTimeScaleLocal(1.0f);
                if (grounded) {
                    Punch(direction, 40000);
                    punchTimer = 0;
                    audioSource.clip = punchClip;
                    audioSource.Play();
                }

                leftPressed = false;
            }
        }

        //En caso de pulsar el botón izquierdo del ratón y estar grounded, se pone el timepo a 0.5
        else if (/*Input.GetMouseButtonDown(0)*/InputManager.instance.dashButton&&!InputManager.instance.prevDashButton && grounded&&punchTimer>punchCoolDown) {
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
            if (/*Input.GetMouseButtonDown(1)*/InputManager.instance.deflectButton && !InputManager.instance.prevDeflectButton) {
                foreach (GameObject g in NearbyObjects) {
                    if (g.GetComponent<DoubleObject>().isMovable) {
                        distanceToGrabbedObject = transform.position - NearbyObjects[0].transform.position;
                        grabbing = true;

                    }
                }
            }else if(/*Input.GetMouseButtonUp(1)*/!InputManager.instance.deflectButton && InputManager.instance.prevDeflectButton) {
                grabbing = false;
            }
        }
        if (/*Input.GetKeyDown(KeyCode.LeftControl)*/InputManager.instance.crawlButton && !InputManager.instance.prevCrawlButton && !grounded) {
            Smash();
            audioSource.pitch = 1.0f;

            audioSource.clip = smashClip;
            audioSource.Play();
        }

    }

    //Método que pone smashing en true modificando la velocidad del personaje para que solo vaya hacia abajo
    void Smash() {
        if (!smashing) {
            rb.AddForce(new Vector2(-rb.velocity.x,0),ForceMode2D.Impulse);
            audioSource.clip = smashClip;
            audioSource.pitch = 1.0f;

            audioSource.Play();
            rb.velocity = new Vector2(0, 0);
            rb.AddForce(new Vector2(0, -700));
            smashing = true;
        }
    }

    //Método que comprueba que objeto hay debajo y si es rompible y en caso afirmativo llama a GetBroken
    void DoSmash() {
        RaycastHit2D hit2D = Physics2D.Raycast(rb.position - new Vector2(0f, 0.5f), Vector2.down, 0.2f, groundMask);
        if (hit2D) {
            Debug.Log("HittingStuff");
            if (hit2D.transform.gameObject.GetComponent<DoubleObject>().isBreakable) {
                hit2D.transform.gameObject.GetComponent<DoubleObject>().GetBroken();
            }
        }
    }

    //Método intermediario para ciertos eventos temporales
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
        if (/*Input.GetKeyDown(KeyCode.Space)*/InputManager.instance.jumpButton&& !InputManager.instance.prevJumpButton && grounded&&!crawling||/*Input.GetKeyDown(KeyCode.Space)*/InputManager.instance.jumpButton && !InputManager.instance.prevJumpButton && sliding) {
            Jump();
        }

        if (onImpulsor) {

            if (someRayCastChecks && /*Input.GetKeyDown(KeyCode.Space)*/InputManager.instance.jumpButton && !InputManager.instance.prevJumpButton) {
                if (!calledImpuslorBool) {
                    Invoke("ImpulsorBool", 0.2f);
                    calledImpuslorBool = true;
                }
                canJumpOnImpulsor = false;
                //Debug.Log("JUMP");
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

        if (interactableObject != null) {
            if (/*Input.GetKeyDown(KeyCode.E)*/InputManager.instance.interactButton && !InputManager.instance.prevInteractButton) {
                interactableObject.Interact();
            }
        }


    }

    //Método que comprueba si el objeto delante es posible de mover con un puñetazo y lo hace en consecuencia
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
        PlayerUtilsStatic.ResetDirectionCircle(arrow);
        leftPressed = false;
        GameLogic.instance.SetTimeScaleLocal(1.0f);

        interactableObject = null;
        //Vector2 newPosition;

        DirectionCircle.SetOnce(false);


        //Si antes del cambio estaba en dusk, pasara a hacerse dynamic y al otro kinematic, además de darle su velocidad 


        if (dawn) {
            dawn = false;
            GameLogic.instance.SetTimeScaleLocal(1.0f);

            //activar el shader
            if (worldAssignation == world.DAWN) {
                myAnimator.SetBool("deflect_press", false);
                myAnimator.SetBool("dash_press", false);
                GameLogic.instance.currentPlayer = brotherScript;
                dominantVelocity = rb.velocity;
                brotherScript.dominantVelocity = rb.velocity;
                brotherScript.rb.bodyType = RigidbodyType2D.Dynamic;
                rb.bodyType = RigidbodyType2D.Kinematic;
                OnlyFreezeRotation();
                brotherScript.rb.velocity = dominantVelocity;
                rb.velocity = new Vector2(0.0f, 0.0f);
                brotherScript.prevHorizontalMov = prevHorizontalMov;
            }
            maskObjectScript.change = false;
        }
        else {
            prevHorizontalMov = brotherScript.prevHorizontalMov;

            dawn = true;
            //newPosition = transform.position;
            //newPosition.y += GameLogic.instance.worldOffset;
            //transform.position = newPosition;
            if (worldAssignation == world.DAWN){

                myAnimator.SetBool("deflect_press", false);
                myAnimator.SetBool("dash_press", false);

                GameLogic.instance.currentPlayer = this;
                dominantVelocity = brotherScript.rb.velocity;
                brotherScript.dominantVelocity = brotherScript.rb.velocity;
                rb.bodyType = RigidbodyType2D.Dynamic;
                brotherScript.rb.bodyType = RigidbodyType2D.Kinematic;
                brotherScript.rb.velocity = new Vector2(0.0f, 0.0f);
                rb.velocity = dominantVelocity;
            }

            //activar el shader
            maskObjectScript.change = true;
        }
    }

    //Método de carga de recursos (en este caso sprites y sonidos)
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

    //Método que reinicia la posición del personaje y aumenta la variable de muertes en GameLogic
    public override void Kill() {
        behindBush = false;
        sliding = false;

        GameLogic.instance.additionalOffset = new Vector3(0, 0, 0);

        if (worldAssignation == world.DUSK) {
            transform.position = GameLogic.instance.spawnPoint;
        } else {
            Debug.Log(GameLogic.instance.spawnPoint);
            transform.position = GameLogic.instance.spawnPoint + new Vector3(0, GameLogic.instance.worldOffset, 0);
        }

        if (rb != null) {
            rb.velocity = new Vector2(0, 0);
        }
        audioSource.clip = dieClip;
        audioSource.Play();
        GameLogic.instance.timesDied++;
    }

}
