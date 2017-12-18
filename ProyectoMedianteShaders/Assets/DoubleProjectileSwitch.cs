﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoubleProjectileSwitch : DoubleObject{
    //Tamaño del array de objetos 

    [Tooltip("Aumentar esta variable permite activar más objetos")]
    static public int size;

    [Tooltip("Hay que aumentar size para poder añadir nuevos objetos")]
    public DoubleObject[] objectsToTrigger = new DoubleObject[size];

    //sistema de particulas para cuando se activa el switch
    private ParticleSystem particles;
    private int listCount; //counter for iterate the list of elements to activate.

    // Use this for initialization
    void Start(){
        InitTransformable();
        
        offset = GameLogic.instance.worldOffset;
        GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezeAll;

        if (worldAssignation == world.DAWN){
            GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Kinematic;
        }
        GetComponent<Rigidbody2D>().gravityScale = 0;

        particles = GetComponentInChildren<ParticleSystem>();
        particles.Stop();
        listCount = 0;
    }

    protected override void BrotherBehavior(){
        Vector3 positionWithOffset;
        if (GetComponent<Rigidbody2D>().bodyType == RigidbodyType2D.Kinematic){
            positionWithOffset = brotherObject.transform.position;

            if (worldAssignation == world.DAWN) {
                positionWithOffset.y += offset;
            } else {
                positionWithOffset.y -= offset;
            }

            transform.position = positionWithOffset;
            transform.rotation = brotherObject.transform.rotation;
        }

    }



    // Update is called once per frame
    void Update()
    {
        AddToGameLogicList();
        BrotherBehavior();
        if(particles != null) {
            ParticleSystemBehavior();
        }
        
    }

    //Manages the particle system.
    private void ParticleSystemBehavior() {
        if (particles.isEmitting) {
            if(listCount < objectsToTrigger.Length) {
                particles.gameObject.transform.position = Vector3.MoveTowards(particles.gameObject.transform.position, objectsToTrigger[listCount].transform.position, 10 * Time.deltaTime);
                if((particles.gameObject.transform.position - objectsToTrigger[listCount].transform.position).magnitude < 0.1) {
                    particles.startSpeed += 1;
                    particles.Emit(200);
                    particles.startSpeed -= 1;
                    listCount++;
                }
            } else {
                particles.Stop();
            }
        }
    }

    public override void Activate() {
        base.Activate();
        particles.Play();
    }

    //Se comprueba si el objeto que ha entrado en la zona de trigger es un proyectil y en caso afirmativo se activan con el método Activate todos los objetos
    //Que se encuentran en objectsToTrigger
    public void OnTriggerEnter2D(Collider2D collision){
        if (collision.gameObject.tag == "Projectile"&&!activated){
            Activate();
            /*
            foreach (DoubleObject g in objectsToTrigger)
            {
                Debug.Log("ACTIVATE");
                g.Activate();
                brotherObject.GetComponent<DoubleObject>().Activate();
            }
            */
        }

    }
}

