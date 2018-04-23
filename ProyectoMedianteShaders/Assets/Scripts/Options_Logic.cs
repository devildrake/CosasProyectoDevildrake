using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Options_Logic : MonoBehaviour {
    enum OptionsState { NONE, DESPLEGANDO_AUDIO, AUDIO_DESPLEGADO, REPLEGANDO_AUDIO, DESPLEGANDO_VIDEO, VIDEO_DESPLEGADO, REPLEGANDO_VIDEO, CERRAR };
    private OptionsState currentState, goingTo;
    [SerializeField] private GameObject bg;
    [SerializeField] private List<GameObject> menuElements; //Audio - Video - Aceptar - Cancelar
    [SerializeField] private GameObject setaAudio, setaVideo;
    private RectTransform transformSetaAudio, transformSetaVideo,transformBG;
    [SerializeField] private int bgOffset, scrollSpeed = 3500, setaOffset = 30;
    private Vector2 originalBgPos, originalSetaAudioPos, originalSetaVideoPos;
    private int bgMovThreshold = 3;

    //VARIABLES PARA CONTROLAR LA SELECCION DE LAS OPCIONES
    //VARIABLES PARA CONTROLAR LA SELECCION DE LAS OPCIONES
    //VARIABLES PARA CONTROLAR LA SELECCION DE LAS OPCIONES
    [SerializeField] private Slider musica, sfx;
    [SerializeField] private Toggle mute, mostrarTiempo, pantallaCompleta;

    [SerializeField] private int slideItemOffset = 205;//distancia de separación entre elementos en nuestro slider de las opciones.
    private List<Vector2> resolutions = new List<Vector2> {new Vector2(640,480), new Vector2(800,600), new Vector2(1024,600), new Vector2(1280,720),
                                                            new Vector2(1280,1024), new Vector2(1400,1050), new Vector2(1600, 900), new Vector2(1920,1080)};
    private List<int> fpsList = new List<int> { -1, 30, 60, 90, 120 };
    private const int qualityElements = 6;

    private int resolutionSel = 0, fpsSel = 0, qualitySel = 0; //Que elemento de nuestro slider estas seleccionando.
    [SerializeField] private GameObject resolutionGroup, fpsGroup, qualityGroup; //Referencias a los grupos moviles de mis sliders
    //valores previos de las variables
    private float prevMusic, prevSfx;
    private Vector2 initialResolutionGroupPos, initialQualityGroupPos, initialFpsGroupPos;
    private int prevResolution, prevQuality, prevFps;
    private bool prevMute, prevMostrarTiempo;

    void Awake() {
        transformSetaAudio = setaAudio.GetComponent<RectTransform>();
        transformSetaVideo = setaVideo.GetComponent<RectTransform>();
        transformBG = bg.GetComponent<RectTransform>();
        originalBgPos = transformBG.localPosition;
        originalSetaAudioPos = transformSetaAudio.localPosition;
        originalSetaVideoPos = transformSetaVideo.localPosition;
        initialFpsGroupPos = fpsGroup.transform.localPosition;
        initialQualityGroupPos = qualityGroup.transform.localPosition;
        initialResolutionGroupPos = resolutionGroup.transform.localPosition;

        //se ponen por defecto los valores de las opciones y se guardan los iniciales por si se le da a cancelar
        //para que la proxima vez que se abran las opciones no aparezcan cambiadas.
        //AUDIO
        musica.value = prevMusic = 1;
        sfx.value = prevSfx = 1;
        mute.isOn = prevMute = false;
        //VIDEO
        mostrarTiempo.isOn = prevMostrarTiempo = false;
        pantallaCompleta.isOn = Screen.fullScreen;
        
        //buscar la resolucion de la pantalla para marcarla.
        bool found = false;
        for(int i = 0; i<resolutions.Count && !found; i++) {
            if(resolutions[i].x == Screen.width && resolutions[i].y == Screen.height) {
                found = true;
                resolutionSel = i;
                prevResolution = i;
            }
        }
        if (found) {
            resolutionGroup.transform.localPosition = initialResolutionGroupPos - new Vector2(resolutionSel * slideItemOffset, 0);
        }
        else {
            resolutionGroup.transform.localPosition = initialResolutionGroupPos - new Vector2(resolutions.Count * slideItemOffset, 0);
        }

        //buscar la limitación de fps
        found = false;
        for (int i = 0; i<fpsList.Count && !found; i++) {
            if(fpsList[i] == Application.targetFrameRate) {
                found = true;
                fpsSel = i;
                prevFps = i;
            }
        }
        if (found) {
            fpsGroup.transform.localPosition = initialFpsGroupPos - new Vector2(fpsSel * slideItemOffset, 0);
        }
        else {
            fpsGroup.transform.localPosition = initialFpsGroupPos;
        }

        //Buscar la configuración de graficos
        qualitySel = prevQuality = QualitySettings.GetQualityLevel();
        qualityGroup.transform.localPosition = initialQualityGroupPos - new Vector2(qualitySel * slideItemOffset, 0);

    }

    void Start() {
        currentState = OptionsState.DESPLEGANDO_AUDIO;
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

        //se le pasan los valores de configuración al gamelogic y se actualizan los valores previos
        GameLogic.instance.musicVolume = prevMusic = musica.value;
        GameLogic.instance.sfxVolume = prevSfx = sfx.value;
        GameLogic.instance.muteVolume = prevMute = mute.isOn;
        GameLogic.instance.mostrarTiempo = prevMostrarTiempo = mostrarTiempo.isOn;
        GameLogic.instance.fullscreen = pantallaCompleta.isOn;
        prevResolution = resolutionSel;
        GameLogic.instance.resolutionSelected = resolutions[resolutionSel];
        prevFps = fpsSel;
        GameLogic.instance.maxFrameRate = fpsList[fpsSel];
        GameLogic.instance.graficos = prevQuality = qualitySel;

        GameLogic.instance.changeGameSettings();
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

        //resetear todos los cambios que se le hayan hecho al menu
        musica.value = prevMusic;
        sfx.value = prevSfx;
        mute.isOn = prevMute;
        mostrarTiempo.isOn = prevMostrarTiempo;
        pantallaCompleta.isOn = Screen.fullScreen;
        resolutionSel = prevResolution;
        resolutionGroup.transform.localPosition = initialResolutionGroupPos - new Vector2(prevResolution * slideItemOffset, 0);
        fpsSel = prevFps;
        fpsGroup.transform.localPosition = initialFpsGroupPos - new Vector2(prevFps * slideItemOffset, 0);
        qualitySel = prevQuality;
        qualityGroup.transform.localPosition = initialQualityGroupPos - new Vector2(prevQuality * slideItemOffset, 0);
    }

    /*
     * Controla los botones de las opciones para deslizar la seleccion a derecha e izquierda.
     * Recibe un gameobject que es el que se va a deslizar y un bool que marca si va a la derecha (true)
     * o a la izquierda (false)
     */
    public void MyScrollingOptionRight(GameObject go) {
        if (go.name.Equals("resolution_group")) {
            if (resolutionSel < resolutions.Count - 1) {
                go.transform.position -= new Vector3(slideItemOffset, 0, 0);
                resolutionSel++;
            }
        }
        else if (go.name.Equals("fps_group")) {
            if (fpsSel < fpsList.Count - 1) {
                go.transform.position -= new Vector3(slideItemOffset, 0, 0);
                fpsSel++;
            }
        }
        else if (go.name.Equals("calidad_group")) {
            if (qualitySel < qualityElements - 1) {
                go.transform.position -= new Vector3(slideItemOffset, 0, 0);
                qualitySel++;
            }
        }
    }

    public void MyScrollingOptionsLeft(GameObject go) {
        if (go.name.Equals("resolution_group")) {
            if (resolutionSel > 0) {
                go.transform.position += new Vector3(slideItemOffset, 0, 0);
                resolutionSel--;
            }
        }
        else if (go.name.Equals("fps_group")) {
            if (fpsSel > 0) {
                go.transform.position += new Vector3(slideItemOffset, 0, 0);
                fpsSel--;
            }
        }
        else if (go.name.Equals("calidad_group")) {
            if (qualitySel > 0) {
                go.transform.position += new Vector3(slideItemOffset, 0, 0);
                qualitySel--;
            }
        }
    }
}
