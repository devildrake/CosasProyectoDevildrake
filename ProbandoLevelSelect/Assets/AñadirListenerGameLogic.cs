using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AñadirListenerGameLogic : MonoBehaviour {

    public int index;
    private Button myselfButton;

    void Start()
    {
        myselfButton = GetComponent<Button>();
        myselfButton.onClick.AddListener(() => GameLogic.instance.LoadMenu());
    }
}
