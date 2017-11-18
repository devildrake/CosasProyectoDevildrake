﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class PlayerUtilsStatic {
    public static void DoDash(GameObject PJ, Vector2 direction, float MAX_FORCE) {
        Rigidbody2D rigidbody = PJ.GetComponent<Rigidbody2D>();
        rigidbody.velocity = new Vector2(0, 0);
        rigidbody.AddForce(rigidbody.velocity, ForceMode2D.Impulse);
        rigidbody.AddForce(direction * MAX_FORCE, ForceMode2D.Impulse);
    }
    public static void Punch(Vector2 direction, float MAX_FORCE, List<GameObject>NearbyObjects) {
        foreach (GameObject g in NearbyObjects) {
            if (g.GetComponent<DoubleObject>().isPunchable) {
                g.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezeRotation;
                g.GetComponent<Rigidbody2D>().AddForce(direction * MAX_FORCE, ForceMode2D.Impulse);
                g.GetComponent<DoubleObject>().isPunchable = false;
            }
        }

    }

    public static void Smash(GameObject g) {
        Rigidbody2D rb = g.GetComponent<Rigidbody2D>();
        if (!g.GetComponent<PlayerController>().smashing) {
            rb.velocity = new Vector2(0, 0);
            rb.AddForce(new Vector2(0, -700));
            g.GetComponent<PlayerController>().smashing = true;
        }
    }

    public static void DoSmash(GameObject g) {
        Rigidbody2D rb = g.GetComponent<Rigidbody2D>();
        RaycastHit2D hit2D = Physics2D.Raycast(rb.position - new Vector2(0f, 0.5f), Vector2.down, 0.2f, g.GetComponent<PlayerController>().groundMask);
        if (hit2D) {
            Debug.Log("HittingStuff");
            if (hit2D.transform.gameObject.GetComponent<DoubleObject>().isBreakable) {
                hit2D.transform.gameObject.GetComponent<DoubleObject>().GetBroken();
            }
        }
    }


    /* ==========================================================================================
     * ===================================DIRECTION CIRCLE=======================================
     * ==========================================================================================
     */
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
        if (behav == 0) {
            arrowAnchor.transform.position = PJ.transform.position; //se asigna la posicion del personaje a la flecha para que esta siempre aparezca sobre el PJ

            if (once) {
                Cursor.visible = false;
                Cursor.lockState = CursorLockMode.Locked; //Se engancha el cursor en el centro de la pantalla.
                arrowAnchor.SetActive(false); //no se ve la flecha
                once = false;
                mousePositions = new Vector2[2];
            }
            if (Input.GetMouseButtonDown(0)) {
                Cursor.lockState = CursorLockMode.None; //Se puede volver a mover el cursor
                mousePositions[1] = Input.mousePosition; //posicion donde se ha hecho click
                                                         //se activan la linea y la flecha para que se pinten en la pantalla.
                draw = true;
                arrowAnchor.SetActive(true);

            } else if (Input.GetMouseButtonUp(0)) {
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
            if (Input.GetMouseButtonDown(1)) {
                Cursor.lockState = CursorLockMode.None; //Se puede volver a mover el cursor
                mousePositions[1] = Input.mousePosition; //posicion donde se ha hecho click
                                                         //se activan la linea y la flecha para que se pinten en la pantalla.
                draw = true;
                arrowAnchor.SetActive(true);

            } else if (Input.GetMouseButtonUp(1)) {
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

    /*
     * Sobrecarga del direction circle con angulo maximo y minimo.
     */
    public static Vector2 UseDirectionCircle(GameObject arrowAnchor, GameObject PJ, int behav, float minAngle, float maxAngle) {
        Vector3 finalDirection = new Vector3();
        if (behav == 0) {
            arrowAnchor.transform.position = PJ.transform.position; //se asigna la posicion del personaje a la flecha para que esta siempre aparezca sobre el PJ

            if (once) {
                Cursor.visible = false;
                Cursor.lockState = CursorLockMode.Locked; //Se engancha el cursor en el centro de la pantalla.
                arrowAnchor.SetActive(false); //no se ve la flecha
                once = false;
                mousePositions = new Vector2[2];
            }
            if (Input.GetMouseButtonDown(0)) {
                Cursor.lockState = CursorLockMode.None; //Se puede volver a mover el cursor
                mousePositions[1] = Input.mousePosition; //posicion donde se ha hecho click
                                                         //se activan la linea y la flecha para que se pinten en la pantalla.
                draw = true;
                arrowAnchor.SetActive(true);

            } else if (Input.GetMouseButtonUp(0)) {
                Cursor.lockState = CursorLockMode.Locked; //se vuelve a bloquear el raton en el centro
                draw = false;
                arrowAnchor.SetActive(false);
                once = false;
            }

            //EL CONDICIONAL GESTIONA CUANDO TIENE QUE IR CALCULANDO EL ANGULO Y PINTANDO LOS SPRITES
            if (draw) {
                mousePositions[0] = Input.mousePosition;//actualiza el punto de la linea para que esta cambie en funcion de
                                                        //de la posicion del raton a cada frame


                float rotation;
                if (PJ.GetComponent<PlayerController>().facingRight) {
                    rotation = Vector3.Angle(new Vector3(1, 0, 0), (mousePositions[1] - mousePositions[0])); //calcula el angulo de inclinacion que tiene tu
                                                                                                             //drag del raton
                    rotation = Mathf.Clamp(rotation, minAngle, maxAngle);
                    if ((mousePositions[1] - mousePositions[0]).y < 0) {
                        rotation *= -1;
                    }
                    finalDirection = new Vector3(Mathf.Cos(rotation * Mathf.Deg2Rad), Mathf.Sin(rotation * Mathf.Deg2Rad), 0);
                } else {
                    rotation = Vector3.Angle(new Vector3(-1, 0, 0), (mousePositions[1] - mousePositions[0]));
                    rotation = Mathf.Clamp(rotation, minAngle, maxAngle);
                    //rotation += 180;
                    if ((mousePositions[1] - mousePositions[0]).y < 0) {

                        rotation = -180 + rotation;
                    } else {
                        rotation = 180 - rotation;
                    }

                    finalDirection = new Vector3(Mathf.Cos(rotation * Mathf.Deg2Rad), Mathf.Sin(rotation * Mathf.Deg2Rad), 0);
                }

                //si la linea es descendente el angulo que calculo antes sera negativo, si no hago esto el angulo siempre será positivo. (SOLUCION RADEV)
                /* if ((mousePositions[1] - mousePositions[0]).y < 0) {
                     rotation *= -1;
                 }*/
                //if (!PJ.GetComponent<PlayerController>().facingRight) {
                //    if (rotation > 0 && rotation < 180 - maxAngle) {
                //        rotation = 180 - maxAngle;
                //    }
                //    if (rotation < 0 && rotation > -180 + minAngle) {
                //        rotation = -180 + minAngle;
                //    }
                //}

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
            if (Input.GetMouseButtonDown(1)) {
                Cursor.lockState = CursorLockMode.None; //Se puede volver a mover el cursor
                mousePositions[1] = Input.mousePosition; //posicion donde se ha hecho click
                                                         //se activan la linea y la flecha para que se pinten en la pantalla.
                draw = true;
                arrowAnchor.SetActive(true);

            } else if (Input.GetMouseButtonUp(1)) {
                Cursor.lockState = CursorLockMode.Locked; //se vuelve a bloquear el raton en el centro
                draw = false;
                arrowAnchor.SetActive(false);
                once = false;
            }

            //EL CONDICIONAL GESTIONA CUANDO TIENE QUE IR CALCULANDO EL ANGULO Y PINTANDO LOS SPRITES
            if (draw) {
                mousePositions[0] = Input.mousePosition;//actualiza el punto de la linea para que esta cambie en funcion de
                                                        //de la posicion del raton a cada frame


                float rotation;
                if (PJ.GetComponent<PlayerController>().facingRight) {
                    rotation = Vector3.Angle(new Vector3(1, 0, 0), (mousePositions[1] - mousePositions[0])); //calcula el angulo de inclinacion que tiene tu
                                                                                                             //drag del raton
                    rotation = Mathf.Clamp(rotation, minAngle, maxAngle);
                    if ((mousePositions[1] - mousePositions[0]).y < 0) {
                        rotation *= -1;
                    }
                    finalDirection = new Vector3(Mathf.Cos(rotation * Mathf.Deg2Rad), Mathf.Sin(rotation * Mathf.Deg2Rad), 0);
                } else {
                    rotation = Vector3.Angle(new Vector3(-1, 0, 0), (mousePositions[1] - mousePositions[0]));
                    rotation = Mathf.Clamp(rotation, minAngle, maxAngle);
                    //rotation += 180;
                    if ((mousePositions[1] - mousePositions[0]).y < 0) {

                        rotation = -180 + rotation;
                    } else {
                        rotation = 180 - rotation;
                    }
                    finalDirection = new Vector3(Mathf.Cos(rotation * Mathf.Deg2Rad), Mathf.Sin(rotation * Mathf.Deg2Rad), 0);
                }
                rot.eulerAngles = new Vector3(0, 0, rotation); //qaternion de rotacion que es el que le aplico luego a la flecha
                arrowAnchor.transform.rotation = rot; //rota el sprite
            }
        }
        //Debug.Log(finalDirection);
        return (mousePositions[1] - mousePositions[0]).normalized;
    }

    public static void ResetDirectionCircle() {
        draw = false;
        once = true;
        Cursor.lockState = CursorLockMode.Locked;
    }
}
