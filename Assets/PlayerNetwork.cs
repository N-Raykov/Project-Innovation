using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
using UnityEngine.InputSystem;
using Cinemachine;
using System;

public class PlayerNetwork : NetworkBehaviour,IDamagable{


    public Action<float,float,float> OnSpeedChange;

    [SerializeField] Transform shootPoint;

    [SerializeField] InputActionReference moveActionToUse;
    [SerializeField] float speed;
    [SerializeField] GameObject bulletPrefab;
    [SerializeField] float cooldown;

    [Header("Side Tilt Values")]

    [SerializeField] AnimationCurve sideTiltCurve;
    [SerializeField] float zRotationMultiplier;

    [Header("Backward Tilt Values")]//rotating towards your feet

    [SerializeField] AnimationCurve backwardTiltCurve;
    [SerializeField] float modelTiltMultiplier;
    [SerializeField] float maxModelTiltValue;

    [Header("Movement")]

    [SerializeField] AnimationCurve accelerationCurve;

    [SerializeField] float maxSpeed;//perSec
    [SerializeField] float trueMaxSpeed;
    public float currentSpeed=0;

    [Header("Camera Controls")]

    [SerializeField] CinemachineVirtualCamera mainCameraController;
    [SerializeField] Camera mainCamera;

    [SerializeField] Transform modelHolder;
    Vector3 modelHolderRotation=Vector3.zero;

    Rigidbody rb;
    float lastShotTime = -1000000;

    Vector3 rotation;

    private void Awake(){
        rb = GetComponent<Rigidbody>();
        Input.gyro.enabled = true;

        mainCameraController.transform.parent = null;
        mainCamera.transform.parent = null;
    }

    private void FixedUpdate(){
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

        currentSpeed += accelerationCurve.Evaluate(currentSpeed) * Time.fixedDeltaTime;

        OnSpeedChange?.Invoke(currentSpeed,maxSpeed,trueMaxSpeed);

        rb.AddForce(transform.forward*currentSpeed,ForceMode.VelocityChange);

    }


    void FinalRotation(){

        transform.localRotation = Quaternion.identity;
        modelHolder.localRotation = Quaternion.identity;



        rotation += new Vector3(-backwardTiltCurve.Evaluate(Input.acceleration.z), 0, 0)*Time.deltaTime;

        modelHolderRotation += new Vector3(-modelTiltMultiplier * backwardTiltCurve.Evaluate(Input.acceleration.z), 0, 0) * Time.deltaTime;

        rotation += new Vector3(0, sideTiltCurve.Evaluate(Input.acceleration.x), -zRotationMultiplier * sideTiltCurve.Evaluate(Input.acceleration.x)) * Time.deltaTime;



        modelHolderRotation.x = Mathf.Max(-maxModelTiltValue, modelHolderRotation.x);
        modelHolderRotation.x = Mathf.Min(maxModelTiltValue, modelHolderRotation.x);

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

    public void AddSpeed(float pSpeed) {
        //currentSpeed += pSpeed;
        currentSpeed = Mathf.Min(currentSpeed + pSpeed, trueMaxSpeed);
    }

    public void TakeDamage(float pDamage) {
        currentSpeed = Mathf.Max(currentSpeed-pDamage,0);
    }

}
