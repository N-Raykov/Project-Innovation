using FMODUnity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;





public class AudioManager : MonoBehaviour
{
    [SerializeField] EventReference BGM;
    [SerializeField] GameObject player;

    public void PlayBGM()
    {
        RuntimeManager.PlayOneShot(BGM);
    }
}
