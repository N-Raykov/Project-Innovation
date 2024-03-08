using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameMusicManager : MonoBehaviour{

    public static GameMusicManager instance { get; private set; }

    [SerializeField] List<string> scenesAllowedToExistIn;
    [SerializeField] AudioSource audioSource;

    private void Awake()
    {

        if (instance != null)
        {
            Destroy(this.gameObject);
        }
        else
        {
            audioSource.Play();
            instance = this;
            DontDestroyOnLoad(this.gameObject);
        }

    }

    private void Update()
    {

        if (!scenesAllowedToExistIn.Contains(SceneManager.GetActiveScene().name))
        {
            Destroy(instance.gameObject);
        }

    }

}
