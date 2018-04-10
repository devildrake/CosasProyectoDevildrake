using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Options_Logic : MonoBehaviour {
    enum OptionsState {DESPLEGANDO_AUDIO, AUDIO_DESPLEGADO, REPLEGANDO_AUDIO, DESPLEGANDO_VIDEO, VIDEO_DESPLEGADO, REPLEGANDO_VIDEO };
    private OptionsState currentState, goingTo;
    private Animation_Event animationCheck;
    [SerializeField] private GameObject bg;
    [SerializeField] private List<GameObject> menuElements; //Audio - Video - Aceptar - Cancelar
    [SerializeField] private GameObject setaAudio, setaVideo;

    void Start() {
        currentState = OptionsState.DESPLEGANDO_AUDIO;
        animationCheck = GetComponentInChildren<Animation_Event>();
        goingTo = OptionsState.VIDEO_DESPLEGADO;
        foreach(GameObject g in menuElements) {
            g.SetActive(false);
        }
    }

    void Update() {
        Debug.Log("current state--> "+currentState);
        switch (currentState) {
            case OptionsState.DESPLEGANDO_AUDIO:
                DesplegandoAudio();
                break;
            case OptionsState.AUDIO_DESPLEGADO:
                AudioDesplegado();
                break;
            case OptionsState.REPLEGANDO_AUDIO:
                ReplegandoAudio();
                break;
            case OptionsState.DESPLEGANDO_VIDEO:
                DesplegandoVideo();
                break;
            case OptionsState.VIDEO_DESPLEGADO:
                VideoDesplegado();
                break;
            case OptionsState.REPLEGANDO_VIDEO:
                ReplegandoVideo();
                break;
        }
    }


    private void AudioDesplegado() {

    }

    private void VideoDesplegado() {

    }

    private void ReplegandoVideo() {
        foreach (GameObject g in menuElements) {
            g.SetActive(false);
        }
        if (animationCheck.isDown == 0) {
            currentState = OptionsState.DESPLEGANDO_AUDIO;
        }
    }

    private void ReplegandoAudio() {
        foreach (GameObject g in menuElements) {
            g.SetActive(false);
        }
        if (animationCheck.isDown == 0) {
            currentState = OptionsState.DESPLEGANDO_VIDEO;
        }
    }

    private void DesplegandoVideo() {
        //bgAnimator.SetBool("Desplegar", true);
        if (animationCheck.isDown == 1) {
            currentState = OptionsState.VIDEO_DESPLEGADO;
            menuElements[1].SetActive(true);
            menuElements[2].SetActive(true);
            menuElements[3].SetActive(true);
        }
    }

    private void DesplegandoAudio() {
        //bgAnimator.SetBool("Desplegar", true);
        if (animationCheck.isDown == 1) {
            currentState = OptionsState.AUDIO_DESPLEGADO;
            menuElements[0].SetActive(true);
            menuElements[2].SetActive(true);
            menuElements[3].SetActive(true);
        }
    }

    public void ClickAudio() {
        if(currentState == OptionsState.VIDEO_DESPLEGADO) {
            currentState = OptionsState.REPLEGANDO_VIDEO;
            goingTo = OptionsState.AUDIO_DESPLEGADO;
            //bgAnimator.SetBool("Desplegar", false);
        }
    }

    public void ClickVideo() {
        if(currentState == OptionsState.AUDIO_DESPLEGADO) {
            currentState = OptionsState.REPLEGANDO_AUDIO;
            goingTo = OptionsState.VIDEO_DESPLEGADO;
        }
        //bgAnimator.SetBool("Desplegar", false);
    }

    private void OnEnable() {
        currentState = OptionsState.DESPLEGANDO_AUDIO;
        //bgAnimator.SetBool("Desplegar", true);
    }
}
