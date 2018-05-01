using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;

public class VisualLobbyController : NetworkBehaviour {

    public Sprite[] spriteArray;
    public Image carImg;
    public Image teamImg;

    public Text pCar;
    public Text pTeam;

    [SyncVar]
    public int pCarChoice = 0;
    [SyncVar]
    public int pTeamChoice = 0;


    // Use this for initialization
    void Start()
    {
        //spriteArray = GameObject.Find("OfflineLobby").GetComponent<LobbyOverlord>().spriteList;
        carImg.sprite = spriteArray[pCarChoice];
    }

    // Update is called once per frame
    void Update()
    {
        switch (pCarChoice)
        {
            case 0:
                pCar.text = "Gun Car";
                break;
            case 1:
                pCar.text = "Laser Car";
                break;
            case 2:
                pCar.text = "Ram Train";
                break;
            case 3:
                pCar.text = "Broadside";
                break;
        }
        carImg.sprite = spriteArray[pCarChoice];

        switch (pTeamChoice)
        {
            //case 0:
            //    pTeam.text = "Red";
            //    teamImg.color = Color.red;
            //    break;
            //case 1:
            //    pTeam.text = "Blue";
            //    teamImg.color = Color.blue;
            //    break;
            //case 2:
            //    pTeam.text = "Yellow";
            //    teamImg.color = Color.yellow;
            //    break;
            //case 3:
            //    pTeam.text = "Green";
            //    teamImg.color = Color.green;
            //    break;
        }



        //PlayerPrefs.SetInt("pCar", pCarChoice);
        //PlayerPrefs.SetInt("pTeam", pTeamChoice);
    }



    public void IncCar()
    {
        pCarChoice++;
        if (pCarChoice > 3)
            pCarChoice = 0;
    }

    public void DecCar()
    {
        pCarChoice--;
        if (pCarChoice < 0)
            pCarChoice = 3;
    }

    void IncTeam()
    {
        pTeamChoice++;
        if (pTeamChoice > 3)
            pTeamChoice = 0;
    }

    void DecTeam()
    {
        pTeamChoice--;
        if (pTeamChoice < 0)
            pTeamChoice = 3;
    }
}
