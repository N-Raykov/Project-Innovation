using FMODUnity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class AudioManager : MonoBehaviour
{

    private static FMOD.Studio.EventInstance Music;
    void Start()
    {
        Music = FMODUnity.RuntimeManager.CreateInstance("event:/Music/In-game");
        Music.start();
        Music.release();
    }
    [SerializeField] EventReference BGM;
    [SerializeField] GameObject player;

    public void PlayBGM()
    {
        RuntimeManager.PlayOneShot(BGM);
    }
}
