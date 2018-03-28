using UnityEngine;

public class CameraScript : MonoBehaviour {
    public enum CameraState { CLOSE,FAR,TRANSITION}
    public enum LevelType { STATIC, CHASE }

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


    float slidingMultiplier;

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

    }

    public void ResetCamera() {
        transform.position = overViewPosition;
        GetComponent<Camera>().orthographicSize = farDistance;
    }

    bool once;

    private void FixedUpdate() {
        if (GameLogic.instance != null) {
            if (target.GetComponent<PlayerController>() != null) {
                if (target.GetComponent<PlayerController>().sliding) {
                    slidingMultiplier = 3;

                } else {
                    slidingMultiplier = 1;

                }


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
            }

            ////////////////////////////STATICLEVEL////////////////////////////////////////
            ////////////////////////////STATICLEVEL////////////////////////////////////////
            ////////////////////////////STATICLEVEL////////////////////////////////////////
            ////////////////////////////STATICLEVEL////////////////////////////////////////
            ////////////////////////////STATICLEVEL////////////////////////////////////////
            ////////////////////////////STATICLEVEL////////////////////////////////////////

            if (target.gameObject.GetComponent<PlayerController>() != null) {
                if (target.gameObject.GetComponent<PlayerController>().facingRight) {
                    offset.x = OffsetX;
                } else
                    offset.x = -OffsetX;
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
                ////////////////////////////STATICLEVEL////////////////////////////////////////
                ////////////////////////////STATICLEVEL////////////////////////////////////////
                ////////////////////////////STATICLEVEL////////////////////////////////////////
                ////////////////////////////STATICLEVEL////////////////////////////////////////
                ////////////////////////////STATICLEVEL////////////////////////////////////////
                ////////////////////////////STATICLEVEL////////////////////////////////////////









                ////////////////////////////CHASELEVEL/////////////////////////////////////////
                ////////////////////////////CHASELEVEL/////////////////////////////////////////
                ////////////////////////////CHASELEVEL/////////////////////////////////////////
                ////////////////////////////CHASELEVEL/////////////////////////////////////////
                ////////////////////////////CHASELEVEL/////////////////////////////////////////
                ////////////////////////////CHASELEVEL/////////////////////////////////////////



                //if (GetComponent<Camera>().orthographicSize < farDistance) {
                //    GetComponent<Camera>().orthographicSize = Mathf.Lerp(GetComponent<Camera>().orthographicSize, farDistance, Time.deltaTime * 2);
                //}

                //overViewPosition.x += cameraSpeed*Time.deltaTime;

                //if ((Vector3.Distance(transform.position, overViewPosition) > transitionThreshold)) {
                //    smoothedPosition = Vector3.Lerp(transform.position, overViewPosition, Time.deltaTime * 2);
                //    transform.position = smoothedPosition;
                //}










                ////////////////////////////CHASELEVEL/////////////////////////////////////////
                ////////////////////////////CHASELEVEL/////////////////////////////////////////
                ////////////////////////////CHASELEVEL/////////////////////////////////////////
                ////////////////////////////CHASELEVEL/////////////////////////////////////////
                ////////////////////////////CHASELEVEL/////////////////////////////////////////
                ////////////////////////////CHASELEVEL/////////////////////////////////////////







                //InputManager.instance.UpdatePreviousCamera();



            }
        }
    }
}
