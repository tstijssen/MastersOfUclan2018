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

    [SyncVar]
    public int rulesNumber;

    public GameObject[] vehicles;
    public GameObject[] levelPrefabs;
    public string[] levelNames;
    GameObject go = null;
    public Material[] skyBoxes;

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

    void Update()
    {
        if (!go)
        {
            go = Instantiate(levelPrefabs[levelNumber]);
            RenderSettings.skybox = skyBoxes[levelNumber];

            string levelType = levelNames[levelNumber].Substring(0, 3); // first 3 chars in name is type (e.g. FFA, CTF)

            switch (levelType)
            {
                case "FFA":
                    // find scoreboard and assign the kill limit players need to reach
                    OnlineScoreboard scoreboard = GameObject.Find("MainCanvas").GetComponent<OnlineScoreboard>();
                    scoreboard.KillLimit = rulesNumber;
                    break;
                case "CTF":
                    // get all flags and assign the number needed to reach for victory
                    GameObject[] flags = GameObject.FindGameObjectsWithTag("Flag");
                    for(int i = 0; i < flags.Length; ++i)
                    {
                        // this is done in a for loop in case more teams are added in future
                        flags[i].GetComponent<FlagCaptureScript>().m_VictoryNumber = rulesNumber;
                    }
                    break;
                case "HAT":
                    GameObject hat = GameObject.FindGameObjectWithTag("Hat");
                    // TODO: implement hat victory after branch merge
                    break;
            }
            Debug.Log("Spawning level " + levelNumber);
        }
        else
            Cmd_ReplacePlayer();

    }

}
