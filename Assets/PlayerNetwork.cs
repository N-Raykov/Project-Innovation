using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
using UnityEngine.InputSystem;

public class PlayerNetwork : NetworkBehaviour{


    [SerializeField] Transform shootPoint;

    [SerializeField] InputActionReference moveActionToUse;
    [SerializeField] float speed;
    [SerializeField] GameObject bulletPrefab;
    [SerializeField] float cooldown;

    [Header("Rotation")]
    [SerializeField] float minRotationValue;
    [SerializeField] float maxRotationValue;

    [SerializeField] float minGyroValue;
    [SerializeField] float maxGyroValue;

    [Header("Movement")]
    [SerializeField] float acceleration;//perSec
    [SerializeField] float maxSpeed;//perSec
    public float currentSpeed=0;



    Vector3 velocity;
    Rigidbody rb;
    float lastShotTime = -1000000;

    Quaternion refRotation;
    Quaternion currRotation;
    Quaternion lastFrameRotation;

    Vector3 rotation;

    private void Awake(){
        rb = GetComponent<Rigidbody>();
        Input.gyro.enabled = true;
        refRotation = GetDeviceRotation();
        
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

        //Debug.Log( Input.gyro.attitude.eulerAngles);
        
    }

    private void Move(){
        rb.velocity = Vector3.zero;
        velocity = Vector3.zero;

        currentSpeed = Mathf.Min(currentSpeed + acceleration*Time.fixedDeltaTime, maxSpeed);

        rb.AddForce(transform.forward*currentSpeed,ForceMode.VelocityChange);

        //velocity = new Vector3(moveActionToUse.action.ReadValue<Vector2>().x, 0, moveActionToUse.action.ReadValue<Vector2>().y);

        //rb.AddForce(velocity * speed, ForceMode.VelocityChange);

        //currRotation = GetDeviceRotation();

        //Rotation();
    }


    void FinalRotation(){

        Debug.Log(Input.acceleration);

        transform.localRotation = Quaternion.identity;


        if (Mathf.Abs(Input.acceleration.z) > minGyroValue){
            float accelerationZ = Mathf.Clamp(Mathf.Abs(Input.acceleration.z), minGyroValue, maxGyroValue);
            float mappedZValue = Map(accelerationZ, minGyroValue, maxGyroValue, minRotationValue, maxRotationValue);
            rotation += new Vector3(-Mathf.Sign(Input.acceleration.z) * mappedZValue, 0, 0)*Time.deltaTime;

            //transform.Rotate(new Vector3(-Mathf.Sign(Input.acceleration.z) * mappedZValue, 0, 0),Space.World);
            //transform.Rotate(new Vector3(1, 0, 0), -Mathf.Sign(Input.acceleration.z));
        }


        if (Mathf.Abs(Input.acceleration.x) > minGyroValue)
        {
            float accelerationX = Mathf.Clamp(Mathf.Abs(Input.acceleration.x), minGyroValue, maxGyroValue);
            float mappedXValue = Map(accelerationX, minGyroValue, maxGyroValue, minRotationValue, maxRotationValue);
            rotation += new Vector3(0, Mathf.Sign(Input.acceleration.x) * mappedXValue, 0)*Time.deltaTime;

            //transform.Rotate(new Vector3(0, Mathf.Sign(Input.acceleration.x) * mappedXValue, 0), Space.World);
            //transform.Rotate(new Vector3(0, 1, 0), Mathf.Sign(Input.acceleration.x) * mappedXValue);

        }

        

        transform.Rotate(rotation);

    }

    void Rotation()
    {
        transform.eulerAngles = new Vector3(GetPitchAngle(refRotation, currRotation), GetYallAngle(Quaternion.identity, currRotation), GetRollAngle(refRotation, currRotation));
    }

    private float GetRollAngle(Quaternion refRotation, Quaternion currentRotation)
    {
        Quaternion rotFromRefRot = Quaternion.Inverse(Quaternion.FromToRotation(refRotation * Vector3.forward, currentRotation * Vector3.forward));

        Quaternion rotationFixedZ = rotFromRefRot * currentRotation;
        return rotationFixedZ.eulerAngles.z;
    }

    private float GetPitchAngle(Quaternion refRotation, Quaternion currentRotation)
    {
        Quaternion rotFromRefRot = Quaternion.Inverse(Quaternion.FromToRotation(refRotation * Vector3.right, currentRotation * Vector3.right));

        Quaternion rotationFixedX = rotFromRefRot * currentRotation;
        return rotationFixedX.eulerAngles.x;
    }

    private float GetYallAngle(Quaternion refRotation, Quaternion currentRotation)
    {
        Quaternion rotFromRefRot = Quaternion.Inverse(Quaternion.FromToRotation(refRotation * Vector3.up, currentRotation * Vector3.up));

        Quaternion rotationFixedY = rotFromRefRot * currentRotation;
        return rotationFixedY.eulerAngles.y;
    }

    private Quaternion GetDeviceRotation()
    {
        return new Quaternion(1f, 1f, -1f, 1f) * Input.gyro.attitude;
        //return new Quaternion(0.5f, 0.5f, -0.5f, 0.5f) * Input.gyro.attitude;
        //return new Quaternion(0.5f, 0.5f, -0.5f, 0.5f) * Input.gyro.attitude * new Quaternion(0, 0, 1, 0);
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
