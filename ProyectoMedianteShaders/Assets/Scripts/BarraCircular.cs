using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BarraCircular : MonoBehaviour {
    public Transform barraCarga;
    public Transform tiempoTexto;

	// Use this for initialization
	void Start () {

    }
	
	// Update is called once per frame
	void Update () {
        if (GameLogic.instance!=null) {
            barraCarga.GetComponent<Image>().fillAmount =  GameLogic.instance.timerToReset / GameLogic.instance.maxTimeToReset;
            tiempoTexto.GetComponent<Text>().text = (Mathf.Ceil(GameLogic.instance.maxTimeToReset - GameLogic.instance.timerToReset)).ToString();
        }

	}
}
