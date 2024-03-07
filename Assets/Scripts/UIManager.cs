using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UIManager : MonoBehaviour{

    public static UIManager instance { get; private set; }

    [SerializeField] PlayerNetwork targetPlayer;

    [Header("Player Speed")]
    [SerializeField] RectTransform speedIndicatorNormal;
    [SerializeField] RectTransform speedIndicatorOverdrive;

    [Header("Round Start Countdown")]
    [SerializeField] TextMeshProUGUI countdownText;
    [SerializeField] int countdownDuration;

    [Header("Skill")]
    [SerializeField] RectTransform SkillCooldownIndicator;



    private void Awake(){

        if (instance != null)
            Destroy(instance);

        instance = this;
        targetPlayer.OnSpeedChange += UpdateSpeedIndicator;
    }

    private void Start(){
        StartCountdown(countdownDuration);
    }

    void UpdateSpeedIndicator(float pCurrentSpeed,float pMaxSpeed,float pTrueMaxSpeed) {
        float cappedSpeed = Mathf.Min(pCurrentSpeed, pMaxSpeed);
        speedIndicatorNormal.localScale = new Vector3(cappedSpeed/pMaxSpeed, speedIndicatorNormal.localScale.y, speedIndicatorNormal.localScale.z);

        if (pCurrentSpeed > pMaxSpeed) {
            float speedDifMaxTrueMax = pTrueMaxSpeed - pMaxSpeed;
            float speedDif = pCurrentSpeed - pMaxSpeed;
            speedIndicatorOverdrive.localScale = new Vector3(speedDif/speedDifMaxTrueMax, speedIndicatorOverdrive.localScale.y, speedIndicatorOverdrive.localScale.z);
        } else {
            speedIndicatorOverdrive.localScale = new Vector3(0, speedIndicatorOverdrive.localScale.y, speedIndicatorOverdrive.localScale.z);
        }
    }

    public void StartCountdown(int duration) {
        StartCoroutine(CountdownCoroutine(duration));
    }

    IEnumerator CountdownCoroutine(int duration) {

        while (duration >= 0) {
            countdownText.text = duration.ToString();
            duration--;
            yield return new WaitForSeconds(1);
        }

        targetPlayer.isMovementEnabled = true;
        countdownText.enabled = false;

    }


}
