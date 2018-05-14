using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoubleMessage : Transformable {

    DoubleMessage(Sprite sp) {
        spriteRenderer.sprite = sp;
    }

    DoubleMessage brotherScript;
    SpriteRenderer spriteRenderer;
    float myAlpha=1;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        AddToGameLogicList();	
	}

    void FadeIn() {
        if (myAlpha < 0) {
            myAlpha = 0;
        }


        if (spriteRenderer.color.a < 0.75f) {
            myAlpha += Time.deltaTime;
            if (spriteRenderer.sprite != null) {
                spriteRenderer.color = new Color(spriteRenderer.color.r, spriteRenderer.color.g, spriteRenderer.color.b, myAlpha);
            }
        }
        brotherScript.myAlpha = myAlpha;

    }

    void FadeOut() {
        if (myAlpha < 0) {
            myAlpha = 0;
        }

        if (spriteRenderer.color.a > 0) {

            myAlpha -= Time.deltaTime;
            if (spriteRenderer.sprite != null) {
                spriteRenderer.color = new Color(spriteRenderer.color.r, spriteRenderer.color.g, spriteRenderer.color.b, myAlpha);
            }
        }
        brotherScript.myAlpha = myAlpha;

    }


}
