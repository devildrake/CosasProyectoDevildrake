using UnityEngine;
using UnityEngine.UI;

/*
 * Esta clase actualiza el texto de UI leyendo directamente del json de idioma que tiene cargado el gamelogic.
 */
public class TextLanguage : MonoBehaviour {
    public string key;
    Text t;

	void Start () {
        t = GetComponent<Text>();
        t.text = GameLogic.instance.languageData[key];
    }

    /*
     * Este metodo lo llama gamelogic cuando detecta un cambio en el idioma.
     */
    public void Change() {
        t.text = GameLogic.instance.languageData[key];
    }
}
