using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoubleDirectionalLight : DoubleObject {

    public Color duskColor, dawnColor;
    public float duskIntensity, dawnIntensity;

	void Start () {
        if (dawn) {
            GetComponent<Light>().color = dawnColor;
            GetComponent<Light>().intensity = dawnIntensity;
        }
        else {
            GetComponent<Light>().color = duskColor;
            GetComponent<Light>().intensity = duskIntensity;
        }
	}
	
	void Update () {
        AddToGameLogicList();	
	}

    public override void Change() {
        base.Change();

        if (dawn) {
            GetComponent<Light>().color = dawnColor;
            GetComponent<Light>().intensity = dawnIntensity;
        }
        else {
            GetComponent<Light>().color = duskColor;
            GetComponent<Light>().intensity = duskIntensity;
        }
    }
}
