using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Options_Logic : MonoBehaviour {
    enum OptionsState {DESPLEGANDO_AUDIO, AUDIO_DESPLEGADO, REPLEGANDO_AUDIO, DESPLEGANDO_VIDEO, VIDEO_DESPLEGADO, REPLEGANDO_VIDEO };
    private OptionsState currentState;
    private Animation_Event animationCheck;
    [SerializeField] private Animator bgAnimator;
    [SerializeField] private List<GameObject> menuElements; //Audio - Video - Aceptar - Cancelar
    [SerializeField] private GameObject setaAudio, setaVideo;

    private void Start() {
        currentState = OptionsState.DESPLEGANDO_AUDIO;
        animationCheck = GetComponentInChildren<Animation_Event>();
        bgAnimator.SetBool("Desplegar", true);
        foreach(GameObject g in menuElements) {
            g.SetActive(false);
        }
    }

    private void Update() {
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

    private void ReplegandoVideo() {

    }

    private void VideoDesplegado() {

    }

    private void DesplegandoVideo() {

    }

    private void ReplegandoAudio() {

    }

    private void AudioDesplegado() {

    }

    private void DesplegandoAudio() {
        if(animationCheck.isDown == 1) {
            currentState = OptionsState.AUDIO_DESPLEGADO;
            menuElements[0].SetActive(true);
            menuElements[2].SetActive(true);
            menuElements[3].SetActive(true);
        }
    }

    private void OnEnable() {
        currentState = OptionsState.DESPLEGANDO_AUDIO;
    }
}
