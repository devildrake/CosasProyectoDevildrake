using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameLogic : MonoBehaviour
{
    public static GameLogic instance = null;              //Static instance of GameManager which allows it to be accessed by any other script.
    [SerializeField]
    private bool isInMainMenu;
    private bool checkMainMenu;
    private bool waitAFrame;
    private MenuScripts menuScripts;
    static string currentSceneName = null;
    public PauseCanvas pauseCanvas;


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

    void setWaitAFrame(bool a)
    {
        waitAFrame = a;
    }

    void Start()
    {
        setWaitAFrame(false);

    }

    void Update()
    {
        if (currentSceneName != SceneManager.GetActiveScene().name)
        {
            setWaitAFrame(false);
            checkMainMenu = false;
            isInMainMenu = false;
            currentSceneName = SceneManager.GetActiveScene().name;

        }


        //Espera de un frame para comprobar que se ejecuta en el orden deseado
        if (!waitAFrame)
        {
            waitAFrame = true;
        }

        //Una vez realizada la espera, sin haber comprobado si se esta en el menu principal
        else if(!checkMainMenu)
        {
            menuScripts = FindObjectOfType<MenuScripts>();
            if (menuScripts != null)
            {
                isInMainMenu = true;
            }
            checkMainMenu = true;
        }

        //Una vez comprobado si estamos o no en el menu principal
        else
        {

        }
    }

    public void LoadMenu()
    {
        SceneManager.LoadScene(0);
    }
}