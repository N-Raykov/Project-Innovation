using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PlayerDisplayObjectScript : MonoBehaviour{

    [SerializeField] TextMeshProUGUI playerNameText;

    public void UpdatePlayerName(string pName) {
        playerNameText.text = pName;
    }

}
