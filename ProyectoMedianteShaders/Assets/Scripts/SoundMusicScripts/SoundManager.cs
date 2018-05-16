using FMOD.Studio;
using FMODUnity;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.InteropServices;
using System;

public class SoundManager : MonoBehaviour {
    #region Parameters

    [Range(0f, 1f)]
    float musicVolume = 1, fxVolume = 1;

    private static SoundManager instance;

    private List<EventInstance> eventsList;

    public EventInstance music;

    private List<SoundManagerMovingSound> positionEvents;

    private float savedFXVolume;

//    private EVENT_CALLBACK dialogueCallback;

    #endregion Parameters

    #region Initialization

    public static SoundManager Instance {
        get {
            if (instance == null) {
                GameObject go = new GameObject();
                instance = go.AddComponent<SoundManager>();
                instance.eventsList = new List<EventInstance>();
                instance.positionEvents = new List<SoundManagerMovingSound>();
                instance.name = "SoundManager";
            }
            return instance;
        }
    }

    void Awake() {
        if ((instance != null && instance != this)) {
            DestroyObject(this.gameObject);
            return;
        } else {
            instance = this;
            DontDestroyOnLoad(this);
            Init();
        }
    }

    void Update() {
        // Actualizamos las posiciones de los sonidos que tienen efecto Estéreo
        if (positionEvents != null && positionEvents.Count > 0) {
            for (int i = 0; i < positionEvents.Count; i++) {
                PLAYBACK_STATE state;
                EventInstance eventInst = positionEvents[i].GetEventInstance();
                eventInst.getPlaybackState(out state);
                if (state == PLAYBACK_STATE.STOPPED) {
                    positionEvents.RemoveAt(i);
                } else {
                    if (!eventInst.Equals(null) && positionEvents.Count > i) {
                        if (!positionEvents[i].Equals(null)) {
                            if (positionEvents[i].GetTransform() != null) {
                                eventInst.set3DAttributes(RuntimeUtils.To3DAttributes(positionEvents[i].GetTransform().position));
                            }
                        }
                    }
                }
            }
        }

        SetFXVolume();
        SetMusicVolume();
    }

    private void Init() {
        if (instance == null) {
            GameObject go = new GameObject();
            instance = go.AddComponent<SoundManager>();
            instance.name = "SoundManager";
            instance.eventsList = new List<EventInstance>();
            instance.positionEvents = new List<SoundManagerMovingSound>();
        }
        //eventsList = new List<EventInstance>();
        //positionEvents = new List<SoundManagerMovingSound>();
        //dialogueCallback = new EVENT_CALLBACK(DialogueEventCallback);
    }

    #endregion Initialization

    #region FMOD Wrapper

    #region Events

    // Usamos esta para objetos con parámetros
    public void PlayOneShotSound(string path, Vector3 pos, List<SoundManagerParameter> parameters = null) {
        EventInstance soundEvent = RuntimeManager.CreateInstance(path);
        if (!soundEvent.Equals(null)) {
            if (parameters != null)
                for (int i = 0; i < parameters.Count; i++)
                    soundEvent.setParameterValue(parameters[i].GetName(), parameters[i].GetValue());
            if (!pos.Equals(null))
                soundEvent.set3DAttributes(RuntimeUtils.To3DAttributes(pos));
            soundEvent.start();
            soundEvent.release();
        }
    }

    // Usamos esta para objetos en movimiento que actualizan la posición del sonido
    public void PlayOneShotSound(string path, Transform transform) {
        EventInstance soundEvent = RuntimeManager.CreateInstance(path);
        if (!soundEvent.Equals(null)) {
            soundEvent.set3DAttributes(RuntimeUtils.To3DAttributes(transform.position));
            soundEvent.start();
            SoundManagerMovingSound movingSound = new SoundManagerMovingSound(transform, soundEvent);
            positionEvents.Add(movingSound);
            soundEvent.release();
        }
    }

    public EventInstance PlayEvent(string path, Vector3 pos) {
        EventInstance soundEvent = RuntimeManager.CreateInstance(path);
        if (!soundEvent.Equals(null)) {
            soundEvent.set3DAttributes(RuntimeUtils.To3DAttributes(pos));
            soundEvent.start();
            Instance.eventsList.Add(soundEvent);
        }
        return soundEvent;
    }


    public EventInstance PlayMusic(string path, Vector3 pos) {
        EventInstance soundEvent = RuntimeManager.CreateInstance(path);
        if (!soundEvent.Equals(null)) {
            soundEvent.set3DAttributes(RuntimeUtils.To3DAttributes(pos));
            soundEvent.start();
            //Instance.eventsList.Add(soundEvent);
            Instance.music = soundEvent;
        }
        return soundEvent;
    }

    // Usamos esta para objetos en movimiento que actualizan la posición del sonido
    public EventInstance PlayEvent(string path, Transform transform) {
        EventInstance soundEvent = RuntimeManager.CreateInstance(path);
        if (!soundEvent.Equals(null)) {
            soundEvent.set3DAttributes(RuntimeUtils.To3DAttributes(transform.position));
            soundEvent.start();
            SoundManagerMovingSound movingSound = new SoundManagerMovingSound(transform, soundEvent);
            positionEvents.Add(movingSound);
            eventsList.Add(soundEvent);
        }
        return soundEvent;
    }

    //public void PlayDialogue(string key, string eventPath, Vector3 pos) {
    //    EventInstance dialogueInstance = RuntimeManager.CreateInstance(eventPath);
    //    if (!dialogueInstance.Equals(null)) {
    //        dialogueInstance.set3DAttributes(RuntimeUtils.To3DAttributes(pos));
    //        dialogueInstance.start();
    //        eventsList.Add(dialogueInstance);
    //    }

    //    // Pin the key string in memory and pass a pointer through the user data
    //    GCHandle stringHandle = GCHandle.Alloc(key, GCHandleType.Pinned);
    //    dialogueInstance.setUserData(GCHandle.ToIntPtr(stringHandle));

    //    //dialogueInstance.setCallback(dialogueCallback);
    //    dialogueInstance.start();
    //    dialogueInstance.release();
    ////}

    //public FMOD.RESULT DialogueEventCallback(FMOD.Studio.EVENT_CALLBACK_TYPE type, FMOD.Studio.EventInstance instance, IntPtr parameterPtr) {
    //    //Retrieve the user data
    //    IntPtr stringPtr;
    //    instance.getUserData(out stringPtr);

    //    //Get the string object
    //    GCHandle stringHandle = GCHandle.FromIntPtr(stringPtr);
    //    String key = stringHandle.Target as String;

    //    switch (type) {
    //        case EVENT_CALLBACK_TYPE.CREATE_PROGRAMMER_SOUND: {
    //                var parameter = (PROGRAMMER_SOUND_PROPERTIES)Marshal.PtrToStructure(parameterPtr, typeof(PROGRAMMER_SOUND_PROPERTIES));
    //                SOUND_INFO dialogueSoundInfo;
    //                var keyResult = RuntimeManager.StudioSystem.getSoundInfo(key, out dialogueSoundInfo);
    //                if (keyResult != FMOD.RESULT.OK)
    //                    break;
    //                FMOD.Sound dialogueSound;
    //                var soundResult = RuntimeManager.LowlevelSystem.createSound(dialogueSoundInfo.name_or_data, dialogueSoundInfo.mode, ref dialogueSoundInfo.exinfo, out dialogueSound);
    //                if (soundResult == FMOD.RESULT.OK) {
    //                    parameter.sound = dialogueSound.handle;
    //                    parameter.subsoundIndex = dialogueSoundInfo.subsoundindex;
    //                    Marshal.StructureToPtr(parameter, parameterPtr, false);
    //                }
    //            }
    //            break;
    //        case EVENT_CALLBACK_TYPE.DESTROY_PROGRAMMER_SOUND: {
    //                var parameter = (PROGRAMMER_SOUND_PROPERTIES)Marshal.PtrToStructure(parameterPtr, typeof(PROGRAMMER_SOUND_PROPERTIES));
    //                var sound = new FMOD.Sound();
    //                sound.handle = parameter.sound;
    //                sound.release();
    //            }
    //            break;
    //        case EVENT_CALLBACK_TYPE.DESTROYED:
    //            stringHandle.Free();
    //            break;
    //    }
    //    return FMOD.RESULT.OK;
    //}

    public void UpdateEventParameter(EventInstance soundEvent, SoundManagerParameter parameter) {
        soundEvent.setParameterValue(parameter.GetName(), parameter.GetValue());
    }

    public void UpdateEventParameters(EventInstance soundEvent, List<SoundManagerParameter> parameters) {
        for (int i = 0; i < parameters.Count; i++)
            soundEvent.setParameterValue(parameters[i].GetName(), parameters[i].GetValue());
    }

    public void StopEvent(EventInstance soundEvent, bool fadeout = true) {
        if (eventsList.Remove(soundEvent)) {
            if (fadeout)
                soundEvent.stop(STOP_MODE.ALLOWFADEOUT);
            else
                soundEvent.stop(STOP_MODE.IMMEDIATE);
        }
    }

    public void PauseEvent(EventInstance soundEvent) {
        if (eventsList.Contains(soundEvent)) {
            soundEvent.setPaused(true);
        }
    }

    public void ResumeEvent(EventInstance soundEvent) {
        if (eventsList.Contains(soundEvent)) {
            soundEvent.setPaused(false);
        }
    }

    public void StopAllEvents(bool fadeout) {
        for (int i = 0; i < eventsList.Count; i++) {
            if (fadeout)
                eventsList[i].stop(STOP_MODE.ALLOWFADEOUT);
            else
                eventsList[i].stop(STOP_MODE.IMMEDIATE);
        }

        eventsList.Clear();
    }

    public void PauseAllEvents() {
        for (int i = 0; i < eventsList.Count; i++) {
            eventsList[i].setPaused(true);
        }
    }

    public void ResumeAllEvents() {
        for (int i = 0; i < eventsList.Count; i++) {
            eventsList[i].setPaused(false);
        }
    }

    public bool isPlaying(EventInstance e) {
        PLAYBACK_STATE state;
        e.getPlaybackState(out state);
        return state.Equals(PLAYBACK_STATE.PLAYING);
    }

    #endregion Events

    #region Mixer

    public void ChangeFXVolume(float newVolume) {
        fxVolume = newVolume;
    }

    public void ChangeAndSaveFXVolume(float newVolume) {
        savedFXVolume = fxVolume;
        fxVolume = newVolume;
    }

    public void RestoreFXVolume() {
        fxVolume = savedFXVolume;
    }

    void SetFXVolume() {
        VCA vca;

        if (RuntimeManager.StudioSystem.getVCA("vca:/FX", out vca) != FMOD.RESULT.OK)
            return;

        vca.setVolume(fxVolume);
    }

    public void ChangeMusicVolume(float newVolume) {
        musicVolume = newVolume;

    }

    void SetMusicVolume() {
        VCA vca;

        if (RuntimeManager.StudioSystem.getVCA("vca:/Music", out vca) != FMOD.RESULT.OK)
            return;

        vca.setVolume(musicVolume);
    }

    public float GetMusicVolume() {
        return musicVolume;
    }

    public float GetFXVolume() {
        return fxVolume;
    }

    #endregion Mixer

    #endregion FMOD Wrapper
}

#region ExtraClasses

//Parametro genérico de FMOD para pasar a los eventos
public class SoundManagerParameter {
    string name;
    float value;

    public SoundManagerParameter(string name, float value) {
        this.name = name;
        this.value = value;
    }

    public string GetName() {
        return name;
    }

    public float GetValue() {
        return value;
    }
}

//Parametro genérico de FMOD para pasar a los eventos
class SoundManagerMovingSound {
    Transform transform;
    EventInstance eventIns;

    public SoundManagerMovingSound(Transform transform, EventInstance eventIns) {
        this.transform = transform;
        this.eventIns = eventIns;
    }

    public Transform GetTransform() {
        return transform;
    }

    public EventInstance GetEventInstance() {
        return eventIns;
    }
}

#endregion ExtraClasses
