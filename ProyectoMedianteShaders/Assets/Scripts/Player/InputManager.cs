using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour {
    public static InputManager instance = null;

    [SerializeField]
    private static bool blocked;

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
    //[HideInInspector]
    //public bool prevDashButtonPlayer;
    //[HideInInspector]
    //public bool prevDeflectButtonPlayer;





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

    // Use this for initialization
    void Start () {
		
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
        instance.horizontalAxis = 0;
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
        instance.prevHorizontalAxis = 0;
        instance.prevRightHorizontalAxis = 0;
        instance.prevRightVerticalAxis = 0;
        instance.prevAnyKeyDown = false;

        blocked = true;
    }

    public static void UnBlockInput() {
        blocked = false;
    }

    //public void UpdatePreviousGameLogic() {
    //    //prevSelectButton = selectButton;
    //    //prevChangeButton = changeButton;
    //    //prevPauseButton = pauseButton;

    //}

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
        bool PS4_Controller = false;
        bool Xbox_One_Controller = false;
        string[] names = Input.GetJoystickNames();


        for (int x = 0; x < names.Length; x++) {
            //print(names[x].Length);
            if (names[x].Length == 19) {
                print("PS4 CONTROLLER IS CONNECTED");
                PS4_Controller = true;
                Xbox_One_Controller = false;
            }
            if (names[x].Length == 33 || names[x].Length == 24) {
                print("XBOX ONE CONTROLLER IS CONNECTED");
                //set a controller bool to true
                PS4_Controller = false;
                Xbox_One_Controller = true;

            }
        }


        if (Xbox_One_Controller|| PS4_Controller) {
            InputManager.gamePadConnected = true;
            //Debug.Log("TRUE");

        } else {

            InputManager.gamePadConnected = false;
            //Debug.Log("FALSE");
        }

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


            instance.prevCameraButton = instance.cameraButton;

            instance.prevDashButton = instance.dashButton;
            instance.prevDeflectButton = instance.deflectButton;

            instance.prevSelectButton = instance.selectButton;
            instance.prevChangeButton = instance.changeButton;
            instance.prevPauseButton = instance.pauseButton;

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




            if (!Xbox_One_Controller && !PS4_Controller) {
                instance.changeButton = (Input.GetAxisRaw("Change") == 1.0);
                instance.horizontalAxis = Input.GetAxisRaw("Horizontal");
                instance.verticalAxis = (Input.GetAxisRaw("Vertical"));

            } else {
                instance.horizontalAxis = Input.GetAxisRaw("HorizontalPad");
                instance.verticalAxis =Input.GetAxisRaw("VerticalPad");
                instance.rightHorizontalAxis = Input.GetAxisRaw("RightJoyStickHorizontal");
                instance.rightVerticalAxis = Input.GetAxisRaw("RightJoyStickVertical");
                instance.dashButton = (Input.GetAxisRaw("ChangePad") == -1.0);
                instance.changeButton = (Input.GetAxisRaw("ChangePad") == 1);
                //dashButtonPlayer = (Input.GetAxisRaw("ChangePad") == -1.0);


            }
        }
    }
}
