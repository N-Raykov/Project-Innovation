using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;

public class DisplayCharacterStats : MonoBehaviour{

    public Action<Sprite, int, float,float,float,string,string,string,int,int,GameObject,bool> OnClick;
    [SerializeField] Toggle toggle;
    [SerializeField] Image image;

    [SerializeField] Sprite sprite;
    [SerializeField] int damageValue;
    [SerializeField] float maxSpeed;
    [SerializeField] float acceleration;
    [SerializeField] float attackCooldown;

    [SerializeField] string characterName;

    [Header("Skill Page")]
    [SerializeField] string skillName;
    [SerializeField] string skillDescription;
    [SerializeField] int skillDuration;
    [SerializeField] int SkillCooldown;

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
