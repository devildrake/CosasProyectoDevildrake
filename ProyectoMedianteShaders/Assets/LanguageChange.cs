using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LanguageChange : MonoBehaviour {
    [SerializeField] private string key;
    public Main m;
    private Text t;

    private void Start() {
        t = GetComponent<Text>();
        t.text = m.language[key];
    }

    public void Change() {
        t.text = m.language[key];
    }
}
