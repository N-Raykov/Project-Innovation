using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class LevelManager : MonoBehaviour{

    public static LevelManager instance { get; private set; }

    [SerializeField] int countdownDuration;
    [SerializeField] int _respawnDuration;
    [SerializeField] SplineNetwork splineNetwork;


    [Header("Laps")]
    [SerializeField] int _lapCountToWin;
    public int lapCountToWin { get { return _lapCountToWin; } }


    public int respawnDuration { get { return _respawnDuration; } }
    


    PlayerNetwork player;
    AI_Test[] ai;
    BezierSpline[] splines;

    int playerLap;

    private void Awake(){
        if (instance != null) {
            Destroy(instance.gameObject);
        }

        instance = this;
    }

    private void Start(){
        splines = FindObjectsByType<BezierSpline>(FindObjectsSortMode.None);

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

    public Vector3 RequestNearestPoint(Vector3 pPlanePos,out Vector3 pForward){

        Vector3 minDistance = new Vector3(1000000,1000000,100000);
        float progress = 0;
        BezierSpline closestSpline=new BezierSpline();

        foreach (BezierSpline b in splines) {
            float p=0;
            Vector3 pos=b.PointOnTrack(pPlanePos,out p);
            if ((pos - pPlanePos).magnitude < (minDistance - pPlanePos).magnitude) {
                progress = p;
                minDistance = pos;
                closestSpline = b;
            }

        }

        pForward = closestSpline.GetDirection(progress).normalized;
        return minDistance;

    }

}
