using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;


public class PlayerSetup : NetworkBehaviour {

    [SyncVar]
    public Color playerColor;

    [SyncVar]
    public int playerCarSelection;
    public GameObject[] vehiclePrefabs;

    private void Start()
    {
        GameObject playerCar = Instantiate(vehiclePrefabs[playerCarSelection]) as GameObject;
        playerCar.transform.parent = this.transform;
        playerCar.transform.position = this.transform.position;
        //vehiclePrefabs[playerCarSelection].SetActive(true);
    }
}
