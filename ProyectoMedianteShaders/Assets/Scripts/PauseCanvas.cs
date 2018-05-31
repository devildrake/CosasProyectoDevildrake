using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PauseCanvas : MonoBehaviour {

    public int nextSceneIndex;
    public GameObject timeFeedBackObject;
    public GameObject salir;
    public GameObject comprovacion;
    public GameObject Barra;
    bool textSet;

    //Referencia al gameObject que pone un tono gris a la escena
    public GameObject gris;
    Text[] texts;

    public GameObject fairyTextObject;
    public Text fairyText;
    public GameObject blackForFade;
    public GameObject scoreScreen;

    public static int textIndex;
    public static int lastIndex;


    //referencia al canvas de las opciones
    public GameObject opcionesCanvas;
    
	// Use this for initialization
	void Start () {
        //blackForFade.SetActive(false);
        blackForFade.GetComponent<Image>().color = new Color(blackForFade.GetComponent<Image>().color.r, blackForFade.GetComponent<Image>().color.g, blackForFade.GetComponent<Image>().color.b, 0);
        GameLogic.instance.pauseCanvas = this;
        scoreScreen.SetActive(false);
        scoreScreen.GetComponent<Image>().color = new Color(scoreScreen.GetComponent<Image>().color.r, scoreScreen.GetComponent<Image>().color.g, scoreScreen.GetComponent<Image>().color.b, 0);
        //Debug.Log("Alpha a 0 -> " + scoreScreen.GetComponent<Image>().color.a);
        texts = scoreScreen.GetComponentsInChildren<Text>();
    }

    /*
    //Método que se asegura que el singletond de Gamelogic tenga una referencia a pauseCanvas
    void CheckNull() {
        if (GameLogic.instance.pauseCanvas == null) {
            GameLogic.instance.pauseCanvas = this;
        }
    }*/

    //Método que controla cuando se le da al escape para pausar
    void CheckPause() {

        if (GameLogic.instance != null) {
            gris.SetActive(GameLogic.instance.isPaused);
            if (!GameLogic.instance.isPaused) {

                if (comprovacion != null) {
                    comprovacion.SetActive(false);
                }else {
                    //Debug.Log("Falta asignar la variable comprovación");
                }

                if(salir != null) {
                    salir.SetActive(true);

                }
                else {
                    Debug.Log("Falta asignar la variable salir");

                }
            }

            if (GameLogic.instance.levelFinished) {
                //Debug.Log("LevelFinish");
                if (!textSet) {
                    textSet = true;
                    texts[4].text = GameLogic.instance.pickedFragments + "/3";
                    texts[6].text = (GameLogic.instance.timesDied + 1).ToString();
                    texts[5].text = Mathf.Floor((GameLogic.instance.timeElapsed * 100))/100 + " segundos";
                }

                for (int i = 0; i < GameLogic.instance.interactuableLevelIndexes.Length; i++) {
                    bool goingToSelector = false;
                    if (GameLogic.instance.interactuableLevelIndexes[i] == nextSceneIndex) {
                        goingToSelector = true;
                    }

                    if (goingToSelector) {
                        scoreScreen.SetActive(true);
                    } else {
                        GameLogic.instance.LoadScene(GameLogic.instance.pauseCanvas.nextSceneIndex);
                    }
                }
                Cursor.visible = true;
                Cursor.lockState = CursorLockMode.None; //Se puede volver a mover el cursor

                Color copy = scoreScreen.GetComponent<Image>().color;
                float alpha = copy.a;
                //Debug.Log("Alpha = " +alpha);

                if (alpha != 1) {
                    if ((alpha + alpha * Time.deltaTime) < 1) {
                        //Debug.Log("Sumando");
                        alpha = alpha + 1 * Time.deltaTime;
                        //Debug.Log("Sumando Alpha es = " + alpha);

                    } else {

                        //Debug.Log("Alfa a 1");
                        alpha = 1;
                    }
                    copy.a = alpha;
                    scoreScreen.GetComponent<Image>().color = copy;
                }
            }
        }
    }

    // Update is called once per frame
    void Update () {
        if (GameLogic.instance!=null) {
            if (GameLogic.instance.showTimeCounter) {
                if (timeFeedBackObject != null) {
                    timeFeedBackObject.SetActive(true);
                    timeFeedBackObject.GetComponent<Text>().text = (Mathf.Floor((GameLogic.instance.timeElapsed))).ToString() + "s";
                }
            }


            //CheckNull();
            CheckPause();
            if (InputManager.instance.resetButton) {
                Barra.SetActive(true);
            } else
                Barra.SetActive(false);

            if (GameLogic.instance.eventState == GameLogic.EventState.TEXT) {
                fairyTextObject.SetActive(true);

                if (textIndex > -1) {
                    if (InputManager.gamePadConnected) {
                        fairyText.text = MessagesFairy.GetMessage(textIndex, 1);
                    } else {
                        fairyText.text = MessagesFairy.GetMessage(textIndex, 0);
                    }
                } else {
                    if (!InputManager.gamePadConnected&& MessagesFairy.asked) {
                        Debug.Log("AskedForAdvice");
                        fairyText.text = MessagesFairy.GetAdvice(0);
                    } else if(MessagesFairy.asked) {
                        fairyText.text = MessagesFairy.GetAdvice(1);
                    }

                }

            } else {
                fairyTextObject.SetActive(false);

                if (GameLogic.instance.eventState == GameLogic.EventState.IMAGE) {

                }

            }

            


        }

    }
}
