using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Checkpoint : MonoBehaviour{

    PlayerNetwork lastPlayerHit;
    float lastHitTime=-1000000;
    float timeUntilReset=5;


    private void OnTriggerEnter(Collider other){

        PlayerNetwork player = other.transform.root.GetComponent<PlayerNetwork>();

        if (player&&player!=lastPlayerHit) {
            player.lastCheckpoint = this;
        }

    }

    private void Update(){
        if (Time.time - lastHitTime > timeUntilReset) {
            lastPlayerHit = null;
            lastHitTime = Time.time;
        }
    }

}
