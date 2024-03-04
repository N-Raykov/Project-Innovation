using System.Collections;
using System.Collections.Generic;
using UnityEngine;

abstract public class Attacker:MonoBehaviour{

    public int damageMod { get; set; }

    abstract public void Shoot();

}
