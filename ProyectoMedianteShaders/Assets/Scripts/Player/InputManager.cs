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
    [HideInInspector]
    public bool prevChangeButton;
    [HideInInspector]
    public bool prevPauseButton;
    [HideInInspector]
    public bool prevSelectButton;
    [HideInInspector]
    public bool prevInteractButton;

    public bool dashButton;
    public bool deflectButton;
    public bool crawlButton;
    public bool jumpButton;
    public bool changeButton;
    public bool pauseButton;
    public bool selectButton;
    public bool interactButton;

    private void Awake() {
        if (instance == null)

            //if not, set instance to this
            instance = this;

        //If instance already exists and it's not this:
        else if (instance != this)

            //Then destroy this. This enforces our singleton pattern, meaning there can only ever be one instance of a GameManager.
            Destroy(gameObject);
    }

    // Use this for initialization
    void Start () {
		
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

        blocked = true;
    }

    public static void UnBlockInput() {
        blocked = false;
    }
	
    public void UpdatePrevious() {
        if (!blocked) {
            prevDashButton = dashButton;
            prevDeflectButton = deflectButton;
            prevCrawlButton = crawlButton;
            prevJumpButton = jumpButton;
            prevChangeButton = changeButton;
            prevPauseButton = pauseButton;
            prevSelectButton = selectButton;
            prevInteractButton = interactButton;
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
            selectButton = (Input.GetAxis("Select") == 1.0);
            interactButton = (Input.GetAxis("Interact") == 1.0);
        }
    }
}
