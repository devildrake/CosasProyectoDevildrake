using FMOD.Studio;
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

    float timeGrounded;
    bool groundedSound;
    public MascaraRayCast mascaraRayCast;
    public Vector3 originalSizeCollider;
    public Vector3 originalCenterCollider;
    //public Vector3 originalOffsetCollider;

    BoxCollider boxCollider;

    public bool hasACrystal;
    public bool savedFragment;

    //Lista de objetos en el area de Deflect
    public List<GameObject> objectsInDeflectArea;

    //COOLDOWNS
    private float punchCoolDown;
    private float punchTimer;
    private float dashCoolDown;

    //Temporizador para byPassear posible problema de Z al entrar en portales
    private float timer;

    //Float que indica si el personaje ha de ir hacia la derecha o hacia la izquierda en su transición inicial, de todas formas
    //Se ha de setear a 0 para que haga la comprovación en un principio.
    public float whichX;
    //Objeto con el transform de la posición a la que queremos que llegue el pj
    public GameObject placeToGo;
    public AnimationSounds animationSound;
    public bool mustEnd;
    public CustomGravity customGravity;
    private float dashTimer;

    private float deflectCoolDown;
    private float deflectTimer;


    float timeNotMoving;
    float timeNotGrounded;
    float timeNotGroundedThreshold = 0.1f;
    float slowMotionTimeScale;
    float dashForce;

    float timeToRest;
    float timeToDrag;
    float timeCountToDrag;
    float maxSpeedY;
    float timeMoving;

    bool leftPressed;
    public List<GameObject> NearbyObjects;
    [HideInInspector]
    public bool crawling; 

    public bool moving;

    [HideInInspector]
    public bool smashing;
    bool grabbing;
    GameObject capaObject;

    //Bool para determinar si esta detrás de un bush
    public bool behindBush;

    //Bool para determinar si esta dentro del area de un impulsor
    [HideInInspector]
    public bool onImpulsor;

    [HideInInspector]
    public bool canJumpOnImpulsor;

    [HideInInspector]
    public bool calledImpuslorBool;

    [HideInInspector]
    public bool sliding;

    [HideInInspector]
    public float timeNotSliding;

    [HideInInspector]
    public DoubleObject interactableObject;
    Vector3 distanceToGrabbedObject;

    public float distanciaBordeSprite = 0.745f;

    //Booleano privado que maneja que el personaje pueda utilizar el dash, se pone en true a la vez que el grounded y en false cuando se usa el Dash
    [SerializeField]
    [HideInInspector]
    public bool canDash;

    //Vector dirección para el dash/Hit
    private Vector2 direction;

    //Vector dirección para el deflect
    private Vector2 deflectDirection;

    //Hijo asignado a mano por ahora, anchor de rotación
    public ArrowScript arrow;

    //Velocidad hacia los laterales base
    float characterSpeed = 6;

    //Fuerza del salto
    float jumpStrenght = 7;

    //Booleano que gestiona si el personaje esta en el suelo (Para saber si puede saltar o no)
    [SerializeField]
    [HideInInspector]
    public bool grounded = false;

    [HideInInspector]
    public bool facingRight;

    //Mascara de suelo (Mas tarde podria ser util, ahora esta aqui por que para gestionar grounded hice pruebas varias)
    public LayerMask groundMask;
    public LayerMask slideMask;
    public LayerMask[] grabbableMask;
    //EventInstance punchChargeEvent;
    bool firstPunch;
    //Referencia al RigidBody2D del personaje, se inicializa en el start.
    public Rigidbody rb;

    //ESTE INT ES -1 SI EL MOVIMIENTO PREVIO FUE HACIA LA IZQUIERDA Y ES 1 SI EL MOVIMIENTO PREVIO FUE HACIA LA DERECHA
    public float prevHorizontalMov;

    [SerializeField]
    //Booleano que comprueba si el personaje deberia estar ralentizado en el aire, se reinicia al estar en el suelo (Que se comprueba con colliders2D y tags)
    bool slowedInTheAir;

    //Referencia al script que controla la mascara alfa que se utiliza para el shader de mundos.
    //Queremos acceder a este script para cambiar un flag y activar la transición de un mundo a otro.
    public Change_Scale maskObjectScript;

    //Referencia al sistema de particulas de dawn para el deflect.
    public ParticleSystem PSdawnDeflectCast, PSdawnDeflectRelease, PSdawnDash1, PSdawnDash2, PSDawnDashRelease, PSdawnDeflectFeedback;
    private ParticleSystem.EmissionModule emissionModuleDash1, emissionModuleDash2;
    [SerializeField]private Color particleDashColor, particleHasDashedColor;
    private ParticleSystem.MainModule mainModuleDash1, mainModuleDash2;
    private ParticleSystem.TrailModule trailModuleDash1, trailModuleDash2;

    AudioSource audioSource;
    GroundCheck groundCheck;
    public PlayerController brotherScript;
    Animator myAnimator;
    Animator brotherAnimator;
    //BoxCollider2D myBoxCollider;

    public GameObject armTarget;
    public IK_FABRIK_UNITY arm;
    int currentArmTargetIndex=8;
    public Transform[] punchRightPositions;
    public Transform[] punchLeftPositions;
    public Transform[] punchChargePositions;
    public Transform[] punchChargePositions2;
    enum ARMSTATE { PUNCH, GRAB, IDLE, PUNCHCHARGE};
    ARMSTATE armstate;
    public PunchContact punchContact;
    public BoxCollider2D punchTrigger;
    [SerializeField] private ParticleSystem armParticleSystem;
    public bool useXOffset = true;
    public bool lookAtMe = false;

    Vector3[] grabPositions;

    //Se inicializan las cosas
    void Start() {
        customGravity = gameObject.AddComponent<CustomGravity>();
        animationSound = GetComponentInChildren<AnimationSounds>();
        grabPositions = new Vector3[4];

        //armPositions = new Vector3[9];
        armstate = ARMSTATE.IDLE;

        timeNotSliding = 0.2f;
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
            mainModuleDash1 = PSdawnDash1.main;
            trailModuleDash1 = PSdawnDash1.trails;
        }
        if(PSdawnDash2 != null) {
            emissionModuleDash2 = PSdawnDash2.emission;
            emissionModuleDash2.enabled = false;
            mainModuleDash2 = PSdawnDash2.main;
            trailModuleDash2 = PSdawnDash2.trails;
        }
        if(PSdawnDeflectFeedback != null) {
            PSdawnDeflectFeedback.Stop();
        }
        if(PSDawnDashRelease != null) {
            PSDawnDashRelease.Stop();
        }
        audioSource = GetComponent<AudioSource>();
        groundCheck = GetComponentInChildren<GroundCheck>();
        brotherScript = brotherObject.GetComponent<PlayerController>();
        //myBoxCollider = GetComponent<BoxCollider2D>(); 
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
        // originalOffsetCollider = GetComponent<BoxCollider>().;
        boxCollider = GetComponent<BoxCollider>();
        originalSizeCollider = boxCollider.size;
        originalCenterCollider = boxCollider.center;

        leftPressed = false;
        prevHorizontalMov = 1;
        facingRight = true;
        groundMask = LayerMask.GetMask("Ground");
        slideMask = LayerMask.GetMask("Slide");
        offset = GameLogic.instance.worldOffset;

        rb = GetComponent<Rigidbody>();
        slowedInTheAir = false;
        slowMotionTimeScale = 0.75f;
        dashForce = 7;
        //Se hace esto para que UseDirectionCircle haga uso de su bool once y inicialize las cosas que le interesan
        //direction = DirectionCircle.UseDirectionCircle(arrow, gameObject,0);


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
        if (arrow != null) {
            arrow.gameObject.SetActive(false);
        }

        if(armTarget!=null)
        armTarget.transform.position = transform.position + new Vector3(1, 0.5f, 0);

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
            if (arrow != null) {
                arrow.gameObject.SetActive(false);
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
                    if (Mathf.Abs(placeToGo.transform.position.x - transform.position.x) > 0.01f) {

                        if (whichX == 0) {
                            if (placeToGo.transform.position.x - transform.position.x > 0) {
                                whichX = 1.0f;
                                //Debug.Log("Place " + placeToGo.transform.position.x);
                                //Debug.Log("Me " + transform.position.x);

                            } else {
                                whichX = -1.0f;
                                //Debug.Log("MustGoLeft");
                            }
                        } else {
                            if (whichX > 0) {
                                if (transform.position.x > placeToGo.transform.position.x) {
                                    transform.position = new Vector3(placeToGo.transform.position.x, transform.position.y, transform.position.z);

                                } else {
                                    transform.rotation = Quaternion.AngleAxis(0, new Vector3(0, 1, 0));
                                    transform.Translate(Vector3.right * 2.0f * Time.deltaTime);
                                    myAnimator.SetBool("grounded", true);
                                    myAnimator.SetBool("moving", true);
                                    myAnimator.SetFloat("timeMoving", 1.0f);

                                }

                            } else {
                                if (transform.position.x < placeToGo.transform.position.x) {
                                    transform.position = new Vector3(placeToGo.transform.position.x, transform.position.y, transform.position.z);
                                } else {
                                    Debug.Log("LeFtu");
                                    transform.rotation = Quaternion.AngleAxis(180, new Vector3(0, 1, 0));
                                    transform.Translate(Vector3.right * 2.0f * Time.deltaTime);
                                    myAnimator.SetBool("grounded", true);
                                    myAnimator.SetBool("moving", true);
                                    myAnimator.SetFloat("timeMoving", 1.0f);
                                }
                            }
                        }
                    } else {

                        if (placeToGo.tag == "Finish") {
                            if (grounded) {
                                myAnimator.gameObject.transform.rotation = Quaternion.identity;
                                //brotherAnimator.gameObject.transform.rotation = q;
                                timer += Time.deltaTime;

                                if (transform.position.z > 1.8f || timer > 2.0f) {

                                    //InputManager.UnBlockInput();
                                    GameLogic.instance.levelFinished = true;
                                } else {
                                    myAnimator.SetBool("grounded", true);
                                    myAnimator.SetBool("moving", true);
                                    myAnimator.SetFloat("timeMoving", 1.0f);
                                    if (whichX > 0) {
                                        //Debug.Log("A");
                                        transform.Translate(Vector3.forward * 1.4f * Time.deltaTime);
                                    } else if (whichX < 0) {
                                        //Debug.Log("B");
                                        transform.Translate(Vector3.back * 1.4f * Time.deltaTime);
                                    }
                                }
                            } else {
                                //rb.s = 3.0f;
                                customGravity.gravityScale = 3.0f;
                            }

                        } else {
                            InputManager.UnBlockInput();
                            whichX = 0;
                            if (placeToGo != null&&placeToGo.tag!="Finish") {
                                Destroy(placeToGo.gameObject);
                            }
                        }


                    }
                    rb.velocity = new Vector3(rb.velocity.x, 0);
                } else if (GameLogic.instance.GetCurrentLevel() == "MenuInteractuable") {
                    if (transform.position.z > 0) {

                        Quaternion q = Quaternion.AngleAxis(180, new Vector3(0, 1, 0));

                        myAnimator.transform.rotation = q;
                        brotherAnimator.transform.rotation = q;

                        transform.Translate(Vector3.back * 1.2f * Time.deltaTime);



                    } else if (transform.position.z > 0.1f || transform.position.z < -0.1f) {
                       // Debug.Log(transform.position.z);

                        transform.position = new Vector3(transform.position.x, transform.position.y, 0);
                        brotherObject.transform.position = new Vector3(brotherObject.transform.position.x, brotherObject.transform.position.y, 0);
                       // Debug.Log("DeSPUES " + transform.position);



                    } else {
                        InputManager.UnBlockInput();
                        Quaternion q = Quaternion.AngleAxis(90, new Vector3(0, 1, 0));

                        myAnimator.transform.rotation = q;
                        brotherAnimator.transform.rotation = q;
                      //  Debug.Log("UNBLOCKBRUH");
                    }

                } else {
                InputManager.UnBlockInput();
                //Debug.Log("UNBLOCKBRUH");
                }
            }
        }
    }

    void Update() {

        if (grounded) {
            timeGrounded += Time.deltaTime;
            timeNotGrounded = 0;
        } else {
            timeGrounded = 0;
            if (timeNotGrounded < timeNotGroundedThreshold) {
                timeNotGrounded += Time.deltaTime;
            }
        }


        if (SoundManager.Instance != null) {
            if (timeGrounded > 0.1f) {
                if (!groundedSound) {
                    groundedSound = true;
                    SoundManager.Instance.PlayOneShotSound("event:/Dusk/StepsDusk", transform);
                    SoundManager.Instance.PlayOneShotSound("event:/Dusk/StepsDusk", transform);
                    SoundManager.Instance.PlayOneShotSound("event:/Dusk/StepsDusk", transform);
                }
            } else {
                groundedSound = false;
            }
        }

        //if (placeToGo == null) {
        if (rb.velocity.y < 0) {
            //rb.gravityScale = 2.5f;
            customGravity.gravityScale = 2.5f;

        } else {
            //rb.gravityScale = 1;
            customGravity.gravityScale = 1;

        }
        //} else {
        //    rb.gravityScale = 0;
        //    rb.velocity = new Vector2(rb.velocity.x, 0);
        //}

        //if (!sliding) {
        //    rb.gravityScale = 1.5f;
        //}

        AddToGameLogicList();

        if (added) {
            if ((worldAssignation == world.DAWN && dawn) || (worldAssignation == world.DUSK && !dawn)) {
                animationSound.active = true;
                MoveEvents();
            } else {
                animationSound.active = false;
            }

            if (armTarget != null) {
                arm.transform.position = transform.position;
                if (worldAssignation == world.DUSK && !dawn) {
                    if (Vector3.Distance(armTarget.transform.position, transform.position) > 3) {
                        armTarget.transform.position = transform.position + new Vector3(1, 0.5f, 0);
                    }

                    #region armStuff
                    PLAYBACK_STATE state = PLAYBACK_STATE.PLAYING;
                    //punchChargeEvent.getPlaybackState(out state);

                    switch (armstate){
                        case ARMSTATE.IDLE:
                            //arm.gameObject.SetActive(false);
                            arm.meshObject.SetActive(false);
                            punchTrigger.enabled = false;
                            punchContact.enabled = false;
                            armParticleSystem.Stop();
                            //punchChargeEvent.getPlaybackState(out state);
                            if (state == PLAYBACK_STATE.PLAYING) {
                                //punchChargeEvent.stop(STOP_MODE.IMMEDIATE);
                                //SoundManager.Instance.StopEvent(punchChargeEvent,false);
                            }
                            break;
                        case ARMSTATE.PUNCHCHARGE:

                            if (/*punchChargeEvent.Equals(null)||*/!firstPunch) {
                                //Debug.Log("CREADO");
                                firstPunch = true;
                                //punchChargeEvent = SoundManager.Instance.PlayEvent("event:/Dusk/ArmCharge", transform);
                            } else {
                                if (state != PLAYBACK_STATE.PLAYING) {
                                    //Debug.Log("USADO");
                                    //punchChargeEvent = SoundManager.Instance.PlayEvent("event:/Dusk/ArmCharge", transform);
                                    //punchChargeEvent.start();
                                }
                            }

                            if (currentArmTargetIndex < 4) {
                                arm.meshObject.SetActive(true);
                                armParticleSystem.Play();
                                if (facingRight)
                                {
                                    if (Vector3.Distance(armTarget.transform.position, punchChargePositions[currentArmTargetIndex].position) < 0.2f)
                                    {
                                        currentArmTargetIndex++;
                                    }
                                    else
                                    {
                                        Vector3 velocity = punchChargePositions[currentArmTargetIndex].position - armTarget.transform.position;
                                        velocity.Normalize();
                                        velocity *= Time.deltaTime;
                                        armTarget.transform.position += velocity * 12;
                                    }
                                }
                                else
                                {
                                    if (Vector3.Distance(armTarget.transform.position, punchChargePositions2[currentArmTargetIndex].position) < 0.2f)
                                    {
                                        currentArmTargetIndex++;
                                    }
                                    else
                                    {
                                        Vector3 velocity = punchChargePositions2[currentArmTargetIndex].position - armTarget.transform.position;
                                        velocity.Normalize();
                                        velocity *= Time.deltaTime;
                                        armTarget.transform.position += velocity * 12;
                                    }
                                }


                            }

                            if (!grounded) {
                                armstate = ARMSTATE.IDLE;
                                arm.meshObject.SetActive(false);
                            }

                            break;
                        case ARMSTATE.PUNCH:
                            if (state == PLAYBACK_STATE.PLAYING) {
                                //punchChargeEvent.stop(STOP_MODE.IMMEDIATE);
                                //SoundManager.Instance.StopEvent(punchChargeEvent, false);

                            }
                            armParticleSystem.Play();
                            if (currentArmTargetIndex < 8) { 
                                if (!arm.meshObject.activeInHierarchy) {
                                   // arm.gameObject.SetActive(true);
                                    arm.meshObject.SetActive(true);
                                }
                                if (facingRight) {
                                    if (currentArmTargetIndex < 7) {
                                        arm.punchContact.enabled = false;
                                        punchTrigger.enabled = false;
                                    } else if (Vector2.Distance(punchRightPositions[7].position, arm.joints[28].position) < 0.25f) {
                                        arm.punchContact.enabled = true;
                                        punchTrigger.enabled = true;
                                    }

                                    if (Vector3.Distance(armTarget.transform.position, punchRightPositions[currentArmTargetIndex].position) < 0.2f) {
                                        currentArmTargetIndex++;
                                    } else {
                                        Vector3 velocity = punchRightPositions[currentArmTargetIndex].position - armTarget.transform.position;
                                        velocity.Normalize();
                                        velocity *= Time.deltaTime;
                                        armTarget.transform.position += velocity * 12;
                                    }
                                } else {

                                    if (currentArmTargetIndex < 7) {
                                        arm.punchContact.enabled = false;
                                        punchTrigger.enabled = false;

                                    } else if (Vector2.Distance(punchLeftPositions[7].position, arm.joints[28].position) < 0.25f) {
                                        arm.punchContact.enabled = true;
                                        punchTrigger.enabled = true;
                                    }

                                    if (Vector3.Distance(armTarget.transform.position, punchLeftPositions[currentArmTargetIndex].position) < 0.2f) {
                                        currentArmTargetIndex++;
                                    } else {
                                        Vector3 velocity = punchLeftPositions[currentArmTargetIndex].position - armTarget.transform.position;
                                        velocity.Normalize();
                                        velocity *= Time.deltaTime;
                                        armTarget.transform.position += velocity * 12;
                                    }
                                }
                            } else {
                                arm.meshObject.SetActive(false);
                                arm.punchContact.enabled = false;
                                armTarget.transform.position = transform.position + new Vector3(0, 2, 0);
                                armstate = ARMSTATE.IDLE;
                            }
                            break;
                        case ARMSTATE.GRAB:
                            //if (punchChargeEvent.Equals(null)) {
                            //    Debug.Log("CREADO");
                            //    punchChargeEvent = SoundManager.Instance.PlayEvent("event:/Dusk/ArmCharge", transform);
                            //} else {
                            //    punchChargeEvent.getPlaybackState(out state);
                            //    if (state != PLAYBACK_STATE.PLAYING) {
                            //        Debug.Log("Play");
                            //        //punchChargeEvent.start();
                            //    }
                            //}

                            if (NearbyObjects.Count == 0) {
                               // SoundManager.Instance.StopEvent(punchChargeEvent);
                                armstate = ARMSTATE.IDLE;
                                break;
                            }

                            //Aqui hay que hacer que el brazo se coloque en la posición original y vaya justo a la posición central + new Vector3(0,2,0) del objeto grabbeable delante suyo 
                            //Y después hasta alcanzar a chocar con el objeto Draggable
                            armParticleSystem.Play();
                            if (currentArmTargetIndex ==0) {
                                if (facingRight) {
                                    //grabPositions[0] = transform.position + new Vector3(-2,2,0);
                                    grabPositions[0] = transform.position + new Vector3(-0.5f, 1, 0);
                                    grabPositions[1] = transform.position + new Vector3(-0.75f, 1.5f, 0);

                                } else {
                                    //grabPositions[0] = transform.position + new Vector3(2,2,0);
                                    grabPositions[0] = transform.position + new Vector3(0.5f, 1, 0);
                                    grabPositions[1] = transform.position + new Vector3(0.75f, 1.5f, 0);

                                }
                                //grabPositions[1] = transform.position + new Vector3(0, 2.5f, 0);
                                grabPositions[2] = NearbyObjects[0].transform.position + new Vector3(0, 2f, 0);
                            grabPositions[3] = NearbyObjects[0].transform.position + new Vector3(0, 1f, 0);
                                arm.meshObject.SetActive(true);
                                arm.punchContact.enabled = false;
                                armTarget.transform.position = grabPositions[0];
                                currentArmTargetIndex++;
                            }else if (currentArmTargetIndex < 3) {
                                if (Vector3.Distance(armTarget.transform.position, grabPositions[currentArmTargetIndex]) < 0.2f) {
                                    currentArmTargetIndex++;
                                } else {
                                    Vector3 velocity = grabPositions[currentArmTargetIndex] - armTarget.transform.position;
                                    velocity.Normalize();
                                    velocity *= Time.deltaTime;
                                    armTarget.transform.position += velocity * 8;
                                }
                            } else {
                                grabPositions[3] = NearbyObjects[0].transform.position + new Vector3(0, 1f, 0);
                                //currentArmTargetIndex = 2;

                                if (!(Vector3.Distance(armTarget.transform.position, grabPositions[currentArmTargetIndex]) < 0.2f)) {
                                    Vector3 velocity = grabPositions[currentArmTargetIndex] - armTarget.transform.position;
                                    velocity.Normalize();
                                    velocity *= Time.deltaTime;
                                    armTarget.transform.position += velocity * 8;
                                }

                            }
                            break;
                    }

                    //if (!moving&&grounded) {
                    //    if (Vector3.Distance(armTarget.transform.position, armPositions[currentArmTargetIndex]) < 0.2f) {
                    //        currentArmTargetIndex++;
                    //        currentArmTargetIndex = currentArmTargetIndex % 9;
                    //    } else {
                    //        Vector3 velocity = armPositions[currentArmTargetIndex] - armTarget.transform.position;
                    //        velocity.Normalize();
                    //        velocity *= Time.deltaTime;
                    //        armTarget.transform.position += velocity * 2;
                    //    }
                    //    armPositions[0] = transform.position;
                    //    armPositions[1] = transform.position + new Vector3(1, 0, 0);
                    //    armPositions[2] = transform.position + new Vector3(-1, 0, 0);
                    //    armPositions[3] = transform.position + new Vector3(1, 1, 0);
                    //    armPositions[4] = transform.position + new Vector3(-1, 1, 0);
                    //    armPositions[5] = transform.position + new Vector3(1, 0.5f, 0);
                    //    armPositions[6] = transform.position + new Vector3(1, -0.5f, 0);
                    //    armPositions[7] = transform.position + new Vector3(-1, 0.5f, 0);
                    //    armPositions[8] = transform.position + new Vector3(-1, -0.5f, 0);
                    //} else if(moving){
                    //    int facing = 0;
                    //    if (facingRight) {
                    //        facing = 1;
                    //    } else {
                    //        facing = -1;
                    //    }
                    //    Debug.Log(facing);
                    //    if (Vector3.Distance(armTarget.transform.position, transform.position + new Vector3(1, 0, 0) * facing + new Vector3(0, 1, 0)) < 0.2f) {
                    //        currentArmTargetIndex++;
                    //        currentArmTargetIndex = currentArmTargetIndex % 9;
                    //    } else {
                    //        Vector3 velocity = (transform.position + new Vector3(1, 0, 0) * facing + new Vector3(0, 1, 0)) - armTarget.transform.position;
                    //        velocity.Normalize();
                    //        //velocity *= Time.deltaTime;
                    //        armTarget.transform.position += velocity * 2 * Time.deltaTime;
                    //        Debug.Log(velocity);

                    //    }
                    //}else if (!grounded) {

                    //}
                    #endregion

                }
            }

            if (!GameLogic.instance.levelFinished) {
                //Debug.Log(grounded);
                //Add del transformable

                //if (arrow == null) {
                //    arrow = GameObject.FindGameObjectWithTag("Arrow").GetComponent<ArrowScript>();
                //}

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


    public void LateUpdate() {
        if (placeToGo != null){
            //rb.gravityScale = 10;
            customGravity.gravityScale = 10;

        }
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
                myAnimator.SetBool("dash_press", InputManager.instance.dashButton && dashTimer>=dashCoolDown&&canDash&&!grounded);
                myAnimator.SetBool("deflecting", deflectTimer < deflectCoolDown);
                myAnimator.SetBool("deflect_press", InputManager.instance.deflectButton && deflectTimer >= deflectCoolDown&&objectsInDeflectArea.Count>0);
                brotherAnimator.SetBool("punching", punchTimer < punchCoolDown);

            }

        } else {
            //Debug.Log("NullAnimator or Block in progress");
        }
    }

    //Clampeo de la velocidad del personaje
    void ClampSpeed() {
        if (rb != null) {
            if (rb.velocity.y > maxSpeedY) {
                rb.velocity = new Vector2(rb.velocity.x, maxSpeedY);
            }
        } else {
            rb = GetComponent<Rigidbody>();
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
        if (rb.isKinematic)
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


            //InputManager.instance.UpdatePreviousPlayer();
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
            if (timeNotSliding < 0.3f) {
                timeNotSliding += Time.deltaTime;
            }
            if (!grabbing) {
                timeCountToDrag = 0;
                bool changed = false;
                float mustSlow = 1;

                if (armstate == ARMSTATE.IDLE) {
                    if (dawn) {
                        if (InputManager.instance.horizontalAxis * (float)prevHorizontalMov < 0 && InputManager.instance.horizontalAxis != 0.0f && !InputManager.GetBlocked()) {
                            changed = true;
                            //Debug.Log("CHANGE");
                            prevHorizontalMov = InputManager.instance.horizontalAxis;
                        }
                        if (grounded) {
                            if (dawn && worldAssignation == world.DAWN && InputManager.instance.dashButton) {
                                PlayerUtilsStatic.ResetDirectionCircle(arrow);
                            }

                            rb.velocity = new Vector2(0, rb.velocity.y);
                            //Debug.Log("Se para");
                            if (facingRight&&!mascaraRayCast.hit2D) {
                                transform.Translate(Vector3.right * InputManager.instance.horizontalAxis * characterSpeed * Time.deltaTime);
                            } else if (!mascaraRayCast.hit2D){
                                transform.Translate(Vector3.left * InputManager.instance.horizontalAxis * characterSpeed * Time.deltaTime);
                            } else {
                                myAnimator.SetBool("moving", false);
                                myAnimator.SetFloat("timeMoving", 0);
                            }
                            slowedInTheAir = false;
                            if (InputManager.instance.horizontalAxis != 0) {
                                timeMoving += Time.deltaTime;
                                timeNotMoving = 0;
                                moving = true;
                                if (!audioSource.isPlaying && !crawling) {
                                    audioSource.pitch = 0.3f;
                                    //audioSource.clip = walkClip;
                                    //audioSource.Play();
                                } else if (crawling) {
                                    audioSource.pitch = 0.3f;
                                    audioSource.clip = crawlClip;
                                    audioSource.Play();
                                }
                            } else {
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

                            if (facingRight && !mascaraRayCast.hit2D) {
                                transform.Translate(Vector3.right * InputManager.instance.horizontalAxis * characterSpeed * 0.75f * mustSlow * Time.deltaTime);
                            } else if (!mascaraRayCast.hit2D) {
                                transform.Translate(Vector3.left * InputManager.instance.horizontalAxis * characterSpeed * 0.75f * mustSlow * Time.deltaTime);
                            }

                            //Velocidad X a 0
                            rb.AddForce(new Vector2(-rb.velocity.x, 0), ForceMode.Impulse);

                        }
                    //Not Grounded and not changed
                        else {
                            if (slowedInTheAir) {
                                //rb.AddForce(new Vector2(-rb.velocity.x, 0), ForceMode2D.Impulse);
                                mustSlow = 0.5f;
                            }

                            if (facingRight && !mascaraRayCast.hit2D) {
                                transform.Translate(Vector3.right * InputManager.instance.horizontalAxis * characterSpeed * 0.75f * mustSlow * Time.deltaTime);
                            } else if (!mascaraRayCast.hit2D) {
                                transform.Translate(Vector3.left * InputManager.instance.horizontalAxis * characterSpeed * 0.75f * mustSlow * Time.deltaTime);
                            }
                        }

                        if (changed) {
                            Flip();
                        }
                    } else { //AQUI METO LOS INPUTS DE DUSK (PLAYER2)
                        if (InputManager.instance.horizontalAxis2 * (float)prevHorizontalMov < 0 && InputManager.instance.horizontalAxis2 != 0.0f && !InputManager.GetBlocked()) {
                            changed = true;
                            //Debug.Log("CHANGE");
                            prevHorizontalMov = InputManager.instance.horizontalAxis2;
                        }
                        if (grounded) {
                            if (dawn && worldAssignation == world.DAWN && InputManager.instance.dashButton2) {
                                PlayerUtilsStatic.ResetDirectionCircle(arrow);
                            }

                            rb.velocity = new Vector2(0, rb.velocity.y);
                            //Debug.Log("Se para");
                            if (facingRight&& !mascaraRayCast.hit2D) {
                                transform.Translate(Vector3.right * InputManager.instance.horizontalAxis2 * characterSpeed * Time.deltaTime);
                            } else if(!mascaraRayCast.hit2D) {
                                transform.Translate(Vector3.left * InputManager.instance.horizontalAxis2 * characterSpeed * Time.deltaTime);
                            } else {
                                myAnimator.SetBool("moving", false);
                                myAnimator.SetFloat("timeMoving", 0);
                            }
                            slowedInTheAir = false;
                            if (InputManager.instance.horizontalAxis2 != 0) {
                                timeMoving += Time.deltaTime;
                                timeNotMoving = 0;
                                moving = true;
                                if (!audioSource.isPlaying && !crawling) {
                                    audioSource.pitch = 0.3f;
                                    //audioSource.clip = walkClip;
                                    //audioSource.Play();
                                } else if (crawling) {
                                    audioSource.pitch = 0.3f;
                                    audioSource.clip = crawlClip;
                                    audioSource.Play();
                                }
                            } else {
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

                            if (facingRight) {
                                transform.Translate(Vector3.right * InputManager.instance.horizontalAxis2 * characterSpeed * 0.75f * mustSlow * Time.deltaTime);
                            } else if (!mascaraRayCast.hit2D) {
                                transform.Translate(Vector3.left * InputManager.instance.horizontalAxis2 * characterSpeed * 0.75f * mustSlow * Time.deltaTime);
                            }

                            //Velocidad X a 0
                            rb.AddForce(new Vector2(-rb.velocity.x, 0), ForceMode.Impulse);

                        }
                        //Not Grounded and not changed
                        else {
                            if (slowedInTheAir) {
                                //rb.AddForce(new Vector2(-rb.velocity.x, 0), ForceMode2D.Impulse);
                                mustSlow = 0.5f;
                            }

                            if (facingRight && !mascaraRayCast.hit2D) {
                                transform.Translate(Vector3.right * InputManager.instance.horizontalAxis2 * characterSpeed * 0.75f * mustSlow * Time.deltaTime);
                            } else if (!mascaraRayCast.hit2D) {
                                transform.Translate(Vector3.left * InputManager.instance.horizontalAxis2 * characterSpeed * 0.75f * mustSlow * Time.deltaTime);
                            }
                        }

                        if (changed) {
                            Flip();
                        }

                    }
                }
            }
            //Grabbing es true
            else {
                if (dawn) {
                    if (grounded) {
                        if (NearbyObjects[0].GetComponent<DoubleObject>().isMovable) {
                            timeCountToDrag += Time.deltaTime;

                            if (timeCountToDrag < timeToRest) {

                            } else if (timeCountToDrag < timeToRest + timeToDrag) {

                                if (facingRight) {
                                    if (InputManager.instance.horizontalAxis < 0) {
                                        Debug.Log("A");
                                        transform.Translate(Vector3.right * InputManager.instance.horizontalAxis * characterSpeed / 2 * Time.deltaTime);
                                    } else {
                                        Debug.Log("b");

                                        RaycastHit2D hitFromBox = Physics2D.Raycast(NearbyObjects[0].transform.position, Vector2.right, 24.0f);
                                        if (!hitFromBox) {
                                            Debug.Log("c");

                                            transform.Translate(Vector3.right * InputManager.instance.horizontalAxis * characterSpeed / 2 * Time.deltaTime);
                                        } else {
                                            Debug.Log(hitFromBox.collider.gameObject);
                                        }
                                    }
                                } else {
                                    if (InputManager.instance.horizontalAxis > 0) {
                                        transform.Translate(Vector3.left * InputManager.instance.horizontalAxis * characterSpeed / 2 * Time.deltaTime);
                                    } else {
                                        RaycastHit2D hitFromBox = Physics2D.Raycast(NearbyObjects[0].transform.position, Vector2.right, 4.0f);
                                        if (!hitFromBox) {
                                            transform.Translate(Vector3.left * InputManager.instance.horizontalAxis * characterSpeed / 2 * Time.deltaTime);
                                        } else {
                                            Debug.Log(hitFromBox.collider.gameObject);
                                        }
                                    }
                                }

                                NearbyObjects[0].transform.position = transform.position - distanceToGrabbedObject;
                                //Debug.Log(distanceToGrabbedObject);
                                if (!audioSource.isPlaying) {
                                    audioSource.clip = grabClip;
                                    audioSource.Play();
                                }
                            } else {
                                timeCountToDrag = 0;
                            }
                        } else {
                            grabbing = false;
                        }
                    } else {
                        grabbing = false;
                    }
                } else {
                    if (grounded) {
                        if (NearbyObjects[0].GetComponent<DoubleObject>().isMovable) {
                            timeCountToDrag += Time.deltaTime;

                            if (timeCountToDrag < timeToRest) {

                            } else if (timeCountToDrag < timeToRest + timeToDrag) {

                                if (facingRight) {
                                    if (InputManager.instance.horizontalAxis2 < 0) {
                                        Debug.Log("A");
                                        transform.Translate(Vector3.right * InputManager.instance.horizontalAxis2 * characterSpeed / 2 * Time.deltaTime);
                                    } else {
                                        Debug.Log("b");

                                        RaycastHit2D hitFromBox = Physics2D.Raycast(NearbyObjects[0].transform.position, Vector2.right, 0.7f, LayerMask.GetMask("Ground"));

                                        if (!hitFromBox) {
                                            hitFromBox = Physics2D.Raycast(NearbyObjects[0].transform.position, Vector2.right, 0.7f, LayerMask.GetMask("Platform"));
                                        }

                                        if (!hitFromBox) {
                                            Debug.Log("c");

                                            transform.Translate(Vector3.right * InputManager.instance.horizontalAxis2 * characterSpeed / 2 * Time.deltaTime);
                                        } else {
                                            Debug.Log(hitFromBox.collider.gameObject);
                                        }
                                    }
                                } else {
                                    if (InputManager.instance.horizontalAxis2 > 0) {
                                        transform.Translate(Vector3.left * InputManager.instance.horizontalAxis2 * characterSpeed / 2 * Time.deltaTime);
                                    } else {
                                        RaycastHit2D hitFromBox = Physics2D.Raycast(NearbyObjects[0].transform.position, Vector2.left, 0.7f, LayerMask.GetMask("Ground"));

                                        if (!hitFromBox) {
                                            hitFromBox = Physics2D.Raycast(NearbyObjects[0].transform.position, Vector2.right, 0.7f, LayerMask.GetMask("Platform"));
                                        }

                                        if (!hitFromBox) {
                                            transform.Translate(Vector3.left * InputManager.instance.horizontalAxis2 * characterSpeed / 2 * Time.deltaTime);
                                        } else {
                                            Debug.Log(hitFromBox.collider.gameObject);
                                        }
                                    }
                                }


                                NearbyObjects[0].transform.position = transform.position - distanceToGrabbedObject;
                                //Debug.Log(distanceToGrabbedObject);
                                if (!audioSource.isPlaying) {
                                    audioSource.clip = grabClip;
                                    audioSource.Play();
                                }
                            } else {
                                timeCountToDrag = 0;
                            }
                        } else {
                            grabbing = false;
                        }
                    } else {
                        grabbing = false;
                    }
                }

            }
        }else {//Sliding es true
            //rb.gravityScale = 15;
            customGravity.gravityScale = 15;

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
            //Debug.DrawRay(rb.position, Vector2.right * 1.5f);
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
    public void Jump() {
        if (arm != null) {
            armstate = ARMSTATE.IDLE;
            arm.meshObject.SetActive(false);
        }

        rb.velocity= new Vector2(0,0);
        //audioSource.pitch = 1.0f;

        rb.AddForce(transform.up * jumpStrenght, ForceMode.Impulse);
        //audioSource.clip = jumpClip;
        //audioSource.Play();
        string characterToString = "";
        if (worldAssignation == world.DAWN) {
            characterToString = "Dawn";
        } else {
            characterToString = "Dusk";
        }

        SoundManager.Instance.PlayOneShotSound("event:/" + characterToString + "/Jump" + characterToString, transform);

        sliding = false;
    }

    void DawnBehavior() {

        if (!grounded) {
            if (canDash && dashTimer > dashCoolDown) {
                if (arrow != null) {
                    direction = PlayerUtilsStatic.UseDirectionCircle(arrow, gameObject, 0, dawn);
                } else {
                    arrow = GameObject.FindGameObjectWithTag("Arrow").GetComponent<ArrowScript>();
                }

                if (PSdawnDash1 != null) {
                    mainModuleDash1.simulationSpeed = 1;
                    emissionModuleDash1.enabled = true;
                }
                if (PSdawnDash2 != null) {
                    mainModuleDash2.simulationSpeed = 1;
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
            if (arrow != null) {
                deflectDirection = PlayerUtilsStatic.UseDirectionCircle(arrow, gameObject, 1, -10, 60, dawn);
            } else {
                arrow = GameObject.FindGameObjectWithTag("Arrow").GetComponent<ArrowScript>();
            }

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
        if (/*Input.GetMouseButtonUp(0)*/!InputManager.instance.dashButton && InputManager.instance.prevDashButton && canDash && dashTimer > dashCoolDown) {
            if (leftPressed&&!grounded) {
                GameLogic.instance.SetTimeScaleLocal(1.0f);

                //En caso de ser la dirección hacia la derecha se comprueba si la prevHorizontalMov no era 1, en caso de no serlo, significa que el personaje antes estaba 
                //mirando a la izquierda, por tanto se hace un flip y se cambia prevHorizontalMov al que corresponde.
                if (direction.x > 0) {
                    if (prevHorizontalMov <0) {
                        prevHorizontalMov = 1.0f;
                        Flip();
                    }
                    //Debug.Log("Cambio de Prev");
                }

                //En este else se hace lo mismo pero hacia la izquierda
                else if(direction.x<0){
                    if (prevHorizontalMov >0) {
                        prevHorizontalMov = -1.0f;
                        Flip();
                    }

                }
                //audioSource.pitch = 1.0f;

                //audioSource.clip = dashClip;
                //audioSource.Play();
                canJumpOnImpulsor = false;
                rb.velocity = new Vector2(0, 0);
                if (direction.x == 0 && direction.y == 0) {
                    direction.x = 0;
                    direction.y = 1;
                }


                SoundManager.Instance.PlayOneShotSound("event:/Dawn/DashEffort",transform);
                PlayerUtilsStatic.DoDash(gameObject, direction, dashForce,true);
                //control de error del sistema de particulas del release del dash
                if (PSDawnDashRelease != null) {
                    PSDawnDashRelease.Play();
                }
                else {
                    Debug.Log("El sistema de particulas del release del dash no esta referenciado");
                }
                if(PSdawnDash1 != null) {
                    emissionModuleDash1.enabled = false;
                }
                if(PSdawnDash2 != null) {
                    emissionModuleDash2.enabled = false;
                }
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
            Vector3 newSize = boxCollider.size;
            newSize.y /= 2;
            Vector3 newCenter= boxCollider.center;
            newCenter.y = newSize.y / 2;




            //GetComponent<BoxCollider2D>().size = newSize;
            //GetComponent<BoxCollider2D>().offset -= newOffset;
        }
        else if (/*Input.GetKeyUp(KeyCode.LeftControl)*/!InputManager.instance.crawlButton && InputManager.instance.prevCrawlButton) {
            //GetComponent<BoxCollider2D>().size = originalSizeCollider;
            //GetComponent<BoxCollider2D>().offset = originalOffsetCollider;

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

                g.GetComponent<Rigidbody>().velocity = new Vector2(0, 0);
                PlayerUtilsStatic.DoDash(g, deflectDirection, 20*g.GetComponent<Rigidbody>().mass/2,false);
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




        if (armstate == ARMSTATE.IDLE && grounded && leftPressed) {
            //Debug.Log("BASD");
            armstate = ARMSTATE.PUNCHCHARGE;
            currentArmTargetIndex = 0;
        }

        //Si se suelta el botón izquierdo del ratón y se habia pulsado previamente, el tiempo pasa a cero
        //En caso de estar grounded Se hace una llamada a Punch
        if (/*Input.GetMouseButtonUp(0)*/!InputManager.instance.dashButton2 && InputManager.instance.prevDashButton2 && punchTimer > punchCoolDown) {
            if (leftPressed && (armstate == ARMSTATE.IDLE || armstate == ARMSTATE.PUNCHCHARGE)) {
                GameLogic.instance.SetTimeScaleLocal(1.0f);
                if (grounded) {
                    currentArmTargetIndex = 0;
                    //Punch(direction, 40000);
                    punchTimer = 0;
                    armstate = ARMSTATE.PUNCH;
                    //punchChargeEvent.stop(STOP_MODE.ALLOWFADEOUT);
                   // SoundManager.Instance.StopEvent(punchChargeEvent);
                    //SoundManager.Instance.StopAllEvents(true);
                  //  Debug.Log("Trying to stop " + punchChargeEvent);
                    if (armTarget != null) {
                        if (facingRight) {
                            armTarget.transform.position = punchRightPositions[0].position;
                        } else {
                            armTarget.transform.position = punchLeftPositions[0].position;
                        }
                    }

                    if (punchContact != null) {
                        punchContact.direction = direction;
                        punchContact.mustPunch = true;
                    }

                    audioSource.clip = punchClip;
                    audioSource.Play();
                }

                leftPressed = false;
            }
        }
        //En caso de pulsar el botón izquierdo del ratón y estar grounded, se pone el timepo a 0.5
        else if (/*Input.GetMouseButtonDown(0)*/InputManager.instance.dashButton2 && !InputManager.instance.prevDashButton2 && grounded&&punchTimer>punchCoolDown) {
            if (!grabbing) {
                GameLogic.instance.SetTimeScaleLocal(slowMotionTimeScale);
                leftPressed = true;
            }
        }

        //Si el personaje no esta en el suelo el tiempo pasa a ser 1 en Dusk
        if (!grounded) {
            GameLogic.instance.SetTimeScaleLocal(1.0f);
            leftPressed = false;
            arrow.gameObject.SetActive(false);
        }
        //Se renderizan las flechas en caso de clicar solo si esta en el suelo
        else {
            if (!grabbing) {
                if (arrow != null) {
                    direction = PlayerUtilsStatic.UseDirectionCircle(arrow, gameObject, 0, 0, 60, dawn);
                } else {
                    arrow = GameObject.FindGameObjectWithTag("Arrow").GetComponent<ArrowScript>();
                }
            }
                if (/*Input.GetMouseButtonDown(1)*/InputManager.instance.deflectButton2 && !InputManager.instance.prevDeflectButton2) {
                    if (armstate == ARMSTATE.IDLE) {

                        foreach (GameObject g in NearbyObjects) {
                            if (g.GetComponent<DoubleObject>().isMovable) {
                                distanceToGrabbedObject = transform.position - NearbyObjects[0].transform.position;
                                grabbing = true;
                                //arm.ResetPositions();
                                armstate = ARMSTATE.GRAB;
                            currentArmTargetIndex = 0;
                            }
                        }
                    }
                } else if (/*Input.GetMouseButtonUp(1)*/!InputManager.instance.deflectButton2 && InputManager.instance.prevDeflectButton2) {
                    grabbing = false;
                armstate = ARMSTATE.IDLE;
                }
            
        }
        if (/*Input.GetKeyDown(KeyCode.LeftControl)*/InputManager.instance.crawlButton2 && !InputManager.instance.prevCrawlButton2 && !grounded) {
            Smash();
            audioSource.pitch = 1.0f;

            audioSource.clip = smashClip;
            audioSource.Play();
        }


}

//Método que pone smashing en true modificando la velocidad del personaje para que solo vaya hacia abajo
void Smash() {
        if (!smashing) {
            rb.AddForce(new Vector2(-rb.velocity.x,0),ForceMode.Impulse);
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
        RaycastHit2D hit2D = Physics2D.Raycast(rb.position - new Vector3(0f, 0.5f,0), Vector2.down, 0.2f, groundMask);
        if (hit2D) {
            //Debug.Log("HittingStuff");
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
        if (InputManager.instance != null) {
            bool someRayCastChecks = false;


            RaycastHit2D hit2D = Physics2D.Raycast(rb.position - new Vector3(0f, 0.5f,0), Vector2.down, 0.1f, groundMask);
            RaycastHit2D hit2DLeft = Physics2D.Raycast(rb.position - new Vector3(0f, 0.5f,0) + new Vector3(-distanciaBordeSprite, 0,0), Vector2.down, 0.1f, groundMask);
            RaycastHit2D hit2DRight = Physics2D.Raycast(rb.position - new Vector3(0f, 0.5f,0) + new Vector3(distanciaBordeSprite, 0,0), Vector2.down, 0.1f, groundMask);

            if (hit2D || hit2DLeft || hit2DRight) {
                someRayCastChecks = true;
            }
            if (!calledImpuslorBool) {
                Invoke("ImpulsorBool", 0.4f);
                calledImpuslorBool = true;
            }

            if (dawn) {
                //BARRA ESPACIADORA = SALTO
                if (/*Input.GetKeyDown(KeyCode.Space)*/InputManager.instance.jumpButton && !InputManager.instance.prevJumpButton && timeNotGrounded< timeNotGroundedThreshold/*grounded*/ && !crawling ||/*Input.GetKeyDown(KeyCode.Space)*/InputManager.instance.jumpButton && !InputManager.instance.prevJumpButton && timeNotSliding < 0.2f) {
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
                DawnBehavior();



                if (interactableObject != null) {
                    if (/*Input.GetKeyDown(KeyCode.E)*/InputManager.instance.interactButton && !InputManager.instance.prevInteractButton) {
                        interactableObject.Interact();
                    }
                }
            } else {
                //BARRA ESPACIADORA = SALTO
                if (/*Input.GetKeyDown(KeyCode.Space)*/InputManager.instance.jumpButton2 && !InputManager.instance.prevJumpButton2 && timeNotGrounded < timeNotGroundedThreshold/*grounded */&& !crawling ||/*Input.GetKeyDown(KeyCode.Space)*/InputManager.instance.jumpButton2 && !InputManager.instance.prevJumpButton2 && timeNotSliding < 0.2f) {
                    Jump();
                }

                if (onImpulsor) {

                    if (someRayCastChecks && /*Input.GetKeyDown(KeyCode.Space)*/InputManager.instance.jumpButton2 && !InputManager.instance.prevJumpButton2) {
                        if (!calledImpuslorBool) {
                            Invoke("ImpulsorBool", 0.2f);
                            calledImpuslorBool = true;
                        }
                        canJumpOnImpulsor = false;
                        //Debug.Log("JUMP");
                        Jump();
                    }
                }



                //Comportamiento de dusk
                DuskBehavior();


                if (interactableObject != null) {
                    if (/*Input.GetKeyDown(KeyCode.E)*/InputManager.instance.interactButton2 && !InputManager.instance.prevInteractButton2) {
                        interactableObject.Interact();
                    }
                }
            }
        }


    }

    //Método que comprueba si el objeto delante es posible de mover con un puñetazo y lo hace en consecuencia
    public void Punch(Vector2 direction, float MAX_FORCE) {
        foreach (GameObject g in NearbyObjects) {
            if (g.GetComponent<DoubleObject>().isPunchable) {
                g.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeRotation;
                g.GetComponent<Rigidbody>().AddForce(direction * MAX_FORCE, ForceMode.Impulse);
                g.GetComponent<DoubleObject>().isPunchable = false;
            }

        }

    }

    //Método de cambio, varia con respecto a los demás ya que además modifica variables especiales
    public override void Change() {
        grabbing = false;
        PlayerUtilsStatic.ResetDirectionCircle(arrow);
        leftPressed = false;
        GameLogic.instance.SetTimeScaleLocal(1.0f);

        interactableObject = null;
        //Vector2 newPosition;

        DirectionCircle.SetOnce(false);

        if (arm != null) {
            armstate = ARMSTATE.IDLE;
        }

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
                //brotherScript.rb.bodyType = RigidbodyType2D.Dynamic;
                //rb.bodyType = RigidbodyType.Kinematic;
                rb.isKinematic = true;
                brotherScript.rb.isKinematic = false;

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


                /*rb.bodyType = RigidbodyType2D.Dynamic;
                brotherScript.rb.bodyType = RigidbodyType2D.Kinematic;
                */

                rb.isKinematic = false;
                brotherScript.rb.isKinematic = true;

                brotherScript.rb.velocity = new Vector2(0.0f, 0.0f);
                rb.velocity = dominantVelocity;
            }

            //activar el shader
            maskObjectScript.change = true;
        }
    }

    //Método de carga de recursos (en este caso sprites y sonidos)
    protected override void LoadResources() {
        //imagenDawn = Resources.Load<Sprite>("Presentacion/DawnSprites/Dawn");
        //imagenDusk = Resources.Load<Sprite>("Presentacion/DuskSprites/Dusk"); ;
        //dashClip = Resources.Load<AudioClip>("Sounds/Dash");
        //jumpClip = Resources.Load<AudioClip>("Sounds/Jump");
        smashClip = Resources.Load<AudioClip>("Sounds/Smash");
        crawlClip = Resources.Load<AudioClip>("Sounds/CrawlWalk");
        //walkClip = Resources.Load<AudioClip>("Sounds/WalkLoop");
        deflectClip = Resources.Load<AudioClip>("Sounds/Deflect");
        grabClip = Resources.Load<AudioClip>("Sounds/Grab");
        slideClip = Resources.Load<AudioClip>("Sounds/Slide");
        //dieClip = Resources.Load<AudioClip>("Sounds/Die");
        punchClip = Resources.Load<AudioClip>("Sounds/Punch");
    }

    private void OnCollisionEnter(Collision collision) {
        Debug.Log(collision.gameObject);
    }

    //Método que reinicia la posición del personaje y aumenta la variable de muertes en GameLogic
    public override void Kill() {
        behindBush = false;
        sliding = false;
        grabbing = false;
        armstate = ARMSTATE.IDLE;

        GameLogic.instance.additionalOffset = new Vector3(0, 0, 0);

        if (worldAssignation == world.DUSK) {
            transform.position = GameLogic.instance.spawnPoint;
            //Debug.Log("Spawning at" + transform.position);

        } else {
            //Debug.Log(GameLogic.instance.spawnPoint);
            transform.position = GameLogic.instance.spawnPoint + new Vector3(0, GameLogic.instance.worldOffset, 0);
            //Debug.Log("Spawning at" + transform.position);

        }

        if (rb != null) {
            rb.velocity = new Vector2(0, 0);
        }
        //audioSource.clip = dieClip;
        if (worldAssignation == world.DAWN) {
            SoundManager.Instance.PlayOneShotSound("event:/Dawn/Death",transform.position);
        } else {
            SoundManager.Instance.PlayOneShotSound("event:/Dusk/Death", transform.position);
        }


        //audioSource.Play();
        GameLogic.instance.timesDied++;
    }

}
