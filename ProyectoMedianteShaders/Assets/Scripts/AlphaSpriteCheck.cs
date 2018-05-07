using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AlphaSpriteCheck : MonoBehaviour {
    private Image i;
    [SerializeField]
    [Range(0.0f,1.0f)]
    private float alphaThreshhold = 0.5f;

	// Use this for initialization
	void Start () {
        i = GetComponent<Image>();
        i.alphaHitTestMinimumThreshold = alphaThreshhold;
	}
}
