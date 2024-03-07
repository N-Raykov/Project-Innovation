using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class LobbyButtonScript : MonoBehaviour{
    [SerializeField] TextMeshProUGUI lobbyNameText;
    [SerializeField] TextMeshProUGUI playerCount;

    public void SetLobbyName(string pText) {
        lobbyNameText.text = pText;
    }

    public void SetPlayerCountText(string pText) {
        playerCount.text = pText;
    }
}
