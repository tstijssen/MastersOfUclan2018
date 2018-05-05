using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class OfflineLobbyPlayer : MonoBehaviour {

    public GameObject[] playerCarChoices;
    public Text pCar;

    public int pCarChoice = 0;
    public bool m_Ready;

    public Image m_ButtonImage;
    public int m_PlayerNumber;
    string m_PlayerPref;
    // Use this for initialization
    void Start ()
    {
        playerCarChoices[pCarChoice].SetActive(true);
        m_PlayerPref = "PVehicle" + m_PlayerNumber;
        PlayerPrefs.SetInt("m_PlayerPref", pCarChoice);
    }
	
	// Update is called once per frame
	void Update ()
    {
        if(m_Ready)
        {
            switch (m_PlayerNumber)
            {
                case 1:
                    m_ButtonImage.color = new Color(0.5f, 0f, 0f, 1.0f);
                    break;
                case 2:
                    m_ButtonImage.color = new Color(0f, 0f, 0.5f, 1.0f);
                    break;
                case 3:
                    m_ButtonImage.color = new Color(0f, 0.5f, 0f, 1.0f);
                    break;
                case 4:
                    m_ButtonImage.color = new Color(0.5f, 0.5f, 0f, 1.0f);
                    break;
            }
        }
        else
            m_ButtonImage.color = Color.white;

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

        for (int i = 0; i < playerCarChoices.Length; ++i)
        {
            if (i == pCarChoice)
                playerCarChoices[i].SetActive(true);
            else
                playerCarChoices[i].SetActive(false);
        }
    }

    public void IncCar()
    {
        pCarChoice++;
        if (pCarChoice > 3)
            pCarChoice = 0;
        PlayerPrefs.SetInt("m_PlayerPref", pCarChoice);
    }

    public void ReadyPlayer()
    {
        m_Ready = !m_Ready;
    }
}
