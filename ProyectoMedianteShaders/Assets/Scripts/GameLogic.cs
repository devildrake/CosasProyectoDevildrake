using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using FMOD.Studio;
using UnityEngine.PostProcessing;

[Serializable]
public class LevelData {
    public bool fragment;
    public bool fragmentFeedBack;
    public bool completed;
    public bool completedFeedBack;
    public float timeLapse;
    public bool timeLapseFeedBack;
}

//Clase singleton (solo debe existir uno) referenciable a partir de una instancia estatica a si mismo que gestiona
//si el jugador se encuentra en el menu 

public class GameLogic : MonoBehaviour {
    private const string GAME_VERSION = "1.0";

    public MessagesFairy.LANGUAGE currentLanguage;
    public int levelToLoad;

    public PostProcessingBehaviour[] ppp; //referencias a los perfiles de post processo
    public PostProcessingProfile pppDkNormal, pppDnNormal, pppDkClosed, pppDnClosed;
    private const float iIntensity = 0.627f, iSmoothness = 0.508f, iRoundness = 0.506f; //Valores de configuración del vignetting normal
    private const float dIntensity = 0.771f, dSmoothness = 0.411f, dRoundness = 0.95f; //Valores de configuración del vignetting en modo conversación.


    SoundManager soundManager;
    static bool awoken = false;
    public EventInstance musicEvent;
    //////////////////////VARIABLES LOAD/SAVE////////////////////////////
    //////////////////////VARIABLES LOAD/SAVE////////////////////////////
    //////////////////////VARIABLES LOAD/SAVE////////////////////////////
    int prevSongId;
    public bool saved;
    //public bool firstOpening;
    public bool playedTutorial;

    //public Dictionary<int, bool> completedLevels;
    //public Dictionary<int, bool> fragments;
    public Dictionary<int, LevelData> levelsData;
    public int lastEntranceIndex;
    public int[] interactuableLevelIndexes = { 2 };
    //public Dictionary<int,FragmentData> savedFragments;


    //////////////////////VARIABLES LOAD/SAVE////////////////////////////
    //////////////////////VARIABLES LOAD/SAVE////////////////////////////
    //////////////////////VARIABLES LOAD/SAVE////////////////////////////

    public bool canBePaused = true;
    public bool showTimeCounter = false;
    public string levelName = "NonSet";

    //Flag para la transición inicial de la camara
    public bool cameraTransition;
    public Vector3 additionalOffset;
    public float cameraAttenuation;


    public PlayerController currentPlayer;

    //private int lastFragmentId = -1;

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
    //[SerializeField]
    //private bool isInMainMenu;
    public enum GameState { LEVEL, MENU, LOADINGSCREEN }
    public GameState gameState = GameState.MENU;
    public enum EventState { NONE, TEXT, IMAGE }
    public EventState eventState;

    [HideInInspector]
    public bool setSpawnPoint;

    //Booleano que determina si se ha mirado si se esta en el menu principal o no
    private bool checkMainMenu;

    //Booleano que gestiona la espera de un frame para la busqueda de referencias
    private bool waitAFrame;

    //Referencia a MenuLogic (Solo deja de ser null en el menu principal)
    private MenuLogic menuScripts;

    //Nombre de la escena actual
    static string currentSceneName = null;
    public PauseCanvas pauseCanvas;

    public List<GameObject> transformableObjects;

    public Vector3 spawnPoint;

    public float worldOffset;

    bool dawn = false;
    float dawnValue = 0;

    public bool newVersion = false; //Si hay una nueva versión más actualizada del juego se pone a true para que se lance un aviso.

    ///////////////////////////////OPCIONES///////////////////////////
    ///////////////////////////////OPCIONES///////////////////////////
    ///////////////////////////////OPCIONES///////////////////////////
    ///////////////////////////////OPCIONES///////////////////////////
    //Variables para controlar las opciones del juego
    //Se inicializan en valores por defecto para evitar errores
    [HideInInspector] public float musicVolume = 1; //modificador del volumen de la musica
    [HideInInspector] public float sfxVolume = 1; //modificador del volumen de los efectos de sonido
    [HideInInspector] public bool muteVolume = false; //controla si todo el audio esta desactivado
    [HideInInspector] public bool mostrarTiempo = false; //controla si se muestra el cronometro en partida
    [HideInInspector] public bool fullscreen; //controlar el fullscreen
    [HideInInspector] public int graficos = 0; //Varia la configuración grafica del juego
    [HideInInspector] public int maxFrameRate = -1; //limita el framerate
    [HideInInspector] public Vector2 resolutionSelected = new Vector2(1920, 1080); //que resolucion de pantalla se ha escogido. Se inicializa desde SetUpMenu

    FMOD.Studio.ParameterInstance dawnParameter;

    [HideInInspector] public Dictionary<string, string> languageData; //Se guarda la info leida desde el archivo json en formato key-value

    #region OPTIONS
    /*
     * Conjunto de metodos que se llamaran al cambiar las opciones de juego
     */
    public void ChangeGameSettings() {
        //Mostrar tiempo de partida
        showTimeCounter = mostrarTiempo;

        //CONFIGURACION DE PANTALLA
        Screen.SetResolution((int)resolutionSelected.x, (int)resolutionSelected.y, fullscreen, 0);
        Application.targetFrameRate = maxFrameRate;

        //CONFIGURACION DE GRAFICOS
        QualitySettings.SetQualityLevel(graficos);
    }
    //=====================FINAL DE METODOS DE OPCIONES========================
    //=====================FINAL DE METODOS DE OPCIONES========================
    //=====================FINAL DE METODOS DE OPCIONES========================
    #endregion

    public void KillPlayer() {
        currentPlayer.Kill();
    }

    void Awake() {
        eventState = EventState.NONE;
        soundManager = SoundManager.Instance;



        cameraAttenuation = 1;
        //completedLevels = new Dictionary<int, bool>();
        //fragments = new Dictionary<int, bool>();
        levelsData = new Dictionary<int, LevelData>();

        Application.targetFrameRate = -1;
        fullscreen = Screen.fullScreen; //¿Está la aplicacion en pantalla completa?

        currentSceneName = SceneManager.GetActiveScene().name;



        //fragments = new List<FragmentData>();

        worldOffset = 300;

        //Check if instance already exists
        if (instance == null)

            //if not, set instance to this
            instance = this;

        //If instance already exists and it's not this:
        else if (instance != this) {

            //Then destroy this. This enforces our singleton pattern, meaning there can only ever be one instance of a GameManager.
            Destroy(gameObject);
        }


        //0--> dawn
        //1--> dusk
        //ppp = FindObjectsOfType<PostProcessingBehaviour>();
        if (instance.gameState == GameState.LEVEL) {
            if (ppp.Length > 1) {
                ppp[1].profile = pppDkNormal;
                ppp[0].profile = pppDnNormal;
            }
        }
        //VignetteModel vignetting1 = null, vignetting2 = null;

        //vignetting1 = ppp[0].profile.vignette;
        //vignetting2 = ppp[1].profile.vignette;

        //vignetting1

        transform.parent = null;

        //Sets this to not be destroyed when reloading scene
        DontDestroyOnLoad(gameObject);

        //Call the InitGame function to initialize the first level 


        cameraTransition = true;

        if (!awoken) {
            Load();
            awoken = true;
        }


        //Cargar el json del idioma inicial
        languageData = new Dictionary<string, string>();
        string lan = "";
        switch (currentLanguage) {
            case MessagesFairy.LANGUAGE.English:
                lan = "en";
                break;
            case MessagesFairy.LANGUAGE.Spanish:
                lan = "es";
                break;
        }
        if (!LoadJSONFile(lan)) { //Se carga el JSON del idioma
            Debug.LogError("No se ha podido cargar el archivo de idioma correctamente");
        }

        StartCoroutine("CheckVersion");
    }

        #region CHECK_GAME_VERSION
    //Comprueva la ultima version del juego, si hay otra mas actual se lanzará un aviso
    IEnumerator CheckVersion() {
        WWW info = new WWW("duskndawn.000webhostapp.com/game_info/game_version.php");
        yield return info;
        string textInfo = info.text;

        if (!textInfo.Equals(GAME_VERSION)) {
            newVersion = true;
        }
    }
    #endregion


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

    public void SaveFragment(bool a) {

        //fragments[GetCurrentLevelIndex()] = a;
        levelsData[GetCurrentLevelIndex()].fragment = a;

        Debug.Log("Saving Fragment " + GetCurrentLevelIndex() + " as " + a);
        if (a) {
            pickedFragments++;
        }
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
        //currentLanguage = MessagesFairy.LANGUAGE.English;
        CheckVersion();
        prevSongId = -1;
        SetWaitAFrame(false);
        SetCheckMainMenu(false);
        setSpawnPoint = false;
        MessagesFairy.StartMessages();
        //Pongo esta condicion para ver si estamos en el menu principal
        //si no me machacaba la cancion.
        if (FindObjectOfType<MenuLogic>() == null) {
            changeWorldClip = Resources.Load<AudioClip>("Sounds/ChangeWorld");
            gameObject.GetComponent<AudioSource>().clip = changeWorldClip;
        }
    }

    //Se comprueba si la escena se ha cambiado
    bool ChangedScene() {
        if (currentSceneName != SceneManager.GetActiveScene().name) {
            Debug.Log("Detecting ChangedScene");
        }
        return currentSceneName != SceneManager.GetActiveScene().name;
    }

    //Método que reinicia la espera del frame para buscar referencias y reinicia el booleano isInMainMenu
    public void ResetSceneData() {
        Debug.Log("ResetSceneData");
        if (InputManager.instance != null) {
            InputManager.UnBlockInput();
        }

        //timeElapsed = 0;
        levelName = "NonSet";
        cameraAttenuation = 1;
        cameraTransition = true;
        SetWaitAFrame(false);
        //isInMainMenu = false;
        checkMainMenu = false;
        currentSceneName = SceneManager.GetActiveScene().name;
        DirectionCircle.SetOnce(true);
        //timeElapsed = 0;
        //Debug.Log("RESET");
        //pickedFragments = 0;
        additionalOffset = new Vector3(0, 0, 0);
        timesDied = 0;

        if (SoundManager.Instance != null) {
            SoundManager.Instance.StopAllEvents(true);
        }

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

    public void PlaySong(int songId) {
        if (songId != prevSongId) {
            if (!musicEvent.Equals(null)) {
                musicEvent.stop(STOP_MODE.ALLOWFADEOUT);
            }
            musicEvent = SoundManager.Instance.PlayMusic("event:/Music/song" + songId.ToString(), transform.position);
            prevSongId = songId;
            SoundManager.Instance.music = musicEvent;

            FMOD.RESULT result = musicEvent.getParameter("Dawn", out dawnParameter);

            //Debug.Log(result);


            //Debug.Log("PlaySong");
        }
    }

    public void UpdateEventParameter(EventInstance soundEvent, SoundManagerParameter parameter) {
        soundEvent.setParameterValue(parameter.GetName(), parameter.GetValue());
    }

    void Update() {

        if (!soundManager.music.Equals(null)) {
            if (dawn) {
                dawnValue += Time.deltaTime;
                dawnValue = Mathf.Clamp(dawnValue, 0, 1);
            } else {
                dawnValue -= Time.deltaTime;
                dawnValue = Mathf.Clamp(dawnValue, 0, 1);
            }

            dawnParameter.setValue(dawnValue);


            //SoundManager.Instance.UpdateEventParameter(musicEvent, dawnParameter);

            //FMOD.RESULT result;

            //result = soundManager.music.setParameterValue("Dawn", dawnValue);

            //if (result == FMOD.RESULT.OK) {
            //    Debug.Log(dawnValue);
            //} else {
            //    Debug.Log(result);
            //}
        }

        if (currentPlayer != null) {
            if (Input.GetKey(KeyCode.N)) {
                Debug.Log("Trying to load scene" + instance.pauseCanvas.nextSceneIndex);
                instance.LoadScene(instance.pauseCanvas.nextSceneIndex);
            }
        }

        transformableObjects.RemoveAll(isNull);

        TimeScaleStuff();

        //Se comprueba si ha habido cambio de escena, si lo ha habido se reinician los booleanos waitAFrame, checkMainMenu e isInManMenu además de actualizar la variable currentSceneName
        if (ChangedScene()) {
            ResetSceneData();
        }

        if (!SoundManager.Instance.Equals(null)) {
            if (!musicEvent.Equals(null)) {
                if (SoundManager.Instance.isPlaying(musicEvent)) {
                    //SoundManager.Instance.music.setParameterValue("Dawn", dawnValue);
                }
            }
        }

        //Espera de un frame para comprobar que se ejecuta en el orden deseado
        if (!waitAFrame) {
            SetWaitAFrame(true);
            SetCheckMainMenu(false);
            setSpawnPoint = false;
            Debug.Log("WaitAFrameFalse");
        }

        //Una vez realizada la espera, sin haber comprobado si se esta en el menu principal
        else if (!checkMainMenu) {
            Debug.Log("CheckMainMenuFalse");
            menuScripts = FindObjectOfType<MenuLogic>();
            if (menuScripts != null) {
                //isInMainMenu = true;
                Debug.Log("MENUSET");

                gameState = GameState.MENU;
            } else if (FindObjectOfType<LoadingScreenLogic>() != null) {
                gameState = GameState.LOADINGSCREEN;
                Debug.Log("LOADINGSCREENSET");


            } else {
                gameState = GameState.LEVEL;
            }
            SetCheckMainMenu(true);
        }

        //Una vez comprobado si estamos o no en el menu principal se pondria el comportamiento deseado
        else {
            switch (gameState) {
                case GameState.LEVEL:
                    switch (eventState) {
                        case EventState.NONE:
                            if (ppp.Length == 2) {
                                ppp[1].profile = pppDkNormal;
                                ppp[0].profile = pppDnNormal;
                            }
                            break;
                        case EventState.TEXT:
                            ppp[1].profile = pppDkClosed;
                            ppp[0].profile = pppDnClosed;
                            break;
                        case EventState.IMAGE:
                            ppp[1].profile = pppDkClosed;
                            ppp[0].profile = pppDnClosed;
                            break;
                    }

                    if (!levelFinished) {
                        if (/*Input.GetKey(KeyCode.R)*/InputManager.instance.resetButton) {
                            timerToReset += Time.deltaTime;
                        } else {
                            timerToReset = 0;
                        }

                        if (timerToReset > maxTimeToReset) {
                            timerToReset = 0;
                            //Debug.Log("Bruh");
                            RestartScene();
                        }

                        //Si el juego no esta pausado
                        if (!isPaused) {

                            if (currentPlayer != null) {
                                if (currentPlayer.placeToGo == null && currentSceneName != "MenuInteractuable") {
                                    timeElapsed += Time.deltaTime;
                                }

                                if (!setSpawnPoint) {
                                    GameObject playerObject = GameObject.FindGameObjectWithTag("Player");
                                    GameObject startingPoint = GameObject.FindGameObjectWithTag("Start");
                                    if (playerObject.GetComponent<PlayerController>().worldAssignation == DoubleObject.world.DUSK) {
                                        if (startingPoint != null) {
                                            spawnPoint = startingPoint.transform.position;
                                        } else {
                                            spawnPoint = playerObject.transform.position;
                                        }
                                    } else {
                                        if (startingPoint != null) {
                                            //spawnPoint = startingPoint.transform.position;
                                            spawnPoint = startingPoint.transform.position - new Vector3(0, worldOffset);
                                        } else {
                                            //spawnPoint = playerObject.transform.position;
                                            spawnPoint = playerObject.transform.position - new Vector3(0, worldOffset);
                                        }

                                        //spawnPoint = playerObject.transform.position - new Vector3(0, worldOffset);

                                    }
                                    setSpawnPoint = true;
                                }
                            }
                            if (currentPlayer != null) {
                                if (currentPlayer.dawn) {
                                    //CAMBIO DE MUNDO
                                    if (!cameraTransition) {
                                        if (InputManager.instance.changeButton && !InputManager.instance.prevChangeButton) {

                                            if (!currentPlayer.crawling) {

                                                if (gameObject.GetComponent<AudioSource>().pitch == 1.5) {
                                                    gameObject.GetComponent<AudioSource>().pitch = 0.5f;
                                                } else {
                                                    gameObject.GetComponent<AudioSource>().pitch = 1.5f;
                                                }
                                                gameObject.GetComponent<AudioSource>().Play();
                                                dawn = !dawn;
                                                foreach (GameObject g in transformableObjects) {
                                                    g.GetComponent<DoubleObject>().Change();
                                                }
                                            }
                                        }
                                    }
                                } else {
                                    if (!cameraTransition) {

                                        if (InputManager.instance.changeButton2 && !InputManager.instance.prevChangeButton2) {

                                            if (!currentPlayer.crawling) {
                                                if (gameObject.GetComponent<AudioSource>().pitch == 1.5) {
                                                    gameObject.GetComponent<AudioSource>().pitch = 0.5f;
                                                } else {
                                                    gameObject.GetComponent<AudioSource>().pitch = 1.5f;
                                                }
                                                gameObject.GetComponent<AudioSource>().Play();
                                                dawn = !dawn;
                                                foreach (GameObject g in transformableObjects) {
                                                    g.GetComponent<DoubleObject>().Change();
                                                }
                                            }
                                        }
                                    }
                                }
                            } else {
                                PlayerController[] playerObjects;
                                playerObjects = FindObjectsOfType<PlayerController>();
                                if (playerObjects.Length == 2) {
                                    for (int i = 0; i < 2; i++) {
                                        if (playerObjects[i].worldAssignation == DoubleObject.world.DUSK) {
                                            currentPlayer = playerObjects[i];
                                        }
                                    }
                                } else {
                                    Debug.Log("No encuentra players");
                                }
                            }
                        }
                        //Si el juego SI esta pausado
                        else {
                            Cursor.visible = true;
                            Cursor.lockState = CursorLockMode.None; //Se puede volver a mover el cursor

                        }
                        //En cualquier caso se comprueba el input
                        if (currentPlayer != null) {
                            CheckPauseInput();
                        }
                    } else {
                        //LevelFinishStuff
                        //Save();


                    }

                    break;
                case GameState.MENU:

                    currentPlayer = null;
                    Cursor.visible = true;
                    Cursor.lockState = CursorLockMode.None; //Se puede volver a mover el cursor
                    break;
                case GameState.LOADINGSCREEN:
                    break;
            }
        }

        if (InputManager.instance != null) {
            //InputManager.instance.UpdatePreviousGameLogic();
        }


    }

    public void LoadScene(string which) {
        instance.levelFinished = false;
        StartCoroutine(LoadYourAsyncScene(which));
        ResetSceneData();
        saved = false;
    }

    IEnumerator LoadYourAsyncScene(string name) {
        // The Application loads the Scene in the background as the current Scene runs.
        // This is particularly good for creating loading screens.
        // You could also load the Scene by using sceneBuildIndex. In this case Scene2 has
        // a sceneBuildIndex of 1 as shown in Build Settings.

        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(name);

        // Wait until the asynchronous scene fully loads
        while (!asyncLoad.isDone) {
            yield return null;
        }
    }

    IEnumerator LoadYourAsyncScene(int buildIndex) {
        // The Application loads the Scene in the background as the current Scene runs.
        // This is particularly good for creating loading screens.
        // You could also load the Scene by using sceneBuildIndex. In this case Scene2 has
        // a sceneBuildIndex of 1 as shown in Build Settings.

        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(buildIndex);

        // Wait until the asynchronous scene fully loads
        while (!asyncLoad.isDone) {
            yield return null;
        }
    }

    public void LoadScene(Scene which) {
        instance.levelFinished = false;
        //SceneManager.LoadScene(which.name);
        StartCoroutine(LoadYourAsyncScene(which.name));
        ResetSceneData();
        saved = false;
    }

    public void LoadScene(int which) {
        instance.levelFinished = false;
        //SceneManager.LoadScene(which);
        StartCoroutine(LoadYourAsyncScene(which));
        ResetSceneData();
        saved = false;
    }

    //Método para cargar el menu desde cualquier escena
    public void LoadMenu() {
        instance.levelFinished = false;
        SceneManager.LoadScene(0);
        ResetSceneData();
    }

    //Método para reiniciar la escena
    public void RestartScene() {
        saved = false;
        instance.levelFinished = false;
        //SceneManager.LoadScene(currentSceneName);
        StartCoroutine(LoadYourAsyncScene(currentSceneName));
        isPaused = false;
        ResetSceneData();
    }

    //Método que controla cuando se le da al escape para pausar, a su vez activa y desactiva el cursor en función de si 
    //Se abre el menú in game y se modificar la variable isPaused
    void CheckPauseInput() {
        //print(isPaused);
        if ((currentPlayer.dawn && InputManager.instance.pauseButton && !InputManager.instance.prevPauseButton) || !currentPlayer.dawn && InputManager.instance.pauseButton2 && !InputManager.instance.prevPauseButton2 && canBePaused) {
            /*if (isPaused) {
                Cursor.visible = false;
                Cursor.lockState = CursorLockMode.None;
            } else*/
            //print("PAUSAAAA");
            if (!isPaused) {
                isPaused = true;
                Cursor.visible = true;
                Cursor.lockState = CursorLockMode.Locked; //Se engancha el cursor en el centro de la pantalla.
                return;
            }
            //isPaused = !isPaused;
        }
        if (!isPaused) {
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.None;
        }
    }

    public void SetPause(bool p) {
        isPaused = p;
    }

    public void FinishTutorial() {
        playedTutorial = true;
        Save();
    }

    public void Save() {


        //if (!saved) {
            levelsData[GetCurrentLevelIndex()].completed = levelFinished;
            levelsData[GetCurrentLevelIndex()].timeLapse = timeElapsed;


            //Debug.Log("SAVE");
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Create(Application.persistentDataPath + "/saveData_2.15.dat");
            PlayerData data = new PlayerData();

            //Igualar variables a cargar (Locales) a las de data
            data.playedTutorial = playedTutorial;
            //data.firstOpening = true;
        
//          Debug.Log("Saving firstOpening as" + data.firstOpening);
            //data.completedLevels = completedLevels;
            //data.fragments = fragments;
            data.lastEntranceIndex = lastEntranceIndex;


            data.levelData = levelsData;
            bf.Serialize(file, data);
            file.Close();
           //saved = true;
        //}
    }

    void InitLoadSaveVariables() {
        Debug.Log("InoitVariables");
        PlayerPrefs.SetFloat("musicVolume", musicVolume);
        PlayerPrefs.SetFloat("sfxVolume", sfxVolume);
        Debug.Log("Setting mostratTiempo as" + mostrarTiempo);
        PlayerPrefs.SetInt("mostrarTiempo", Options_Logic.BoolToInt(mostrarTiempo));
        PlayerPrefs.SetFloat("resolutionSelectedX", resolutionSelected.x);
        PlayerPrefs.SetFloat("resolutionSelectedY", resolutionSelected.y);
        PlayerPrefs.SetInt("maxFrameRate", maxFrameRate);
        PlayerPrefs.SetInt("graficos", graficos);
        PlayerPrefs.SetInt("Language", 1);
        
        //firstOpening = false;
        playedTutorial = false;
        for (int i = 0; i < 30; i++) {
            //fragments[i] = false;
            //completedLevels[i] = false;
            levelsData[i] = new LevelData();
            levelsData[i].completed = false;
            levelsData[i].fragment = false;
            levelsData[i].timeLapse = 0.0f;
        }
        //completedLevels[2] = true;
        levelsData[2].completed = true;
        levelsData[3].completedFeedBack = true;
        levelsData[2].completedFeedBack = true;

        lastEntranceIndex = 3;

        Save();
    }

    public void Load() {
        Debug.Log("Cargando Archivos desde " + Application.persistentDataPath);
        if (File.Exists(Application.persistentDataPath + "/saveData_2.15.dat")) {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Open(Application.persistentDataPath + "/saveData_2.15.dat", FileMode.Open);
            PlayerData data = (PlayerData)bf.Deserialize(file);
            file.Close();

            //firstOpening = data.firstOpening;
            playedTutorial = data.playedTutorial;
            levelsData = data.levelData;
            lastEntranceIndex = data.lastEntranceIndex;

            #region debugStuff
            //completedLevels = data.completedLevels;
            //fragments = data.fragments;
            //levelsData[9].completed = true;
            //levelsData[9].fragmentFeedBack = false;
            //levelsData[9].timeLapseFeedBack = false;
            //levelsData[9].completedFeedBack = false;
            //levelsData[9].fragment = true;
            //levelsData[9].timeLapse = 0;
            //levelsData[10].completed = true;
            //levelsData[10].fragmentFeedBack = false;
            //levelsData[10].timeLapseFeedBack = false;
            //levelsData[10].completedFeedBack = false;
            //levelsData[10].fragment = true;
            //levelsData[10].timeLapse = 0;
            //levelsData[11].completed = true;
            //levelsData[11].fragmentFeedBack = false;
            //levelsData[11].timeLapseFeedBack = false;
            //levelsData[11].completedFeedBack = false;
            //levelsData[11].fragment = true;
            //levelsData[11].timeLapse = 0;
            //levelsData[8].completed = true;
            //levelsData[3].fragment = true;
            //levelsData[4].fragment = true;
            //levelsData[5].fragment = true;
            //Igualar variables a cargar (Locales) a las de data

            //for(int i=2;i<30;i++) {
            //    Debug.Log("Level->  " + i + " " + levelsData[i].timeLapse + " " + levelsData[i].completed);
            //}
            #endregion

        }
        else {
            InitLoadSaveVariables();
        }
    }

    [Serializable]
    class PlayerData{
        //public bool firstOpening, playedTutorial;
        public bool playedTutorial;
        public Dictionary<int, LevelData> levelData;
        public int lastEntranceIndex;
        //public Dictionary<int, bool> completedLevels;
        //public Dictionary<int, bool> fragments;
    };

    private void OnLevelWasLoaded(int level) {
        SetWaitAFrame(false);
        checkMainMenu = false;
    }

    #region JSON_METHODS
    public bool LoadJSONFile(string filename) {
        languageData.Clear();
        string path = Path.Combine(Path.Combine(Application.streamingAssetsPath, "lan"), filename + ".json");
        if (File.Exists(path)) {

            string json = File.ReadAllText(path);
            LocalizationData jsonItems = JsonUtility.FromJson<LocalizationData>(json);

            for (int i = 0; i < jsonItems.items.Length; i++) {
                languageData.Add(jsonItems.items[i].key, jsonItems.items[i].value);
            }
            return true;
        }
        else {
            Debug.LogError("Language file not found!");
            return false;
        }
    }

    public void ChangeLanguage(string lan) {
        if (LoadJSONFile(lan)) {
            PlayerPrefs.SetInt("Language", (int)currentLanguage);
            TextLanguage[] languageElements = FindObjectsOfType<TextLanguage>();
            foreach (TextLanguage l in languageElements) {
                l.Change();
            }
        }
    }
    #endregion
}