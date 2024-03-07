using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Destroyable : MonoBehaviour, IDamagable
{
   public void TakeDamage(float dmg)
    {
        GameObject.Destroy(gameObject);
    }
}
