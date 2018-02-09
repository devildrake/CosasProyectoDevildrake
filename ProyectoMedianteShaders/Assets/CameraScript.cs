using UnityEngine;

public class CameraScript : MonoBehaviour {
    public enum CameraState { CLOSE,FAR,TRANSITION}
    public Transform target;
    public Vector3 offset; //-10
    public Vector3 overViewPosition;
    public float farDistance;
    public float closeDistance;
    public float transitionThreshold;
    public float OffsetX;
    public float transitionTime;
    public float transitionTimer;
    public float distanceThreshold;
    //public float smoothSpeed = 10.0f;
    public CameraState cameraState;
    private void Start() {
        cameraState = CameraState.CLOSE;
        overViewPosition = transform.position;
        farDistance = 10;
        closeDistance = 4.5f;
        OffsetX = 3;
        transitionTime = 2;
        distanceThreshold = 0.3f;
    }

    public void ResetCamera() {
        transform.position = overViewPosition;
        GetComponent<Camera>().orthographicSize = farDistance;
    }

    private void FixedUpdate() {
        if (GameLogic.instance.cameraTransition) {
            cameraState = CameraState.FAR;
            if (GetComponent<Camera>().orthographicSize > farDistance+distanceThreshold|| GetComponent<Camera>().orthographicSize > farDistance - distanceThreshold) {
                transitionTimer += Time.deltaTime;
                if (transitionTimer > transitionTime) {
                    GameLogic.instance.cameraTransition = false;
                    transitionTimer = 0;
                }
            }
        }

            if (target.gameObject.GetComponent<PlayerController>().facingRight) {
            offset.x = OffsetX;
        } else
            offset.x = -OffsetX;

        Vector3 desiredPosition;
        Vector3 smoothedPosition;
        switch (cameraState) {
            case CameraState.CLOSE:
                if (Input.GetKeyDown(KeyCode.Q)) {
                    cameraState = CameraState.FAR;
                }
                desiredPosition = target.position + offset;
                smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, Time.deltaTime*2);
                transform.position = smoothedPosition;
                //transform.LookAt(target.position);
                if (GetComponent<Camera>().orthographicSize > closeDistance) {
                    GetComponent<Camera>().orthographicSize = Mathf.Lerp(GetComponent<Camera>().orthographicSize, closeDistance, Time.deltaTime);
                }
                
                break;
            case CameraState.FAR:
                if (!Input.GetKey(KeyCode.Q)) {
                    cameraState = CameraState.CLOSE;
                }
                if (GetComponent<Camera>().orthographicSize < farDistance) {
                    GetComponent<Camera>().orthographicSize = Mathf.Lerp(GetComponent<Camera>().orthographicSize, farDistance, Time.deltaTime*2);
                }

                if ((Vector3.Distance(transform.position,overViewPosition) > transitionThreshold)) {
                    smoothedPosition = Vector3.Lerp(transform.position, overViewPosition, Time.deltaTime*2);
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

}
