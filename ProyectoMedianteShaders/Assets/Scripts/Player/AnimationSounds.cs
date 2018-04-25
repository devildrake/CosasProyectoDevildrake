using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationSounds : MonoBehaviour {
    // Use this for initialization
    public bool active;
	void Start () {
        active = true;
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void PlayOneShot(string path) {
        if (active) {
            SoundManager.Instance.PlayOneShotSound(path, transform);
        } else {
            Debug.Log("NotActive");
        }
    }
}
