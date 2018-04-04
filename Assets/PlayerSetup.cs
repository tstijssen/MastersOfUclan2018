using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;


public class PlayerSetup : NetworkBehaviour {

    [SyncVar]
    public Color playerColor;

    [SyncVar]
    public int playerCarSelection;

    [SyncVar]
    public int playerNumber;

    //public NetworkTransformChild vehicleNetworkTransform;
    public GameObject[] vehiclePrefabs;
    public OnlineFireControl fireControl;


    private void Start()
    {
        //GameObject playerCar = Instantiate(vehiclePrefabs[playerCarSelection]) as GameObject;
        //playerCar.transform.parent = this.transform;
        //playerCar.transform.position = this.transform.position;
        //vehicleNetworkTransform.target = playerCar.transform;
        fireControl.m_PlayerNumber = playerNumber;
        
        if (playerColor == Color.red)
        {
            fireControl.m_PlayerTeam = TeamColours.Red;
        }
        else if (playerColor == Color.blue)
        {
            fireControl.m_PlayerTeam = TeamColours.Blue;
        }
        else if (playerColor == Color.green)
        {
            fireControl.m_PlayerTeam = TeamColours.Green;
        }
        else if (playerColor == Color.yellow)
        {
            fireControl.m_PlayerTeam = TeamColours.Yellow;
        }

        //switch (playerCarSelection)
        //{
        //    case 0: // twin gun car
        //        fireControl.m_GunData.gunType = FireType.TwinGuns;
        //        break;
        //    case 1: // train
        //        break;
        //    case 2: // ship
        //        break;
        //    case 3: // beamer
        //        fireControl.m_GunData.gunType = FireType.Beam;
        //        break;
        //    default:
        //        break;
        //}
        //vehiclePrefabs[playerCarSelection].SetActive(true);
    }
}
