using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelMusic : DoubleObject {
    AudioSource musicPlayer;
    AudioClip music;
    public int level;
    // Use this for initialization
    void Start() {
        level = 4;
        //Start del transformable
        musicPlayer = GetComponent<AudioSource>();

        InitTransformable();
        musicPlayer.clip = music;
        musicPlayer.Play();
    }

    // Update is called once per frame
    void Update() {
        AddToGameLogicList();
        //if (GameLogic.instance.isPaused) {
        //    musicPlayer.Pause();

        //}
        //else{
        //    if (!musicPlayer.isPlaying) {
        //        musicPlayer.Play();
        //    }
        //}
    }

    protected override void LoadResources() {
        switch (level) {
            case 0:
                break;
            default:
                music = Resources.Load<AudioClip>("Music/PixelLoop");
                break;
        }
    }

    public override void Change() {
        dawn = !dawn;
        if (dawn) {
            GetComponent<AudioSource>().pitch = 1f;
        }else {
            GetComponent<AudioSource>().pitch = 0.75f;
        }
    }
}