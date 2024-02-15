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

    Vector3 velocity;
    Rigidbody rb;
    float lastShotTime = -1000000;

    Quaternion refRotation;
    Quaternion currRotation;

    private void Awake(){
        rb = GetComponent<Rigidbody>();
        Input.gyro.enabled = true;
        refRotation = GetDeviceRotation();
    }

    private void FixedUpdate(){
        if (!IsOwner)
            return;
        Move();
        Shoot();
    }

    private void Move(){
        rb.velocity = Vector3.zero;
        velocity = Vector3.zero;

        velocity = new Vector3(moveActionToUse.action.ReadValue<Vector2>().x, 0, moveActionToUse.action.ReadValue<Vector2>().y);

        rb.AddForce(velocity * speed, ForceMode.VelocityChange);

        currRotation = GetDeviceRotation();

        Rotation();
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
        return new Quaternion(0.5f, 0.5f, -0.5f, 0.5f) * Input.gyro.attitude * new Quaternion(0, 0, 1, 0);
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

}
