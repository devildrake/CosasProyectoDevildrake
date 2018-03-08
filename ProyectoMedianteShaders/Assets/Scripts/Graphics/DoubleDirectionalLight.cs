using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoubleDirectionalLight : DoubleObject {

    public Color duskColor, dawnColor;
    public float duskIntensity, dawnIntensity;
    public Material duskSkybox, dawnSkybox;

	void Start () {
        if (dawn) {
            //GetComponent<Light>().color = dawnColor;
            //GetComponent<Light>().intensity = dawnIntensity;
            RenderSettings.skybox = dawnSkybox;
        }
        else {
            //GetComponent<Light>().color = duskColor;
            //GetComponent<Light>().intensity = duskIntensity;
            RenderSettings.skybox = duskSkybox;
        }
	}
	
	void Update () {
        AddToGameLogicList();	
	}

    public override void Change() {
        base.Change();

        if (dawn) {
            //GetComponent<Light>().color = dawnColor;
            //GetComponent<Light>().intensity = dawnIntensity;
            RenderSettings.skybox = dawnSkybox;
        }
        else {
            //GetComponent<Light>().color = duskColor;
            //GetComponent<Light>().intensity = duskIntensity;
            RenderSettings.skybox = duskSkybox;
        }
    }
}
