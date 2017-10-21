using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//Script para añadir un listener al método GameLogic.LoadMenu() a los botones que pertoque;
public class AñadirListenerGameLogic : MonoBehaviour {

    public int index;
    private Button myselfButton;

    void Start()
    {
        myselfButton = GetComponent<Button>();
        myselfButton.onClick.AddListener(() => GameLogic.instance.LoadMenu());
    }
}
