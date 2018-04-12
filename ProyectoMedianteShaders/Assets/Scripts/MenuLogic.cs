using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuLogic : MonoBehaviour {
    //referencia al Canvas para hacer el fade in
    public Canvas canvas;
    private CanvasGroup canvasGroup;

    //-----------------------state 0
    public GameObject pressAnyKeyObj;//elemento del state 0
    //------------------------------------------------------------

    //-----------------------state 1
    //El primer elemento es el highlight del seleccionado, los siguientes son los botones que hay
    public GameObject[] state1Elements = new GameObject[3];
    public GameObject optionsCanvas; //referencia al canvas que tiene el menu de las opciones
    [SerializeField] private GameObject newOptionsCanvas;
    private Animator newOptionsAnimator;
    private int selected; //variable para controlar que elemento esta seleccionado en el menu
    private Transform highlight;
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
    private bool upAlpha=true, downAlpha= true;
    private int delayCounter;

	// Use this for initialization
	void Start () {
        print("calidad--> "+QualitySettings.GetQualityLevel());
        GameLogic.instance.isPaused = false;
        GameLogic.instance.SetTimeScaleLocal(0.5f);
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

        foreach(GameObject go in state1Elements) {
            go.SetActive(false);
        }
        selected = 0;
        highlight = state1Elements[state1Elements.Length - 1].transform;
        optionsCanvas.SetActive(false);
        newOptionsAnimator = newOptionsCanvas.GetComponentInChildren<Animator>();
        print(newOptionsAnimator);
        newOptionsCanvas.SetActive(false);
        GameLogic.instance.transformableObjects.Add(gameObject);
        InputManager.UnBlockInput();
	}
	
	// Update is called once per frame
	void Update () { 
        switch (menuState) {
            case -2:
                if(InputManager.instance.pauseButton && !InputManager.instance.prevPauseButton) {
                    menuState = -1;
                }
                StateM2Behavior();
                break;

            case -1:
                canvasGroup.gameObject.SetActive(true);
                canvasFadeSplash.gameObject.SetActive(false);
                Destroy(uselessCanvas);
                if(canvasGroup.alpha > 0) {
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
                    foreach (GameObject go in state1Elements) {
                        go.SetActive(true);
                    }
                }
                break;

            case 1:
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
        if(canvasFadeSplash.alpha < 0.5f && !doneFadeIn) {
            canvasFadeSplash.alpha += Time.deltaTime/5;
        }
        else {
            doneFadeIn = true;
            timerM2 += Time.deltaTime;
            if(timerM2 > 0.5f) {
                if (upAlpha) {
                    if(canvasFadeSplash.alpha < 0.999) {
                        canvasFadeSplash.alpha += Time.deltaTime;
                    }
                    else {
                        upAlpha = false;
                    }
                }
                else if (downAlpha) {
                    if(canvasFadeSplash.alpha > 0.5) {
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
        //TODO cambiar por el inputmanager para que funcione tambien con el mando
        if (Input.GetAxisRaw("Vertical") < 0) {
            if (!axisInUse) {
                axisInUse = true;
                selected--;
                if (selected < 0) {
                    selected = state1Elements.Length - 2;
                }
            }
        } else if(Input.GetAxisRaw("Vertical") > 0) {
            if (!axisInUse) {
                axisInUse = true;
                selected++;
                if(selected > state1Elements.Length - 2) {
                    selected = 0;
                }
            }
        } else {
            axisInUse = false;
        }
        highlight.position = new Vector3(highlight.position.x, state1Elements[selected].transform.position.y, highlight.position.z);

        //SELECCION
        if(!InputManager.instance.prevSelectButton && InputManager.instance.selectButton) {
            /*
             * Selected = 0 --> Boton jugar
             * Selected = 1 --> Boton salir
             * Selected = 2 --> Boton opciones
             */
            if (selected == 0) {
                if (GameLogic.instance.firstOpening) {
                    GameLogic.instance.SetTimeScaleLocal(1);
                    InputManager.BlockInput();
                    GameLogic.instance.LoadScene(2);
                    GameLogic.instance.firstOpening = true;
                } else {
                    GameLogic.instance.SetTimeScaleLocal(1);
                    InputManager.BlockInput();
                    GameLogic.instance.LoadScene(2);
                }
            }
            else if (selected == 2) {
                //optionsCanvas.SetActive(true);
                newOptionsCanvas.SetActive(true);
                menuState = 2;
            }
            else if (selected == 1) {
                controlErrores.SetActive(true);
                menuState = 3;
            }
        }

        //CLICK CON EL RATON
        if (Input.GetMouseButtonDown(0)) {
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if(Physics.Raycast(ray, out hit, 100.0f)) {
                if(hit.transform.gameObject.name == "jugar") {
                    selected = 0;
                    if (GameLogic.instance.firstOpening) {
                        GameLogic.instance.SetTimeScaleLocal(1);
                        GameLogic.instance.LoadScene(2);

                        GameLogic.instance.firstOpening = true;
                    } else {
                        GameLogic.instance.SetTimeScaleLocal(1);
                        GameLogic.instance.LoadScene(2);

                    }
                }
                else if(hit.transform.gameObject.name == "opciones") {
                    selected = 2;
                    optionsCanvas.SetActive(true);
                    newOptionsCanvas.SetActive(true);
                    newOptionsAnimator.SetBool("Desplegar", true);
                    menuState = 2;
                }
                else if(hit.transform.gameObject.name == "salir") {
                    selected = 1;
                    controlErrores.SetActive(true);
                    menuState = 3;
                }
            }
        }
    }

    //Funcionamiento del menu cuando tienes la ventana de opciones abierta
    void State2Behavior() {
        //al pulsar escape se cierran las opciones y vuelve al menu
        if (Input.GetAxisRaw("Cancel") == 1 || !optionsCanvas.activeInHierarchy) { //pulsando boton atrás o haciendo click en cancelar
            optionsCanvas.SetActive(false);
            newOptionsAnimator.SetBool("Desplegar", false);
            menuState = 1;
        }
    }

    //Funcionamiento del control de errores para salir
    void State3Behavior() {
        if (!controlErrores.activeInHierarchy) {
            menuState = 1;
        }
        if (Input.GetKeyDown(KeyCode.Escape)) {
            controlErrores.SetActive(false);
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

    public void QuitGame() {
        Application.Quit();
    }

    public void AnimationEvent(string msg) {
        print(msg);
    }
}
