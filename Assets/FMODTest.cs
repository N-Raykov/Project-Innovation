using FMODUnity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FMODTest : MonoBehaviour
{
  [SerializeField] StudioEventEmitter emitter;

    [Range(0, 1)] [SerializeField] private float rpm;
    private void Update()
    {
        emitter.SetParameter("RPM", rpm);
    }
}
