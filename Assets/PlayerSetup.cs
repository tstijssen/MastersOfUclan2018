using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class PlayerSetup : NetworkBehaviour {
     
    [SyncVar]
    public Color playerColor;

    [SyncVar]
    public int playerCarSelection;

    [SyncVar]
    public int playerNumber;


    public TeamColours m_Team;
    public GameObject[] vehicles;
    public OnlineFireControl fireControl;
    
    public OnlineFollowCamera localCamera;

    public override void OnStartLocalPlayer()
    {
        Debug.Log("Setting camera");
        localCamera = Camera.main.GetComponent<OnlineFollowCamera>();
        GetComponent<UnityStandardAssets.Vehicles.Car.OnlineUserControl>().m_Camera = localCamera.gameObject;
        localCamera.container = this.gameObject;
        localCamera.targets = vehicles;
        fireControl.m_HitIndicator = GameObject.Find("HitIndicator").GetComponent<Image>();
        GameObject.Find("ControllerMouse").GetComponent<OnlineControllerInput>().container = fireControl.gameObject;

        Debug.Log("camera done");


    }

    private void Start()
    {
        GetComponent<Rigidbody>().useGravity = false;

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

        fireControl.ColourInPlayer();

        Debug.Log("Player setup done");
        //for (int i = 0; i < vehiclePrefabs.Length; ++i)
        //{
        //    Renderer ren = vehiclePrefabs[i].GetComponent<Renderer>();
        //    Material carMat;
        //    if (ren.materials.Length == 1)
        //        carMat = ren.material;
        //    else
        //        carMat = ren.materials[1];

        //    carMat.color = playerColor;
        //}



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
