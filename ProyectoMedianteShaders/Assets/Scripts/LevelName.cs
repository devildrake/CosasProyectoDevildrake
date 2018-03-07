using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelName : MonoBehaviour {
    float timer=0;
    float timeToStartFade = 3.0f;
    float fadeTime = 6.0f;
    GameObject levelNameObject;

    void Start() {
        levelNameObject = GetComponentInChildren<Text>().gameObject;
        //levelNameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update () {
        if (GameLogic.instance != null&&levelNameObject!=null) {

            if(GameLogic.instance.levelName!= "NonSet") {
                levelNameObject.GetComponent<Text>().text = GameLogic.instance.levelName;
            } else {
                levelNameObject.GetComponent<Text>().text = GameLogic.instance.GetCurrentLevel();
            }

            timer += Time.deltaTime;

            if (timer > timeToStartFade) {
                float remainingTime = fadeTime - timer;
                float delta = remainingTime / 3.0f;
                GetComponent<CanvasGroup>().alpha = delta;
                if(delta <= 0) {
                    timer = 0;
                    gameObject.SetActive(false);
                }
            }

        }
	}
}
