using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class LoadingScreenLogic : MonoBehaviour {
    float timer;
    bool called;
	// Use this for initialization
	void Start () {
        timer = 0;
        called = false;
	}
	
	// Update is called once per frame
	void Update () {
        timer += Time.deltaTime;

        if (!called&&timer>2.0f) {
            StartCoroutine(LoadYourAsyncScene(GameLogic.instance.levelToLoad));
            called = true;
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
        GameLogic.instance.gameState = GameLogic.GameState.LEVEL;
    }

}

