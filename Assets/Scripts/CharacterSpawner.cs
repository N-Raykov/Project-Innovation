using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class CharacterSpawner : MonoBehaviour{

    public static CharacterSpawner instance { get; private set; }
    [SerializeField] string[] scenesToSpawnIn;


    [SerializeField] GameObject _selectedPlayerPrefab;
    public GameObject selectedPlayerPrefab { get; private set; }


    private void Awake(){
        if (instance == null){
            selectedPlayerPrefab = _selectedPlayerPrefab;
            instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
        else {
            if (instance != this) {
                selectedPlayerPrefab = instance.selectedPlayerPrefab;
                Destroy(instance.gameObject);
                instance = this;
            }

        }

        string currentScene = SceneManager.GetActiveScene().name;

        foreach (string s in scenesToSpawnIn)
        {
            if (s == currentScene)
            {
                Instantiate(selectedPlayerPrefab, transform.position, transform.rotation);
                Debug.Log("player created");
                return;
            }
        }

    }

    private void Start(){

        
    }

    public void SetSelectedCharacter(GameObject pPrefab) {
        selectedPlayerPrefab = pPrefab;
        Debug.Log(selectedPlayerPrefab);
    }

    
}
