using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoubleUselessDoor : DoubleObject {
    public ParticleSystem particleSystem;
    public BoxCollider collider;
    bool done = false;
    private void Start() {
        collider.enabled = false;
        if (worldAssignation == world.DAWN) {
            transform.position = new Vector3(brotherObject.transform.position.x, brotherObject.transform.position.y + GameLogic.instance.worldOffset, brotherObject.transform.position.z);
        }
    }

    private void Update() {
        if (!done) {
            if (GameLogic.instance != null) {
                if (GameLogic.instance.currentPlayer != null&&GameLogic.instance.currentPlayer.placeToGo==null) {
                    if (GameLogic.instance.currentPlayer.transform.position.x - transform.position.x > 1.3f) {
                        if (particleSystem != null) {
                            particleSystem.Stop();
                            collider.enabled = true;
                            done = true;
                        }
                    }
                }
            }
        }
    }
}
