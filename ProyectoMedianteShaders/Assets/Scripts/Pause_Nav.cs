using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

/*
 * Controla la navegacion con mando y teclado del menu de pausa
 */ 

public class Pause_Nav : MonoBehaviour {
    private enum PAUSE_STATE {MAIN, OPTIONS, CLOSE, NULL };
    private PAUSE_STATE state;
    private EventSystem eventSystem;
    public GameObject[] mainButtons, closeButtons;
    public GameObject optionsCanvas, controlErroresSalir;

	void Start () {
        state = PAUSE_STATE.NULL;
        eventSystem = FindObjectOfType<EventSystem>();
	}
	
	void Update () {
		 switch (state){
            case PAUSE_STATE.MAIN:
                MainState();
                break;

            case PAUSE_STATE.OPTIONS:
                if (!optionsCanvas.activeInHierarchy) {
                    state = PAUSE_STATE.MAIN;
                }
                break;

            case PAUSE_STATE.CLOSE:
                CloseState();
                break;
        }
	}

    private void OnDisable() {
        state = PAUSE_STATE.NULL;
    }

    private void OnEnable() {
        state = PAUSE_STATE.MAIN;
    }

    #region STATE_METHODS
    private void MainState() {
        if((InputManager.instance.pauseButton && !InputManager.instance.prevPauseButton) || (InputManager.instance.pauseButton2 && !InputManager.instance.prevPauseButton2)) {
            GameLogic.instance.SetPause(false);
        }

        eventSystem.SetSelectedGameObject(mainButtons[0]);
    }

    private void CloseState() {
        if ((InputManager.instance.pauseButton && !InputManager.instance.prevPauseButton) || (InputManager.instance.pauseButton2 && !InputManager.instance.prevPauseButton2)) {
            controlErroresSalir.SetActive(false);
            state = PAUSE_STATE.MAIN;
        }
    }
    #endregion

    #region BUTTON_METHODS
    public void OpcionesButton() {
        state = PAUSE_STATE.OPTIONS;
        optionsCanvas.SetActive(true);
    }

    public void ExitGame() {
        controlErroresSalir.SetActive(true);
        state = PAUSE_STATE.CLOSE;
    }
    #endregion
}
