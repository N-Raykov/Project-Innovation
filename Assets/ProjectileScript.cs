using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class ProjectileScript : NetworkBehaviour {

    [SerializeField] float projectileSpeed;
    [SerializeField] int damage;

    public int damageMod { get; set; }

    Rigidbody rb;

    private void Awake(){
        rb = GetComponent<Rigidbody>();
    }

    public void LaunchProjectile() {
        rb.AddForce(transform.forward*projectileSpeed, ForceMode.VelocityChange);
    }

    public void IgnoreColliders(List<Collider> pColliders) {
        Collider collider = GetComponent<Collider>();

        foreach (Collider col in pColliders) {
            Physics.IgnoreCollision(collider, col);
        }

    }

    private void OnCollisionEnter(Collision collision){
        IDamagable damagable= collision.transform.root.GetComponent<IDamagable>();

        if (damagable!=null) {
            damagable.TakeDamage(damage+damageMod);
        }

        Destroy(this.gameObject);
    }

}
