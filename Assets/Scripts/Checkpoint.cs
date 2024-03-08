using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Checkpoint : MonoBehaviour{

    private void OnTriggerEnter(Collider other){
        PlayerNetwork p = other.transform.root.GetComponent<PlayerNetwork>();

        if (p != null) {
            p.currentLap++;
        }

        AI_Test a= other.transform.root.GetComponent<AI_Test>();

        if (a != null) {
            a.currentLap++;
        }
    }

}
