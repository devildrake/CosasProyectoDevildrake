using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FinalScreenManager : MonoBehaviour {
    float scrollSpeed = 20.0f;
    float multiplier = 1.0f;
    public float creditsLimitY;
    bool earlyFade;
    [SerializeField] private CanvasGroup fadeCanvas;
    private float timer;
    [SerializeField] private float screenTime = 5.0f; //cuanto tiempo tarda la escena en hacer fade out.
    public GameObject creditsObject;
    public GameObject toBeContinued;
	// Use this for initialization
	void Start () {
        timer = 0;
        toBeContinued.SetActive(false);
	}
	
	// Update is called once per frame
	void Update () {
        Debug.Log(creditsObject.transform.position);
        GameLogic.instance.canBePaused = false;
        GameLogic.instance.SetTimeScaleLocal(1);
        //InputManager.BlockInput();
        print(timer);

        if (InputManager.instance.jumpButton && InputManager.instance.prevJumpButton) {
            multiplier = 30;
            Debug.Log(multiplier);
        } else {
            multiplier = 1;
        }
        if (InputManager.instance.pauseButton && !InputManager.instance.prevPauseButton) {
            earlyFade = true;
        }

        if (creditsObject.transform.position.y > creditsLimitY||earlyFade) {

            if(!earlyFade)
            toBeContinued.SetActive(true);

            timer += Time.deltaTime;
            if (timer > screenTime) {
                fadeCanvas.alpha -= Time.deltaTime;
                if (fadeCanvas.alpha < 0.001f) {
                    GameLogic.instance.canBePaused = true;
                    GameLogic.instance.LoadScene(0);
                }
            }
        } else {
            creditsObject.transform.Translate(Vector2.up * scrollSpeed * Time.deltaTime * multiplier);
        }



	}
}
