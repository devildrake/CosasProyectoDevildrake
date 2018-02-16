﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/*
 * Al activar el canvas de opciones se añaden todas las opciones a los
 * elementos del menu.
 */
public class SetUpOptions : MonoBehaviour {
    public Slider music, sfx;
    public Dropdown resolution, fullscreen, refreshRate, fps;
    public Button aceptar, cancelar;
    public GameObject optionsCanvas; //referencia al canvas de todo el menu de opciones para cerrarlo despues de aceptar o cancelar.
    public Scrollbar scroll;

    //valores previos de las variables
    private float prevMusic, prevSfx;
    private int prevResolution, prevFullscreen, prevRefreshRate, prevFps;



    private void Start() {
        music.maxValue = 1.0f;
        music.minValue = 0.0f;
        music.value = 1.0f;

        sfx.maxValue = 1.0f;
        sfx.minValue = 0.0f;
        sfx.value = 1.0f;

        //Configuracion del dropdown de las resoluciones
        resolution.ClearOptions();
        int currentResolution = 0;
        List<string> resolutionOptions = new List<string>();
        for (int i = 0; i<Screen.resolutions.Length; i++) {
            resolutionOptions.Add("" + Screen.resolutions[i]);
            if(Screen.resolutions[i].Equals(Screen.currentResolution)) {
                currentResolution = i;
                GameLogic.instance.resolutionSelected = i;
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

        aceptar.onClick.AddListener(delegate {
            //Los cambios de los listeners los guardo localmente en el script
            //y cuando pulso aceptar se aplican al gamelogic y luego se llama 
            //al metodo de cambiar
            //Quiero que los valores solo se cambien en el gamelogic si pulso
            //aceptar, si no no tienen que variar y tienen que quedarse igual
            //que estaban antes.

            //guardamos los valores cambiados
            prevMusic = music.value;
            prevSfx = sfx.value;
            prevResolution = resolution.value;
            prevFullscreen = fullscreen.value;
            prevRefreshRate = refreshRate.value;
            prevFps = fps.value;

            //Añadir valores al gamelogic
            //Resolucion
            GameLogic.instance.resolutionSelected = resolution.value;
            //Fullscreen
            if (fullscreen.value == 0) {
                GameLogic.instance.fullscreen = true;
            }
            else {
                GameLogic.instance.fullscreen = false;
            }
            //Screen refresh rate
            if (refreshRate.value == 0) {
                GameLogic.instance.screenRefreshRate = 0;
            }
            else if (refreshRate.value == 1) {
                GameLogic.instance.screenRefreshRate = 30;
            }
            else if (refreshRate.value == 2) {
                GameLogic.instance.screenRefreshRate = 60;
            }
            else if (refreshRate.value == 3) {
                GameLogic.instance.screenRefreshRate = 120;
            }

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
    }
}
