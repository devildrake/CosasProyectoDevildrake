using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LoadingIcon : MonoBehaviour {
    public Image dawnImage;
    public Image duskImage;
    public float delta;
    public bool up;
	// Use this for initialization
	void Start () {
        delta = 1;
        dawnImage.color = new Color(1,1,1,0);
        duskImage.color = new Color(1,1,1,0);
	}
	
	// Update is called once per frame
	void Update () {
        if (up) {
            if (delta < 1) {
                delta += Time.deltaTime;
            } else {
                up = false;
            }
        } else {
            if (delta > 0) {
                delta -= Time.deltaTime;
            } else {
                up = true;
            }
        }

        dawnImage.color = new Color(1, 1, 1, delta);
        duskImage.color = new Color(1, 1, 1, 1-delta);

    }
}
