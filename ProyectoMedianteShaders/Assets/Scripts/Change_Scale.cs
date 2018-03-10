﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * Este script controla la escala de la esfera que usa el shader de cambio de mundo como máscara.
 * 
 * También controla el sistema de particulas que acompaña al efecto de cambio de mundo.
 */ 

public class Change_Scale : MonoBehaviour {

    [HideInInspector]public bool change;
    public float velocity = 50;
    public float maxScale = 25;
    [SerializeField] private ParticleSystem psChangeToDawn, psChangeToDusk;
    [SerializeField] private Material dawnParticlesMat, duskParticlesMat;
    private bool emissionToDawn;
    private ParticleSystemRenderer particleSystemRenderer;
    private ParticleSystem.MainModule psMainModule;
    private ParticleSystem.ShapeModule psShapeModule;

    void Start() {
        transform.localScale = new Vector3(0, 0, 0);
        emissionToDawn = true;
        psChangeToDawn.Play();
        //psChangeToDusk.Play();
        particleSystemRenderer = psChangeToDawn.GetComponent<ParticleSystemRenderer>();
        psMainModule = psChangeToDawn.main;
        psShapeModule = psChangeToDawn.shape;
    }

    void Update() { 
        if (change) { //CRECER
            if (transform.localScale.x < maxScale) {
                transform.localScale += new Vector3(1.0f, 1.0f, 1.0f) * velocity * Time.deltaTime;
            }

            if (emissionToDawn) { //particulas hacia fuera, cambia a dawn
                //psChangeWorld.Play();
                particleSystemRenderer.material = dawnParticlesMat;
                particleSystemRenderer.trailMaterial = dawnParticlesMat;
                psMainModule.startLifetime = 0.3f;
                psMainModule.startSpeed = 50;
                psShapeModule.radius = 0.01f;
                psChangeToDawn.Emit(300);
                print("Play to dawn");
                emissionToDawn = false;
            }
        } else { //DECRECER
            if (transform.localScale.x > 1) {
                transform.localScale -= new Vector3(1.0f, 1.0f, 1.0f) * velocity * Time.deltaTime;
                if (!emissionToDawn) { //particulas hacia dentro
                    //psChangeWorld.Play();
                    particleSystemRenderer.material = duskParticlesMat;
                    particleSystemRenderer.trailMaterial = duskParticlesMat;
                    psMainModule.startLifetime = 0.25f;
                    psMainModule.startSpeed = -50;
                    psShapeModule.radius = 10.5f;
                    psChangeToDawn.Emit(300);
                    print("play to dusk");
                    emissionToDawn = true;
                }
            } else {
                transform.localScale = new Vector3(0, 0, 0);
                
            }
        }
    }
}
