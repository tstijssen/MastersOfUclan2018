using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class OnlineCharacterSelect : NetworkBehaviour {

    [SyncVar]
    public Color playerColor;

    [SyncVar]
    public int playerCarSelection;

    [SyncVar]
    public int playerNumber;

    public GameObject[] vehicles;


    [Command]
    void Cmd_ReplacePlayer()
    {
        GameObject go = Instantiate(vehicles[playerCarSelection], transform.position, transform.rotation);
        go.GetComponent<PlayerSetup>().playerNumber = playerNumber;
        go.GetComponent<PlayerSetup>().playerColor = playerColor;

        NetworkServer.Spawn(go);

        if(NetworkServer.ReplacePlayerForConnection(connectionToClient, go, playerControllerId))
        {
            NetworkServer.Destroy(gameObject);
        }
    }

    // Use this for initialization
    void Start () {
        //NetworkManager.singleton.ServerChangeScene("OnlineBattle");
        Cmd_ReplacePlayer();
    }
}
