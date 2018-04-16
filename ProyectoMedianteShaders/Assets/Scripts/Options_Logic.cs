﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Options_Logic : MonoBehaviour {
    enum OptionsState { NONE, DESPLEGANDO_AUDIO, AUDIO_DESPLEGADO, REPLEGANDO_AUDIO, DESPLEGANDO_VIDEO, VIDEO_DESPLEGADO, REPLEGANDO_VIDEO, CERRAR };
    private OptionsState currentState, goingTo;
    private Animation_Event animationCheck;
    [SerializeField] private GameObject bg;
    [SerializeField] private List<GameObject> menuElements; //Audio - Video - Aceptar - Cancelar
    [SerializeField] private GameObject setaAudio, setaVideo;
    private RectTransform transformSetaAudio, transformSetaVideo,transformBG;
    [SerializeField] private int bgOffset, scrollSpeed = 3500, setaOffset = 30;
    private Vector2 originalBgPos, originalSetaAudioPos, originalSetaVideoPos;
    private int bgMovThreshold = 3;

    void Awake() {
        transformSetaAudio = setaAudio.GetComponent<RectTransform>();
        transformSetaVideo = setaVideo.GetComponent<RectTransform>();
        transformBG = bg.GetComponent<RectTransform>();
        originalBgPos = transformBG.localPosition;
        originalSetaAudioPos = transformSetaAudio.localPosition;
        originalSetaVideoPos = transformSetaVideo.localPosition;
    }

    void Start() {
        currentState = OptionsState.DESPLEGANDO_AUDIO;
        animationCheck = GetComponentInChildren<Animation_Event>();
        goingTo = OptionsState.VIDEO_DESPLEGADO;
        foreach(GameObject g in menuElements) {
            g.SetActive(false);
        }
        
    }

    void Update() {
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
        if (InputManager.instance.cancelButton) {
            goingTo = OptionsState.CERRAR;
            currentState = OptionsState.REPLEGANDO_AUDIO;
            foreach (GameObject go in menuElements) {
                go.SetActive(false);
            }
        }
    
    }

    private void VideoDesplegado() {
        if (InputManager.instance.cancelButton) {
            goingTo = OptionsState.CERRAR;
            currentState = OptionsState.REPLEGANDO_VIDEO;
            foreach (GameObject go in menuElements) {
                go.SetActive(false);
            }
        }
    }

    private void ReplegandoVideo() {
        if (transformBG.localPosition.y < originalBgPos.y+bgOffset) {
            transformBG.localPosition += new Vector3(0, scrollSpeed * Time.deltaTime, 0);
        }
        else {
            if (goingTo == OptionsState.CERRAR) {
                gameObject.SetActive(false);
            }
            else {
                currentState = OptionsState.DESPLEGANDO_AUDIO;
            }
        }
    }

    private void ReplegandoAudio() {
        if (transformBG.localPosition.y < originalBgPos.y + bgOffset) {
            transformBG.localPosition += new Vector3(0,  scrollSpeed * Time.deltaTime, 0);
        }
        else {
            if (goingTo == OptionsState.CERRAR) {
                gameObject.SetActive(false);
            }
            else {
                currentState = OptionsState.DESPLEGANDO_VIDEO;
            }
        }
    }

    private void DesplegandoVideo() {
        if (transformBG.localPosition.y > originalBgPos.y) {
            transformBG.localPosition -= new Vector3(0, scrollSpeed * Time.deltaTime, 0);
        }
        else {
            transformBG.localPosition = originalBgPos;
            currentState = OptionsState.VIDEO_DESPLEGADO;
            menuElements[1].SetActive(true);
            menuElements[2].SetActive(true);
            menuElements[3].SetActive(true);
        }
    }

    private void DesplegandoAudio() {
        if (transformBG.localPosition.y > originalBgPos.y) {
            transformBG.localPosition -= new Vector3(0, scrollSpeed * Time.deltaTime, 0);
        }
        else {
            transformBG.localPosition = originalBgPos;
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
            foreach (GameObject g in menuElements) {
                g.SetActive(false);
            }
            transformSetaVideo.localPosition = originalSetaVideoPos;
            transformSetaAudio.localPosition += new Vector3(0, setaOffset, 0);
        }
    }

    public void ClickVideo() {
        if(currentState == OptionsState.AUDIO_DESPLEGADO) {
            currentState = OptionsState.REPLEGANDO_AUDIO;
            goingTo = OptionsState.VIDEO_DESPLEGADO;
            foreach (GameObject g in menuElements) {
                g.SetActive(false);
            }
            transformSetaAudio.localPosition = originalSetaAudioPos;
            transformSetaVideo.localPosition += new Vector3(0, setaOffset, 0);
        }
    }

    public void ClickAceptar() {
        if(currentState == OptionsState.AUDIO_DESPLEGADO) {
            currentState = OptionsState.REPLEGANDO_AUDIO;
            goingTo = OptionsState.CERRAR;
            foreach (GameObject g in menuElements) {
                g.SetActive(false);
            }
        }
        else if(currentState == OptionsState.VIDEO_DESPLEGADO) {
            currentState = OptionsState.REPLEGANDO_VIDEO;
            goingTo = OptionsState.CERRAR;
            foreach (GameObject g in menuElements) {
                g.SetActive(false);
            }
        }
    }

    public void ClickCancelar() {
        if (currentState == OptionsState.AUDIO_DESPLEGADO) {
            currentState = OptionsState.REPLEGANDO_AUDIO;
            goingTo = OptionsState.CERRAR;
            foreach (GameObject g in menuElements) {
                g.SetActive(false);
            }
        }
        else if (currentState == OptionsState.VIDEO_DESPLEGADO) {
            currentState = OptionsState.REPLEGANDO_VIDEO;
            goingTo = OptionsState.CERRAR;
            foreach (GameObject g in menuElements) {
                g.SetActive(false);
            }
        }
    }

    private void OnEnable() {
        currentState = OptionsState.DESPLEGANDO_AUDIO;
        goingTo = OptionsState.NONE;
        transformSetaAudio.localPosition = originalSetaAudioPos;
        transformSetaVideo.localPosition = originalSetaVideoPos;
        transformSetaAudio.localPosition += new Vector3(0, setaOffset, 0);
        transformBG.localPosition += new Vector3(0, bgOffset, 0);
    }
}
