using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraDataProvider : MonoBehaviour {

    public float farDistance;
    public float closeDistance;
    public Vector3 offset;
    CameraScript[] cameraScripts;

    private void Awake() {
        cameraScripts = new CameraScript[2];
        GameObject camObj =  GameObject.Find("Cameras");
        cameraScripts = camObj.GetComponentsInChildren<CameraScript>();
        cameraScripts[0].farDistance = farDistance;
        cameraScripts[0].closeDistance = closeDistance;
        cameraScripts[0].transform.position = transform.position;
        cameraScripts[0].offset = offset;
        cameraScripts[0].OffsetX = offset.x;

        cameraScripts[1].farDistance = farDistance;
        cameraScripts[1].closeDistance = closeDistance;
        cameraScripts[1].transform.position = transform.position;
        cameraScripts[1].offset = offset;
        cameraScripts[1].OffsetX = offset.x;

        Destroy(gameObject);
    }

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
