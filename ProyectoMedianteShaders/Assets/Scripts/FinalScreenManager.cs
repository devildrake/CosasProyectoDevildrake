using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FinalScreenManager : MonoBehaviour {
    [SerializeField] private CanvasGroup fadeCanvas;
    private float timer;
    [SerializeField] private float screenTime = 5.0f; //cuanto tiempo tarda la escena en hacer fade out.

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
        if(timer > screenTime) {
            fadeCanvas.alpha -= Time.deltaTime;
            if(fadeCanvas.alpha < 0.001f) {
                GameLogic.instance.LoadScene(0);
            }
        }
	}
}
