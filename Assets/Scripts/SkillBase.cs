using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public abstract class SkillBase : MonoBehaviour{

    [Header("Data")]
    [SerializeField] protected float cooldown;
    [SerializeField] protected float duration;

    protected float lastUseTime=-1000000;

    public abstract void UseSkill();

    public float ReturnSkillRechargeTime() { //from 0 to 1

        return (Mathf.Min((Time.time-lastUseTime)/cooldown, 1f));
    
    }
}
