using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowScript : MonoBehaviour {
    public SpriteRenderer sprite;
    public Sprite dashSprite;
    public Sprite punchSprite;
    public Sprite deflectSprite;
    string spritesPath = "Sprites";
    bool set;
	// Use this for initialization
	void Start () {
        sprite = GetComponentInChildren<SpriteRenderer>();
    }

    public void SetSprite(string skill) {
        //Debug.Log("Set sprite as " + skill);
        switch (skill) {
            case "dash":
                sprite.sprite = dashSprite;
                break;
            case "punch":
                sprite.sprite = punchSprite;
                break;
            case "deflect":
                sprite.sprite = deflectSprite;
                break;
        }

    }

    // Update is called once per frame
    void Update () {
        if (!set) {
            if (GameLogic.instance.currentPlayer != null) {
                gameObject.SetActive(false);
                GameLogic.instance.currentPlayer.arrow = this;
                GameLogic.instance.currentPlayer.brotherScript.arrow = this;
                set = true;
            }
        }
    }
}
