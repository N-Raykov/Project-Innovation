using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpeedBoostRing : MonoBehaviour{

    [SerializeField] float speedIncrease; 

    private void OnTriggerEnter(Collider other){
        Debug.Log(other.name);

        PlayerNetwork player = other.transform.root.gameObject.GetComponent<PlayerNetwork>();
        
        if (player != null) {
            player.AddSpeed(speedIncrease);
        }

    }
}
