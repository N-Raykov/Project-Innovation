using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageSkill : SkillBase{

    [Header("Damage Data")]
    [SerializeField] Attacker attacker;
    [SerializeField] int damageIncrease;


    public override void UseSkill() {

        if (Time.time - lastUseTime > cooldown){
            StartCoroutine(ChangeDamage());
            lastUseTime = Time.time;
        }

    }

    IEnumerator ChangeDamage() {
        attacker.damageMod += damageIncrease;
        yield return new WaitForSeconds(duration);
        attacker.damageMod -= damageIncrease;

    }

}
