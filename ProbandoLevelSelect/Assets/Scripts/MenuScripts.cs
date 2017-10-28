using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
//Clase que gestionara las cosas que ocurran en el menú, por ahora existe solo para que el gameLogic sepa que se encuentra en el menu y para que los pauseCanvas de los niveles puedan cargar nivel (Menu principal) usando el metodo LoadLevel.

public class MenuScripts : MonoBehaviour
{
    bool set;
    // Use this for initialization
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update() {
        if (!set) {
            if (GameLogic.instance != null) {
                GameLogic.instance.isPaused = false;
                set = true;
            }
        }
    }
    //Método para cambiar de escena usando el nombre
    public void LoadLevel(string name)
    {
        SceneManager.LoadScene(name);
    }
}
