using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class MenuLogic : MonoBehaviour {
    //referencia al Canvas para hacer el fade in
    public Canvas canvas;
    private CanvasGroup canvasGroup;
    private float auxTimer;
    //-----------------------state 0
    public GameObject pressAnyKeyObj;//elemento del state 0
    //------------------------------------------------------------

    //-----------------------state 1
    // 0--> 1 jugador
    // 1--> 2 jugdores
    // 3--> opciones
    // 4--> salir
    private const int totalMainMenuItems = 4;
    public Button[] state1Elements = new Button[totalMainMenuItems];
    [SerializeField] private GameObject mainCanvas;
    public GameObject optionsCanvas; //referencia al canvas que tiene el menu de las opciones
    [SerializeField] private GameObject newOptionsCanvas;
    private int selected; //variable para controlar que elemento esta seleccionado en el menu
    private CanvasGroup mainCanvasGroup;
    private int middleSelected = 0; //me guardo una variable de que item estaba seleccionado de la columna central de elementos para volver al mismo
    //------------------------------------------------------------

    //------------------------state 3
    public Button[] controlErroresButtons = new Button[2];
    private short controlErroresSelected;
    //------------------------------------------------------------

    /*
     * menuState = -2 --> Splash del equipo.
     * menuState = -1 --> FadeIn del juego.
     * menuState = 0 --> Pantalla de pulsa cualquier tecla para continuar
     * menuState = 1 --> Pantalla jugar/salir
     * menuState = 2 --> Opciones abiertas
     * menuState = 3 --> Control de errores para salir
     */
    private int menuState;
    private bool axisInUse = false; //para detectar el input solo una vez hasta que sueltas

    //Canvas con los elementos para el control de errores
    [SerializeField] private GameObject controlErrores;

    //variables para controlar el blink
    private float timer, timeToBlink;

    [SerializeField] private CanvasGroup canvasFadeSplash;
    private bool doneFadeIn = false;
    private float timerM2; //timer para el estado -2
    public GameObject uselessCanvas;
    private bool upAlpha = true, downAlpha = true;
    private int delayCounter;
    private EventSystem eventSystem; //referencia al event system para highlightear los botones del canvas.

    // Use this for initialization
    void Start() {
        auxTimer = 0;
        GameLogic.instance.SetTimeScaleLocal(0.5f);
        GameLogic.instance.isPaused = false;
        canvas.gameObject.SetActive(false);
        canvasGroup = canvas.GetComponent<CanvasGroup>();
        canvasGroup.alpha = 1;
        canvasFadeSplash.alpha = 0;
        menuState = -2;
        timer = 0f;
        timerM2 = 0f;
        timeToBlink = 0.4f;
        pressAnyKeyObj.SetActive(false);
        delayCounter = 0;
        selected = 0;
        newOptionsCanvas.SetActive(false);
        GameLogic.instance.transformableObjects.Add(gameObject);
        InputManager.UnBlockInput();
        mainCanvas.SetActive(false);
        eventSystem = FindObjectOfType<EventSystem>();
        controlErroresSelected = 0;
        mainCanvasGroup = mainCanvas.GetComponent<CanvasGroup>();
        mainCanvasGroup.alpha = 0.0f;
    }

    // Update is called once per frame
    void Update() {
        switch (menuState) {
            case -2:
                auxTimer += Time.deltaTime;
                if (auxTimer > 2.5) {
                    auxTimer = 0;
                    menuState = -1;
                }
                if (InputManager.instance.pauseButton && !InputManager.instance.prevPauseButton) {
                    menuState = -1;
                }
                StateM2Behavior();
                break;

            case -1:
                canvasGroup.gameObject.SetActive(true);
                canvasFadeSplash.gameObject.SetActive(false);
                Destroy(uselessCanvas);

                auxTimer += Time.deltaTime;
                if (auxTimer > 2.0) {
                    auxTimer = 0;
                    menuState = 0;
                }

                if (canvasGroup.alpha > 0) {
                    canvasGroup.alpha -= Time.deltaTime;
                } else {
                    pressAnyKeyObj.SetActive(true);
                    menuState = 0;
                }
                break;

            case 0:
                Blink(pressAnyKeyObj);
                if (Input.anyKeyDown) {
                    menuState = 1;
                    pressAnyKeyObj.SetActive(false);
                    mainCanvas.SetActive(true);
                }
                break;

            case 1:
                if(mainCanvasGroup.alpha < 1.0f) {
                    mainCanvasGroup.alpha += Time.deltaTime*2;
                }
                if (delayCounter < 2) {
                    delayCounter++;
                }
                else {
                    State1Behavior();
                }
                break;

            case 2:
                State2Behavior();
                break;

            case 3:
                State3Behavior();
                break;

            default:
                break;
        }
    }

    private void StateM2Behavior() {
        if (canvasFadeSplash.alpha < 0.5f && !doneFadeIn) {
            canvasFadeSplash.alpha += Time.deltaTime / 5;
        }
        else {
            doneFadeIn = true;
            timerM2 += Time.deltaTime;
            if (timerM2 > 0.5f) {
                if (upAlpha) {
                    if (canvasFadeSplash.alpha < 0.999) {
                        canvasFadeSplash.alpha += Time.deltaTime;
                    }
                    else {
                        upAlpha = false;
                    }
                }
                else if (downAlpha) {
                    if (canvasFadeSplash.alpha > 0.5) {
                        canvasFadeSplash.alpha -= Time.deltaTime;
                    }
                    else {
                        downAlpha = false;
                    }
                }
                else {
                    if (canvasFadeSplash.alpha > 0.0001) {
                        canvasFadeSplash.alpha -= Time.deltaTime / 2;
                    }
                    else {
                        menuState = -1;
                    }
                }
            }
        }
    }

    private void State1Behavior() {
        //MOVIMIENTO ENTRE LAS OPCIONES
        //arriba
        if (((InputManager.instance.prevVerticalAxis == 0 && InputManager.instance.prevVerticalAxis2 == 0) && !InputManager.instance.prevDownKey && !InputManager.instance.prevUpKey) && (InputManager.instance.verticalAxis < 0 || InputManager.instance.verticalAxis2 < 0 || InputManager.instance.downKey)) {//-1
            if (!axisInUse) {
                axisInUse = true;
                if(selected == 4) {
                    selected = 6;
                }
                else if(selected == 6 || selected == 7) {
                    selected = 4;
                }
                else if (selected != 5) {
                    selected--;
                    if (selected < 0) {
                        selected = 3;
                    }
                }
            }
        }
        //abajo
        else if (((InputManager.instance.prevVerticalAxis == 0 && InputManager.instance.prevVerticalAxis2 == 0) && !InputManager.instance.prevDownKey && !InputManager.instance.prevUpKey) && (InputManager.instance.verticalAxis > 0 || InputManager.instance.verticalAxis2 > 0 || InputManager.instance.upKey)) {//+1
            if (!axisInUse) {
                axisInUse = true;
                if (selected == 4) {
                    selected = 6;
                }
                else if (selected == 6 || selected == 7) {
                    selected = 4;
                }
                else if (selected != 4 && selected != 5) {
                    selected++;
                    if (selected > 3) {
                        selected = 0;
                    }
                }
            }
        }
        //derecha
        else if ((InputManager.instance.horizontalAxis > 0 && InputManager.instance.prevHorizontalAxis == 0) || (InputManager.instance.horizontalAxis2 > 0 && InputManager.instance.prevHorizontalAxis2 == 0) || (InputManager.instance.rightKey && !InputManager.instance.prevRightKey)) {
            if (selected == 5) {
                selected = 4;
            }
            else if (selected == 4) {
                selected = middleSelected;
            }
            else if(selected == 6){
                selected = 7;
            }
            else if(selected == 7) {
                selected = middleSelected;
            }
            else {
                middleSelected = selected;
                selected = 5;
            }
        }
        //izquierda
        else if ((InputManager.instance.horizontalAxis < 0 && InputManager.instance.prevHorizontalAxis == 0) || (InputManager.instance.horizontalAxis2 < 0 && InputManager.instance.prevHorizontalAxis2 == 0) || (InputManager.instance.leftKey && !InputManager.instance.prevLeftKey)) {
            if (selected == 5) {
                selected = middleSelected;
            }
            else if (selected == 4) {
                selected = 5;
            }
            else if (selected == 6) {
                selected = 5;
            }
            else if (selected == 7) {
                selected = 6;
            }
            else {
                middleSelected = selected;
                selected = 4;
            }
        }

        if (InputManager.instance.verticalAxis == 0 && InputManager.instance.verticalAxis2 == 0 && !InputManager.instance.downKey && !InputManager.instance.upKey) {
            axisInUse = false;
        }

        //HIGHLIGHT
        eventSystem.SetSelectedGameObject(state1Elements[selected].gameObject);

        //SELECCION
        if (!InputManager.instance.prevSelectButton && InputManager.instance.selectButton) {
            /*
             * Selected = 0 --> Boton 1 jugador
             * Selected = 1 --> 2 jugadores
             * Selected = 2 --> Boton opciones
             * Selected = 3 --> Salir
             */
            if (selected == 0) {
                PlayOnePlayer();
            }
            else if (selected == 1) {
                PlayTwoPlayers();
            }
            else if (selected == 2) {
                Options();
            }
            else if (selected == 3) {
                Exit();
            }else {
                state1Elements[selected].onClick.Invoke();
            }
        }
    }


    //Funcionamiento del menu cuando tienes la ventana de opciones abierta
    void State2Behavior() {
        //al pulsar escape se cierran las opciones y vuelve al menu
        if (!newOptionsCanvas.activeInHierarchy) { //pulsando boton atrás o haciendo click en cancelar
            menuState = 1;
        }
    }

    //Funcionamiento del control de errores para salir
    void State3Behavior() {
        if (!controlErrores.activeInHierarchy) {
            menuState = 1;
        }
        if (InputManager.instance.cancelButton) {
            controlErrores.SetActive(false);
        }
        print("SELECTED--> " + controlErroresSelected);
        eventSystem.SetSelectedGameObject(controlErroresButtons[Mathf.Abs(controlErroresSelected)].gameObject);
        

        if ((InputManager.instance.horizontalAxis < 0 && InputManager.instance.horizontalAxis == 0) || (InputManager.instance.horizontalAxis2 < 0 && InputManager.instance.prevHorizontalAxis2 == 0) || (InputManager.instance.rightKey && !InputManager.instance.prevRightKey)) { //derecha
            controlErroresSelected++;
            controlErroresSelected %= 2;
        }
        else if (((InputManager.instance.horizontalAxis > 0 && InputManager.instance.horizontalAxis == 0) || (InputManager.instance.horizontalAxis2 > 0 && InputManager.instance.prevHorizontalAxis2 == 0) || (InputManager.instance.leftKey && !InputManager.instance.prevLeftKey))) { //izquierda
            controlErroresSelected--;
            controlErroresSelected %= 2;
        }
    }

    void Blink(GameObject go) {
        if(timer > timeToBlink) {
            if(go.activeInHierarchy) {
                go.SetActive(false);
            } else {
                go.SetActive(true);
            }
            timer = 0;
        }
        timer += Time.deltaTime;
    }


#region BUTTON_METHODS
    public void CloseControlErrores() {
        delayCounter = 0;
        controlErrores.SetActive(false);
    }

    public void QuitGame() {
        Application.Quit();
    }

    //Metodo que llama el boton de 1 jugador
    public void PlayOnePlayer() {
        if (!GameLogic.instance.playedTutorial) {
            GameLogic.instance.SetTimeScaleLocal(1);
            InputManager.BlockInput();
            InputManager.currentGameMode = InputManager.GAMEMODE.SINGLEPLAYER;

            SoundManager.Instance.StopEvent(SoundManager.Instance.music,true);

            //GameLogic.instance.firstOpening = true;
            GameLogic.instance.LoadScene("VideoScene");
        } else {
            InputManager.currentGameMode = InputManager.GAMEMODE.SINGLEPLAYER;
            GameLogic.instance.gameState = GameLogic.GameState.LEVEL;
            GameLogic.instance.SetTimeScaleLocal(1);
            InputManager.BlockInput();
            GameLogic.instance.LoadScene(2);
            //GameLogic.instance.LoadScene(3);
        }
    }

    //Metodo que llama el boton de 2 jugadores
    public void PlayTwoPlayers() {
        if (!GameLogic.instance.playedTutorial) {
            //GameLogic.instance.SetTimeScaleLocal(1);
            //InputManager.BlockInput();
            //InputManager.currentGameMode = InputManager.GAMEMODE.MULTI_KEYBOARD_CONTROLLER;

            ////GameLogic.instance.firstOpening = true;
            //SoundManager.Instance.StopEvent(SoundManager.Instance.music, true);
            //GameLogic.instance.LoadScene("VideoScene");

        } else {
            InputManager.currentGameMode = InputManager.GAMEMODE.MULTI_KEYBOARD_CONTROLLER;
            GameLogic.instance.gameState = GameLogic.GameState.LEVEL;

            GameLogic.instance.SetTimeScaleLocal(1);
            InputManager.BlockInput();
            GameLogic.instance.LoadScene(2);
            //GameLogic.instance.LoadScene(3);
        }
    }

    //Metodo que llama el boton de opciones
    public void Options() {
        newOptionsCanvas.SetActive(true);
        menuState = 2;
    }

    //Metodo que llama el boton de salir
    public void Exit() {
        controlErrores.SetActive(true);
        menuState = 3;
    }

    public void OpenLink(string url) {
        if (url != "") {
            Application.OpenURL(url);
        }
        else {
            print("Falta asignar URL al botón");
        }
    }
    #endregion
}
