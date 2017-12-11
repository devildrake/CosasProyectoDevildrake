using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

//Clase singleton (solo debe existir uno) referenciable a partir de una instancia estatica a si mismo que gestiona
//si el jugador se encuentra en el menu principal 

public class GameLogic : MonoBehaviour
{
     AudioClip changeWorldClip;
    //Float privado que se encarga de cambiar timeScale segun combiene cuando el juego no esta pausado
    private float timeScaleLocal = 1;

    //Instancia del singleton que se inicia en null
    public static GameLogic instance = null;

    //Booleano que gestionara si el canvas esta activo y por tanto el juego pausado (AUN NO)
    public bool isPaused;

    //Booleano privado pero visible que gestiona si se esta en el menu principal o no
    [SerializeField]
    private bool isInMainMenu;

    private bool setSpawnPoint;

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

    public Vector3 spawnPoint;

    public float worldOffset;

    void Awake()
    {

        worldOffset = 300;

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
        setSpawnPoint = false;
        changeWorldClip = Resources.Load<AudioClip>("Sounds/ChangeWorld");
        gameObject.GetComponent<AudioSource>().clip = changeWorldClip;
    }

    //Se comprueba si la escena se ha cambiado
    bool ChangedScene() {

        return currentSceneName != SceneManager.GetActiveScene().name;
    }

    //Método que reinicia la espera del frame para buscar referencias y reinicia el booleano isInMainMenu
    public void ResetSceneData() {
        SetWaitAFrame(false);
        isInMainMenu = false;
        currentSceneName = SceneManager.GetActiveScene().name;
        DirectionCircle.SetOnce(true);
    }

    public void SetSpawnPoint(Vector3 a)
    {
        spawnPoint = a;
    }

    //Método para variar el timeScaleLocal siempre y cuando el juego no este pausado
    public void SetTimeScaleLocal(float a) {
        if(!isPaused)
        timeScaleLocal = a;
    }

    //Método que pone el timeScale a 0 cuando el juego esta pausado y que cuando no esta pausado pone Time.timeScale igual a la variable timeScaleLocal
    void TimeScaleStuff() {
        if (isPaused) {
            Time.timeScale = 0;
        }
        else {
            Time.timeScale = timeScaleLocal;
        }
    }

    void Update(){

        TimeScaleStuff();

        //Se comprueba si ha habido cambio de escena, si lo ha habido se reinician los booleanos waitAFrame, checkMainMenu e isInManMenu además de actualizar la variable currentSceneName
        if (ChangedScene()){
            ResetSceneData();
        }


        //Espera de un frame para comprobar que se ejecuta en el orden deseado
        if (!waitAFrame){
            SetWaitAFrame(true);
            SetCheckMainMenu(false);
            setSpawnPoint = false;
        }

        //Una vez realizada la espera, sin haber comprobado si se esta en el menu principal
        else if(!checkMainMenu){
            menuScripts = FindObjectOfType<MenuScripts>();
            if (menuScripts != null){
                isInMainMenu = true;
            }
            SetCheckMainMenu(true);
        }

        //Una vez comprobado si estamos o no en el menu principal se pondria el comportamiento deseado
        else{
            if (!isInMainMenu) {
                //Si el juego no esta pausado
                if (!isPaused) {
                    if (!setSpawnPoint)
                    {
                        spawnPoint = GameObject.FindGameObjectWithTag("Player").transform.position;
                        setSpawnPoint = true;
                    }
                    if (Input.GetKeyDown(KeyCode.LeftShift)) {
                        if (gameObject.GetComponent<AudioSource>().pitch == 1.5) {
                            gameObject.GetComponent<AudioSource>().pitch = 0.5f;
                        }
                        else {
                            gameObject.GetComponent<AudioSource>().pitch = 1.5f;
                        }
                        gameObject.GetComponent<AudioSource>().Play();
                        foreach (GameObject g in transformableObjects) {
                            g.GetComponent<DoubleObject>().Change();
                            //Debug.Log(g);
                        }
                    }

                }
                //Si el juego SI esta pausado
                else {


                }
                //En cualquier caso se comprueba el input
                CheckPauseInput();
            }
            else
            {
                Cursor.visible = true;
                Cursor.lockState = CursorLockMode.None; //Se puede volver a mover el cursor

            }
        }
    }

    //Método para cargar el menu desde cualquier escena
    public void LoadMenu(){
        SceneManager.LoadScene(0);
        ResetSceneData();
    }

    public void RestartScene() {
        SceneManager.LoadScene(currentSceneName);
        isPaused = false;
        ResetSceneData();
    }

    //Método que controla cuando se le da al escape para pausar, a su vez activa y desactiva el cursor en función de si 
    //Se abre el menú in game
    void CheckPauseInput() {
        if (Input.GetKeyDown(KeyCode.Escape)) {
            if (isPaused) {
                Cursor.visible = false;
                Cursor.lockState = CursorLockMode.None; //Se engancha el cursor en el centro de la pantalla.
            }
            else if (!isPaused) {
                Cursor.visible = true;
                Cursor.lockState = CursorLockMode.Locked; //Se engancha el cursor en el centro de la pantalla.
            }
            isPaused = !isPaused;
        }
    }


}