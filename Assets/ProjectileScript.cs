using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileScript : MonoBehaviour {

    [SerializeField] float projectileSpeed;
    [SerializeField] float damage;

    public float damageMod { get; set; }

    Rigidbody rb;

    private void Awake(){
        damageMod = 0;
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
            Debug.Log("Damaged");
        }

        Destroy(this.gameObject);
    }

}
