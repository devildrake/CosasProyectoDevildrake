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
    private int selected; //variable para controlar que elemento esta seleccionado en el menu
    private Transform highlight;
    //------------------------------------------------------------

    /*
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

	// Use this for initialization
	void Start () {
        GameLogic.instance.isPaused = false;
        GameLogic.instance.SetTimeScaleLocal(1);
        canvasGroup = canvas.GetComponent<CanvasGroup>();
        menuState = -1;
        timer = 0f;
        timeToBlink = 0.4f;
        pressAnyKeyObj.SetActive(false);

        foreach(GameObject go in state1Elements) {
            go.SetActive(false);
        }
        selected = 0;
        highlight = state1Elements[state1Elements.Length - 1].transform;
        optionsCanvas.SetActive(false);

        GameLogic.instance.transformableObjects.Add(gameObject);
	}
	
	// Update is called once per frame
	void Update () { 
        switch (menuState) {
        case -1:
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
            State1Behavior();
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

    private void State1Behavior() {
        
        //MOVIMIENTO ENTRE LAS OPCIONES
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
        if(Input.GetAxisRaw("Select") == 1) {
            /*
             * Selected = 0 --> Boton jugar
             * Selected = 1 --> Boton salir
             * Selected = 2 --> Boton opciones
             */
            if (selected == 0) {
                if (GameLogic.instance.firstOpening) {
                    GameLogic.instance.LoadScene(2);
                    GameLogic.instance.firstOpening = true;
                } else {
                    GameLogic.instance.LoadScene(2);
                }
            }
            else if (selected == 2) {
                optionsCanvas.SetActive(true);
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
                        GameLogic.instance.LoadScene(2);
                        GameLogic.instance.firstOpening = true;
                    } else {
                        GameLogic.instance.LoadScene(2);
                    }
                }
                else if(hit.transform.gameObject.name == "opciones") {
                    selected = 2;
                    optionsCanvas.SetActive(true);
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
}
