using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSetup : MonoBehaviour {

    public Color playerColor;
    public string playerName;
    public int playerCarSelection;
    public GameObject[] vehiclePrefabs;

    private void Start()
    {
        //GameObject playerCar = Instantiate(vehiclePrefabs[playerCarSelection]) as GameObject;
        vehiclePrefabs[playerCarSelection].SetActive(true);
    }
}
