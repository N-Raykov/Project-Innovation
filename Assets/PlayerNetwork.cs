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

    private void Update(){
        FinalRotation();
    }

    private void Move(){
        rb.velocity = Vector3.zero;

        currentSpeed = Mathf.Min(currentSpeed + accelerationCurve.Evaluate(currentSpeed) * Time.fixedDeltaTime, maxSpeed);

        OnSpeedChange?.Invoke(currentSpeed,maxSpeed,0);

        rb.AddForce(transform.forward*currentSpeed,ForceMode.VelocityChange);

    }


    void FinalRotation(){

        //Debug.Log(Input.acceleration);

        transform.localRotation = Quaternion.identity;
        modelHolder.localRotation = Quaternion.identity;

        if (Input.acceleration.z > minBackwardTiltValue){
            float accelerationZ = Mathf.Clamp(Mathf.Abs(Input.acceleration.z), minBackwardTiltValue, maxBackwardTiltValue);
            float mappedZValue = Map(accelerationZ, minBackwardTiltValue, maxBackwardTiltValue, minForwardRotationValue, maxForwardRotationValue);
            rotation += new Vector3(-Mathf.Sign(Input.acceleration.z) * mappedZValue, 0, 0) * Time.deltaTime;

            modelHolderRotation += new Vector3(-0.25f, 0, 0);
        }

        if (Input.acceleration.z < minForwardTiltValue){
            float accelerationZ = Mathf.Clamp(Mathf.Abs(Input.acceleration.z), minForwardTiltValue, maxForwardTiltValue);
            float mappedZValue = Map(accelerationZ, minForwardTiltValue, maxForwardTiltValue, minForwardRotationValue, maxForwardRotationValue);
            rotation += new Vector3(-Mathf.Sign(Input.acceleration.z) * mappedZValue, 0, 0) * Time.deltaTime;

            modelHolderRotation += new Vector3(0.25f, 0, 0);

        }

        if (Mathf.Abs(Input.acceleration.x) > minSideTiltValue){
            float accelerationX = Mathf.Clamp(Mathf.Abs(Input.acceleration.x), minSideTiltValue, maxSideTiltValue);
            float mappedXValue = Map(accelerationX, minSideTiltValue, maxSideTiltValue, minSideRotationValue, maxSideRotationValue);
            rotation += new Vector3(0, Mathf.Sign(Input.acceleration.x) * mappedXValue, -2.5f * Mathf.Sign(Input.acceleration.x) * mappedXValue) * Time.deltaTime;

        }

        modelHolderRotation.x = Mathf.Max(-25, modelHolderRotation.x);
        modelHolderRotation.x = Mathf.Min(25, modelHolderRotation.x);

        rotation.z = Mathf.Lerp(rotation.z, 0, Time.deltaTime);
        modelHolderRotation.x = Mathf.Lerp(modelHolderRotation.x, 0, Time.deltaTime);

        transform.Rotate(rotation);
        modelHolder.Rotate(modelHolderRotation);

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
