using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Services.Core;
using Unity.Services.Authentication;
using Unity.Services.Lobbies;
using Unity.Services.Lobbies.Models;
using Unity.Services.Qos;
using TMPro;
using UnityEngine.UI;

public class PlayerData{
    public Player player;
    public GameObject playerDisplay;
}



public class TestLobby : MonoBehaviour{

    [Header("NameSelectionScreen")]
    [SerializeField] GameObject loginPage;
    [SerializeField] GameObject lobbySelectionScreen;
    [SerializeField] TextMeshProUGUI lobbyNameObject;

    [Header("LobbyCreationScreen")]
    [SerializeField] int minPlayerCount = 2;
    [SerializeField] int maxPlayerCount = 4;
    int playerCount = 4;
    const int defaultPlayerCount = 4;
    [SerializeField] TextMeshProUGUI playerCountText;
    [SerializeField] TextMeshProUGUI visibilityText;
    bool isPublicLobby = false;
    const bool defaultIsPublicLobby=false;
    [SerializeField] TMP_InputField lobbyNameInputField;
    string lobbyName;
    const string defaultLobbyName = "Lobby";

    [Header("LobbyScreen")]
    [SerializeField] GameObject lobbyScreen;
    [SerializeField] GameObject playerListParent;
    [SerializeField] GameObject playerDisplayPrefab;
    [SerializeField] RectTransform sizeComparison;
    string lobbyCode;
    Coroutine lobbyPlayerUpdateCoroutine;
    List<GameObject> playerDisplayList = new List<GameObject>();
    List<Player> playersInLobby = new List<Player>();
    List<PlayerData> playerData = new List<PlayerData>();
    [SerializeField] TextMeshProUGUI joinedLobbyNameText;
    [SerializeField] TextMeshProUGUI joinedLobbyPlayersText;
    [SerializeField] TextMeshProUGUI joinedLobbyCode;

    Lobby hostLobby;
    Lobby joinedLobby;
    Coroutine hostHeartbeatCoroutine;

    string playerName="TestName";

    private async void Start(){
        await UnityServices.InitializeAsync();


        AuthenticationService.Instance.SignedIn += () =>{
            Debug.Log("Signed in" + AuthenticationService.Instance.PlayerId);
        };
        await AuthenticationService.Instance.SignInAnonymouslyAsync();

        ResetLobbyCreationText();

    }

    private void Update(){
        if (joinedLobby != null && lobbyPlayerUpdateCoroutine == null) {
            lobbyPlayerUpdateCoroutine = StartCoroutine(CheckForLobbyPlayerUpdate());
        }
    }


    public async void CreateLobby() {
        try{

            CreateLobbyOptions createLobbyOptions = new CreateLobbyOptions {
                IsPrivate = false,
                Player = GetPlayer(),
                Data=new Dictionary<string, DataObject> {
                    { "Character",new DataObject(DataObject.VisibilityOptions.Public,"Character1") }
                }
            };

            Lobby lobby = await LobbyService.Instance.CreateLobbyAsync(lobbyName, playerCount,createLobbyOptions);

            hostLobby = lobby;
            joinedLobby = hostLobby;

            hostHeartbeatCoroutine = StartCoroutine(LobbyHeartbeatCoroutine());
            Debug.Log("Lobby created: " + lobby.Name + " " + lobby.MaxPlayers+" "+lobby.Id+" "+lobby.LobbyCode);

            PrintPlayers(hostLobby);
            
        } catch (LobbyServiceException e) {
            Debug.Log(e);
        }    

    }

    public async void ListLobbies() {
        try {
            QueryLobbiesOptions queryLobbiesOptions = new QueryLobbiesOptions {
                Count = 25,
                Filters = new List<QueryFilter> {
                    new QueryFilter(QueryFilter.FieldOptions.AvailableSlots, "0", QueryFilter.OpOptions.GT),
                },
                Order = new List<QueryOrder> {
                    new QueryOrder(false,QueryOrder.FieldOptions.Created)
                }
                
            };

            QueryResponse queryResponse = await Lobbies.Instance.QueryLobbiesAsync(queryLobbiesOptions);

            Debug.Log("Lobbies found:" + queryResponse.Results.Count);
            foreach (Lobby lobby in queryResponse.Results){
                Debug.Log(lobby.Name + " " + lobby.MaxPlayers+" "+lobby.Players.Count+" "+lobby.LobbyCode+" "+lobby.Data["Character"].Value);
            }
        }
        catch (LobbyServiceException e) {
            Debug.Log(e);
        }
    }

    IEnumerator LobbyHeartbeatCoroutine() {
        while (hostLobby != null) {
            Debug.Log("heartbeat");
            LobbyService.Instance.SendHeartbeatPingAsync(hostLobby.Id);
            yield return new WaitForSecondsRealtime(15);
        
        }
    
    }

    public async void JoinLobby() {

        try{

            QueryResponse queryResponse = await Lobbies.Instance.QueryLobbiesAsync();

            await Lobbies.Instance.JoinLobbyByIdAsync(queryResponse.Results[0].Id);
            


        } catch (LobbyServiceException e){
            Debug.Log(e);
        }
    }

    async void JoinLobbyByCode(string pLobbyCode) {

        try {
            JoinLobbyByCodeOptions joinLobbyByCodeOptions = new JoinLobbyByCodeOptions{
                Player = GetPlayer()

            };

            QueryResponse queryResponse = await Lobbies.Instance.QueryLobbiesAsync();

            Lobby lobby = await Lobbies.Instance.JoinLobbyByCodeAsync(pLobbyCode,joinLobbyByCodeOptions);
            joinedLobby = lobby;

            
            Debug.Log("joined lobby with code" + pLobbyCode);
            PrintPlayers(lobby);

        } catch (LobbyServiceException e) {
            Debug.Log(e);
        }
    }

    public void PrintPlayers(Lobby pLobby) {
        Debug.Log("Players in Lobby " + pLobby.Name+" "+pLobby.Data["Character"].Value);
        Debug.Log(pLobby.Players.Count);

        foreach (Player player in pLobby.Players) {
            //Debug.Log(player.Id);
            //Debug.Log(player.Data);//<------------------data is null
            //Debug.Log(player.Id + " " + player.Data["playerName"].Value);//<------------------------------------------
                                                                         // player name does not exists and causes an error?
                                                                         //host has name; player that joins doesnt
        }
    }

    private Player GetPlayer(){
        return new Player{
            Data = new Dictionary<string, PlayerDataObject>{
                { "playerName", new PlayerDataObject(PlayerDataObject.VisibilityOptions.Public, playerName) }
            }
        };
    }

    async void UpdateLobbyPlayerName(string pNewPlayerName) {

        try{

            playerName = pNewPlayerName;
            await LobbyService.Instance.UpdatePlayerAsync(joinedLobby.Id, AuthenticationService.Instance.PlayerId, new UpdatePlayerOptions{
                Data = new Dictionary<string, PlayerDataObject>{
                    { "playerName", new PlayerDataObject(PlayerDataObject.VisibilityOptions.Member, playerName) }
                }
            });

            Debug.Log(playerName);

        } catch (LobbyServiceException e) {
            Debug.Log(e);
        }

    }

    public async void UpdateCharacter(string pCharacter) {

        try {
            hostLobby = await Lobbies.Instance.UpdateLobbyAsync(hostLobby.Id, new UpdateLobbyOptions{
                Data = new Dictionary<string, DataObject> {
                    { "Character",new DataObject(DataObject.VisibilityOptions.Public,pCharacter) }
            }
            });
            joinedLobby = hostLobby;

            PrintPlayers(hostLobby);

        }
        catch (LobbyServiceException e) {
            Debug.Log(e);
        
        }

    }

    public async void LeaveLobby() {
        try{
            await LobbyService.Instance.RemovePlayerAsync(joinedLobby.Id, AuthenticationService.Instance.PlayerId);
            joinedLobby = null;//might need to delete ----------------------------------------------------------------
            hostLobby = null;  //might need to delete ----------------------------------------------------------------
        } catch (LobbyServiceException e) {
            Debug.Log(e);
        }
    }

    public void KickPlayer() { 
    
    }

    public async void DeleteLobby(){
        try {
            await LobbyService.Instance.DeleteLobbyAsync(joinedLobby.Id);
        } catch (LobbyServiceException e) {
            Debug.Log(e);
        }
    }

    //methods for the UI
    public void SetPlayerName(TMP_InputField pInputField) {
        playerName = pInputField.text;
        Debug.Log(playerName);
    }

    public void LeaveNameSelectionScreen() {
        if (playerName == "TestName")
            return;

        loginPage.SetActive(false);
        lobbySelectionScreen.SetActive(true);
        lobbyNameObject.text = playerName;
    
    }

    public void ChangePlayerNumber(bool isPosivitive) {
        int multiplier = (isPosivitive) ? 1 : -1;
        playerCount += multiplier;
        playerCount = Mathf.Max(playerCount, minPlayerCount);
        playerCount = Mathf.Min(playerCount, maxPlayerCount);
        playerCountText.text = playerCount.ToString();

    }

    public void ChangeVisibility() {
        isPublicLobby = !isPublicLobby;
        RefreshVisibilityText();

    }

    void RefreshVisibilityText() {
        visibilityText.text = (isPublicLobby) ? "Public" : "Private";
    }

    void RefreshLobbyNameInputText() {
        lobbyNameInputField.text = lobbyName;
    }

    void ResetLobbyCreationText() {
        playerCount = defaultPlayerCount;
        isPublicLobby = defaultIsPublicLobby;
        lobbyName = defaultLobbyName;
        lobbyNameInputField.text = lobbyName;
        RefreshVisibilityText();
    }

    public void SetLobbyName() {
        lobbyName = lobbyNameInputField.text;
    }

    public void CreateLobbyButton() {
        CreateLobby();
        lobbyScreen.SetActive(false);
    }

    async void DisplayLobbyPlayers() {
        foreach (GameObject g in playerDisplayList){
            Destroy(g);
        }

        joinedLobby = await Lobbies.Instance.GetLobbyAsync(joinedLobby.Id);

        //foreach (PlayerData p in playerData){
        //    foreach (Player pl in joinedLobby.Players)
        //        Debug.Log(p.player.Id == pl.Id);
        //}

        joinedLobbyNameText.text = joinedLobby.Name;
        joinedLobbyPlayersText.text = string.Format("{0}/{1}",joinedLobby.Players.Count,joinedLobby.MaxPlayers);
        joinedLobbyCode.text = joinedLobby.LobbyCode;

        foreach (Player player in joinedLobby.Players){
            GameObject g = Instantiate(playerDisplayPrefab, playerListParent.transform);
            playerDisplayList.Add(g);
            RectTransform rectTransformPrefab = g.GetComponent<RectTransform>();
            rectTransformPrefab.sizeDelta = new Vector2(sizeComparison.rect.width, sizeComparison.rect.height/5);
            g.GetComponent<PlayerDisplayObjectScript>().UpdatePlayerName(player.Data["playerName"].Value);
        }

    }

    IEnumerator CheckForLobbyPlayerUpdate() { 
    
        while (joinedLobby != null){
            DisplayLobbyPlayers();
            yield return new WaitForSecondsRealtime(1);
        }
    }

    public void UpdateLobbyCode(TMP_InputField pInputField) {
        lobbyCode = pInputField.text;
        Debug.Log(lobbyCode);
    }

    public void JoinLobbyByCodeButton() {
        JoinLobbyByCode(lobbyCode);
    }

    private void OnApplicationQuit(){
        LeaveLobby();
    }
}
