using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageSkill : SkillBase{

    [Header("Damage Data")]
    [SerializeField] PlayerNetwork player;
    [SerializeField] Attacker attacker;
    [SerializeField] float damageIncrease;

    [Header("Visuals")]
    [SerializeField] GameObject prefab;
    [SerializeField] Transform spawnPoint;

    public override void UseSkill() {
        if (!player.isMovementEnabled)
            return;


        if (Time.time - lastUseTime > cooldown){
            StartCoroutine(ChangeDamage());

            GameObject p = Instantiate(prefab,spawnPoint);
            Destroy(p, duration);
            lastUseTime = Time.time;
        }

    }

    IEnumerator ChangeDamage() {
        attacker.damageMod += damageIncrease;
        yield return new WaitForSeconds(duration);
        attacker.damageMod -= damageIncrease;

    }

}
