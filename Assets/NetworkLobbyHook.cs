using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Prototype.NetworkLobby;
using UnityEngine.Networking;

public class NetworkLobbyHook : LobbyHook {

    public override void OnLobbyServerSceneLoadedForPlayer(NetworkManager manager, GameObject lobbyPlayer, GameObject gamePlayer)
    {
        Debug.Log("In Hook!");
        LobbyPlayer lobbyProfile = lobbyPlayer.GetComponent<LobbyPlayer>();
        OnlineCharacterSelect localPlayer = gamePlayer.GetComponent<OnlineCharacterSelect>();

        //localPlayer.playerName = lobbyProfile.playerName;
        localPlayer.playerNumber = lobbyProfile.slot;
        localPlayer.playerColor = lobbyProfile.playerColor;
        localPlayer.playerCarSelection = lobbyProfile.playerVehicle;
        localPlayer.levelNumber = lobbyProfile.hostMap;
        Debug.Log(lobbyProfile.slot + " car = " + localPlayer.playerCarSelection);

    }
}
