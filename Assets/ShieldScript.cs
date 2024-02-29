using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class ShieldScript : NetworkBehaviour,IDamagable{

    [SerializeField] int hp;

    PlayerNetwork owner;

    public void SetOwner(PlayerNetwork pPlayer) {

        owner = pPlayer;
        pPlayer.AddColliderToList(GetComponent<Collider>());
    }

    private void OnDestroy(){
        owner.RemoveColliderFromList(GetComponent<Collider>());
    }

    public void TakeDamage(float pDamage) {
        hp--;
        if (hp <= 0)
            Destroy(this.gameObject);
    }

}
