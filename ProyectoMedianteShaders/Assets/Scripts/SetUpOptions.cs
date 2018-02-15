using System.Collections;
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

        //AÑADIR LISTENERS PARA LOS CAMBIOS DE LOS DROPDOWN
        resolution.onValueChanged.AddListener(delegate {
            GameLogic.instance.resolutionSelected = resolution.value;
        });
        refreshRate.onValueChanged.AddListener(delegate {
            if(refreshRate.value == 0) {
                GameLogic.instance.fullscreen = true;
            }
            else {
                GameLogic.instance.fullscreen = false;
            }
        });
        refreshRate.onValueChanged.AddListener(delegate {
           if(refreshRate.value == 0) {
                GameLogic.instance.screenRefreshRate = 0;
           }
           else if(refreshRate.value == 1) {
                GameLogic.instance.screenRefreshRate = 30;
           }
           else if (refreshRate.value == 2) {
                GameLogic.instance.screenRefreshRate = 60;
           }
           else if(refreshRate.value == 3) {
                GameLogic.instance.screenRefreshRate = 120;
           }
        });

        aceptar.onClick.AddListener(delegate {
            GameLogic.instance.changeGameSettings();
            optionsCanvas.SetActive(false);
        });
        cancelar.onClick.AddListener(delegate {
            //Guardar valores previos de cada opcion
            //Si se pulsa cancelar estos valores previos se le aplican al gamelogic
            optionsCanvas.SetActive(false);
        });
    }
}
