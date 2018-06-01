using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MessagesFairy : MonoBehaviour {
    public enum LANGUAGE { Spanish,English,None};
    public LANGUAGE language;
    static string[] messagesKeyboardSp;
    static string[] messagesCommandSp;

    static string[] messagesKeyboardEn;
    static string[] messagesCommandEn;

    static string[] advicesSp;
    static string[] advicesEn;

    static List<List<Sprite>> imagesCommand;
    static List<List<Sprite>> imagesKeyboard;
    static string imagesPath;
    public static bool asked;
    public static void StartMessages() {
        imagesPath = "Sprites/Fairy/";
        messagesKeyboardSp = new string[50];
        messagesCommandSp = new string[50];
        messagesKeyboardEn = new string[50];
        messagesCommandEn = new string[50];
        advicesSp = new string[20];
        advicesEn = new string[20];

        imagesCommand = new List<List<Sprite>>();
        imagesKeyboard = new List<List<Sprite>>();

        messagesKeyboardSp[0] = "Sin el espejo los dos mundos están en peligro!";
        messagesKeyboardSp[1] = "Teneis que colaborar para recuperar todos los fragmentos que podais encontrar!";
        messagesKeyboardSp[2] = "Para cambiar entre mundos pulsa SHIFT";
        messagesKeyboardSp[3] = "Como ya habreis observado, debido a que no hay espejo los mundos se están afectando entre ellos";
        messagesKeyboardSp[4] = "Debeis colaborar aprovechando el entorno de cada mundo para avanzar y recuperar los fragmentos";
        messagesKeyboardSp[5] = "Por ejemplo, intenta saltar encima del insecto este en el mundo oscuro";
        messagesKeyboardSp[6] = "No te preocupes si no sabes alcanzar algunos fragmentos ahora, podrás volver más adelante";
        messagesKeyboardSp[7] = "Dawn, ahora es el momento de que demuestres de qué estas hecha, utiliza tu habilidad y elevate";
        messagesKeyboardSp[8] = "Recuerdas? Solo tienes que saltar y hacer Click 'n' Drag con el botón izquierdo del ratón (Mundo claro)";
        messagesKeyboardSp[9] = "Estas semillas voladoras son inofensivas en el mundo claro, pero cuidado en el mundo oscuro";
        messagesKeyboardSp[10] = "Intenta saltarles encima en el mundo claro, si se quedan atascadas, cambia";
        messagesKeyboardSp[11] = "Dusk, te toca a ti, utiliza tu brazo oscuro para mover esa caja";
        messagesKeyboardSp[12] = "Recuerda que puedes arrastrarla manteniendo pulsado Click derecho cerca de la caja y moviendote";
        messagesKeyboardSp[13] = "Para darle un golpe a la caja, haz Click 'n' Drag con el botón izquierdo del ratón (Mundo oscuro)";
        messagesKeyboardSp[14] = "Oh no! ¿Qué hago aquí? ¿Qué ha pasado con el espejo?";
        messagesKeyboardSp[15] = "Tienes que ayudarme a investigar!";
        messagesKeyboardSp[16] = "Puedes moverte con las teclas A/D y saltar con la barra espaciadora";

        messagesKeyboardEn[0] = "Without the mirror both world are in danger!";
        messagesKeyboardEn[1] = "You have to cooperate to regain all fragments you can find!";
        messagesKeyboardEn[2] = "In order to change worlds press SHIFT";
        messagesKeyboardEn[3] = "As you've probably seen, since there's no mirror between both worlds they're afecting on another";
        messagesKeyboardEn[4] = "You have to take advantage of each world's environment to move forward and find all fragments";
        messagesKeyboardEn[5] = "For example, try to jump on that bug while at the dark world";
        messagesKeyboardEn[6] = "Don't worry if you can't reach some fragments right now you will be able to return for them later";
        messagesKeyboardEn[7] = "Dawn, it's time to show what you're made of, use your ability and !rise";
        messagesKeyboardEn[8] = "Remember? You just have to jump and Click 'n' Drag with the left mouse button (Bright world)";
        messagesKeyboardEn[9] = "These flying seeds are harmless at the bright world, but be wary of them if you touch them at the dark world";
        messagesKeyboardEn[10] = "Try jumping on them at the bright world, if they get stuck, change world and let them move";
        messagesKeyboardEn[11] = "Dusk, it's your turn, use your dark arm to move that box";
        messagesKeyboardEn[12] = "Remember you can drag it by holding your right click while looking at it and moving around";
        messagesKeyboardEn[13] = "To hit a box, click & drag with your left mouse button (Dark world)";
        messagesKeyboardEn[14] = "Oh no! Why am I here? What happened to the mirror?";
        messagesKeyboardEn[15] = "You've got to help me investigate!";
        messagesKeyboardEn[16] = "You can move around with A/D keys and jump with the SPACE BAR";

        messagesCommandSp[0] = "¡Sin el espejo los mundos están en peligro!";
        messagesCommandSp[1] = "Debes coger todos los fragmentos que puedas encontrar!";
        messagesCommandSp[2] = "Para cambiar entre mundos pulsa LT";
        messagesCommandSp[3] = "Como ya habreis observado, debido a que no hay espejo los mundos se están afectando entre ellos";
        messagesCommandSp[4] = "Debeis colaborar aprovechando el entorno de cada mundo para avanzar y recuperar los fragmentos";
        messagesCommandSp[5] = "Por ejemplo, intenta saltar encima del insecto este en el mundo oscuro";
        messagesCommandSp[6] = "No te preocupes si no sabes alcanzar algunos fragmentos ahora, podrás volver más adelante";
        messagesCommandSp[7] = "Dawn, ahora es el momento de que demuestres de qué estas hecha, utiliza tu habilidad y elevate";
        messagesCommandSp[8] = "Recuerdas? Solo tienes que saltar y mantener RT y usar el joystick derecho para definir dirección";
        messagesCommandSp[9] = "Estas semillas voladoras son inofensivas en el mundo claro, pero cuidado en el mundo oscuro";
        messagesCommandSp[10] = "Intenta saltarles encima en el mundo claro, si se quedan atascadas, cambia";
        messagesCommandSp[11] = "Dusk, te toca a ti, utiliza tu brazo oscuro para mover esa caja";
        messagesCommandSp[12] = "Recuerda que puedes arrastrarla manteniendo pulsado RB derecho cerca de la caja y moviendote";
        messagesCommandSp[13] = "Para darle un golpe a la caja, mantén el botón RT y usa el joystick derecho para definir dirección (Mundo oscuro)";
        messagesCommandSp[14] = "Oh no! ¿Qué hago aquí? ¿Qué ha pasado con el espejo?";
        messagesCommandSp[15] = "Tienes que ayudarme a investigar!";
        messagesCommandSp[16] = "Puedes moverte con el joystick izquierdo y saltar con la tecla A";

        messagesCommandEn[0] = "Without the mirror both world are in danger!";
        messagesCommandEn[1] = "You have to cooperate to regain all fragments you can find!";
        messagesCommandEn[2] = "In order to change worlds press LT";
        messagesCommandEn[3] = "As you've probably seen, since there's no mirror between both worlds they're afecting on another";
        messagesCommandEn[4] = "You have to take advantage of each world's environment to move forward and find all fragments";
        messagesCommandEn[5] = "For example, try to jump on that bug while at the dark world";
        messagesCommandEn[6] = "Don't worry if you can't reach some fragments right now you will be able to return for them later";
        messagesCommandEn[7] = "Dawn, it's time to show what you're made of, use your ability and !rise";
        messagesCommandEn[8] = "Remember? You just have to jump, press and hold RT and use the right joystick to choose direction (Bright World)";
        messagesCommandEn[9] = "These flying seeds are harmless at the bright world, but be wary of them if you touch them at the dark world";
        messagesCommandEn[10] = "Try jumping on them at the bright world, if they get stuck, change world and let them move";
        messagesCommandEn[11] = "Dusk, it's your turn, use your dark arm to move that box";
        messagesCommandEn[12] = "Remember you can drag it by holding RB while looking at it and moving around";
        messagesCommandEn[13] = "To hit a box, click & press and hold RT and use the right joystick to choose direction (Dark world)";
        messagesCommandEn[14] = "Oh no! Why am I here? What happened to the mirror?";
        messagesCommandEn[15] = "You've got to help me investigate!";
        messagesCommandEn[16] = "You can move around with the left joystick jump with the A button";

        advicesSp[0] = "Recuerda que las semillas voladoras pueden quedarse atascadas, si lo hacen, cambia de mundo";
        advicesSp[1] = "Puedes reiniciar cualquier nivel manteniendo pulsada la tecla R/LB";
        advicesSp[2] = "Puedes acceder al menú de juego pulsando Escape/Start";
        advicesSp[3] = "Puedes superar los niveles de nuevo para completarlos más rápido o conseguir fragmentos perdidos";
        advicesSp[4] = "Recuerda que Dusk puede arrastrar las cajas manteniendo puslado Click derecho y moviendose";
        advicesSp[5] = "Las puertas de nivel indican que niveles tienes disponibles y que logros has obtenido";

        advicesEn[0] = "Remember flying seeds may get stuck, change worlds and let them move around";
        advicesEn[1] = "You can restart any level by holding R/LB";
        advicesEn[2] = "You can acces the in game menu by pressing Escape/Start";
        advicesEn[3] = "You can repeat levels to finish them faster or find forgotten fragments";
        advicesEn[4] = "Remember Dusk cna drag boxes around by right clicking/holding RB and moving";
        advicesEn[5] = "Door levels indicate which levels are available as well as which achievements you've got";


        List<Sprite> animation0A = new List<Sprite>();
        List<Sprite> animation0B = new List<Sprite>();
        List<Sprite> animation1A = new List<Sprite>();
        List<Sprite> animation1B = new List<Sprite>();

        Sprite frameMoveKeys1 = Resources.Load<Sprite>(imagesPath+"MoveKeys") as Sprite;
        Sprite frameMoveKeys2 = Resources.Load<Sprite>(imagesPath + "MoveKeys1") as Sprite;
        Sprite frameMoveKeys3 = Resources.Load<Sprite>(imagesPath + "MoveKeys2") as Sprite;

        Sprite frameChangeKeys1 = Resources.Load<Sprite>(imagesPath + "Shift") as Sprite;
        Sprite frameChangeKeys2 = Resources.Load<Sprite>(imagesPath + "Shift1") as Sprite;
    
        animation0A.Add(frameMoveKeys1);
        animation0A.Add(frameMoveKeys2);
        animation0A.Add(frameMoveKeys1);
        animation0A.Add(frameMoveKeys3);

        animation1A.Add(frameChangeKeys1);
        animation1A.Add(frameChangeKeys2);

        Sprite frameMoveJoys0 = Resources.Load<Sprite>(imagesPath+"MoveJoy0") as Sprite;
        Sprite frameMoveJoys1 = Resources.Load<Sprite>(imagesPath + "MoveJoy1") as Sprite;
        Sprite frameMoveJoys2 = Resources.Load<Sprite>(imagesPath + "MoveJoy2") as Sprite;
        Sprite frameMoveJoys3 = Resources.Load<Sprite>(imagesPath + "MoveJoy3") as Sprite;
        Sprite frameMoveJoys4 = Resources.Load<Sprite>(imagesPath + "MoveJoy4") as Sprite;
        Sprite frameMoveJoys5 = Resources.Load<Sprite>(imagesPath + "MoveJoy5") as Sprite;
        Sprite frameMoveJoys6 = Resources.Load<Sprite>(imagesPath + "MoveJoy6") as Sprite;

        Sprite frameChangeButton0 = Resources.Load<Sprite>(imagesPath + "LT1") as Sprite;
        Sprite frameChangeButton1 = Resources.Load<Sprite>(imagesPath + "LT2") as Sprite;

        animation0B.Add(frameMoveJoys0);
        animation0B.Add(frameMoveJoys1);
        animation0B.Add(frameMoveJoys2);
        animation0B.Add(frameMoveJoys3);
        animation0B.Add(frameMoveJoys4);
        animation0B.Add(frameMoveJoys5);
        animation0B.Add(frameMoveJoys6);

        animation1B.Add(frameChangeButton0);
        animation1B.Add(frameChangeButton1);

        imagesKeyboard.Add(animation0A);
        imagesKeyboard.Add(animation1A);
        imagesCommand.Add(animation0B);
        imagesCommand.Add(animation1B);

        //imagesKeyboard[0] = Resources.Load<Sprite>(imagesPath + "showJump") as Sprite;
        //imagesCommand[0] = Resources.Load<Sprite>(imagesPath + "showJump") as Sprite;
    }

    public static List<Sprite> GetSpriteList(int id, int wh) {
        if (wh == 0) {
            if (imagesKeyboard.Count > id) {
                return imagesKeyboard[id];
            }
        } else {
            if (imagesCommand.Count > id) {
                return imagesCommand[id];
            }
        }


        return null;
    }

    public static string GetAdvice(int wh, LANGUAGE language) {
        if (language == LANGUAGE.Spanish) {
            asked = false;
            if (wh == 0) {
                int id = Random.Range(0, 5);

                return advicesSp[id];
            } else {
                int id = Random.Range(0, 5);

                return advicesSp[id];
            }
        } else {
            asked = false;
            if (wh == 0) {
                int id = Random.Range(0, 5);

                return advicesEn[id];
            } else {
                int id = Random.Range(0, 5);

                return advicesEn[id];
            }
        }
    }

    public static string GetMessage(int id,int wh, LANGUAGE language) {
        if (language == LANGUAGE.Spanish) {
            if (wh == 0) {
                if (id < messagesKeyboardSp.Length) {
                    return messagesKeyboardSp[id];
                } else {
                    return "OutOfRangeMessage";
                }
            } else if (wh == 1) {
                if (id < messagesCommandSp.Length) {
                    return messagesCommandSp[id];
                } else {
                    return "OutOfRangeMessage";
                }
            } else {
                if (id < advicesSp.Length) {
                    return advicesSp[id];
                } else {
                    return "OutOfRangeMessage";
                }
            }
        } else {
            if (wh == 0) {
                if (id < messagesKeyboardEn.Length) {
                    return messagesKeyboardEn[id];
                } else {
                    return "OutOfRangeMessage";
                }
            } else if (wh == 1) {
                if (id < messagesCommandEn.Length) {
                    return messagesCommandEn[id];
                } else {
                    return "OutOfRangeMessage";
                }
            } else {
                if (id < advicesEn.Length) {
                    return advicesEn[id];
                } else {
                    return "OutOfRangeMessage";
                }
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
