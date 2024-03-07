using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpeedSkill : SkillBase{

    [Header("Speed Data")]
    [SerializeField] PlayerNetwork player;
    [SerializeField] float speedIncrease;

    public override void UseSkill(){
        if (!player.isMovementEnabled)
            return;

        if (Time.time - lastUseTime > cooldown){
            player.AddSpeed(speedIncrease);
            lastUseTime = Time.time;
        }

    }


}
