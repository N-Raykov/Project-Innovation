using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UIManager : MonoBehaviour{

    [SerializeField] PlayerNetwork targetPlayer;

    [Header("Player Speed")]
    [SerializeField] RectTransform speedIndicatorNormal;
    [SerializeField] RectTransform speedIndicatorOverdrive;
    [SerializeField] TextMeshProUGUI speedText;

    private void Awake(){
        targetPlayer.OnSpeedChange += UpdateSpeedIndicator;
    }

    void UpdateSpeedIndicator(float pCurrentSpeed,float pMaxSpeed,float pTrueMaxSpeed) {
        float cappedSpeed = Mathf.Min(pCurrentSpeed, pMaxSpeed);
        speedIndicatorNormal.localScale = new Vector3(cappedSpeed/pMaxSpeed, speedIndicatorNormal.localScale.y, speedIndicatorNormal.localScale.z);
        speedText.text = pCurrentSpeed.ToString();

        if (pCurrentSpeed > pMaxSpeed) {
            float speedDifMaxTrueMax = pTrueMaxSpeed - pMaxSpeed;
            float speedDif = pCurrentSpeed - pMaxSpeed;
            speedIndicatorOverdrive.localScale = new Vector3(speedDif/speedDifMaxTrueMax, speedIndicatorOverdrive.localScale.y, speedIndicatorOverdrive.localScale.z);
        } else {
            speedIndicatorOverdrive.localScale = new Vector3(0, speedIndicatorOverdrive.localScale.y, speedIndicatorOverdrive.localScale.z);
        }
    }




}
