using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PauseCanvas : MonoBehaviour {

    public GameObject salir;
    public GameObject comprovacion;

    //Referencia al gameObject que pone un tono gris a la escena
    public GameObject gris;

    public GameObject blackForFade;
	// Use this for initialization
	void Start () {
        //blackForFade.SetActive(false);
        blackForFade.GetComponent<Image>().color = new Color(blackForFade.GetComponent<Image>().color.r, blackForFade.GetComponent<Image>().color.g, blackForFade.GetComponent<Image>().color.b, 0);
        GameLogic.instance.pauseCanvas = this;
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
        }
    }

    // Update is called once per frame
    void Update () {
        //CheckNull();
        CheckPause();

    }
}
