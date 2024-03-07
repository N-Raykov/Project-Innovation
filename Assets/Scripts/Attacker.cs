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

    [Header("Overheating")]
    [SerializeField] float overheatAmountPerShot;
    [SerializeField] float overheatDecreasePerSec;


    protected bool isOverheated=false;
    public float overheatAmount { get; protected set; }
    protected float overheatMaxAmount=100f;

    public float timeHeatWasAtZero { get; protected set; }

    private void Start(){
        overheatAmount = 0f;
        timeHeatWasAtZero = 0f;
    }

    abstract public void Shoot();

    protected void IncreaseOverheatAmount() {
        overheatAmount = Mathf.Min(overheatAmount + overheatAmountPerShot, overheatMaxAmount);
        if (overheatAmount == overheatMaxAmount)
            isOverheated = true;
    }

    protected void DecreaseOverheatAmount() {

        if (overheatAmount == 0)
            return;

        overheatAmount = Mathf.Max(overheatAmount-overheatDecreasePerSec*Time.deltaTime,0);

        if (overheatAmount == 0) {
            timeHeatWasAtZero = Time.time;
            isOverheated = false;
        }

    }
        
    public float ReturnLastShotTime() {
        return lastShotTime;
    }

    public float ReturnMaxOverheatAmount() {
        return overheatMaxAmount;
    }
    
    

}
