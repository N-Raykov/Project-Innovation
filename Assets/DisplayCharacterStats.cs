using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class DisplayCharacterStats : MonoBehaviour{

    public Action<Sprite, int, float,float,float,int> OnClick;

    [SerializeField] Sprite sprite;
    [SerializeField] int damageValue;
    [SerializeField] float maxSpeed;
    [SerializeField] float acceleration;
    [SerializeField] float attackCooldown;
    [SerializeField] int attackDamage;
 
    public void ChangeCharacter() {

        OnClick?.Invoke(sprite,damageValue,maxSpeed,acceleration,attackCooldown,attackDamage);

    }

}
