using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZoomSpeedEffect : MonoBehaviour
{
    [SerializeField] Material material;
    Rigidbody rigidbody;
    PlayerNetwork playerScript;

    // Start is called before the first frame update
    void Start()
    {
        rigidbody = GetComponent<Rigidbody>();
        playerScript = GetComponent<PlayerNetwork>();
    }

    // Update is called once per frame
    void Update()
    {
        material.SetFloat("_EffectOpacity", playerScript.currentSpeed / 50.0f);
    }
}
