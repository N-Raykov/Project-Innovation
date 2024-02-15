using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestMOve : MonoBehaviour{

    Rigidbody rb;

    private void Awake(){
        rb = GetComponent<Rigidbody>();
    }


    private void FixedUpdate(){
        Move();
    }

    private void Move(){

        rb.velocity = Vector3.zero;
        rb.AddForce(transform.forward * 4,ForceMode.VelocityChange);
    
    }

}
