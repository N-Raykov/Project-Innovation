using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShieldSkill : SkillBase{

    [Header("Shield Data")]

    [SerializeField] PlayerNetwork playerOwner;
    [SerializeField] GameObject shieldPrefab;
    [SerializeField] Transform shieldParent;

    public override void UseSkill(){
        if (Time.time - lastUseTime > cooldown) {

            GameObject shield = Instantiate(shieldPrefab, shieldParent);
            shield.GetComponent<ShieldScript>().SetOwner(playerOwner);

            Destroy(shield, duration);

            lastUseTime = Time.time;
        }
    }


}
