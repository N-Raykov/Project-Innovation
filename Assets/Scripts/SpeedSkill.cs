using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpeedSkill : SkillBase{

    [Header("Speed Data")]
    [SerializeField] PlayerNetwork player;
    [SerializeField] float speedIncrease;

    [Header("Visuals")]
    [SerializeField] Transform boostSpawnPoint;
    [SerializeField] float boostLifetime;
    [SerializeField] GameObject boostPrefab;

    public override void UseSkill(){
        if (!player.isMovementEnabled)
            return;

        if (Time.time - lastUseTime > cooldown){
            player.AddSpeed(speedIncrease);
            GameObject boost = Instantiate(boostPrefab,boostSpawnPoint);
            Destroy(boost, boostLifetime);
            lastUseTime = Time.time;
        }

    }


}
