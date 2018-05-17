using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

/*
 * Controla la navegacion con mando y teclado del menu de pausa
 */

public class Pause_Nav : MonoBehaviour {
    private enum PAUSE_STATE {MAIN, OPTIONS, CLOSE, NULL };
    private PAUSE_STATE state;
    private EventSystem eventSystem;
    public GameObject[] mainButtons, closeButtons;
    public GameObject optionsCanvas, controlErroresSalir;
    private int mainSelected, salirSelected, waitAFrame;

	void Start () {
        state = PAUSE_STATE.NULL;
        eventSystem = FindObjectOfType<EventSystem>();
        mainSelected = salirSelected = 0;
	}
	
	void Update () {
		 switch (state){
            case PAUSE_STATE.MAIN:
                if (waitAFrame < 1) {
                    waitAFrame++;
                }
                else {
                    MainState();
                }
                break;

            case PAUSE_STATE.OPTIONS:
                if (!optionsCanvas.activeInHierarchy) {
                    state = PAUSE_STATE.MAIN;
                    
                }
                break;

            case PAUSE_STATE.CLOSE:
                if (!controlErroresSalir.activeInHierarchy) {
                    state = PAUSE_STATE.MAIN;
                    waitAFrame = 0;
                }
                CloseState();
                break;
        }
	}

    private void OnDisable() {
        state = PAUSE_STATE.NULL;
    }

    private void OnEnable() {
        state = PAUSE_STATE.MAIN;
        mainSelected = 0;
        salirSelected = 0;
    }

    #region STATE_METHODS
    private void MainState() {
        if ((InputManager.instance.verticalAxis < 0 && InputManager.instance.prevVerticalAxis == 0) || (InputManager.instance.verticalAxis2 < 0 && InputManager.instance.prevVerticalAxis2 == 0) || (InputManager.instance.downKey && !InputManager.instance.prevDownKey)) {
            mainSelected++;
            mainSelected %= mainButtons.Length;
        }
        else if ((InputManager.instance.verticalAxis > 0 && InputManager.instance.prevVerticalAxis == 0) || (InputManager.instance.verticalAxis2 > 0 && InputManager.instance.prevVerticalAxis2 == 0) || (InputManager.instance.upKey && !InputManager.instance.prevUpKey)) {
            mainSelected--;
            if(mainSelected < 0) {
                mainSelected = mainButtons.Length - 1;
            }
        }
        else if((InputManager.instance.pauseButton && !InputManager.instance.prevPauseButton) || (InputManager.instance.pauseButton2 && !InputManager.instance.prevPauseButton2)) {
            GameLogic.instance.SetPause(false);
        }
        else if((InputManager.instance.selectButton && !InputManager.instance.prevSelectButton) || (InputManager.instance.selectButton2 && !InputManager.instance.prevSelectButton2)) {
            mainButtons[mainSelected].GetComponent<Button>().onClick.Invoke();
        }

        eventSystem.SetSelectedGameObject(mainButtons[mainSelected]);
    }

    private void CloseState() {
        if ((InputManager.instance.horizontalAxis != 0 && InputManager.instance.prevHorizontalAxis == 0) || (InputManager.instance.horizontalAxis2 != 0 && InputManager.instance.prevHorizontalAxis2 == 0) || (InputManager.instance.leftKey && !InputManager.instance.prevLeftKey) || (InputManager.instance.rightKey && !InputManager.instance.prevRightKey)) {
            salirSelected++;
            salirSelected %= 2;
        }
        else if ((InputManager.instance.pauseButton && !InputManager.instance.prevPauseButton) || (InputManager.instance.pauseButton2 && !InputManager.instance.prevPauseButton2)) {
            controlErroresSalir.SetActive(false);
            state = PAUSE_STATE.MAIN;
        }

        eventSystem.SetSelectedGameObject(closeButtons[salirSelected]);
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
