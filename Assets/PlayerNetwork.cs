using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
using UnityEngine.InputSystem;
using Cinemachine;
using System;

public class PlayerNetwork : NetworkBehaviour{


    public Action<float,float,float> OnSpeedChange;

    [SerializeField] Transform shootPoint;

    [SerializeField] InputActionReference moveActionToUse;
    [SerializeField] float speed;
    [SerializeField] GameObject bulletPrefab;
    [SerializeField] float cooldown;

    [Header("Side Rotation")]
    [SerializeField] float minSideRotationValue;
    [SerializeField] float maxSideRotationValue;

    [Header("Forward Rotation")]

    [SerializeField] float minForwardRotationValue;
    [SerializeField] float maxForwardRotationValue;

    [Header("Side Tilt Values")]

    [SerializeField] float minSideTiltValue;
    [SerializeField] float maxSideTiltValue;

    [Header("Forward Tilt Values")]

    [SerializeField] float minForwardTiltValue;
    [SerializeField] float maxForwardTiltValue;

    [Header("Backward Tilt Values")]

    [SerializeField] float minBackwardTiltValue;
    [SerializeField] float maxBackwardTiltValue;

    [Header("Movement")]

    [SerializeField] AnimationCurve accelerationCurve;

    [SerializeField] float maxSpeed;//perSec
    public float currentSpeed=0;

    [Header("Camera Controls")]

    [SerializeField] CinemachineVirtualCamera mainCameraController;
    [SerializeField] Camera mainCamera;

    [SerializeField] Transform modelHolder;
    Vector3 modelHolderRotation=Vector3.zero;



    Rigidbody rb;
    float lastShotTime = -1000000;

    Quaternion refRotation;
    Quaternion currRotation;
    Quaternion lastFrameRotation;

    Vector3 rotation;

    private void Awake(){
        rb = GetComponent<Rigidbody>();
        Input.gyro.enabled = true;

        mainCameraController.transform.parent = null;
        mainCamera.transform.parent = null;
    }

    private void FixedUpdate(){

        //Debug.Log(Input.gyro.rotationRateUnbiased + " " + Input.gyro.rotationRate);

        if (!IsOwner)
            return;
        
        Move();
        Shoot();
        
    }

    private void Move(){
        rb.velocity = Vector3.zero;

        currentSpeed = Mathf.Min(currentSpeed + accelerationCurve.Evaluate(currentSpeed) * Time.fixedDeltaTime, maxSpeed);

        OnSpeedChange?.Invoke(currentSpeed,maxSpeed,0);

        rb.AddForce(transform.forward*currentSpeed,ForceMode.VelocityChange);

        Rotation();
    }



    float pitchOffset = 0.45f;

    void Rotation()
    {
        float rotationSpeed = -5;
        transform.Rotate(easeInCirc(Input.gyro.gravity.z + pitchOffset) * rotationSpeed, 0, 0);
        if (transform.rotation.z >= 0 && transform.rotation.z <= 45 || transform.rotation.z < 0 && transform.rotation.z >= -45)
        {
            transform.Rotate(0, 0, easeInCirc(Input.gyro.gravity.x) * rotationSpeed);
        }
        Debug.Log(transform.rotation);
    }

    float lowerThreshold = 0.05f;
    float upperThreshold = 0.5f;

    float easeInCirc(float progress)
    {
        int dir;

        if (progress >= 0)
        {
            dir = 1;
        }
        else
        {
            dir = -1;
        }

        if (Mathf.Abs(progress) + lowerThreshold >= upperThreshold)
        {
            progress = upperThreshold * dir;
        }
        else if (Mathf.Abs(progress) >= lowerThreshold)
        {
            progress = progress + lowerThreshold * dir;
        }
        else
        {
            return 0;
        }

        float easing = 1 - Mathf.Sqrt(1 - Mathf.Abs(Mathf.Pow(progress, 3)));

        if (dir == 1)
        {
            return easing;
        }
        else
        {
            return -easing;
        }
    }

    private void Shoot() {
        if (Input.GetKey(KeyCode.Space) && Time.time - lastShotTime > cooldown) {
            ShootBulletServerRpc();
            lastShotTime = Time.time;
        }
    }

    [ServerRpc]
    private void ShootBulletServerRpc() {
        GameObject gameObject = Instantiate(bulletPrefab, shootPoint.position, shootPoint.rotation);
        gameObject.GetComponent<NetworkObject>().Spawn(true);
        gameObject.GetComponent<ProjectileScript>().LaunchProjectile();

    }

    float Map(float s, float a1, float a2, float b1, float b2){
        return b1 + (s - a1) * (b2 - b1) / (a2 - a1);
    }

    //float MapNonLinear(float value, float minRange1, float maxRange1, float minRange2, float maxRange2) { 
    
    
    //}

}
