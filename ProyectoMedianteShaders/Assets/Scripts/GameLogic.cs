using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using FragmentDataNamespace;
using System;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

//Clase singleton (solo debe existir uno) referenciable a partir de una instancia estatica a si mismo que gestiona
//si el jugador se encuentra en el menu principal 

public class GameLogic : MonoBehaviour {
    //////////////////////VARIABLES LOAD/SAVE////////////////////////////
    //////////////////////VARIABLES LOAD/SAVE////////////////////////////
    //////////////////////VARIABLES LOAD/SAVE////////////////////////////

    bool firstOpening;
    public Dictionary<int, bool> completedLevels;



    //////////////////////VARIABLES LOAD/SAVE////////////////////////////
    //////////////////////VARIABLES LOAD/SAVE////////////////////////////
    //////////////////////VARIABLES LOAD/SAVE////////////////////////////


    //Flag para la transición inicial de la camara
    public bool cameraTransition;

    public PlayerController currentPlayer;

    public List<FragmentData> fragments;
    private int lastFragmentId = -1;

    //Tiempo que hay que pulsar para reinciar
    public float maxTimeToReset = 3;

    //Tiempo que lleva el jugador pulsando
    public float timerToReset;

    //Variables de estadísticas 
    public int timesDied = 0;
    public float timeElapsed = 0;
    public int pickedFragments = 0;

    //Clip de cambio de mundo
    AudioClip changeWorldClip;

    //Float privado que se encarga de cambiar timeScale segun combiene cuando el juego no esta pausado
    private float timeScaleLocal = 1;

    //Instancia del singleton que se inicia en null
    public static GameLogic instance = null;

    //Booleano que gestionara si el canvas esta activo y por tanto el juego pausado (AUN NO)
    public bool isPaused;

    //Booleano que gestiona si el jugador ha llegado al final del nivel
    public bool levelFinished;

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

    public float musicVolume; //modificador del volumen de la musica
    public float sfxVolume; //modificador del volumen de los efectos de sonido
    public int screenRefreshRate; //varia los Hz de refresco de la pantalla
    public int maxFrameRate; //limita el framerate

    /*
     * Conjunto de metodos que se llamaran al cambiar las opciones de juego
     */

    //=====================FINAL DE METODOS DE OPCIONES========================

    public void KillPlayer() {
        currentPlayer.Kill();
    }

    void Awake() {
        completedLevels = new Dictionary<int, bool>();
        Application.targetFrameRate = 60;

        currentSceneName = SceneManager.GetActiveScene().name;

        fragments = new List<FragmentData>();

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


        cameraTransition = true;
        Load();


    }


    //Setter de WaitAFrame
    void SetWaitAFrame(bool a) {
        waitAFrame = a;
    }

    public int GetCurrentLevelIndex() {
        return SceneManager.GetActiveScene().buildIndex;
    }

    public string GetCurrentLevel() {
        return currentSceneName;
    }

    public void AddFragmentData(FragmentData a) {

        bool temp = false;
        foreach (FragmentData saved in fragments) {
            if (saved.levelName == a.levelName) {
                a.picked = saved.picked;
                temp = true;
            }
        }
        if (!temp) {
            a.picked = false;
            fragments.Add(a);
            //Debug.Log("Adding a Fragment");
        }

    }

    public void GrabFragment(FragmentData a) {
        foreach (FragmentData f in fragments) {
            if (f.levelName == a.levelName) {
                a.picked = f.picked = true;
            }
        }
        pickedFragments++;
    }


    //Setter de CheckMainMenu
    void SetCheckMainMenu(bool a) {
        checkMainMenu = a;
    }
    public void SafelyDestroy(DoubleObject g) {
        if (transformableObjects.Contains(g.gameObject)) {
            transformableObjects.Remove(g.gameObject);
            transformableObjects.Remove(g.brotherObject);
        }
        Destroy(g.transform.parent.gameObject);
    }

    void Start() {
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
        cameraTransition = true;
        SetWaitAFrame(false);
        isInMainMenu = false;
        currentSceneName = SceneManager.GetActiveScene().name;
        DirectionCircle.SetOnce(true);
        timeElapsed = 0;
        pickedFragments = 0;
    }

    public static bool isNull(GameObject g) {
        return g == null;
    }

    public void SetSpawnPoint(Vector3 a) {
        spawnPoint = a;
    }

    //Método para variar el timeScaleLocal siempre y cuando el juego no este pausado
    public void SetTimeScaleLocal(float a) {
        if (!isPaused)
            timeScaleLocal = a;
    }

    //Método que pone el timeScale a 0 cuando el juego esta pausado y que cuando no esta pausado pone Time.timeScale igual a la variable timeScaleLocal
    void TimeScaleStuff() {
        if (isPaused) {
            Time.timeScale = 0;
        } else {
            Time.timeScale = timeScaleLocal;
        }
    }

    void Update() {

        TimeScaleStuff();

        //Se comprueba si ha habido cambio de escena, si lo ha habido se reinician los booleanos waitAFrame, checkMainMenu e isInManMenu además de actualizar la variable currentSceneName
        if (ChangedScene()) {
            ResetSceneData();
        }


        //Espera de un frame para comprobar que se ejecuta en el orden deseado
        if (!waitAFrame) {
            SetWaitAFrame(true);
            SetCheckMainMenu(false);
            setSpawnPoint = false;
        }

        //Una vez realizada la espera, sin haber comprobado si se esta en el menu principal
        else if (!checkMainMenu) {
            menuScripts = FindObjectOfType<MenuScripts>();
            if (menuScripts != null) {
                isInMainMenu = true;
            }
            SetCheckMainMenu(true);
        }

        //Una vez comprobado si estamos o no en el menu principal se pondria el comportamiento deseado
        else {
            if (!isInMainMenu) {
                if (!levelFinished) {
                    if (Input.GetKey(KeyCode.R)) {
                        timerToReset += Time.deltaTime;
                    } else {
                        timerToReset = 0;
                    }

                    if (timerToReset > maxTimeToReset) {
                        timerToReset = 0;
                        Debug.Log("Bruh");
                        RestartScene();
                    }

                    //Si el juego no esta pausado
                    if (!isPaused) {
                        timeElapsed += Time.deltaTime;

                        if (!setSpawnPoint) {
                            GameObject playerObject = GameObject.FindGameObjectWithTag("Player");
                            if (playerObject.GetComponent<PlayerController>().worldAssignation == DoubleObject.world.DUSK) {
                                spawnPoint = playerObject.transform.position;
                            } else {
                                spawnPoint = playerObject.transform.position - new Vector3(0, worldOffset);

                            }
                            setSpawnPoint = true;
                        }

                        //CAMBIO DE MUNDO
                        if (Input.GetKeyDown(KeyCode.LeftShift)) {
                            if (currentPlayer != null) {
                                if (!currentPlayer.crawling) {
                                    if (gameObject.GetComponent<AudioSource>().pitch == 1.5) {
                                        gameObject.GetComponent<AudioSource>().pitch = 0.5f;
                                    } else {
                                        gameObject.GetComponent<AudioSource>().pitch = 1.5f;
                                    }
                                    gameObject.GetComponent<AudioSource>().Play();
                                    foreach (GameObject g in transformableObjects) {
                                        g.GetComponent<DoubleObject>().Change();
                                    }
                                }
                            }
                        }

                    }
                    //Si el juego SI esta pausado
                    else {


                    }
                    //En cualquier caso se comprueba el input
                    CheckPauseInput();
                } else {
                    //LevelFinishStuff
                    Save();


                }
            } else {
                currentPlayer = null;
                Cursor.visible = true;
                Cursor.lockState = CursorLockMode.None; //Se puede volver a mover el cursor
            }
        }
    }

    public void LoadScene(int which) {
        instance.levelFinished = false;
        SceneManager.LoadScene(which);
        ResetSceneData();
    }

    //Método para cargar el menu desde cualquier escena
    public void LoadMenu() {
        instance.levelFinished = false;
        SceneManager.LoadScene(0);
        ResetSceneData();
    }

    //Método para reiniciar la escena
    public void RestartScene() {
        instance.levelFinished = false;
        SceneManager.LoadScene(currentSceneName);
        isPaused = false;
        ResetSceneData();
    }

    //Método que controla cuando se le da al escape para pausar, a su vez activa y desactiva el cursor en función de si 
    //Se abre el menú in game y se modificar la variable isPaused
    void CheckPauseInput() {
        if (Input.GetKeyDown(KeyCode.Escape)) {
            if (isPaused) {
                Cursor.visible = false;
                Cursor.lockState = CursorLockMode.None; //Se engancha el cursor en el centro de la pantalla.
            } else if (!isPaused) {
                Cursor.visible = true;
                Cursor.lockState = CursorLockMode.Locked; //Se engancha el cursor en el centro de la pantalla.
            }
            isPaused = !isPaused;
        }
    }

    public void Save() {
        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = File.Create(Application.persistentDataPath + "/playerInfoSave.dat");
        PlayerData data = new PlayerData();

        //Igualar variables a cargar (Locales) a las de data

        data.firstOpening = firstOpening;
        data.completedLevels = completedLevels;

        bf.Serialize(file, data);
        file.Close();
    }

    void InitLoadSaveVariables() {
        firstOpening = true;
        completedLevels[1] = false;
        completedLevels[2] = false;
        completedLevels[3] = false;
        Save();
    }

    public void Load() {
        Debug.Log(Application.persistentDataPath);
        if (File.Exists(Application.persistentDataPath + "/playerInfoSave.dat")) {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Open(Application.persistentDataPath + "/playerInfoSave.dat",FileMode.Open);
            PlayerData data = (PlayerData)bf.Deserialize(file);
            file.Close();

            firstOpening = data.firstOpening;
            completedLevels = data.completedLevels;
            //Igualar variables a cargar (Locales) a las de data

        } else {
            InitLoadSaveVariables();
        }


    }

    [Serializable]
    class PlayerData{
        public bool firstOpening;
        public Dictionary<int, bool> completedLevels;

    };

}