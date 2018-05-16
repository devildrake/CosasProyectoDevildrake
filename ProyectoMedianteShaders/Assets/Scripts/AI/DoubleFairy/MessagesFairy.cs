using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MessagesFairy : MonoBehaviour {
    static string[] messages;
    static Sprite[] imagesCommand;
    static Sprite[] imagesKeyboard;
    string imagesPath;

    private void Start() {
        imagesPath = "Sprites/FeedBackText/";
        messages = new string[50];
        imagesCommand = new Sprite[10];
        imagesKeyboard= new Sprite[10];

        messages[0] = "Esto es un mensaje de prueba";
        messages[1] = "Necesito saber si puedo concatenar varios mensajes";
        messages[2] = "Más o menos serán 3 o así";

        imagesKeyboard[0] = Resources.Load<Sprite>(imagesPath + "showJump") as Sprite;
        imagesCommand[0] = Resources.Load<Sprite>(imagesPath + "showJump") as Sprite;

        Destroy(this);
    }

    public static string GetMessage(int id) {
        return messages[id];
    }

    public static string[] GetMessages(int from, int to) {
        string[] strings;

        if (to - from > 0) {

            strings = new string[(to - from) + 1];

            for (int i = 0; i < strings.Length; i++) {
                strings[i] = messages[i];
            }
        } else {
            strings = new string[1];
            strings[0] = "ERROR";
        }

        return strings;
    }

}
