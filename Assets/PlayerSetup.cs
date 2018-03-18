using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSetup : MonoBehaviour {

    public Color playerColor;
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
