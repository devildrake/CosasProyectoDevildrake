using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/*
 * Al activar el canvas de opciones se añaden todas las opciones a los
 * elementos del menu.
 */
 [Obsolete("Class deprecated, use options_logic instead")]
public class SetUpOptions : MonoBehaviour {
    public Slider music, sfx;
    public Dropdown resolution, fullscreen, refreshRate, fps;
    public Button aceptar, cancelar;
    public GameObject optionsCanvas; //referencia al canvas de todo el menu de opciones para cerrarlo despues de aceptar o cancelar.
    public Scrollbar scroll;
    public Toggle mute, timeOnScreen;
    public float maxScroll = 50;
    [SerializeField] private int slideItemOffset = 205;//distancia de separación entre elementos en nuestro slider de las opciones.
    private List<Vector2> resolutions = new List<Vector2> {new Vector2(640,480), new Vector2(800,600), new Vector2(1024,600), new Vector2(1280,720),
                                                            new Vector2(1280,1024), new Vector2(1400,1050), new Vector2(1600, 900), new Vector2(1920,1080)};
    private List<int> fpsList = new List<int> { -1, 30, 60, 90, 120 };
    private const int qualityElements = 6;
    
    //valores previos de las variables
    private float prevMusic, prevSfx;
    private int prevResolution, prevFullscreen, prevRefreshRate, prevFps;

    private int resolutionSel = 0, fpsSel = 0, qualitySel = 0; //Que elemento de nuestro slider estas seleccionando.

    private Vector3 initialOptionsPosition;

    [SerializeField] private GameObject resolutionGroup, fpsGroup, qualityGroup;

    private void Start() {
        initialOptionsPosition = GetComponent<RectTransform>().localPosition; //para controlar el scroll

        music.maxValue = 1.0f;
        music.minValue = 0.0f;
        music.value = 1.0f;

        sfx.maxValue = 1.0f;
        sfx.minValue = 0.0f;
        sfx.value = 1.0f;

        mute.isOn = false;

        //Configuracion del dropdown de las resoluciones
        resolution.ClearOptions();
        int currentResolution = 0;
        List<string> resolutionOptions = new List<string>();
        for (int i = 0; i<Screen.resolutions.Length; i++) {
            resolutionOptions.Add("" + Screen.resolutions[i]);
            if(Screen.resolutions[i].Equals(Screen.currentResolution)) {
                currentResolution = i;
                GameLogic.instance.resolutionSelected = new Vector2(0,0);
            }
        }
        resolution.AddOptions(resolutionOptions);
        resolution.value = currentResolution;

        //Configuracion del dropdown de fullscreen
        fullscreen.ClearOptions();
        List<string> fullscreenOptions = new List<string> { "Activada", "Desactivada" };
        fullscreen.AddOptions(fullscreenOptions);
        if (Screen.fullScreen) {
            fullscreen.value = 0;
        }
        else {
            fullscreen.value = 1;
        }

        //Configuracion del dropdown del refresh rate de la pantalla
        refreshRate.ClearOptions();
        List<string> refreshRateOptions = new List<string> { "Automatico", "30Hz", "60Hz", "120Hz" };
        refreshRate.AddOptions(refreshRateOptions);
        refreshRate.value = 0;

        //Configuracion del dropdown de la limitacion de fps
        fps.ClearOptions();
        List<string> fpsOptions = new List<string> { "Sin limitación", "30fps", "60fps", "90fps", "120fps" };
        fps.AddOptions(fpsOptions);
        fps.value = 0;

        //TODO on value changed para la scrollbar
        scroll.onValueChanged.AddListener(delegate {
            GetComponent<RectTransform>().localPosition = initialOptionsPosition + new Vector3(0, scroll.value*maxScroll, 0);
        });

        aceptar.onClick.AddListener(delegate {
            //guardamos los valores cambiados
            prevMusic = music.value;
            prevSfx = sfx.value;
            prevResolution = resolution.value;
            prevFullscreen = fullscreen.value;
            prevRefreshRate = refreshRate.value;
            prevFps = fps.value;

            //Añadir valores al gamelogic
            //Resolucion
            GameLogic.instance.resolutionSelected = new Vector2(0,0);
            //Fullscreen
            if (fullscreen.value == 0) {
                GameLogic.instance.fullscreen = true;
            }
            else {
                GameLogic.instance.fullscreen = false;
            }
            //Screen refresh rate
            //if (refreshRate.value == 0) {
            //    GameLogic.instance.screenRefreshRate = 0;
            //}
            //else if (refreshRate.value == 1) {
            //    GameLogic.instance.screenRefreshRate = 30;
            //}
            //else if (refreshRate.value == 2) {
            //    GameLogic.instance.screenRefreshRate = 60;
            //}
            //else if (refreshRate.value == 3) {
            //    GameLogic.instance.screenRefreshRate = 120;
            //}

            //fps
            if(fps.value == 0) {
                GameLogic.instance.maxFrameRate = -1;
            }
            else if(fps.value == 1) {
                GameLogic.instance.maxFrameRate = 30;
            }
            else if(fps.value == 2) {
                GameLogic.instance.maxFrameRate = 60;
            }
            else if(fps.value == 3) {
                GameLogic.instance.maxFrameRate = 90;
            }
            else if(fps.value == 4){
                GameLogic.instance.maxFrameRate = 120;
            }

            //music
            GameLogic.instance.musicVolume = music.value;

            //sfx
            GameLogic.instance.sfxVolume = sfx.value;

            //mute
            GameLogic.instance.muteVolume = mute.isOn;

            GameLogic.instance.changeGameSettings();
            optionsCanvas.SetActive(false);
        });



        cancelar.onClick.AddListener(delegate {
            music.value = prevMusic;
            sfx.value = prevSfx;
            resolution.value = prevResolution;
            fullscreen.value = prevFullscreen;
            refreshRate.value = prevRefreshRate;
            fps.value = prevFps;
            optionsCanvas.SetActive(false);
        });
    }

    /*
     * Cuando se activa el menu se guarda el estado en el que estaba en ese
     * momento por si se pulsa cancelar
     */
    private void OnEnable() {
        prevMusic = music.value;
        prevSfx = sfx.value;
        prevResolution = resolution.value;
        prevFullscreen = fullscreen.value;
        prevRefreshRate = refreshRate.value;
        prevFps = fps.value;
        scroll.value = 0;

        //Cuando se activa ponemos las opciones con el scroll a 0;
        //GetComponent<RectTransform>().localPosition = initialOptionsPosition;

        //Al activar ponemos nuestros sliders en la posición del elemento que está seleccionado

    }

    private void Update() {
        if(InputManager.instance.cancelButton) {
            music.value = prevMusic;
            sfx.value = prevSfx;
            resolution.value = prevResolution;
            fullscreen.value = prevFullscreen;
            refreshRate.value = prevRefreshRate;
            fps.value = prevFps;
            optionsCanvas.SetActive(false);
        }
    }

    /*
     * Controla los botones de las opciones para deslizar la seleccion a derecha e izquierda.
     * Recibe un gameobject que es el que se va a deslizar y un bool que marca si va a la derecha (true)
     * o a la izquierda (false)
     */
    public void MyScrollingOptionRight(GameObject go) {
        if (go.name.Equals("resolution_group")) {
            if(resolutionSel < resolutions.Count - 1) {
                go.transform.position -= new Vector3(slideItemOffset, 0, 0);
                resolutionSel++;
            }
        }
        else if (go.name.Equals("fps_group")) {
            if(fpsSel < fpsList.Count - 1) {
                go.transform.position -= new Vector3(slideItemOffset, 0, 0);
                fpsSel++;
            }
        }
        else if (go.name.Equals("calidad_group")) {
            if(qualitySel < qualityElements - 1) {
                go.transform.position -= new Vector3(slideItemOffset, 0, 0);
                qualitySel++;
            }
        }
    }

    public void MyScrollingOptionsLeft(GameObject go) {
        if (go.name.Equals("resolution_group")) {
            if (resolutionSel > 0) {
                go.transform.position += new Vector3(slideItemOffset, 0, 0);
                resolutionSel--;
            }
        }
        else if (go.name.Equals("fps_group")) {
            if(fpsSel > 0) {
                go.transform.position += new Vector3(slideItemOffset, 0, 0);
                fpsSel--;
            }
        }
        else if (go.name.Equals("calidad_group")) {
            if(qualitySel > 0) {
                go.transform.position += new Vector3(slideItemOffset, 0, 0);
                qualitySel--;
            }
        }
    }
}
