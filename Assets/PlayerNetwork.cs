using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;


public class PlayerNetwork : NetworkBehaviour{


    [SerializeField] Transform shootPoint;

    [SerializeField] float speed;
    [SerializeField] GameObject bulletPrefab;
    [SerializeField] float cooldown;


    Vector3 velocity;
    Rigidbody rb;
    float lastShotTime = -1000000;

    private void Awake(){
        rb = GetComponent<Rigidbody>();
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

        if (Input.GetKey(KeyCode.W))
            velocity.z += 1;
        if (Input.GetKey(KeyCode.S))
            velocity.z -= 1;
        if (Input.GetKey(KeyCode.D))
            velocity.x += 1;
        if (Input.GetKey(KeyCode.A))
            velocity.x -= 1;

        rb.AddForce(velocity * speed, ForceMode.VelocityChange);

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
