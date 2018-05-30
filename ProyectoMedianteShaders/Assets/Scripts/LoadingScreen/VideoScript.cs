using System.Collections;
using UnityEngine;
using UnityEngine.Video;
using UnityEngine.SceneManagement;

public class VideoScript : MonoBehaviour {
    VideoPlayer videoPlayer;
    float timer = 0;
    bool called = false;
    // Use this for initialization
    void Start () {
        if (GameLogic.instance != null) {
            GameLogic.instance.gameState = GameLogic.GameState.LOADINGSCREEN;
            Debug.Log(GameLogic.instance.gameState);
        }
        videoPlayer = GetComponent<VideoPlayer>();
	}
	
	// Update is called once per frame
	void Update () {
        if (timer < 10) {
            timer += Time.deltaTime;
        } else {
            if (videoPlayer != null) {
                if (!videoPlayer.isPlaying) {
                    if (!called) {
                        StartCoroutine(LoadYourAsyncScene(3));
                        called = true;
                    }
                }
            }
        }

        if (Input.anyKeyDown) {
            if (!called) {
                StartCoroutine(LoadYourAsyncScene(3));
                called = true;
            }
        }

    }

    IEnumerator LoadYourAsyncScene(int id) {
        // The Application loads the Scene in the background as the current Scene runs.
        // This is particularly good for creating loading screens.
        // You could also load the Scene by using sceneBuildIndex. In this case Scene2 has
        // a sceneBuildIndex of 1 as shown in Build Settings.

        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(id);

        // Wait until the asynchronous scene fully loads
        while (!asyncLoad.isDone) {
            yield return null;
        }
    }

    private void OnDestroy() {
        if (GameLogic.instance != null) {
            GameLogic.instance.gameState = GameLogic.GameState.LEVEL;
        }
    }

}
