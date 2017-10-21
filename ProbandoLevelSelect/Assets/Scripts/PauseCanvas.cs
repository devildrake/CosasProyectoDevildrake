using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseCanvas : MonoBehaviour {

    //Booleano que gestionara si el canvas esta activo y por tanto el juego pausado (AUN NO)
    bool isPaused;

    //Referencia al gameObject que pone un tono gris a la escena
    public GameObject gris;

	// Use this for initialization
	void Start () {
	}

    //Método que se asegura que el singletond de Gamelogic tenga una referencia a pauseCanvas
    void CheckNull() {
        if (GameLogic.instance.pauseCanvas == null) {
            GameLogic.instance.pauseCanvas = this;
        }
    }

    //Método que controla cuando se le da al escape para pausar
    void CheckPauseInput() {
        if (Input.GetKeyDown(KeyCode.Escape)){
            isPaused = !isPaused;
            gris.SetActive(isPaused);
        }
    }

    // Update is called once per frame
    void Update () {
        CheckNull();
        CheckPauseInput();

    }
}
