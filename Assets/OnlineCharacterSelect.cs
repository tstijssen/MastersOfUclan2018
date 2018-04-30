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

    [SyncVar]
    public int levelNumber;

    public GameObject[] vehicles;
    public GameObject[] levelPrefabs;
    public string[] levels;
    GameObject go;

    [Command]
    void Cmd_ReplacePlayer()
    {
        GameObject go = Instantiate(vehicles[playerCarSelection], transform.position, transform.rotation);
        go.GetComponent<PlayerSetup>().playerNumber = playerNumber;
        go.GetComponent<PlayerSetup>().playerColor = playerColor;
        go.GetComponent<OnlineFireControl>().RpcRespawn();
        NetworkServer.Spawn(go);

        if(NetworkServer.ReplacePlayerForConnection(connectionToClient, go, playerControllerId))
        {
            Debug.Log("Replacing player!");
            NetworkServer.Destroy(gameObject);
        }
    }

    // Use this for initialization
    void Start () {

        //if(playerNumber == 0)
        //{
        //    NetworkManager.singleton.ServerChangeScene(levels[levelNumber]);
        //}
        go = Instantiate(levelPrefabs[levelNumber]);    
    }

    void Update()
    {
        if (go.activeInHierarchy)
        {
            Cmd_ReplacePlayer();
        }
    }
}
