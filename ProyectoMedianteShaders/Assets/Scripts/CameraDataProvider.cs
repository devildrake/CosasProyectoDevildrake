using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraDataProvider : MonoBehaviour {
    public string levelName;
    public float farDistance;
    public float closeDistance;
    public Vector3 offset;
    CameraScript[] cameraScripts;

    public int nextSceneIndex;
    public float sceneMaxTime;
    public GameObject startPosObj;

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

    }

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        if (GameLogic.instance != null&&GameLogic.instance.pauseCanvas!=null) {
            GameLogic.instance.pauseCanvas.nextSceneIndex = nextSceneIndex;
            GameLogic.instance.levelToLoad = nextSceneIndex;

            GameLogic.instance.levelName = levelName;

            if (startPosObj != null) {
                if (GameLogic.instance.currentPlayer != null) {
                    if (!GameLogic.instance.setSpawnPoint) {
                        GameLogic.instance.setSpawnPoint = true;
                        GameLogic.instance.SetSpawnPoint(startPosObj.transform.position+new Vector3(1.2f,0));

                        if (GameLogic.instance.currentPlayer.worldAssignation == DoubleObject.world.DUSK) {
                            GameLogic.instance.currentPlayer.transform.position = startPosObj.transform.position+new Vector3(-2,0.25f);
                        } else {
                            GameLogic.instance.currentPlayer.transform.position = startPosObj.transform.position+new Vector3(-2,GameLogic.instance.worldOffset+0.25f,0);
                        }
                    } else {
                        Debug.Log("BRUH");
                        GameLogic.instance.SetSpawnPoint(startPosObj.transform.position + new Vector3(1.2f, 0));
                        if (GameLogic.instance.currentPlayer.worldAssignation == DoubleObject.world.DUSK) {
                                GameLogic.instance.currentPlayer.transform.position = startPosObj.transform.position + new Vector3(-2, 0.25f);
                            } else {
                                GameLogic.instance.currentPlayer.transform.position = startPosObj.transform.position + new Vector3(-2, GameLogic.instance.worldOffset + 0.25f, 0);
                            }
                    }
                    GameLogic.instance.gameState = GameLogic.GameState.LEVEL;
                    Destroy(gameObject);
                }
            } 



        }
    }
}
