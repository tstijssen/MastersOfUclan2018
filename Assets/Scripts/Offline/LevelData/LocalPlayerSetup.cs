using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XInputDotNetPure;

public class LocalPlayerSetup : MonoBehaviour {

    public TeamColours m_PlayerTeam;
    public string m_HorizontalMove;
    public string m_VerticalMove;
    public string m_FireCommand;
    public GamePadState m_GamePadState;
    public GameObject m_Camera;
    float cameraMovement;
    public void SetColour(int colourNum)
    {
        m_PlayerTeam = (TeamColours)colourNum;
    }

    void Start()
    {
        int vehicleSelection;
        switch (this.name)
        {
            case "Player1":
                vehicleSelection = PlayerPrefs.GetInt("P1");
                break;

            case "Player2":
                vehicleSelection = PlayerPrefs.GetInt("P2");
                break;

            case "Player3":
                vehicleSelection = PlayerPrefs.GetInt("P3");
                break;
            case "Player4":
                vehicleSelection = PlayerPrefs.GetInt("P4");
                break;
        }
        m_Camera.GetComponent<FollowCamera>().SetSelection(0);
    }

    void Update()
    {

    }

}
