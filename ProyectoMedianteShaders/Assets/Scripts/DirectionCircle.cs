using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class DirectionCircle {
    private static bool draw; //controla cuando se mantiene pulsado el raton y tiene que calcular el movimiento del raton para hacer girar la flecha
    private static Quaternion rot; //Quaternion para hacer rotar el sprite de las flechas
    private static Vector2[] mousePositions; //Guarda las dos posiciones del raton para calcular el vector
    private static bool once = true; //Esto es para hacer un pseudo-start


    /*
     * Parametros que recibe
     *      arrowAnchor: gameobject que tiene los sprites de las flechas
     *      PJ: personaje sobre el que queremos que aparezcan las flechas
     *      
     * Return: Devuelve un vector normalizado equivalente a la flecha que aparece en la pantalla (sprite que se pinta)
     */
    public static Vector2 UseDirectionCircle(GameObject arrowAnchor, GameObject PJ, int behav) {
        if (!GameLogic.instance.levelFinished) {

            if (behav == 0) {
                arrowAnchor.transform.position = PJ.transform.position; //se asigna la posicion del personaje a la flecha para que esta siempre aparezca sobre el PJ

                if (once) {
                    Cursor.visible = false;
                    Cursor.lockState = CursorLockMode.Locked; //Se engancha el cursor en el centro de la pantalla.
                    arrowAnchor.SetActive(false); //no se ve la flecha
                    once = false;
                    mousePositions = new Vector2[2];
                }
                if (/*Input.GetMouseButtonDown(0)*/InputManager.instance.dashButton&& !InputManager.instance.prevDashButton) {
                    Cursor.lockState = CursorLockMode.None; //Se puede volver a mover el cursor
                    mousePositions[1] = Input.mousePosition; //posicion donde se ha hecho click
                                                             //se activan la linea y la flecha para que se pinten en la pantalla.
                    draw = true;
                    arrowAnchor.SetActive(true);

                } else if (/*Input.GetMouseButtonUp(0)*/!InputManager.instance.dashButton && InputManager.instance.prevDashButton) {
                    Cursor.lockState = CursorLockMode.Locked; //se vuelve a bloquear el raton en el centro
                    draw = false;
                    arrowAnchor.SetActive(false);
                    once = false;
                }

                //EL CONDICIONAL GESTIONA CUANDO TIENE QUE IR CALCULANDO EL ANGULO Y PINTANDO LOS SPRITES
                if (draw) {
                    mousePositions[0] = Input.mousePosition;//actualiza el punto de la linea para que esta cambie en funcion de
                                                            //de la posicion del raton a cada frame


                    float rotation = Vector3.Angle(new Vector3(1, 0, 0), (mousePositions[1] - mousePositions[0])); //calcula el angulo de inclinacion que tiene tu
                                                                                                                   //drag del raton

                    //si la linea es descendente el angulo que calculo antes sera negativo, si no hago esto el angulo siempre será positivo. (SOLUCION RADEV)
                    if ((mousePositions[1] - mousePositions[0]).y < 0) {
                        rotation *= -1;
                    }

                    rot.eulerAngles = new Vector3(0, 0, rotation); //qaternion de rotacion que es el que le aplico luego a la flecha
                    arrowAnchor.transform.rotation = rot; //rota el sprite
                }

            } else {
                arrowAnchor.transform.position = PJ.transform.position; //se asigna la posicion del personaje a la flecha para que esta siempre aparezca sobre el PJ

                if (once) {
                    Cursor.visible = false;
                    Cursor.lockState = CursorLockMode.Locked; //Se engancha el cursor en el centro de la pantalla.
                    arrowAnchor.SetActive(false); //no se ve la flecha
                    once = false;
                    mousePositions = new Vector2[2];
                }
                if (/*Input.GetMouseButtonDown(1)*/InputManager.instance.deflectButton && !InputManager.instance.prevDeflectButton) {
                    Cursor.lockState = CursorLockMode.None; //Se puede volver a mover el cursor
                    mousePositions[1] = Input.mousePosition; //posicion donde se ha hecho click
                                                             //se activan la linea y la flecha para que se pinten en la pantalla.
                    draw = true;
                    arrowAnchor.SetActive(true);

                } else if (/*Input.GetMouseButtonUp(1)*/!InputManager.instance.deflectButton && InputManager.instance.prevDeflectButton) {
                    Cursor.lockState = CursorLockMode.Locked; //se vuelve a bloquear el raton en el centro
                    draw = false;
                    arrowAnchor.SetActive(false);
                    once = false;
                }

                //EL CONDICIONAL GESTIONA CUANDO TIENE QUE IR CALCULANDO EL ANGULO Y PINTANDO LOS SPRITES
                if (draw) {
                    mousePositions[0] = Input.mousePosition;//actualiza el punto de la linea para que esta cambie en funcion de
                                                            //de la posicion del raton a cada frame


                    float rotation = Vector3.Angle(new Vector3(1, 0, 0), (mousePositions[1] - mousePositions[0])); //calcula el angulo de inclinacion que tiene tu
                                                                                                                   //drag del raton

                    //si la linea es descendente el angulo que calculo antes sera negativo, si no hago esto el angulo siempre será positivo. (SOLUCION RADEV)
                    if ((mousePositions[1] - mousePositions[0]).y < 0) {
                        rotation *= -1;
                    }
                    rot.eulerAngles = new Vector3(0, 0, rotation); //qaternion de rotacion que es el que le aplico luego a la flecha
                    arrowAnchor.transform.rotation = rot; //rota el sprite
                }
            }
            return (mousePositions[1] - mousePositions[0]).normalized;
        }
        return new Vector2(0, 0);
    }
    public static void SetOnce(bool a) {
        once = a;
    }
}
