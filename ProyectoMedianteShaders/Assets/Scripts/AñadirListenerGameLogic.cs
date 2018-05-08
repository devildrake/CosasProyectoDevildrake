using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


//Script para añadir un listener al método GameLogic.LoadMenu() a los botones que pertoque;
public class AñadirListenerGameLogic : MonoBehaviour {
    [Tooltip("0 -> Menu Principal \n1 -> Reincio nivel")]
    public int index;
    private Button myselfButton;

    void Start() {
        myselfButton = GetComponent<Button>();

        switch (index) {
            case 0:
                myselfButton.onClick.AddListener(() => GameLogic.instance.LoadMenu());
                GameLogic.instance.gameState = GameLogic.GameState.MENU;
                break;
            case 1:
                myselfButton.onClick.AddListener(() => GameLogic.instance.RestartScene());
                break;
            case 2:
                myselfButton.onClick.AddListener(() => GameLogic.instance.LoadScene(GameLogic.instance.pauseCanvas.nextSceneIndex));//(GameLogic.instance.GetCurrentLevelIndex()+1));
                break;
            case 3:
                myselfButton.onClick.AddListener(() => GameLogic.instance.LoadScene("LoadingScreen"));//(GameLogic.instance.GetCurrentLevelIndex()+1));
                GameLogic.instance.gameState = GameLogic.GameState.LOADINGSCREEN;
                break;
            default:
                break;
    }
    }
}
