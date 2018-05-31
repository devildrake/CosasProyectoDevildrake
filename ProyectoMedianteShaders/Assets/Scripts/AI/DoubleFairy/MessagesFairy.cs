using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MessagesFairy : MonoBehaviour {
    static string[] messagesKeyboard;
    static string[] messagesCommand;
    static string[] advices;

    static Sprite[] imagesCommand;
    static Sprite[] imagesKeyboard;
    static string imagesPath;
    public static bool asked;
    public static void StartMessages() {
        imagesPath = "Sprites/FeedBackText/";
        messagesKeyboard = new string[50];
        messagesCommand = new string[50];
        advices = new string[20];
        imagesCommand = new Sprite[10];
        imagesKeyboard= new Sprite[10];

        messagesKeyboard[0] = "Oh No!, sin el espejo los mundos están en peligro!";
        messagesKeyboard[1] = "Debes coger todos los fragmentos que puedas encontrar!";
        messagesKeyboard[2] = "Para cambiar entre mundos pulsa SHIFT";
        messagesKeyboard[3] = "Como ya habreis observado, debido a que no hay espejo los mundos se están afectando entre ellos";
        messagesKeyboard[4] = "Debeis colaborar aprovechando el entorno de cada mundo para avanzar y recuperar los fragmentos";
        messagesKeyboard[5] = "Por ejemplo, intenta saltar encima del insecto este en el mundo oscuro";
        messagesKeyboard[6] = "No te preocupes si no sabes alcanzar algunos fragmentos ahora, podrás volver más adelante";
        messagesKeyboard[7] = "Dawn, ahora es el momento de que demuestres de qué estas hecha, utiliza tu habilidad y elevate";
        messagesKeyboard[8] = "Recuerdas? Solo tienes que saltar y hacer Click 'n' Drag con el botón izquierdo del ratón (Mundo claro)";
        messagesKeyboard[9] = "Estas semillas voladoras son inofensivas en el mundo claro, pero cuidado en el mundo oscuro";
        messagesKeyboard[10] = "Intenta saltarles encima en el mundo claro, si se quedan atascadas, cambia";
        messagesKeyboard[11] = "Dusk, te toca a ti, utiliza tu brazo oscuro para mover esa caja";
        messagesKeyboard[12] = "Recuerda que puedes arrastrarla manteniendo pulsado Click derecho cerca de la caja";
        messagesKeyboard[13] = "Para darle un golpe a la caja, haz Click 'n' Drag con el botón izquierdo del ratón (Mundo oscuro)";

        messagesCommand[0] = "Oh No!, sin el espejo los mundos están en peligro!";
        messagesCommand[1] = "Debes coger todos los fragmentos que puedas encontrar!";
        messagesCommand[2] = "Para cambiar entre mundos pulsa LT";
        messagesCommand[3] = "Como ya habreis observado, debido a que no hay espejo los mundos se están afectando entre ellos";
        messagesCommand[4] = "Debeis colaborar aprovechando el entorno de cada mundo para avanzar y recuperar los fragmentos";
        messagesCommand[5] = "Por ejemplo, intenta saltar encima del insecto este en el mundo oscuro";
        messagesCommand[6] = "No te preocupes si no sabes alcanzar algunos fragmentos ahora, podrás volver más adelante";
        messagesCommand[7] = "Dawn, ahora es el momento de que demuestres de qué estas hecha, utiliza tu habilidad y elevate";
        messagesCommand[8] = "Recuerdas? Solo tienes que saltar y mantener RT y usar el joystick derecho para definir dirección";
        messagesCommand[9] = "Estas semillas voladoras son inofensivas en el mundo claro, pero cuidado en el mundo oscuro";
        messagesCommand[10] = "Intenta saltarles encima en el mundo claro, si se quedan atascadas, cambia";
        messagesCommand[11] = "Dusk, te toca a ti, utiliza tu brazo oscuro para mover esa caja";
        messagesCommand[12] = "Recuerda que puedes arrastrarla manteniendo pulsado RB derecho cerca de la caja";
        messagesCommand[13] = "Para darle un golpe a la caja, mantén el botón RT y usa el joystick derecho para definir dirección (Mundo oscuro)";

        advices[0] = "Recuerda que las semillas voladoras pueden quedarse atascadas, si lo hacen, cambia de mundo";
        advices[1] = "Puedes reiniciar cualquier nivel manteniendo pulsada la tecla R/LB";
        advices[2] = "Puedes acceder al menú de juego pulsando Escape/Start";
        advices[3] = "Puedes superar los niveles de nuevo para completarlos más rápido o conseguir fragmentos perdidos";
        advices[4] = "Recuerda que Dusk puede arrastrar las cajas manteniendo puslado Click derecho y moviendose";
        advices[5] = "Las puertas de nivel indican que niveles tienes disponibles y que logros has obtenido";


        imagesKeyboard[0] = Resources.Load<Sprite>(imagesPath + "showJump") as Sprite;
        imagesCommand[0] = Resources.Load<Sprite>(imagesPath + "showJump") as Sprite;
    }

    public static string GetAdvice(int wh) {
        asked = false;
        if (wh == 0) {
            int id = Random.Range(0, 5);

            return advices[id];
        } else {
            int id = Random.Range(0, 5);

            return advices[id];
        }
    }

    public static string GetMessage(int id,int wh) {
        if (wh == 0) {
            if (id < messagesKeyboard.Length) {
                return messagesKeyboard[id];
            } else {
                return "OutOfRangeMessage";
            }
        } else if(wh==1){
            if (id < messagesCommand.Length) {
                return messagesCommand[id];
            } else {
                return "OutOfRangeMessage";
            }
        } else {
            if (id < advices.Length) {
                return advices[id];
            } else {
                return "OutOfRangeMessage";
            }
        }
    }

    //public static string[] GetMessages(int from, int to,int wh) {
    //    string[] strings;

    //    if (to - from > 0) {

    //        strings = new string[(to - from) + 1];

    //        for (int i = 0; i < strings.Length; i++) {
    //            strings[i] = messages[i];
    //        }
    //    } else {
    //        strings = new string[1];
    //        strings[0] = "ERROR";
    //    }

    //    return strings;
    //}

}
