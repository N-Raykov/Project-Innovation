using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileScript : MonoBehaviour {

    [SerializeField] float projectileSpeed;
    [SerializeField] float damage;

    Transform targetTransform;

    public float damageMod { get; set; }

    Rigidbody rb;

    private void Awake(){
        damageMod = 0;
        rb = GetComponent<Rigidbody>();
    }

    public void LaunchProjectile() {
        rb.AddForce(transform.forward*projectileSpeed, ForceMode.VelocityChange);
    }

    public void LaunchProjectile(Transform pTargetTransform) {
        targetTransform = pTargetTransform;
        rb.AddForce(transform.forward * projectileSpeed, ForceMode.VelocityChange);
    }

    private void Update(){
        if (targetTransform != null) {
            transform.forward = (targetTransform.position - transform.position).normalized;
            rb.velocity = transform.forward * projectileSpeed;
        }
    }

    public void IgnoreColliders(List<Collider> pColliders) {
        Collider collider = GetComponent<Collider>();

        foreach (Collider col in pColliders) {
            Physics.IgnoreCollision(collider, col);
        }

    }

    private void OnCollisionEnter(Collision collision){
        IDamagable damagable = collision.transform.root.GetComponent<IDamagable>();
        Debug.Log(collision.gameObject.name + " " + damagable);


        if (damagable!=null) {
            damagable.TakeDamage(damage+damageMod);
            Debug.Log("Damaged");
        }

        Destroy(this.gameObject);
    }

}
