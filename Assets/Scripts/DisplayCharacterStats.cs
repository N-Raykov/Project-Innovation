using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;

public class DisplayCharacterStats : MonoBehaviour{

    public Action<Sprite, float, float,string,float,string,string,string,float,float,GameObject,bool> OnClick;
    [SerializeField] Toggle toggle;
    [SerializeField] Image image;

    [SerializeField] Sprite sprite;
    [SerializeField] float damageValue;
    [SerializeField] float maxSpeed;
    [SerializeField] string acceleration;
    [SerializeField] float attackCooldown;

    [SerializeField] string characterName;

    [Header("Skill Page")]
    [SerializeField] string skillName;
    [SerializeField] string skillDescription;
    [SerializeField] float skillDuration;
    [SerializeField] float SkillCooldown;

    [Header("Character")]
    [SerializeField] GameObject characterPrefab;

    private void Start(){
        if (toggle.isOn)
            ChangeCharacter();
        image.sprite = sprite;
    }

    public void ChangeCharacter() {

        OnClick?.Invoke(sprite,damageValue,maxSpeed,acceleration,attackCooldown,characterName,skillName,skillDescription,skillDuration,SkillCooldown,characterPrefab,toggle.isOn);
    }

}
