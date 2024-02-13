using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class ProjectileScript : NetworkBehaviour{

    [SerializeField] float projectileSpeed;

    Rigidbody rb;

    private void Awake(){
        rb = GetComponent<Rigidbody>();
    }

    public void LaunchProjectile() {
        rb.AddForce(transform.forward*projectileSpeed, ForceMode.VelocityChange);
    }

    private void OnCollisionEnter(Collision collision){
        Debug.Log(collision.gameObject.name);
        Destroy(this.gameObject);
    }

}
