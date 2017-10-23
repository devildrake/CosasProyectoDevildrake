using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

//Clase singleton (solo debe existir uno) referenciable a partir de una instancia estatica a si mismo que gestiona
//si el jugador se encuentra en el menu principal 

public class GameLogic : MonoBehaviour
{

    public static GameLogic instance = null;             

    //Booleano privado pero visible que gestiona si se esta en el menu principal o no
    [SerializeField]
    private bool isInMainMenu;

    //Booleano que determina si se ha mirado si se esta en el menu principal o no
    private bool checkMainMenu;

    //Booleano que gestiona la espera de un frame para la busqueda de referencias
    private bool waitAFrame;

    //Referencia a MenuScripts (Solo deja de ser null en el menu principal)
    private MenuScripts menuScripts;

    //Nombre de la escena actual
    static string currentSceneName = null;
    public PauseCanvas pauseCanvas;

    public List<GameObject> transformableObjects;

    void Awake()
    {
        //Check if instance already exists
        if (instance == null)

            //if not, set instance to this
            instance = this;

        //If instance already exists and it's not this:
        else if (instance != this)

            //Then destroy this. This enforces our singleton pattern, meaning there can only ever be one instance of a GameManager.
            Destroy(gameObject);

        //Sets this to not be destroyed when reloading scene
        DontDestroyOnLoad(gameObject);

        //Call the InitGame function to initialize the first level 
    }

    //Setter de WaitAFrame
    void SetWaitAFrame(bool a)
    {
        waitAFrame = a;
    }

    //Setter de CheckMainMenu
    void SetCheckMainMenu(bool a) {
        checkMainMenu = a;
    }

    void Start()
    {
        SetWaitAFrame(false);
        SetCheckMainMenu(false);
    }

    //Se comprueba si la escena se ha cambiado
    bool ChangedScene() {

        return currentSceneName != SceneManager.GetActiveScene().name;
    }

    //Método que reinicia la espera del frame para buscar referencias y reinicia el booleano isInMainMenu
    void ResetSceneData() {
        SetWaitAFrame(false);
        isInMainMenu = false;
        currentSceneName = SceneManager.GetActiveScene().name;
    }

    void Update()
    {
        //Se comprueba si ha habido cambio de escena, si lo ha habido se reinician los booleanos waitAFrame, checkMainMenu e isInManMenu además de actualizar la variable currentSceneName
        if (ChangedScene())
        {
            ResetSceneData();
        }


        //Espera de un frame para comprobar que se ejecuta en el orden deseado
        if (!waitAFrame)
        {
            SetWaitAFrame(true);
            SetCheckMainMenu(false);
        }

        //Una vez realizada la espera, sin haber comprobado si se esta en el menu principal
        else if(!checkMainMenu)
        {
            menuScripts = FindObjectOfType<MenuScripts>();
            if (menuScripts != null)
            {
                isInMainMenu = true;
            }
            SetCheckMainMenu(true);
        }

        //Una vez comprobado si estamos o no en el menu principal se pondria el comportamiento deseado
        else
        {
            if (!isInMainMenu) {

                if (Input.GetKeyDown(KeyCode.LeftShift)) {
                    foreach(GameObject g in transformableObjects) {
                        g.GetComponent<Transformable>().Change();
                    }
                }

            }


        }
    }

    //Método para cargar el menu desde cualquier escena
    public void LoadMenu()
    {
        SceneManager.LoadScene(0);
    }
}