using UnityEngine;
using UnityEngine.UI;

public class ImageLanguage : MonoBehaviour {
    public MessagesFairy.LANGUAGE prevLanguage;
    public Sprite EngText;
    public Sprite SpaText;

    Image text;
    // Use this for initialization
    void Start() {
        text = GetComponent<Image>();
        prevLanguage = MessagesFairy.LANGUAGE.None;
    }

    // Update is called once per frame
    void Update() {
        if (GameLogic.instance != null) {
            if (prevLanguage != GameLogic.instance.currentLanguage) {
                prevLanguage = GameLogic.instance.currentLanguage;
                if (GameLogic.instance.currentLanguage == MessagesFairy.LANGUAGE.English) {
                    if (EngText != null) {
                        text.sprite = EngText;
                    }
                } else {
                    if (SpaText != null) {
                        text.sprite = SpaText;
                    }
                }
            }
        }
    }
}
