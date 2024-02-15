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

        Rotation();
    }

    void Rotation()
    {
        float gravityOutputMultiplier = -50;
        Vector3 currentRot = transform.eulerAngles;
        transform.eulerAngles = new Vector3(currentRot.x, currentRot.y, Input.gyro.gravity.x * gravityOutputMultiplier);
        Debug.Log(Input.gyro.gravity.x);
        Debug.Log(Input.gyro.gravity.y);
        Debug.Log(Input.gyro.attitude);
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
