using System.Collections.Generic;
using UnityEngine;

public class AI_Test : MonoBehaviour, IDamagable
{

    [SerializeField] Transform shootPoint;

    [SerializeField] float speed;

    [SerializeField] List<Collider> _ownerColliders;

    public List<Collider> ownerColliders { get { return _ownerColliders; } }

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
    public float currentSpeed = 0;


    [SerializeField] Transform modelHolder;
    Vector3 modelHolderRotation = Vector3.zero;

    [SerializeField] Vector3 targetForwardDirection;
    [SerializeField] Transform[] NavPoints;
    [SerializeField] int pointIndex = 0;

    Vector3 targetOffset;
    Vector3 lastDirDiff;

    //[Header("Checkpoints and Laps")]


    public Checkpoint lastCheckpoint { get; set; }

    Rigidbody rb;
    float lastShotTime = -1000000;

    Vector3 rotation;

    public bool isMovementEnabled { get; set; }

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        isMovementEnabled = false;
    }

    private void FixedUpdate(){
        if (!isMovementEnabled)
            return;

        Move();
    }

    private void Update(){
        if (!isMovementEnabled)
            return;

        FinalRotation();

        targetForwardDirection = (NavPoints[pointIndex].position + targetOffset) - transform.position;
        targetForwardDirection.Normalize();

        // print("Current target point: " + NavPoints[pointIndex].name.ToString());
        // print("Next point: " + pointIndex.ToString());
        
        // print("Target Direction: " + targetForwardDirection.ToString());
        
    }

    private void Move()
    {
        rb.velocity = Vector3.zero;
        currentSpeed += accelerationCurve.Evaluate(currentSpeed) * Time.fixedDeltaTime;
        rb.AddForce(transform.forward * currentSpeed, ForceMode.VelocityChange);
    }


    void FinalRotation()
    {
        Vector3 currentForwardDir = transform.forward;
        // Vector3 lerpedDirDiff = Vector3.Lerp(lastDirDiff, (targetForwardDirection - currentForwardDir) * 2, 50 * Time.deltaTime);
        Vector3 lerpedDirDiff = (targetForwardDirection - currentForwardDir) * 2f;
        lerpedDirDiff = transform.InverseTransformDirection(lerpedDirDiff);
        // dirDifference.Normalize();
        lastDirDiff = lerpedDirDiff;

        transform.localRotation = Quaternion.identity;
        modelHolder.localRotation = Quaternion.identity;

        rotation += new Vector3(-backwardTiltCurve.Evaluate(lerpedDirDiff.y) * modelTiltMultiplier, 0, 0) * Time.deltaTime;

        modelHolderRotation += new Vector3(-modelTiltMultiplier * backwardTiltCurve.Evaluate(lerpedDirDiff.y), 0, 0) * Time.deltaTime;

        rotation += new Vector3(0, sideTiltCurve.Evaluate(lerpedDirDiff.x) * zRotationMultiplier, zRotationMultiplier * sideTiltCurve.Evaluate(-lerpedDirDiff.x)) * Time.deltaTime;


        modelHolderRotation.x = Mathf.Max(-maxModelTiltValue, modelHolderRotation.x);
        modelHolderRotation.x = Mathf.Min(maxModelTiltValue, modelHolderRotation.x);

        rotation.z = Mathf.Lerp(rotation.z, 0, Time.deltaTime);
        modelHolderRotation.x = Mathf.Lerp(modelHolderRotation.x, 0, Time.deltaTime);

        transform.Rotate(rotation);
        modelHolder.Rotate(modelHolderRotation);
    }

     public void TakeDamage(float pDamage) {
        currentSpeed = Mathf.Max(currentSpeed-pDamage,0);
        Debug.Log("Took Damage");
    }

     public void AddSpeed(float pSpeed) {
        //currentSpeed += pSpeed;
        currentSpeed = Mathf.Min(currentSpeed + pSpeed, trueMaxSpeed);
    }

    void OnTriggerEnter(Collider other) {
        if (other.tag == "NavPoint" && other.transform == NavPoints[pointIndex].transform) {
            if (pointIndex >= NavPoints.Length - 1) {
                pointIndex = 0;
            } else {
                pointIndex++;
            }

            targetOffset = new Vector3(Random.Range(-2, 2), Random.Range(-2, 2), Random.Range(-2, 2));
        }
    }

}
