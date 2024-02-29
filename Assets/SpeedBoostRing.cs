using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class SpeedBoostRing : NetworkBehaviour{

    [SerializeField] float speedIncrease; 

    private void OnTriggerEnter(Collider other){
        Debug.Log(other.name);

        PlayerNetwork player = other.transform.root.gameObject.GetComponent<PlayerNetwork>();
        
        if (player != null) {
            player.AddSpeed(speedIncrease);
        }

    }
}
