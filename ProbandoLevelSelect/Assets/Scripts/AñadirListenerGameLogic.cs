using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//Script para añadir un listener al método GameLogic.LoadMenu() a los botones que pertoque;
public class AñadirListenerGameLogic : MonoBehaviour {

    public int index;
    private Button myselfButton;

    void Start() {
        myselfButton = GetComponent<Button>();

        switch (index) {
            case 0:
                myselfButton.onClick.AddListener(() => GameLogic.instance.LoadMenu());
                break;
            case 1:
                myselfButton.onClick.AddListener(() => GameLogic.instance.RestartScene());
                break;
            default:
                break;
    }
    }
}
