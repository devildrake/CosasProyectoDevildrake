using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PauseCanvas : MonoBehaviour {

    public GameObject salir;
    public GameObject comprovacion;
    bool textSet;

    //Referencia al gameObject que pone un tono gris a la escena
    public GameObject gris;
    Text[] texts;

    public GameObject blackForFade;
    public GameObject scoreScreen;
	// Use this for initialization
	void Start () {
        //blackForFade.SetActive(false);
        blackForFade.GetComponent<Image>().color = new Color(blackForFade.GetComponent<Image>().color.r, blackForFade.GetComponent<Image>().color.g, blackForFade.GetComponent<Image>().color.b, 0);
        GameLogic.instance.pauseCanvas = this;
        scoreScreen.SetActive(false);
        scoreScreen.GetComponent<Image>().color = new Color(scoreScreen.GetComponent<Image>().color.r, scoreScreen.GetComponent<Image>().color.g, scoreScreen.GetComponent<Image>().color.b, 0);
        Debug.Log("Alpha a 0 -> " + scoreScreen.GetComponent<Image>().color.a);
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
                    Debug.Log("Falta asignar la variable comprovación");
                }

                if(salir != null) {
                    salir.SetActive(true);

                }
                else {
                    Debug.Log("Falta asignar la variable salir");

                }
            }

            if (GameLogic.instance.levelFinished) {
                Debug.Log("LevelFinish");
                if (!textSet) {
                    textSet = true;
                    texts[1].text = "Fragmentos: " + GameLogic.instance.pickedFragments + "/1";
                    texts[2].text = "Intentos: " + (GameLogic.instance.timesDied + 1).ToString();
                    texts[3].text = "Tiempo: " + Mathf.Floor((GameLogic.instance.timeElapsed * 100))/100 + " segundos";
                }
                scoreScreen.SetActive(true);
                Cursor.visible = true;
                Cursor.lockState = CursorLockMode.None; //Se puede volver a mover el cursor

                Color copy = scoreScreen.GetComponent<Image>().color;
                float alpha = copy.a;
                Debug.Log("Alpha = " +alpha);

                if (alpha != 1) {
                    if ((alpha + alpha * Time.deltaTime) < 1) {
                        Debug.Log("Sumando");
                        alpha = alpha + 1 * Time.deltaTime;
                        Debug.Log("Sumando Alpha es = " + alpha);

                    } else {

                        Debug.Log("Alfa a 1");
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
        //CheckNull();
        CheckPause();

    }
}
