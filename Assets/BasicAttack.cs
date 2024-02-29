using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class BasicAttack : NetworkBehaviour, IAttacker{

    [Header("Data")]
    [SerializeField] Camera playerCamera;
    [SerializeField] GameObject bulletPrefab;

    [SerializeField] float fakeTargetRange;

    [Header("Plane Data")]
    [SerializeField] Collider[] ownerColliders;
    [SerializeField] List<Transform> shootpoints;
    [SerializeField] LayerMask playerMask;

    int activeShootPoint=0;

    [Header("Stats")]
    [SerializeField] float aimAssistRadius;
    [SerializeField] float cooldown;

    float lastShotTime = -100000000;

    bool canShoot = false;

    public void ChangeShootingState(bool pState) {
        canShoot = pState;
    }

    void Update() {
        Shoot();
    }

    public void Shoot() {
        if (canShoot && Time.time - lastShotTime >= cooldown) {
            ShootBulletServerRpc();
            Debug.Log("shooting");
            lastShotTime = Time.time;
        }
    }

    [ServerRpc]
    private void ShootBulletServerRpc() {

        Debug.Log("shooting");

        Vector3 target;

        RaycastHit hit;

        foreach (Collider col in ownerColliders) {
            col.enabled = false;
        }

        if (Physics.SphereCast(playerCamera.transform.position, aimAssistRadius, playerCamera.transform.forward, out hit, fakeTargetRange)) {
            target = hit.point;
        } else {
            target = playerCamera.ScreenToWorldPoint(new Vector3(0.5f, 0.5f, 0)) + playerCamera.transform.forward * fakeTargetRange;
        }

        foreach (Collider col in ownerColliders) {
            col.enabled = true;
        }

        GameObject gameObject = Instantiate(bulletPrefab, shootpoints[activeShootPoint].position, Quaternion.identity);
        gameObject.transform.forward = (target - shootpoints[activeShootPoint].position).normalized;

        activeShootPoint++;
        activeShootPoint %= shootpoints.Count;

        gameObject.GetComponent<NetworkObject>().Spawn(true);
        ProjectileScript projectile = gameObject.GetComponent<ProjectileScript>();
        projectile.LaunchProjectile();
        projectile.IgnoreOwnerCollider(ownerColliders);

    }


}
