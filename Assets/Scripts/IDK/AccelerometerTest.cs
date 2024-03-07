using System.Collections;
using System.Threading.Tasks;
using UnityEngine;

//PLEASE REMOVE UNUSED COMMENTS LATER THIS IS SO MESSY D:

public class AccelerometerTest : MonoBehaviour
{
    [SerializeField] float Speed = 5.0f;
    [SerializeField] float PitchSpeed = 30.0f;
    [SerializeField] float RollSpeed = 30.0f;

    // float rollAmount = 0.0f;
    // float pitchAmount = 0.0f;

    Rigidbody rb;

    Vector3 velocityDir;
    
    Vector3 startAccel = Vector3.zero;

    //This is stupid, this is only public because we need the "PlaneRotationVisual" script to access this...
    //Should be fine for testing only.
    public Vector3 accel = Vector3.zero;

    //Doesn't work lol
    //We use a delay since the accelerometer for some reason returns (0,0,0) in Start()...
    //Fine for testing, but please dear god don't use this in production
    // IEnumerator DelayedStart()
    // {
    //     yield return new WaitForSecondsRealtime(1);
    //     startAccel = Input.acceleration;
    //     print(Input.acceleration);
    // }

    //Easing
    public static float EaseOutSine(float start, float end, float value)
    {
        end -= start;
        return end * Mathf.Sin(value * (Mathf.PI * 0.5f)) + start;
    }

    void Start()
    {
        // StartCoroutine("DelayedStart");
        rb = GetComponent<Rigidbody>();
        velocityDir = transform.forward;
    }

    void Update()
    {
        if (Input.touchCount > 0) {
            startAccel = Input.acceleration;
        }

        Vector3 offset = new Vector3(0,0,0);
        accel = Input.acceleration - startAccel + offset;

        print(accel);

        // Normalize acceleration if any of the vector components become larger than 1.
        if (accel.sqrMagnitude > 1)
            accel.Normalize();

        //Check if accelerometer data on the x-axis exceeds the deadzone.
        // if (Mathf.Abs(accel.x) > 0.02f) {
            //transform.Rotate(new Vector3(0, 0, 1), -accel.x * RollSpeed * Time.deltaTime);
            transform.Rotate(new Vector3(0, 1, 0), accel.x * RollSpeed * Time.deltaTime);
            //  * PitchSpeed * Time.deltaTime
        // }

        //Check if accelerometer data on the z-axis exceeds the deadzone.
        // if (Mathf.Abs(accel.z) > 0.02f) {
            // transform.Rotate(new Vector3(1, 0, 0), -accel.z * PitchSpeed * Time.deltaTime);
        // }

        //We're calculating how much the plane has rotated on the (local) Z-axis.
        //Due to issues with Euler angles, we have to use this to get the actual angle.
        // rollAmount = Mathf.DeltaAngle(transform.localEulerAngles.z, 0.0f);
        // print(rollAmount);

        //Same thing for the (local) X-axis
        // pitchAmount = Mathf.DeltaAngle(transform.localEulerAngles.x, 0.0f);

        //Add to rotation for steering
        // transform.Rotate(new Vector3(1, 0, 0), -Mathf.Lerp(0, 1, (rollAmount/90)) * Time.deltaTime * 90f);
        velocityDir = transform.forward;
        velocityDir.y = accel.z * PitchSpeed;
        // if (velocityDir.sqrMagnitude > 1) 
        //     velocityDir.Normalize();
    }

    void FixedUpdate() {
        rb.AddForce(velocityDir * Speed);
    }
}