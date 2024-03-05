using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class SpeedBoostRing : NetworkBehaviour{

    [SerializeField] float speedIncrease;
    PlayerNetwork lastPlayerHit;
    float lastHitTime = -1000000;
    float timeUntilReset = 5;


    private void OnTriggerEnter(Collider other){
        Debug.Log(other.name);

        PlayerNetwork player = other.transform.root.gameObject.GetComponent<PlayerNetwork>();
        AI_Test ai_controller = other.transform.root.gameObject.GetComponent<AI_Test>();
        
        if (player != null && player != lastPlayerHit) {
            player.AddSpeed(speedIncrease);
            lastPlayerHit = player;
        }
        else if (ai_controller != null) {
            ai_controller.AddSpeed(speedIncrease);
        }

    }

    private void Update(){
        if (Time.time - lastHitTime > timeUntilReset){
            lastPlayerHit = null;
            lastHitTime = Time.time;
        }
    }

}
