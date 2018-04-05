using System.Collections;
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
    [SerializeField] private ParticleSystem psChangeWorld;
    [SerializeField] private int particlesToDawn = 150, particlesToDusk = 150;
    [SerializeField] private Material dawnParticlesMat, duskParticlesMat;
    private bool emissionToDawn;
    private ParticleSystemRenderer particleSystemRenderer;
    private ParticleSystem.MainModule psMainModule;
    private ParticleSystem.ShapeModule psShapeModule;

    void Start() {
        transform.localScale = new Vector3(0, 0, 0);
        emissionToDawn = true;
        if (psChangeWorld != null) {
            psChangeWorld.Play();
            particleSystemRenderer = psChangeWorld.GetComponent<ParticleSystemRenderer>();
            psMainModule = psChangeWorld.main;
            psShapeModule = psChangeWorld.shape;
        }
        else {
            print("Sistema de particulas de cambio de mundo no asignado");
        }
    }

    void Update() { 
        if (change) { //CRECER
            if (transform.localScale.x < maxScale) {
                transform.localScale += new Vector3(1.0f, 1.0f, 1.0f) * velocity * Time.deltaTime;
            }

            if (emissionToDawn && psChangeWorld != null) { //particulas hacia fuera, cambia a dawn
                particleSystemRenderer.material = dawnParticlesMat;
                particleSystemRenderer.trailMaterial = dawnParticlesMat;
                psMainModule.startLifetime = 0.3f;
                psMainModule.startSpeed = 50;
                psShapeModule.radius = 0.01f;
                psChangeWorld.Emit(particlesToDawn);
                emissionToDawn = false;
            }
        } else { //DECRECER
            if (transform.localScale.x > 1) {
                transform.localScale -= new Vector3(1.0f, 1.0f, 1.0f) * velocity * Time.deltaTime;
                if (!emissionToDawn && psChangeWorld != null) { //particulas hacia dentro
                    particleSystemRenderer.material = duskParticlesMat;
                    particleSystemRenderer.trailMaterial = duskParticlesMat;
                    psMainModule.startLifetime = 0.25f;
                    psMainModule.startSpeed = -50;
                    psShapeModule.radius = 10.5f;
                    psChangeWorld.Emit(particlesToDusk);
                    //print("play to dusk");
                    emissionToDawn = true;
                }
            } else {
                transform.localScale = new Vector3(0, 0, 0);
                
            }
        }
    }
}
