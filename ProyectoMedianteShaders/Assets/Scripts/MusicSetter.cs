using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicSetter : MonoBehaviour {
    bool done;
    public int songId;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        if (!done) {
            if (GameLogic.instance != null) {
                GameLogic.instance.PlaySong(songId);
                done = true;
            }
        } else {
            Destroy(gameObject);
        }
	}
}
