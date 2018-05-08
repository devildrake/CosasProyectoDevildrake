using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class InputManager : MonoBehaviour {
    public static InputManager instance = null;

    public enum GAMEMODE { SINGLEPLAYER, MULTI_KEYBOARD_CONTROLLER, MULTI_CONTROLLER_KEYBOARD, MULTI_CONTROLLER_CONTROLLER};
    public static GAMEMODE currentGameMode = GAMEMODE.SINGLEPLAYER;

    [SerializeField]
    private static bool blocked;
    float comprovarMandoTimer=3.0f;

    bool[] PS4_Controllers = { false, false };
    bool[] Xbox_One_Controllers = { false, false };
    bool[] Windows_Controllers = { false, false };
    string[] names;
    List<string> namesList = new List<string>();

    public static bool gamePadConnected;

    [HideInInspector]
    public bool prevDashButton;
    [HideInInspector]
    public bool prevDeflectButton;
    [HideInInspector]
    public bool prevCrawlButton;
    [HideInInspector]
    public bool prevJumpButton;
    //[HideInInspector]
    public bool prevChangeButton;
    [HideInInspector]
    public bool prevPauseButton;
    [HideInInspector]
    public bool prevSelectButton;
    [HideInInspector]
    public bool prevInteractButton;
    [HideInInspector]
    public bool prevResetButton;
    [HideInInspector]
    public bool prevCameraButton;
    [HideInInspector]
    public float prevHorizontalAxis;
    [HideInInspector]
    public float prevVerticalAxis;
    [HideInInspector]
    public float prevRightHorizontalAxis;
    [HideInInspector]
    public float prevRightVerticalAxis;
    [HideInInspector]
    public bool prevAnyKeyDown;
    [HideInInspector]
    public bool prevCancelButton;

    [HideInInspector]
    public bool prevDashButton2;
    [HideInInspector]
    public bool prevDeflectButton2;
    [HideInInspector]
    public bool prevCrawlButton2;
    [HideInInspector]
    public bool prevJumpButton2;
    //[HideInInspector]
    public bool prevChangeButton2;
    [HideInInspector]
    public bool prevPauseButton2;
    [HideInInspector]
    public bool prevSelectButton2;
    [HideInInspector]
    public bool prevInteractButton2;
    [HideInInspector]
    public bool prevResetButton2;
    [HideInInspector]
    public bool prevCameraButton2;
    [HideInInspector]
    public float prevHorizontalAxis2;
    [HideInInspector]
    public float prevVerticalAxis2;
    [HideInInspector]
    public float prevRightHorizontalAxis2;
    [HideInInspector]
    public float prevRightVerticalAxis2;
    [HideInInspector]
    public bool prevAnyKeyDown2;
    [HideInInspector]
    public bool prevCancelButton2;
    [HideInInspector]
    public bool prevUpKey;
    [HideInInspector]
    public bool prevDownKey;
    [HideInInspector]
    public bool prevRightKey;
    [HideInInspector]
    public bool prevLeftKey;
    //[HideInInspector]
    //public bool prevDashButtonPlayer;
    //[HideInInspector]
    //public bool prevDeflectButtonPlayer;
    public bool leftKey;
    public bool rightKey;
    public bool upKey;
    public bool downKey;

    public bool cancelButton;
    public bool dashButton;
    public bool deflectButton;
    public bool crawlButton;
    public bool jumpButton;
    public bool changeButton;
    public bool pauseButton;
    public bool selectButton;
    public bool interactButton;
    public bool resetButton;
    public bool cameraButton;
    public float horizontalAxis;
    public float verticalAxis;
    public float rightHorizontalAxis;
    public float rightVerticalAxis;

    public bool cancelButton2;
    public bool dashButton2;
    public bool deflectButton2;
    public bool crawlButton2;
    public bool jumpButton2;
    public bool changeButton2;
    public bool pauseButton2;
    public bool selectButton2;
    public bool interactButton2;
    public bool resetButton2;
    public bool cameraButton2;
    public float horizontalAxis2;
    public float verticalAxis2;
    public float rightHorizontalAxis2;
    public float rightVerticalAxis2;


    //public bool dashButtonPlayer;
    //public bool deflectButtonPlayer;


    private void Awake() {
        if (instance == null) {


            //if not, set instance to this
            instance = this;
            DontDestroyOnLoad(gameObject);

        }
        //If instance already exists and it's not this:
        else if (instance != this) {

            //Then destroy this. This enforces our singleton pattern, meaning there can only ever be one instance of a GameManager.
            Destroy(gameObject);
        }


        BlockInput();


        

    }

    public static bool GetBlocked() {
        return blocked;
    }

    public static void BlockInput() {

        instance.dashButton = false;
        instance.deflectButton = false;
        instance.crawlButton = false;
        instance.jumpButton = false;
        instance.pauseButton = false;
        instance.changeButton = false;
        instance.selectButton = false;
        instance.interactButton = false;
        instance.resetButton = false;
        instance.cancelButton = false;
        instance.horizontalAxis = 0;
        instance.verticalAxis = 0;
        instance.rightHorizontalAxis = 0;
        instance.rightVerticalAxis = 0;

        instance.prevDashButton = false;
        instance.prevDeflectButton = false;
        instance.prevCrawlButton = false;
        instance.prevJumpButton = false;
        instance.prevChangeButton = false;
        instance.prevPauseButton = false;
        instance.prevSelectButton = false;
        instance.prevInteractButton = false;
        instance.prevResetButton = false;
        instance.prevCancelButton = false;
        instance.prevHorizontalAxis = 0;
        instance.prevVerticalAxis = 0;
        instance.prevRightHorizontalAxis = 0;
        instance.prevRightVerticalAxis = 0;
        instance.prevAnyKeyDown = false;

        instance.dashButton2 = false;
        instance.deflectButton2 = false;
        instance.crawlButton2 = false;
        instance.jumpButton2 = false;
        instance.pauseButton2 = false;
        instance.changeButton2 = false;
        instance.selectButton2 = false;
        instance.interactButton2 = false;
        instance.resetButton2 = false;
        instance.cancelButton2 = false;
        instance.horizontalAxis2 = 0;
        instance.verticalAxis2 = 0;
        instance.rightHorizontalAxis2 = 0;
        instance.rightVerticalAxis2 = 0;

        instance.prevDashButton2 = false;
        instance.prevDeflectButton2 = false;
        instance.prevCrawlButton2 = false;
        instance.prevJumpButton2 = false;
        instance.prevChangeButton2 = false;
        instance.prevPauseButton2 = false;
        instance.prevSelectButton2 = false;
        instance.prevInteractButton2 = false;
        instance.prevResetButton2 = false;
        instance.prevCancelButton2 = false;

        instance.prevUpKey = false;
        instance.prevDownKey = false;
        instance.prevRightKey = false;
        instance.prevLeftKey = false;

        instance.prevHorizontalAxis2 = 0;
        instance.prevVerticalAxis2 = 0;
        instance.prevRightHorizontalAxis2 = 0;
        instance.prevRightVerticalAxis2 = 0;

        blocked = true;
    }

    public static void UnBlockInput() {
        blocked = false;
    }

    public void UpdatePrevious() {
        if (!blocked) {
            instance.prevAnyKeyDown = Input.anyKey;
            instance.prevCrawlButton = instance.crawlButton;
            instance.prevJumpButton = instance.jumpButton;
            instance.prevInteractButton = instance.interactButton;
            instance.prevHorizontalAxis = instance.horizontalAxis;
            instance.prevRightHorizontalAxis = instance.rightHorizontalAxis;
            instance.prevRightVerticalAxis = instance.rightVerticalAxis;
            //prevDeflectButtonPlayer = deflectButtonPlayer;
            //prevDashButtonPlayer = dashButtonPlayer;

            instance.prevUpKey = instance.upKey;
            instance.prevDownKey = instance.downKey;
            instance.prevRightKey = instance.rightKey;
            instance.prevLeftKey = instance.leftKey; 

            instance.prevCameraButton = instance.cameraButton;

            instance.prevDashButton = instance.dashButton;
            instance.prevDeflectButton = instance.deflectButton;

            instance.prevSelectButton = instance.selectButton;
            instance.prevChangeButton = instance.changeButton;
            instance.prevPauseButton = instance.pauseButton;
            instance.prevVerticalAxis = instance.verticalAxis;
            instance.prevCancelButton = instance.cancelButton;

            instance.prevCrawlButton2 = instance.crawlButton2;
            instance.prevJumpButton2 = instance.jumpButton2;
            instance.prevInteractButton2 = instance.interactButton2;
            instance.prevHorizontalAxis2 = instance.horizontalAxis2;
            instance.prevRightHorizontalAxis2 = instance.rightHorizontalAxis2;
            instance.prevRightVerticalAxis2 = instance.rightVerticalAxis2;
            instance.prevCameraButton2 = instance.cameraButton2;
            instance.prevDashButton2 = instance.dashButton2;
            instance.prevDeflectButton2 = instance.deflectButton2;
            instance.prevSelectButton2 = instance.selectButton2;
            instance.prevChangeButton2 = instance.changeButton2;
            instance.prevPauseButton2 = instance.pauseButton2;
            instance.prevVerticalAxis2 = instance.verticalAxis2;
            instance.prevCancelButton2 = instance.cancelButton2;
        }

    }

    //public void UpdatePreviousCamera() {
    //    //prevCameraButton = cameraButton;
    //}

    //public void UpdatePreviousUtils() {
    //    //prevDashButton = dashButton;
    //    //prevDeflectButton = deflectButton;
    //}

    //public void UpdatePreviousPlayer() {
    //    if (!blocked) {
    //        //prevCrawlButton = crawlButton;
    //        //prevJumpButton = jumpButton;
    //        //prevInteractButton = interactButton;
    //        //prevHorizontalAxis = horizontalAxis;
    //        //prevRightHorizontalAxis = rightHorizontalAxis;
    //        //prevRightVerticalAxis = rightVerticalAxis;
    //        //prevDeflectButtonPlayer = deflectButtonPlayer;
    //        //prevDashButtonPlayer = dashButtonPlayer;
    //    }
    //}

    // Update is called once per frame
    void Update() {

        if (comprovarMandoTimer > 3.0f) {
            comprovarMandoTimer = 0;
            names = Input.GetJoystickNames();
            namesList.Clear();
            for (int i = 0; i < names.Length; i++) {
                if (names[i].Length > 0) {
                    namesList.Add(names[i]);
                    Debug.Log("DETECTADO : " + names[i]);
                }
            }

            for (int x = 0; x < namesList.Count; x++) {
                //Debug.Log(namesList[x]);
            }

            for (int x = 0; x < namesList.Count; x++) {
                //print(names[x].Length);
                if (namesList[x].Length == 19) {
                    if (x == 0) {
                        PS4_Controllers[0] = true;
                        Xbox_One_Controllers[0] = false;
                        Windows_Controllers[0] = false;
                       // print("PS4 CONTROLLER1 IS CONNECTED");
                    } else if (x == 1) {
                        PS4_Controllers[1] = true;
                        Xbox_One_Controllers[1] = false;
                        Windows_Controllers[1] = false;
                       // print("PS4 CONTROLLER2 IS CONNECTED");
                    }
                }
                if (namesList[x].Length == 33 || namesList[x].Length == 24) {
                    if (x == 0) {
                        Xbox_One_Controllers[0] = true;
                       /// print("XBOX ONE CONTROLLER1 IS CONNECTED");
                    } else if (x == 1) {
                        Xbox_One_Controllers[1] = true;
                        ///print("XBOX ONE CONTROLLER2 IS CONNECTED");
                    }
                }
                if (namesList[x].Length == 31) {
                    if (x == 0) {
                        //print("WINDOWS CONTROLLER1 IS CONNECTED");
                        Windows_Controllers[0] = true;
                    } else if (x == 1) {
                        Windows_Controllers[1] = true;
                        //print("WINDOWS CONTROLLER2 IS CONNECTED");
                    }
                }
            }
        } else {
            comprovarMandoTimer += Time.deltaTime;
        }

        UpdatePrevious();



        /*SE ACTUALIZAN LAS VARIABLES NORMALES Y LAS QUE TIENEN UN 2 DESTRÁS PORQUE EN PLAYERCONTROLLER
         * SE UTILIZAN AMBAS, LO QUE OCURRE ES QUE SI ES SINGLEPLAYER, ESTE ACTUALIZA AMBAS Y SI NO
         * SE VARIA EL QUIEN LAS ACTUALIZA*/
        switch (currentGameMode) {


                case GAMEMODE.SINGLEPLAYER:
                if (Xbox_One_Controllers[0]|| PS4_Controllers[0] ||Windows_Controllers[0]) {
                    gamePadConnected = true;

                } else {
                    gamePadConnected = false;
                }

                if (!blocked) {


                    if (!Xbox_One_Controllers[0] && !PS4_Controllers[0]&&!Windows_Controllers[0]) {

                        instance.dashButton = (Input.GetAxisRaw("DashPunch") == 1.0);
                        //dashButtonPlayer = (Input.GetAxisRaw("DashPunch") == 1.0);

                        instance.deflectButton = (Input.GetAxisRaw("DeflectDrag") == 1.0);
                        //deflectButtonPlayer = (Input.GetAxisRaw("DeflectDrag") == 1.0);
                        instance.crawlButton = (Input.GetAxisRaw("CrawlSmash") == 1.0);
                        instance.jumpButton = (Input.GetAxisRaw("Jump") == 1.0);

                        instance.pauseButton = (Input.GetAxisRaw("Pause") == 1.0);
                        instance.selectButton = (Input.GetAxisRaw("Select") == 1.0);
                        instance.resetButton = Input.GetAxisRaw("Reset") == 1.0;
                        instance.interactButton = (Input.GetAxisRaw("Interact") == 1.0);
                        instance.cameraButton = Input.GetAxisRaw("Camera") == 1.0;
                        instance.cancelButton = Input.GetAxisRaw("Cancel") == 1.0;
                        instance.changeButton = (Input.GetAxisRaw("Change") == 1.0);
                        instance.horizontalAxis = Input.GetAxisRaw("Horizontal");
                        instance.verticalAxis = (Input.GetAxisRaw("Vertical"));
                        /////////////////////////////////////////////////////////////////////////////
                        /////////////////////////////////////////////////////////////////////////////
                        /////////////////////////////////////////////////////////////////////////////
                        /////////////////////////////////////////////////////////////////////////////
                        instance.dashButton2 = (Input.GetAxisRaw("DashPunch") == 1.0);
                        instance.deflectButton2 = (Input.GetAxisRaw("DeflectDrag") == 1.0);
                        instance.crawlButton2 = (Input.GetAxisRaw("CrawlSmash") == 1.0);
                        instance.jumpButton2 = (Input.GetAxisRaw("Jump") == 1.0);
                        instance.pauseButton2 = (Input.GetAxisRaw("Pause") == 1.0);
                        instance.selectButton2 = (Input.GetAxisRaw("Select") == 1.0);
                        instance.resetButton2 = Input.GetAxisRaw("Reset") == 1.0;
                        instance.interactButton2 = (Input.GetAxisRaw("Interact") == 1.0);
                        instance.cameraButton2 = Input.GetAxisRaw("Camera") == 1.0;
                        instance.cancelButton2 = Input.GetAxisRaw("Cancel") == 1.0;
                        instance.changeButton2 = (Input.GetAxisRaw("Change") == 1.0);
                        instance.horizontalAxis2 = Input.GetAxisRaw("Horizontal");
                        instance.verticalAxis2 = (Input.GetAxisRaw("Vertical"));

                    } else {


                        //for (int i = 0; i < 20; i++) {
                        //    if (Input.GetKeyDown("joystick 1 button " + i)) {
                        //        print("joystick 1 button " + i);
                        //    }
                        //}
                        //for (int i = 0; i < 20; i++) {
                        //    if (Input.GetKeyDown("joystick 2 button " + i)) {
                        //        print("joystick 2 button " + i);
                        //    }
                        //}

                        //Debug.Log("MANDO");
                        //dashButtonPlayer = (Input.GetAxisRaw("DashPunch") == 1.0);

                        instance.deflectButton = (Input.GetAxisRaw("DeflectDrag1") == 1.0);
                        //deflectButtonPlayer = (Input.GetAxisRaw("DeflectDrag") == 1.0);
                        instance.crawlButton = (Input.GetAxisRaw("CrawlSmash1") == 1.0);
                        instance.jumpButton = (Input.GetAxisRaw("Jump1") == 1.0);
                        instance.upKey = Input.GetAxisRaw("ControllerUpArrow") == 1.0;
                        instance.downKey = Input.GetAxisRaw("ControllerDownArrow") == -1.0;
                        instance.leftKey = Input.GetAxisRaw("ControllerLeftArrow") == -1.0;
                        instance.rightKey = Input.GetAxisRaw("ControllerRightArrow") == 1.0;


                        if (instance.upKey&&!instance.prevUpKey) {
                            Debug.Log("UP");
                        } else if (instance.downKey&&!instance.prevDownKey) {
                            Debug.Log("DOWN");
                        }
                        if (instance.rightKey && !instance.prevRightKey) {
                            Debug.Log("RIGHT");
                        }else if (instance.leftKey && !instance.prevLeftKey) {
                            Debug.Log("LEFT");
                        }



                        instance.pauseButton = (Input.GetAxisRaw("Pause1") == 1.0);
                        instance.selectButton = (Input.GetAxisRaw("Select1") == 1.0);
                        instance.resetButton = Input.GetAxisRaw("Reset1") == 1.0;
                        instance.interactButton = (Input.GetAxisRaw("Interact1") == 1.0);
                        instance.cameraButton = Input.GetAxisRaw("Camera1") == 1.0;
                        instance.cancelButton = Input.GetAxisRaw("Cancel1") == 1.0;
                        instance.horizontalAxis = Input.GetAxisRaw("HorizontalPad1");
                        instance.verticalAxis = Input.GetAxisRaw("VerticalPad1");
                        instance.rightHorizontalAxis = Input.GetAxisRaw("RightJoyStickHorizontal1");
                        instance.rightVerticalAxis = Input.GetAxisRaw("RightJoyStickVertical1");
                        instance.dashButton = (Input.GetAxisRaw("ChangePad1") < 0);
                        instance.changeButton = (Input.GetAxisRaw("ChangePad1") > 0);
                        /////////////////////////////////////////////////////////////////////////////
                        /////////////////////////////////////////////////////////////////////////////
                        /////////////////////////////////////////////////////////////////////////////
                        /////////////////////////////////////////////////////////////////////////////
                        instance.deflectButton2 = (Input.GetAxisRaw("DeflectDrag1") == 1.0);
                        instance.crawlButton2 = (Input.GetAxisRaw("CrawlSmash1") == 1.0);
                        instance.jumpButton2 = (Input.GetAxisRaw("Jump1") == 1.0);
                        instance.pauseButton2 = (Input.GetAxisRaw("Pause1") == 1.0);
                        instance.selectButton2 = (Input.GetAxisRaw("Select1") == 1.0);
                        instance.resetButton2 = Input.GetAxisRaw("Reset1") == 1.0;
                        instance.interactButton2 = (Input.GetAxisRaw("Interact1") == 1.0);
                        instance.cameraButton2 = Input.GetAxisRaw("Camera1") == 1.0;
                        instance.cancelButton2 = Input.GetAxisRaw("Cancel1") == 1.0;
                        instance.horizontalAxis2 = Input.GetAxisRaw("HorizontalPad1");
                        instance.verticalAxis2 = Input.GetAxisRaw("VerticalPad1");
                        instance.rightHorizontalAxis2 = Input.GetAxisRaw("RightJoyStickHorizontal1");
                        instance.rightVerticalAxis2 = Input.GetAxisRaw("RightJoyStickVertical1");
                        instance.dashButton2 = (Input.GetAxisRaw("ChangePad1") < 0);
                        instance.changeButton2 = (Input.GetAxisRaw("ChangePad1") > 0);


                        //Debug.Log("CHANGEPAD " + Input.GetAxisRaw("ChangePad1"));

                        //dashButtonPlayer = (Input.GetAxisRaw("ChangePad") == -1.0);


                    }
                }
                break;
            case GAMEMODE.MULTI_KEYBOARD_CONTROLLER:
                gamePadConnected = true;

                if (!blocked) {
                    instance.upKey = Input.GetAxisRaw("ControllerUpArrow") == 1.0;
                    instance.downKey = Input.GetAxisRaw("ControllerDownArrow") == -1.0;
                    instance.leftKey = Input.GetAxisRaw("ControllerLeftArrow") == -1.0;
                    instance.rightKey = Input.GetAxisRaw("ControllerRightArrow") == 1.0;
                    ///////////////PLAYER 1 (KEYBOARD)/////////////////
                    ///////////////PLAYER 1 (KEYBOARD)/////////////////
                    ///////////////PLAYER 1 (KEYBOARD)/////////////////
                    ///////////////PLAYER 1 (KEYBOARD)/////////////////
                    instance.dashButton = (Input.GetAxisRaw("DashPunch") == 1.0);
                    instance.deflectButton = (Input.GetAxisRaw("DeflectDrag") == 1.0);
                    instance.crawlButton = (Input.GetAxisRaw("CrawlSmash") == 1.0);
                    instance.jumpButton = (Input.GetAxisRaw("Jump") == 1.0);

                    instance.pauseButton = (Input.GetAxisRaw("Pause") == 1.0);
                    instance.selectButton = (Input.GetAxisRaw("Select") == 1.0);
                    instance.resetButton = Input.GetAxisRaw("Reset") == 1.0;
                    instance.interactButton = (Input.GetAxisRaw("Interact") == 1.0);
                    instance.cameraButton = Input.GetAxisRaw("Camera") == 1.0;
                    instance.cancelButton = Input.GetAxisRaw("Cancel") == 1.0;
                    instance.changeButton = (Input.GetAxisRaw("Change") == 1.0);
                    instance.horizontalAxis = Input.GetAxisRaw("Horizontal");
                    instance.verticalAxis = (Input.GetAxisRaw("Vertical"));
                    ///////////////PLAYER 1 (KEYBOARD)/////////////////
                    ///////////////PLAYER 1 (KEYBOARD)/////////////////
                    ///////////////PLAYER 1 (KEYBOARD)/////////////////
                    ///////////////PLAYER 1 (KEYBOARD)/////////////////

                    ///////////////PLAYER 2 (MANDO)/////////////////
                    ///////////////PLAYER 2 (MANDO)/////////////////
                    ///////////////PLAYER 2 (MANDO)/////////////////
                    ///////////////PLAYER 2 (MANDO)/////////////////

                    instance.deflectButton2 = (Input.GetAxisRaw("DeflectDrag1") == 1.0);
                    instance.crawlButton2 = (Input.GetAxisRaw("CrawlSmash1") == 1.0);
                    instance.jumpButton2 = (Input.GetAxisRaw("Jump1") == 1.0);
                    instance.pauseButton2 = (Input.GetAxisRaw("Pause1") == 1.0);
                    instance.selectButton2 = (Input.GetAxisRaw("Select1") == 1.0);
                    instance.resetButton2 = Input.GetAxisRaw("Reset1") == 1.0;
                    instance.interactButton2 = (Input.GetAxisRaw("Interact1") == 1.0);
                    instance.cameraButton2 = Input.GetAxisRaw("Camera1") == 1.0;
                    instance.cancelButton2 = Input.GetAxisRaw("Cancel1") == 1.0;
                    instance.horizontalAxis2 = Input.GetAxisRaw("HorizontalPad1");
                    instance.verticalAxis2 = Input.GetAxisRaw("VerticalPad1");
                    instance.rightHorizontalAxis2 = Input.GetAxisRaw("RightJoyStickHorizontal1");
                    instance.rightVerticalAxis2 = Input.GetAxisRaw("RightJoyStickVertical1");
                    instance.dashButton2 = (Input.GetAxisRaw("ChangePad1") <0);
                    instance.changeButton2 = (Input.GetAxisRaw("ChangePad1") >0);

                    ///////////////PLAYER 2 (MANDO)/////////////////
                    ///////////////PLAYER 2 (MANDO)/////////////////
                    ///////////////PLAYER 2 (MANDO)/////////////////
                    ///////////////PLAYER 2 (MANDO)/////////////////



                }
                break;
            case GAMEMODE.MULTI_CONTROLLER_KEYBOARD:
                gamePadConnected = true;

                if (!blocked) {
                    ///////////////PLAYER 1 (MANDO)/////////////////
                    ///////////////PLAYER 1 (MANDO)/////////////////
                    ///////////////PLAYER 1 (MANDO)/////////////////
                    ///////////////PLAYER 1 (MANDO)/////////////////
                    instance.deflectButton = (Input.GetAxisRaw("DeflectDrag1") == 1.0);
                    instance.crawlButton = (Input.GetAxisRaw("CrawlSmash1") == 1.0);
                    instance.jumpButton = (Input.GetAxisRaw("Jump1") == 1.0);
                    instance.pauseButton = (Input.GetAxisRaw("Pause1") == 1.0);
                    instance.selectButton = (Input.GetAxisRaw("Select1") == 1.0);
                    instance.resetButton = Input.GetAxisRaw("Reset1") == 1.0;
                    instance.interactButton = (Input.GetAxisRaw("Interact1") == 1.0);
                    instance.cameraButton = Input.GetAxisRaw("Camera1") == 1.0;
                    instance.cancelButton = Input.GetAxisRaw("Cancel1") == 1.0;
                    instance.horizontalAxis = Input.GetAxisRaw("HorizontalPad1");
                    instance.verticalAxis = Input.GetAxisRaw("VerticalPad1");
                    instance.rightHorizontalAxis = Input.GetAxisRaw("RightJoyStickHorizontal1");
                    instance.rightVerticalAxis = Input.GetAxisRaw("RightJoyStickVertical1");
                    instance.dashButton = (Input.GetAxisRaw("ChangePad1") < 0);
                    instance.changeButton = (Input.GetAxisRaw("ChangePad1") > 0);
                    ///////////////PLAYER 1 (MANDO)/////////////////
                    ///////////////PLAYER 1 (MANDO)/////////////////
                    ///////////////PLAYER 1 (MANDO)/////////////////
                    ///////////////PLAYER 1 (MANDO)/////////////////

                    ///////////////PLAYER 2 (KEYBOARD)///////////////////
                    ///////////////PLAYER 2 ((KEYBOARD))/////////////////
                    ///////////////PLAYER 2 ((KEYBOARD))/////////////////
                    ///////////////PLAYER 2 ((KEYBOARD))/////////////////
                    instance.dashButton2 = (Input.GetAxisRaw("DashPunch") == 1.0);
                    instance.deflectButton2 = (Input.GetAxisRaw("DeflectDrag") == 1.0);
                    instance.crawlButton2 = (Input.GetAxisRaw("CrawlSmash") == 1.0);
                    instance.jumpButton2 = (Input.GetAxisRaw("Jump") == 1.0);
                    instance.pauseButton2 = (Input.GetAxisRaw("Pause") == 1.0);
                    instance.selectButton2 = (Input.GetAxisRaw("Select") == 1.0);
                    instance.resetButton2 = Input.GetAxisRaw("Reset") == 1.0;
                    instance.interactButton2 = (Input.GetAxisRaw("Interact") == 1.0);
                    instance.cameraButton2 = Input.GetAxisRaw("Camera") == 1.0;
                    instance.cancelButton2 = Input.GetAxisRaw("Cancel") == 1.0;
                    instance.changeButton2 = (Input.GetAxisRaw("Change") == 1.0);
                    instance.horizontalAxis2 = Input.GetAxisRaw("Horizontal");
                    instance.verticalAxis2 = (Input.GetAxisRaw("Vertical"));
                    instance.upKey = Input.GetAxisRaw("ControllerUpArrow") == 1.0;
                    instance.downKey = Input.GetAxisRaw("ControllerDownArrow") == -1.0;
                    instance.leftKey = Input.GetAxisRaw("ControllerLeftArrow") == -1.0;
                    instance.rightKey = Input.GetAxisRaw("ControllerRightArrow") == 1.0;
                    ///////////////PLAYER 2 (KEYBOARD)///////////////////
                    ///////////////PLAYER 2 ((KEYBOARD))/////////////////
                    ///////////////PLAYER 2 ((KEYBOARD))/////////////////
                    ///////////////PLAYER 2 ((KEYBOARD))/////////////////
                }
                break;
            case GAMEMODE.MULTI_CONTROLLER_CONTROLLER:
                gamePadConnected = true;
                if (!blocked) {

                    //for (int i = 0; i < 20; i++) {
                    //    if (Input.GetKeyDown("joystick 1 button " + i)) {
                    //        print("joystick 1 button " + i);
                    //    }
                    //}
                    //for (int i = 0; i < 20; i++) {
                    //    if (Input.GetKeyDown("joystick 2 button " + i)) {
                    //        print("joystick 2 button " + i);
                    //    }
                    //}


                    ///////////////PLAYER 1 (MANDO)/////////////////
                    ///////////////PLAYER 1 (MANDO)/////////////////
                    ///////////////PLAYER 1 (MANDO)/////////////////
                    ///////////////PLAYER 1 (MANDO)/////////////////
                    //instance.deflectButton = (Input.GetAxisRaw("DeflectDrag1") == 1.0);

                    instance.deflectButton = Input.GetKeyDown("joystick 1 button 5");

                    instance.crawlButton = (Input.GetAxisRaw("CrawlSmash1") == 1.0);
                    //instance.jumpButton = (Input.GetAxisRaw("Jump1") == 1.0);
                    instance.jumpButton = Input.GetKeyDown("joystick 1 button 0");
                    instance.pauseButton = (Input.GetAxisRaw("Pause1") == 1.0);
                    instance.selectButton = (Input.GetAxisRaw("Select1") == 1.0);
                    instance.resetButton = Input.GetAxisRaw("Reset1") == 1.0;
                    //instance.interactButton = (Input.GetAxisRaw("Interact1") == 1.0);

                    instance.interactButton = Input.GetKeyDown("joystick 1 button 1");

                    //instance.cameraButton = Input.GetAxisRaw("Camera1") == 1.0;
                    instance.cameraButton = Input.GetKeyDown("joystick 1 button 3");

                    instance.cancelButton = Input.GetAxisRaw("Cancel1") == 1.0;
                    instance.horizontalAxis = Input.GetAxisRaw("HorizontalPad1");
                    instance.verticalAxis = Input.GetAxisRaw("VerticalPad1");
                    instance.rightHorizontalAxis = Input.GetAxisRaw("RightJoyStickHorizontal1");
                    instance.rightVerticalAxis = Input.GetAxisRaw("RightJoyStickVertical1");
                    instance.dashButton = (Input.GetAxisRaw("ChangePad1") < 0);
                    instance.changeButton = (Input.GetAxisRaw("ChangePad1") > 0);
                    ///////////////PLAYER 1 (MANDO)/////////////////
                    ///////////////PLAYER 1 (MANDO)/////////////////
                    ///////////////PLAYER 1 (MANDO)/////////////////
                    ///////////////PLAYER 1 (MANDO)/////////////////


                    ///////////////PLAYER 2 (MANDO)/////////////////
                    ///////////////PLAYER 2 (MANDO)/////////////////
                    ///////////////PLAYER 2 (MANDO)/////////////////
                    ///////////////PLAYER 2 (MANDO)/////////////////
                    //instance.deflectButton2 = (Input.GetAxisRaw("DeflectDrag2") == 1.0);

                    instance.deflectButton2 = Input.GetKeyDown("joystick 2 button 5");

                    instance.crawlButton2 = (Input.GetAxisRaw("CrawlSmash2") == 1.0);
                    //instance.jumpButton2 = (Input.GetAxisRaw("Jump2") == 1.0);
                    instance.jumpButton2 = Input.GetKeyDown("joystick 2 button 0");



                    instance.pauseButton2 = (Input.GetAxisRaw("Pause2") == 1.0);
                    instance.selectButton2 = (Input.GetAxisRaw("Select2") == 1.0);
                    instance.resetButton2 = Input.GetAxisRaw("Reset2") == 1.0;

                    instance.interactButton2 = Input.GetKeyDown("joystick 2 button 1");

                    //instance.interactButton2 = (Input.GetAxisRaw("Interact2") == 1.0);
                    //instance.cameraButton2 = Input.GetAxisRaw("Camera2") == 1.0;

                    instance.cameraButton2 = Input.GetKeyDown("joystick 2 button 3");

                    instance.interactButton2 = Input.GetKeyDown("joystick 2 button 1");


                    instance.cancelButton2 = Input.GetAxisRaw("Cancel2") == 1.0;
                    instance.horizontalAxis2 = Input.GetAxisRaw("HorizontalPad2");
                    instance.verticalAxis2 = Input.GetAxisRaw("VerticalPad2");
                    instance.rightHorizontalAxis2 = Input.GetAxisRaw("RightJoyStickHorizontal2");
                    instance.rightVerticalAxis2 = Input.GetAxisRaw("RightJoyStickVertical2");
                    instance.dashButton2 = (Input.GetAxisRaw("ChangePad2") < 0);
                    instance.changeButton2 = (Input.GetAxisRaw("ChangePad2") > 0);
                    instance.upKey = Input.GetAxisRaw("ControllerUpArrow") == 1.0;
                    instance.downKey = Input.GetAxisRaw("ControllerDownArrow") == -1.0;
                    instance.leftKey = Input.GetAxisRaw("ControllerLeftArrow") == -1.0;
                    instance.rightKey = Input.GetAxisRaw("ControllerRightArrow") == 1.0;
                    ///////////////PLAYER 2 (MANDO)/////////////////
                    ///////////////PLAYER 2 (MANDO)/////////////////
                    ///////////////PLAYER 2 (MANDO)/////////////////
                    ///////////////PLAYER 2 (MANDO)/////////////////
                }
                break;
        }
    }
}
