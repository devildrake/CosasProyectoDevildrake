using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FinalScreenManager : MonoBehaviour {
    float scrollSpeed = 2.0f;
    float multiplier = 1.0f;
    public float creditsLimitY;
    bool earlyFade;
    [SerializeField] private CanvasGroup fadeCanvas;
    private float timer;
    [SerializeField] private float screenTime = 5.0f; //cuanto tiempo tarda la escena en hacer fade out.
    public GameObject creditsObject;
	// Use this for initialization
	void Start () {
        timer = 0;
	}
	
	// Update is called once per frame
	void Update () {
        GameLogic.instance.isPaused = false;
        GameLogic.instance.SetTimeScaleLocal(1);
        InputManager.BlockInput();
        timer += Time.deltaTime;
        print(timer);
        if (creditsObject.transform.position.y > creditsLimitY||earlyFade) {
            if (timer > screenTime) {
                fadeCanvas.alpha -= Time.deltaTime;
                if (fadeCanvas.alpha < 0.001f) {
                    GameLogic.instance.LoadScene(0);
                }
            }
        } else {
            creditsObject.transform.Translate(Vector2.up * scrollSpeed * Time.deltaTime * multiplier);
        }

        if (InputManager.instance.jumpButton&&InputManager.instance.prevJumpButton) {
            multiplier = 3;
        } else {
            multiplier = 1;
        }
        if (InputManager.instance.pauseButton&&!InputManager.instance.prevPauseButton) {
            earlyFade = true;
        }

	}
}
