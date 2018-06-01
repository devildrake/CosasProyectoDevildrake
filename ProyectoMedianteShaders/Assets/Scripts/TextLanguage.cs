
using UnityEngine;
using UnityEngine.UI;

public class TextLanguage : MonoBehaviour {
    public MessagesFairy.LANGUAGE prevLanguage;
    public string EngText;
    public string SpaText;

    Text text;
	// Use this for initialization
	void Start () {
        text = GetComponent<Text>();
        prevLanguage = MessagesFairy.LANGUAGE.None;
    }

// Update is called once per frame
    void Update () {
        if (GameLogic.instance != null) {
            if (prevLanguage != GameLogic.instance.currentLanguage) {
                prevLanguage = GameLogic.instance.currentLanguage;
                if (GameLogic.instance.currentLanguage == MessagesFairy.LANGUAGE.English) {
                    if (EngText != null) {
                        text.text = EngText;
                    }
                } else {
                    if (SpaText != null) {
                        text.text = SpaText;
                    }
                }
            }
        }
    }
}
