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

    public void SetColour(int colourNum)
    {
        m_PlayerTeam = (TeamColours)colourNum;
    }


    void Update()
    {
        //transform.Rotate(Vector3.up, m_GamePadState.ThumbSticks.Left.X);
    }

}
