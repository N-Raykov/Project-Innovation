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

    private void Awake(){
        rb = GetComponent<Rigidbody>();
        Input.gyro.enabled = true;
    }

    

    private void FixedUpdate(){
        Move();
        Shoot();
    }


    private void Move(){
        rb.velocity = Vector3.zero;
        velocity = Vector3.zero;

        velocity = new Vector3(moveActionToUse.action.ReadValue<Vector2>().x, 0, moveActionToUse.action.ReadValue<Vector2>().y);

        rb.AddForce(velocity * speed, ForceMode.VelocityChange);

        Rotation();
    }

    float pitchOffset = 0.45f;

    void Rotation()
    {
        float rotationSpeed = -50;
        transform.Rotate(easeInCirc(Input.gyro.gravity.z + pitchOffset) * rotationSpeed, 0, easeInCirc(Input.gyro.gravity.x) * rotationSpeed);
    }

    float lowerThreshold = 0.05f;
    float upperThreshold = 0.5f;

    float easeInCirc(float progress)
    {
        int dir;

        if(progress >= 0)
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
            if (!IsOwner)
                return;
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
