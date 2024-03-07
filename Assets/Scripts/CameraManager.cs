using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CameraManager : MonoBehaviour{


    [SerializeField] PlayerNetwork player;
    [SerializeField] CinemachineVirtualCamera virtualCamera;

    [SerializeField] AnimationCurve cameraDistanceCurve;
    [SerializeField] AnimationCurve cameraFOVCurve;

    Cinemachine3rdPersonFollow componentBase;
    

    private void Awake(){

        player.OnSpeedChange += UpdateCameraDistance;

        componentBase = virtualCamera.GetCinemachineComponent<Cinemachine3rdPersonFollow>();
        //componentBase.CameraDistance = cameraDistanceCurve.Evaluate(0);
        virtualCamera.m_Lens.FieldOfView = cameraFOVCurve.Evaluate(0);

    }


    void UpdateCameraDistance(float pCurrentSpeed, float pMaxSpeed, float pTrueMaxSpeed) {
        //componentBase.CameraDistance = cameraDistanceCurve.Evaluate(pCurrentSpeed);

        virtualCamera.m_Lens.FieldOfView = cameraFOVCurve.Evaluate(pCurrentSpeed);
    }

    

}
