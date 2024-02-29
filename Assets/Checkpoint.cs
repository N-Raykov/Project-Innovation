using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Checkpoint : MonoBehaviour{


    private void OnTriggerEnter(Collider other){

        PlayerNetwork player = other.transform.root.GetComponent<PlayerNetwork>();

        if (player) {

            player.lastCheckpoint = this;
        
        }

    }

}
