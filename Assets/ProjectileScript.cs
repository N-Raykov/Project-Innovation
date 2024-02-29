using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class ProjectileScript : NetworkBehaviour{

    [SerializeField] float projectileSpeed;
    [SerializeField] float damage;

    Rigidbody rb;

    private void Awake(){
        rb = GetComponent<Rigidbody>();
    }

    public void LaunchProjectile() {
        rb.AddForce(transform.forward*projectileSpeed, ForceMode.VelocityChange);
    }

    public void IgnoreOwnerCollider(Collider[] pColliders) {
        Collider collider = GetComponent<Collider>();

        foreach (Collider col in pColliders) {
            Physics.IgnoreCollision(collider, col);
        }

    }

    private void OnCollisionEnter(Collision collision){
        //Debug.Log(collision.gameObject.name);

        //PlayerNetwork player = collision.transform.root.GetComponent<PlayerNetwork>();
        IDamagable damagable= collision.transform.root.GetComponent<IDamagable>();

        if (damagable!=null) {
            damagable.TakeDamage(damage);
        }

        Destroy(this.gameObject);
    }

}
