using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class LevelManager : MonoBehaviour{

    public static LevelManager instance { get; private set; }

    [SerializeField] int countdownDuration;
    [SerializeField] int _respawnDuration;
    public int respawnDuration { get { return _respawnDuration; } }

    PlayerNetwork player;
    AI_Test[] ai;

    int playerLap;

    private void Awake(){
        if (instance != null) {
            Destroy(instance.gameObject);
        }

        instance = this;
    }

    private void Start(){
        

        player=FindObjectOfType<PlayerNetwork>();
        ai = FindObjectsByType<AI_Test>(FindObjectsSortMode.None);


        UIManager.instance.StartCountdown(countdownDuration);
        StartCoroutine(ManagerCountdownCoroutine(countdownDuration));
    }

    IEnumerator ManagerCountdownCoroutine(int duration){

        foreach (AI_Test a in ai){
            a.isMovementEnabled = false;
        }

        while (duration >= 0){
            duration--;
            yield return new WaitForSeconds(1);
        }

        foreach (AI_Test a in ai) {
            a.isMovementEnabled = true;
        }
    }

}
