using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoubleDirectionalLight : DoubleObject {

    [SerializeField]private float dawnLighIntensity = 1, duskLightIntensity = 1;
    public Material duskSkybox, dawnSkybox;
    private Light changingLight;

	void Start () {
        changingLight = GetComponent<Light>();
        if (dawn) {
            RenderSettings.skybox = dawnSkybox;
            changingLight.intensity = dawnLighIntensity;
        }
        else {
            RenderSettings.skybox = duskSkybox;
            changingLight.intensity = duskLightIntensity;
        }
        print(RenderSettings.skybox);
	}
	
	void Update () {
        AddToGameLogicList();	
	}

    public override void Change() {
        base.Change();

        if (dawn) {
            changingLight.intensity = dawnLighIntensity;
            RenderSettings.skybox = dawnSkybox;
        }
        else {
            changingLight.intensity = duskLightIntensity;
            RenderSettings.skybox = duskSkybox;
        }
    }
}
