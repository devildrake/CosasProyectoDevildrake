using UnityEngine;

public class CameraScript : MonoBehaviour {
    public enum CameraState { CLOSE,FAR,TRANSITION}
    public enum LevelType { STATIC, CHASE }
    AudioListener audioListener;
    [Tooltip("Target que sigue la cámara en modo CameraState.CLOSE (Debería ser el personaje casi siempre)")]
    public Transform target;

    [Tooltip("Offset de cámara sobre el target")]
    public Vector3 offset; //-10

    [Tooltip("Posición general de vista de la escena, se ha de modificar en los niveles de LevelType Static con la variable cameraSpeed")]
    public Vector3 overViewPosition;

    [Tooltip("Velocidad a la que la cámara se mueve hacia la derecha")]
    public float cameraSpeed;

    [Tooltip("Variable Size de Cámara en CameraState.FAR")]
    public float farDistance;

    [Tooltip("Variable Size de Cámara en CameraState.CLOSE")]
    public float closeDistance;

    [Tooltip("Lindar de transiciones de tamaño de camara")]
    public float transitionThreshold;

    [Tooltip("Offset Solo en X")]
    public float OffsetX;

    [Tooltip("Tiempo hasta realizar la transición inicial de cámara")]
    public float transitionTime;

    [Tooltip("Contador que comprueba el tiempo hasta realizar la transición inicial de cámara")]
    public float transitionTimer;

    [Tooltip("Lindar para transiciones de posición camara")]
    public float distanceThreshold;

    [Tooltip("Estado actual de la cámara")]
    public CameraState cameraState;

    [Tooltip("Tipo de nivel")]
    public LevelType levelType;

    PlayerController playerController;
    float slidingMultiplier;

    Vector3 lookTarget;
    float rotationSpeed = 5;

    private void Start() {
        slidingMultiplier = 1;
        cameraState = CameraState.CLOSE;
        overViewPosition = transform.position;
        //farDistance = 10;
        //closeDistance = 4.5f;
        OffsetX = 3;
        transitionTime = 2;
        distanceThreshold = 0.3f;
        transform.Rotate(new Vector3(1, 1, 0), 2.50f);
        playerController = target.GetComponent<PlayerController>();
        lookTarget = transform.position + Vector3.forward;
        audioListener = GetComponent<AudioListener>();
    }

    public void ResetCamera() {
        transform.position = overViewPosition;
        GetComponent<Camera>().orthographicSize = farDistance;
    }

    bool once;

    void LateUpdate() {
        //if (!playerController.lookAtMe) {
        //    if (Vector3.Distance(lookTarget, transform.position + Vector3.forward) > 0.2f) {
        //        lookTarget = Vector3.Lerp(lookTarget, transform.position + Vector3.forward, Time.deltaTime * 2 * slidingMultiplier * GameLogic.instance.cameraAttenuation);
        //    } else {
        //        lookTarget = transform.position + Vector3.forward;
        //    }
        //    //lookTarget = transform.position + Vector3.forward;

        //} else {
        //    if (Vector3.Distance(lookTarget, playerController.transform.position) > 0.2f) {
        //        lookTarget = Vector3.Lerp(lookTarget, playerController.transform.position, Time.deltaTime * 2 * slidingMultiplier * GameLogic.instance.cameraAttenuation);

        //    } else {
        //        lookTarget = playerController.transform.position;
        //    }
        //}
        //transform.LookAt(lookTarget);
        if (GameLogic.instance.eventState == GameLogic.EventState.NONE) {
            Vector3 lTargetDir = new Vector3(0, 0, 0);
            switch (cameraState) {
                case CameraState.CLOSE:
                    if (playerController.lookAtMe) {
                        lTargetDir = target.position - transform.position;
                        lTargetDir.y = 0.0f;
                        transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.LookRotation(lTargetDir), Time.deltaTime * rotationSpeed);
                    } else {
                        lTargetDir = transform.position + Vector3.forward - transform.position;
                        lTargetDir.y = 0.0f;
                        transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.LookRotation(lTargetDir), Time.deltaTime * rotationSpeed * 2);

                    }
                    break;
                case CameraState.FAR:

                    lTargetDir = transform.position + Vector3.forward - transform.position;
                    lTargetDir.y = 0.0f;
                    transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.LookRotation(lTargetDir), Time.deltaTime * rotationSpeed * 2);


                    break;
            }
        } else {

        }

    }

    private void FixedUpdate() {

        if (GameLogic.instance != null) {
            if (playerController != null) {

                if (playerController.worldAssignation == DoubleObject.world.DAWN) {
                    if (playerController.dawn) {
                        if (!audioListener.enabled) {
                            audioListener.enabled = true;
                        }
                    } else {
                        if (audioListener.enabled) {
                            audioListener.enabled = false;
                        }
                    }
                } else {
                    if (playerController.dawn) {
                        if (audioListener.enabled) {
                            audioListener.enabled = false;
                        }
                    } else {
                        if (!audioListener.enabled) {
                            audioListener.enabled = true;
                        }
                    }
                }


                if (playerController.sliding) {
                    slidingMultiplier = 3;

                } else {
                    slidingMultiplier = 1;

                }


            } else {
                playerController = target.GetComponent<PlayerController>();
            }




            if (GameLogic.instance.cameraTransition) {
                cameraState = CameraState.FAR;
                if (GetComponent<Camera>().orthographicSize > farDistance + distanceThreshold || GetComponent<Camera>().orthographicSize > farDistance - distanceThreshold) {
                    transitionTimer += Time.deltaTime;
                    if (transitionTimer > transitionTime) {
                        GameLogic.instance.cameraTransition = false;
                        transitionTimer = 0;
                    }
                }
            } else {
                if(GameLogic.instance.eventState == GameLogic.EventState.NONE) {

                    if (playerController != null) {
                        if (playerController.useXOffset) {
                            if (playerController.facingRight) {
                                offset.x = OffsetX;
                            } else
                                offset.x = -OffsetX;
                        } else {
                            offset.x = 0;
                            //Debug.Log("NO X");
                        }
                    }

                    Vector3 desiredPosition;
                    Vector3 smoothedPosition;
                    if (InputManager.instance != null) {
                        switch (cameraState) {
                            case CameraState.CLOSE:

                                if (levelType == LevelType.STATIC) {
                                    if (InputManager.instance.cameraButton) {
                                        cameraState = CameraState.FAR;
                                    }
                                }
                                desiredPosition = target.position + offset + GameLogic.instance.additionalOffset;
                                smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, Time.deltaTime * 2 * slidingMultiplier * GameLogic.instance.cameraAttenuation);
                                transform.position = smoothedPosition;

                                //transform.LookAt(target.position);
                                if (GetComponent<Camera>().orthographicSize > closeDistance) {
                                    //Debug.Log(GetComponent<Camera>().orthographicSize - closeDistance);
                                    GetComponent<Camera>().orthographicSize = Mathf.Lerp(GetComponent<Camera>().orthographicSize, closeDistance, Time.deltaTime);
                                }

                                break;
                            case CameraState.FAR:
                                if (!InputManager.instance.cameraButton) {
                                    cameraState = CameraState.CLOSE;
                                }
                                if (GetComponent<Camera>().orthographicSize < farDistance) {
                                    GetComponent<Camera>().orthographicSize = Mathf.Lerp(GetComponent<Camera>().orthographicSize, farDistance, Time.deltaTime * 2);
                                }

                                desiredPosition = target.position + new Vector3(5, 0, -25);

                                if ((Vector3.Distance(transform.position, desiredPosition) > transitionThreshold)) {
                                    smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, Time.deltaTime * 2);
                                    transform.position = smoothedPosition;
                                }
                                break;
                            default:
                                desiredPosition = target.position + offset;
                                smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, Time.deltaTime);
                                transform.position = smoothedPosition;
                                break;
                        }


                    }
                }else if (GameLogic.instance.eventState==GameLogic.EventState.TEXT) {
                    Vector3 desiredPosition;
                    Vector3 smoothedPosition;

                    desiredPosition = target.position + new Vector3(0, 2, -5);
                    smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, Time.deltaTime * 2 * GameLogic.instance.cameraAttenuation);
                    transform.position = smoothedPosition;


                }
            }

        }
    }
}
