using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity;

public class BasicAttack : Attacker{

    public void ChangeShootingState(bool pState) {
        canShoot = pState;
    }

    void Update() {
        Shoot();
    }

    public override void Shoot() {
        if (canShoot && Time.time - lastShotTime >= cooldown) {
            ShootBulletServerRpc();
            lastShotTime = Time.time;
            
        }
    }

    private void ShootBulletServerRpc() {

        Debug.Log("shooting");

        Vector3 target;

        RaycastHit hit;

        foreach (Collider col in playerOwner.ownerColliders) {
            col.enabled = false;
        }

        if (Physics.SphereCast(playerCamera.transform.position, aimAssistRadius, playerCamera.transform.forward, out hit, fakeTargetRange,playerMask,QueryTriggerInteraction.Ignore)) {
            target = hit.point;
        } else {
            target = playerCamera.ScreenToWorldPoint(new Vector3(0.5f, 0.5f, 0)) + playerCamera.transform.forward * fakeTargetRange;
        }

        foreach (Collider col in playerOwner.ownerColliders) {
            col.enabled = true;
        }
        
        GameObject gameObject = Instantiate(bulletPrefab, shootpoints[activeShootPoint].position, Quaternion.identity);
        gameObject.transform.forward = (target - shootpoints[activeShootPoint].position).normalized;

        activeShootPoint++;
        activeShootPoint %= shootpoints.Count;

        ProjectileScript projectile = gameObject.GetComponent<ProjectileScript>();
        projectile.LaunchProjectile();
        projectile.IgnoreColliders(playerOwner.ownerColliders);
        projectile.damageMod = damageMod;

    }


}
