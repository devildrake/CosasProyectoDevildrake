using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour {
    public bool dashButton;
    public bool deflectButton;
    public bool crawlButton;
    public bool jumpButton;

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {

        dashButton = (Input.GetAxisRaw("DashPunch")==1.0);
        deflectButton = (Input.GetAxisRaw("DeflectDrag") == 1.0);
        crawlButton = (Input.GetAxisRaw("CrawlSmash") == 1.0);
        jumpButton = (Input.GetAxisRaw("Jump") == 1.0);



    }
}
