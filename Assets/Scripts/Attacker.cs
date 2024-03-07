using System.Collections;
using System.Collections.Generic;
using UnityEngine;

abstract public class Attacker:MonoBehaviour{

    [Header("Data")]
    [SerializeField] protected Camera playerCamera;
    [SerializeField] protected GameObject bulletPrefab;

    [SerializeField] protected float fakeTargetRange;

    [Header("Plane Data")]
    [SerializeField] protected PlayerNetwork playerOwner;
    [SerializeField] protected List<Transform> shootpoints;
    [SerializeField] protected LayerMask playerMask;

    protected int activeShootPoint = 0;
    public float damageMod { get; set; }

    [Header("Stats")]
    [SerializeField] protected float aimAssistRadius;
    [SerializeField] protected float cooldown;

    protected float lastShotTime = -100000000;

    protected bool canShoot = false;

    abstract public void Shoot();

}
