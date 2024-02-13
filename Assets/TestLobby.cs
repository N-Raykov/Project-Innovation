using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Services.Core;
using Unity.Services.Authentication;
using Unity.Services.Lobbies;
using Unity.Services.Lobbies.Models;
using Unity.Services.Qos;

public class TestLobby : MonoBehaviour{


    Lobby hostLobby;
    Coroutine hostHeartbeatCoroutine;

    private async void Start(){
        await UnityServices.InitializeAsync();


        AuthenticationService.Instance.SignedIn += () =>{
            Debug.Log("Signed in" + AuthenticationService.Instance.PlayerId);
        };
        await AuthenticationService.Instance.SignInAnonymouslyAsync();
    }

    
    public async void CreateLobby() {
        try{

            string lobbyName = "MyLobby";
            int maxPlayers = 4;
            Lobby lobby = await LobbyService.Instance.CreateLobbyAsync(lobbyName, maxPlayers);
            hostLobby = lobby;
            hostHeartbeatCoroutine = StartCoroutine(LobbyHeartbeatCoroutine());
            Debug.Log("Lobby created: " + lobby.Name + " " + lobby.MaxPlayers);

            
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
                Debug.Log(lobby.Name + " " + lobby.MaxPlayers);
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

            
            //Debug.Log(queryResponse.Results[0].Players);

            //Debug.Log("Lobbies found:" + queryResponse.Results.Count);
            //foreach (Lobby lobby in queryResponse.Results){
            //    Debug.Log(lobby.Name + " " + lobby.MaxPlayers);
            //}
        } catch (LobbyServiceException e){
            Debug.Log(e);
        }
    }

}
