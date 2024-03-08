using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallScript : MonoBehaviour{

    private void OnCollisionEnter(Collision collision){

        PlayerNetwork player = collision.transform.root.GetComponent<PlayerNetwork>();

        if (player) {
            Debug.Log("respawn");
            player.Respawn();
        }

    }

}
