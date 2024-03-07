using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class LevelManager : MonoBehaviour{

    public LevelManager instance { get; private set; }

    PlayerNetwork player;
    int playerLap;

    private void Awake(){
        if (instance != null) {
            Destroy(instance.gameObject);
        }

        instance = this;
    }

    private void Start(){
        player=FindObjectOfType<PlayerNetwork>();
    }

}
