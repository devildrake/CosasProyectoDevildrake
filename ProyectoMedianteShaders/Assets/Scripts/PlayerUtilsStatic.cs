using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class PlayerUtilsStatic {
    public static void DoDash(GameObject PJ, Vector2 direction, float MAX_FORCE, bool limitX) {
        Rigidbody2D rigidbody = PJ.GetComponent<Rigidbody2D>();
        //Debug.Log(rigidbody.bodyType);
        //rigidbody.AddForce(direction * MAX_FORCE, ForceMode2D.Impulse);
        if (!limitX) {
            rigidbody.AddForce(direction * MAX_FORCE, ForceMode2D.Impulse);
        } else {
            rigidbody.AddForce(new Vector2(direction.x * MAX_FORCE*0.75f,direction.y*MAX_FORCE), ForceMode2D.Impulse);
        }
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
        if (!GameLogic.instance.levelFinished) {
            if (InputManager.instance != null) { 
                if (!InputManager.gamePadConnected) {
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
                    InputManager.instance.UpdatePreviousUtils();
                    return (mousePositions[1] - mousePositions[0]).normalized;
                } else {
                    ////CONTROL CON EL MANDO
                    //if (InputManager.instance.dashButton) {
                    //    draw = true;
                    //}
                    //Vector2 local;
                    //local = new Vector2(-InputManager.instance.rightHorizontalAxis, InputManager.instance.rightVerticalAxis);
                    //Debug.Log(local);

                    //return local.normalized;
                    /////////////////////////LO DE ARRIBA COMENTADO FUNCIONA/////////////////////
                    /////////////////////////LO DE ARRIBA COMENTADO FUNCIONA/////////////////////
                    /////////////////////////LO DE ARRIBA COMENTADO FUNCIONA/////////////////////
                    /////////////////////////LO DE ARRIBA COMENTADO FUNCIONA/////////////////////
                    /////////////////////////LO DE ARRIBA COMENTADO FUNCIONA/////////////////////

                    if (behav == 0) {
                        arrowAnchor.transform.position = PJ.transform.position; //se asigna la posicion del personaje a la flecha para que esta siempre aparezca sobre el PJ

                        if (once) {
                            Cursor.visible = false;
                            Cursor.lockState = CursorLockMode.Locked; //Se engancha el cursor en el centro de la pantalla.
                            arrowAnchor.SetActive(false); //no se ve la flecha
                            once = false;
                            mousePositions = new Vector2[2];
                        }

                        if (InputManager.instance.dashButton && !InputManager.instance.prevDashButton) {
                            Cursor.lockState = CursorLockMode.None; //Se puede volver a mover el cursor
                            mousePositions[1] = new Vector2(0, 0);
                            draw = true;
                            arrowAnchor.SetActive(true);

                        } else if (!InputManager.instance.dashButton && InputManager.instance.prevDashButton) {
                            Cursor.lockState = CursorLockMode.Locked; //se vuelve a bloquear el raton en el centro
                            draw = false;
                            arrowAnchor.SetActive(false);
                            once = false;
                        }

                        //EL CONDICIONAL GESTIONA CUANDO TIENE QUE IR CALCULANDO EL ANGULO Y PINTANDO LOS SPRITES
                        if (draw) {
                            mousePositions[0] = new Vector2(0, 0) + new Vector2(InputManager.instance.rightHorizontalAxis,-InputManager.instance.rightVerticalAxis).normalized;

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
                        Debug.Log("BEHAV 1");


                        if (once) {
                            Cursor.visible = false;
                            Cursor.lockState = CursorLockMode.Locked; //Se engancha el cursor en el centro de la pantalla.
                            arrowAnchor.SetActive(false); //no se ve la flecha
                            once = false;
                            mousePositions = new Vector2[2];
                        }
                        if (InputManager.instance.deflectButton&&!InputManager.instance.prevDeflectButton) {
                            Cursor.lockState = CursorLockMode.None; //Se puede volver a mover el cursor
                            mousePositions[1] = new Vector2(0, 0);
                            draw = true;
                            arrowAnchor.SetActive(true);

                        } else if (!InputManager.instance.deflectButton && InputManager.instance.prevDeflectButton) {
                            Cursor.lockState = CursorLockMode.Locked; //se vuelve a bloquear el raton en el centro
                            draw = false;
                            arrowAnchor.SetActive(false);
                            once = false;
                        }

                        //EL CONDICIONAL GESTIONA CUANDO TIENE QUE IR CALCULANDO EL ANGULO Y PINTANDO LOS SPRITES
                        if (draw) {
                            mousePositions[0] = new Vector2(0,0) + new Vector2(InputManager.instance.rightHorizontalAxis, -InputManager.instance.rightVerticalAxis).normalized;


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

                    InputManager.instance.UpdatePreviousUtils();

                    return (mousePositions[1] - mousePositions[0]).normalized;

                }

            }
        }
        return new Vector2(0, 0);
    }
    /*
     * Sobrecarga del direction circle con angulo maximo y minimo.
     */
    private static Vector3 finalDirection = new Vector3();
    public static Vector2 UseDirectionCircle(GameObject arrowAnchor, GameObject PJ, int behav, float minAngle, float maxAngle) {
        if (!GameLogic.instance.levelFinished) {
            if (InputManager.instance != null) {
                if (!InputManager.gamePadConnected) {
                    //Vector3 finalDirection = new Vector3();
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
                                if ((mousePositions[1] - mousePositions[0]).y < 0) {
                                    rotation *= -1;
                                }
                                rotation = Mathf.Clamp(rotation, minAngle, maxAngle);
                                finalDirection = new Vector3(Mathf.Cos(rotation * Mathf.Deg2Rad), Mathf.Sin(rotation * Mathf.Deg2Rad), 0);
                            } else {
                                rotation = Vector3.Angle(new Vector3(-1, 0, 0), (mousePositions[1] - mousePositions[0]));

                                if ((mousePositions[1] - mousePositions[0]).y < 0) {
                                    rotation *= -1;
                                    rotation = -180 - rotation;
                                    rotation = Mathf.Clamp(rotation, -180, -180 - minAngle);

                                } else {
                                    rotation = 180 - rotation;
                                    rotation = Mathf.Clamp(rotation, 180 - maxAngle, 180);

                                }
                                //rotation = Mathf.Clamp(rotation, minAngle, 90 - maxAngle);
                                finalDirection = new Vector3(Mathf.Cos(rotation * Mathf.Deg2Rad), Mathf.Sin(rotation * Mathf.Deg2Rad), 0);
                                //Debug.Log(rotation);
                            }

                            rot.eulerAngles = new Vector3(0, 0, rotation); //qaternion de rotacion que es el que le aplico luego a la flecha
                            arrowAnchor.transform.rotation = rot; //rota el sprite
                        }

                    } else { //behav == 1 --> funcionalidad para deflect
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
                                if ((mousePositions[1] - mousePositions[0]).y < 0) {
                                    rotation *= -1;
                                }
                                rotation = Mathf.Clamp(rotation, minAngle, maxAngle);
                                finalDirection = new Vector3(Mathf.Cos(rotation * Mathf.Deg2Rad), Mathf.Sin(rotation * Mathf.Deg2Rad), 0);
                            } else {
                                rotation = Vector3.Angle(new Vector3(-1, 0, 0), (mousePositions[1] - mousePositions[0]));

                                if ((mousePositions[1] - mousePositions[0]).y < 0) {
                                    rotation *= -1;
                                    rotation = -180 - rotation;
                                    rotation = Mathf.Clamp(rotation, -180, -180 - minAngle);

                                } else {
                                    rotation = 180 - rotation;
                                    rotation = Mathf.Clamp(rotation, 180 - maxAngle, 180);

                                }
                                //rotation = Mathf.Clamp(rotation, minAngle, 90 - maxAngle);
                                finalDirection = new Vector3(Mathf.Cos(rotation * Mathf.Deg2Rad), Mathf.Sin(rotation * Mathf.Deg2Rad), 0);
                                //Debug.Log(rotation);
                            }

                            rot.eulerAngles = new Vector3(0, 0, rotation); //qaternion de rotacion que es el que le aplico luego a la flecha
                            arrowAnchor.transform.rotation = rot; //rota el sprite
                        }
                    }
                    //Debug.Log(finalDirection.normalized);
                    return finalDirection.normalized;
                    // return (mousePositions[1] - mousePositions[0]).normalized;
                } else {
                    //Vector3 finalDirection = new Vector3();
                    if (behav == 0) {
                        arrowAnchor.transform.position = PJ.transform.position; //se asigna la posicion del personaje a la flecha para que esta siempre aparezca sobre el PJ

                        if (once) {
                            Cursor.visible = false;
                            Cursor.lockState = CursorLockMode.Locked; //Se engancha el cursor en el centro de la pantalla.
                            arrowAnchor.SetActive(false); //no se ve la flecha
                            once = false;
                            mousePositions = new Vector2[2];
                        }
                        if (InputManager.instance.dashButton&&!InputManager.instance.prevDashButton) {
                            Cursor.lockState = CursorLockMode.None; //Se puede volver a mover el cursor
                            mousePositions[1] = new Vector2(0, 0);
                            draw = true;
                            arrowAnchor.SetActive(true);

                        } else if (!InputManager.instance.dashButton && InputManager.instance.prevDashButton) {
                            Cursor.lockState = CursorLockMode.Locked; //se vuelve a bloquear el raton en el centro
                            draw = false;
                            arrowAnchor.SetActive(false);
                            once = false;
                        }

                        //EL CONDICIONAL GESTIONA CUANDO TIENE QUE IR CALCULANDO EL ANGULO Y PINTANDO LOS SPRITES
                        if (draw) {
                            mousePositions[0] = new Vector2(0, 0) + new Vector2(InputManager.instance.rightHorizontalAxis, -InputManager.instance.rightVerticalAxis);
                            float rotation;
                            if (PJ.GetComponent<PlayerController>().facingRight) {
                                rotation = Vector3.Angle(new Vector3(1, 0, 0), (mousePositions[1] - mousePositions[0])); //calcula el angulo de inclinacion que tiene tu
                                                                                                                         //drag del raton
                                if ((mousePositions[1] - mousePositions[0]).y < 0) {
                                    rotation *= -1;
                                }
                                rotation = Mathf.Clamp(rotation, minAngle, maxAngle);
                                finalDirection = new Vector3(Mathf.Cos(rotation * Mathf.Deg2Rad), Mathf.Sin(rotation * Mathf.Deg2Rad), 0);
                            } else {
                                rotation = Vector3.Angle(new Vector3(-1, 0, 0), (mousePositions[1] - mousePositions[0]));

                                if ((mousePositions[1] - mousePositions[0]).y < 0) {
                                    rotation *= -1;
                                    rotation = -180 - rotation;
                                    rotation = Mathf.Clamp(rotation, -180, -180 - minAngle);

                                } else {
                                    rotation = 180 - rotation;
                                    rotation = Mathf.Clamp(rotation, 180 - maxAngle, 180);

                                }
                                //rotation = Mathf.Clamp(rotation, minAngle, 90 - maxAngle);
                                finalDirection = new Vector3(Mathf.Cos(rotation * Mathf.Deg2Rad), Mathf.Sin(rotation * Mathf.Deg2Rad), 0);
                                //Debug.Log(rotation);
                            }

                            rot.eulerAngles = new Vector3(0, 0, rotation); //qaternion de rotacion que es el que le aplico luego a la flecha
                            arrowAnchor.transform.rotation = rot; //rota el sprite
                        }

                    } else { //behav == 1 --> funcionalidad para deflect
                        arrowAnchor.transform.position = PJ.transform.position; //se asigna la posicion del personaje a la flecha para que esta siempre aparezca sobre el PJ

                        if (once) {
                            Cursor.visible = false;
                            Cursor.lockState = CursorLockMode.Locked; //Se engancha el cursor en el centro de la pantalla.
                            arrowAnchor.SetActive(false); //no se ve la flecha
                            once = false;
                            mousePositions = new Vector2[2];
                        }
                        if (InputManager.instance.deflectButton&&!InputManager.instance.prevDeflectButton) {
                            Cursor.lockState = CursorLockMode.None; //Se puede volver a mover el cursor
                            mousePositions[1] = new Vector2(0, 0);
                            draw = true;
                            arrowAnchor.SetActive(true);

                        } else if (!InputManager.instance.deflectButton && InputManager.instance.prevDeflectButton) {
                            Cursor.lockState = CursorLockMode.Locked; //se vuelve a bloquear el raton en el centro
                            draw = false;
                            arrowAnchor.SetActive(false);
                            once = false;
                        }

                        //EL CONDICIONAL GESTIONA CUANDO TIENE QUE IR CALCULANDO EL ANGULO Y PINTANDO LOS SPRITES
                        if (draw) {
                            mousePositions[0] = new Vector2(0, 0) + new Vector2(InputManager.instance.rightHorizontalAxis, -InputManager.instance.rightVerticalAxis);
                            float rotation;
                            if (PJ.GetComponent<PlayerController>().facingRight) {
                                rotation = Vector3.Angle(new Vector3(1, 0, 0), (mousePositions[1] - mousePositions[0])); //calcula el angulo de inclinacion que tiene tu
                                                                                                                         //drag del raton
                                if ((mousePositions[1] - mousePositions[0]).y < 0) {
                                    rotation *= -1;
                                }
                                rotation = Mathf.Clamp(rotation, minAngle, maxAngle);
                                finalDirection = new Vector3(Mathf.Cos(rotation * Mathf.Deg2Rad), Mathf.Sin(rotation * Mathf.Deg2Rad), 0);
                            } else {
                                rotation = Vector3.Angle(new Vector3(-1, 0, 0), (mousePositions[1] - mousePositions[0]));

                                if ((mousePositions[1] - mousePositions[0]).y < 0) {
                                    rotation *= -1;
                                    rotation = -180 - rotation;
                                    rotation = Mathf.Clamp(rotation, -180, -180 - minAngle);

                                } else {
                                    rotation = 180 - rotation;
                                    rotation = Mathf.Clamp(rotation, 180 - maxAngle, 180);

                                }
                                //rotation = Mathf.Clamp(rotation, minAngle, 90 - maxAngle);
                                finalDirection = new Vector3(Mathf.Cos(rotation * Mathf.Deg2Rad), Mathf.Sin(rotation * Mathf.Deg2Rad), 0);
                                //Debug.Log(rotation);
                            }

                            rot.eulerAngles = new Vector3(0, 0, rotation); //qaternion de rotacion que es el que le aplico luego a la flecha
                            arrowAnchor.transform.rotation = rot; //rota el sprite
                        }
                    }
                    //Debug.Log(finalDirection.normalized);
                    InputManager.instance.UpdatePreviousUtils();
                    return finalDirection.normalized;
                    // return (mousePositions[1] - mousePositions[0]).normalized;
                }
            }
        }
        return new Vector2(0, 0);

    }
    public static void ResetDirectionCircle(GameObject arrow) {
        draw = false;
        once = true;
        Cursor.lockState = CursorLockMode.Locked;
        arrow.SetActive(false);
    }

 /* ==========================================================================================
 * ===================================RAYCASTHITARRAYMASK=====================================
 * ==========================================================================================
 */
 /// <summary>
 /// Se le pasa la posición origen la dirección la distancia y un array de mascaras (minimo 2)
 /// </summary>
 /// <param name="position"></param>
 /// <param name="direction"></param>
 /// <param name="distance"></param>
 /// <param name="mascaras"></param>
 /// <returns></returns>
    public static RaycastHit2D RayCastArrayMask(Vector3 position,Vector3 direction,float distance,LayerMask[] mascaras) {
        RaycastHit2D local;
        bool found = false;
        int i = 0;
        local = Physics2D.Raycast(position - new Vector3(0f, 0.5f, 0f), direction, distance, mascaras[0]);

        while (!found&&i<mascaras.Length) {
            local = Physics2D.Raycast(position - new Vector3(0f, 0.5f, 0f), direction, 0.1f, mascaras[i]);
            if (local) {
                found = true;
            } else {
                i++;
            }
        }
        return local;
    }


}
