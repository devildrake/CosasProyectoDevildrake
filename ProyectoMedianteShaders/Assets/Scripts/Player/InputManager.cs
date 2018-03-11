using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour {
    public static InputManager instance = null;

    [SerializeField]
    private static bool blocked;

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
    public float prevHorizontalAxis;

    public bool dashButton;
    public bool deflectButton;
    public bool crawlButton;
    public bool jumpButton;
    public bool changeButton;
    public bool pauseButton;
    public bool selectButton;
    public bool interactButton;
    public bool resetButton;
    public float horizontalAxis;


    private void Awake() {
        if (instance == null)

            //if not, set instance to this
            instance = this;

        //If instance already exists and it's not this:
        else if (instance != this)

            //Then destroy this. This enforces our singleton pattern, meaning there can only ever be one instance of a GameManager.
            Destroy(gameObject);

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



        blocked = true;
    }

    public static void UnBlockInput() {
        blocked = false;
    }

    public void UpdatePreviousGameLogic() {
        prevSelectButton = selectButton;
        prevChangeButton = changeButton;
    }

    public void UpdatePreviousPlayer() {
        if (!blocked) {
            prevDashButton = dashButton;
            prevDeflectButton = deflectButton;
            prevCrawlButton = crawlButton;
            prevJumpButton = jumpButton;
            prevPauseButton = pauseButton;
            prevInteractButton = interactButton;
            prevHorizontalAxis = horizontalAxis;
        }
}

    // Update is called once per frame
    void Update() {
        if (!blocked) {
            dashButton = (Input.GetAxisRaw("DashPunch") == 1.0);
            deflectButton = (Input.GetAxisRaw("DeflectDrag") == 1.0);
            crawlButton = (Input.GetAxisRaw("CrawlSmash") == 1.0);
            jumpButton = (Input.GetAxisRaw("Jump") == 1.0);
            pauseButton = (Input.GetAxisRaw("Pause") == 1.0);
            changeButton = (Input.GetAxisRaw("Change") == 1.0);
            selectButton = (Input.GetAxisRaw("Select") == 1.0);
            interactButton = (Input.GetAxisRaw("Interact") == 1.0);
            horizontalAxis = Input.GetAxisRaw("Horizontal");
            resetButton = Input.GetAxisRaw("Reset")==1.0;

        }
    }
}
