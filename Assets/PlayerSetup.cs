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

    OnlineFollowCamera localCamera;

    public override void OnStartLocalPlayer()
    {
        Debug.Log("Setting camera");
        localCamera = Camera.main.GetComponent<OnlineFollowCamera>();
        Debug.Log(localCamera.gameObject.activeInHierarchy);

        localCamera.targets[0] = vehiclePrefabs[0];
        localCamera.targets[1] = vehiclePrefabs[1];
        localCamera.targets[2] = vehiclePrefabs[2];
        localCamera.container = this.gameObject;

        Debug.Log("camera done");
    }

    private void Start()
    {
        for (int i = 0; i < vehiclePrefabs.Length; ++i)
        {
            Renderer ren = vehiclePrefabs[i].GetComponent<Renderer>();
            Material carMat;
            if (ren.materials.Length == 1)
                carMat = ren.material;
            else
                carMat = ren.materials[1];

            carMat.color = playerColor;
        }

        fireControl.m_PlayerNumber = playerNumber;
        
        if (playerColor == Color.red)
        {
            fireControl.m_PlayerTeam = TeamColours.Red;
            //fireControl.m_FlagScoreText = GameObject.FindGameObjectWithTag("RedScore").GetComponent<Text>();
        }
        else if (playerColor == Color.blue)
        {
            fireControl.m_PlayerTeam = TeamColours.Blue;
            //fireControl.m_FlagScoreText = GameObject.FindGameObjectWithTag("BlueScore").GetComponent<Text>();

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
