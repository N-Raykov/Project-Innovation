using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class UIManager : MonoBehaviour{

    public static UIManager instance { get; private set; }

    [SerializeField] PlayerNetwork targetPlayer;
    [SerializeField] SkillBase skill;
    [SerializeField] Attacker attacker;

    [Header("Player Speed")]
    [SerializeField] RectTransform speedIndicatorNormal;
    [SerializeField] RectTransform speedIndicatorOverdrive;

    [Header("Round Start Countdown")]
    [SerializeField] TextMeshProUGUI countdownText;
    [SerializeField] int countdownDuration;

    [Header("Skill")]
    [SerializeField] RectTransform skillCooldownIndicator;

    [Header("AttackOverheat")]
    [SerializeField] RectTransform overheatIndicator;
    [SerializeField] Image backgroundImage;
    [SerializeField] Image overheatImage;
    [SerializeField] Gradient gradient;
    [SerializeField] float timeToStartDissapearing;
    [SerializeField] float timeUntilFullDissapear;


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

    private void Update(){
        UpdateSkillCooldown();
        UpdateShootingOverheat();
    }

    void UpdateSkillCooldown() {
        skillCooldownIndicator.localScale = new Vector3(skill.ReturnSkillRechargeTime(), skillCooldownIndicator.localScale.y, skillCooldownIndicator.localScale.z);
    }

    void UpdateShootingOverheat() {

        if (attacker.overheatAmount == 0 && Time.time - attacker.timeHeatWasAtZero > timeToStartDissapearing ){

            float alphaValue = 1 - (Time.time - attacker.timeHeatWasAtZero - timeToStartDissapearing) / (timeUntilFullDissapear - timeToStartDissapearing);

            overheatImage.color = new Color(overheatImage.color.r, overheatImage.color.g, overheatImage.color.b,alphaValue);
            backgroundImage.color = new Color(backgroundImage.color.r, backgroundImage.color.g, backgroundImage.color.b, alphaValue);
        }

        if (attacker.overheatAmount != 0) {
            overheatIndicator.localScale = new Vector3(overheatIndicator.localScale.x, attacker.overheatAmount / attacker.ReturnMaxOverheatAmount(), overheatIndicator.localScale.z);
            overheatImage.color = gradient.Evaluate(attacker.overheatAmount / attacker.ReturnMaxOverheatAmount());
            backgroundImage.color = new Color(backgroundImage.color.r, backgroundImage.color.g, backgroundImage.color.b, 1);
        }

    }


}
