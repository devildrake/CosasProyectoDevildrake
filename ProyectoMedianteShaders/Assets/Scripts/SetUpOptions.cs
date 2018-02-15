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

    private void Start() {
        music.maxValue = 1.0f;
        music.minValue = 0.0f;
        music.value = 1.0f;

        sfx.maxValue = 1.0f;
        sfx.minValue = 0.0f;
        sfx.value = 1.0f;

        //Configuracion del dropdown de las resoluciones
        //resolution.captionText.text = ""+Screen.currentResolution;
        resolution.ClearOptions();
        int currentResolution = 0;
        List<string> resolutionOptions = new List<string>();
        for (int i = 0; i<Screen.resolutions.Length; i++) {
            resolutionOptions.Add("" + Screen.resolutions[i]);
            if(Screen.resolutions[i].Equals(Screen.currentResolution)) {
                currentResolution = i;
                print(Screen.resolutions[i]);
            }
        }
        resolution.AddOptions(resolutionOptions);

        //Configuracion del dropdown de fullscreen

        //Configuracion del dropdown del refresh rate de la pantalla

        //Configuracion del dropdown de la limitacion de fps
    }
}
