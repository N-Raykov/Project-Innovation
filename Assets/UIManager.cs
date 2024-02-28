using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour{

    [SerializeField] PlayerNetwork targetPlayer;


    [SerializeField] RectTransform speedIndicatorNormal;

    private void Awake(){
        targetPlayer.OnSpeedChange += UpdateSpeedIndicator;
    }


    void UpdateSpeedIndicator(float pCurrentSpeed,float pMaxSpeed,float pTrueMaxSpeed) {
        float cappedSpeed = Mathf.Min(pCurrentSpeed, pMaxSpeed);
        speedIndicatorNormal.localScale = new Vector3(cappedSpeed/pMaxSpeed, speedIndicatorNormal.localScale.y, speedIndicatorNormal.localScale.z);
    }




}
