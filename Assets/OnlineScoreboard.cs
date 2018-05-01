using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnlineScoreboard : MonoBehaviour {

    public GameObject[] m_ScorePanels;
    public GameObject[] m_Players;
    public ScorePanelItems[] m_Items;

    public Sprite[] m_CarSprites;
    public int KillLimit = 0;
    OnlineFireControl[] m_Cars = new OnlineFireControl[4];
    bool initialized = false;

	// Use this for initialization
	void Start () {
		


	}
	
	// Update is called once per frame
	void Update () {

        if (m_Players.Length == 0)
        {
            m_Players = GameObject.FindGameObjectsWithTag("Player");
        }
        else
        {
            if(!initialized)
            {
                for (int i = 0; i < m_Players.Length; ++i)
                {
                    if (m_Players[i].activeInHierarchy)
                    {
                        m_ScorePanels[i].SetActive(true);
                        m_Cars[i] = m_Players[i].GetComponentInChildren<OnlineFireControl>();
                        if (m_Cars[i])
                        {
                            switch (m_Cars[i].m_PlayerTeam)
                            {
                                case TeamColours.Red:
                                    m_Items[i].m_Colour.color = Color.red;
                                    break;
                                case TeamColours.Blue:
                                    m_Items[i].m_Colour.color = Color.blue;
                                    break;
                                case TeamColours.Green:
                                    m_Items[i].m_Colour.color = Color.green;
                                    break;
                                case TeamColours.Yellow:
                                    m_Items[i].m_Colour.color = Color.yellow;
                                    break;
                            }

                            switch (m_Cars[i].m_GunData.gunType)
                            {
                                case FireType.TwinGuns:
                                    m_Items[i].m_Vehicle.sprite = m_CarSprites[0];
                                    break;
                                case FireType.Beam:
                                    m_Items[i].m_Vehicle.sprite = m_CarSprites[1];
                                    break;
                                case FireType.Ram:
                                    m_Items[i].m_Vehicle.sprite = m_CarSprites[2];
                                    break;
                                case FireType.Cannon:
                                    m_Items[i].m_Vehicle.sprite = m_CarSprites[3];
                                    break;
                            }
                        }
                    }
                }
                initialized = true;
            }
            else
            {
                for (int i = 0; i < m_ScorePanels.Length; ++i)
                {
                    if (m_ScorePanels[i].activeInHierarchy)
                    {
                        if (m_Cars[i])
                        {
                            m_Items[i].m_Score.text = m_Cars[i].m_Score.ToString();

                            if (KillLimit > 0)
                                m_Items[i].m_Kills.text = m_Cars[i].m_Kills.ToString() + "/" + KillLimit;
                            else
                                m_Items[i].m_Kills.text = m_Cars[i].m_Kills.ToString();

                            m_Items[i].m_Deaths.text = m_Cars[i].m_Deaths.ToString();
                            Debug.Log("Populating scoreboard");
                            if (KillLimit != 0 && m_Cars[i].m_Score >= KillLimit)
                            {
                                m_Cars[i].m_Victory = true;
                            }
                        }
                    }
                }
            }
        }

	}
}
