using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseCanvas : MonoBehaviour {
    private bool isPaused;
    public GameObject gris;
    bool once = false;
	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
        if(GameLogic.instance.pauseCanvas == null)
        {
            GameLogic.instance.pauseCanvas = this;
        }


        if (Input.GetKeyDown(KeyCode.Escape)&&!once)
        {
            isPaused = !isPaused;
            gris.SetActive(isPaused);
            once = true;
        }
        if (Input.GetKeyUp(KeyCode.Escape) && once)
        {
            once = false;
        }




    }
}
