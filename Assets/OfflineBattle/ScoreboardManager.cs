using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public struct ScorePanelItems
{
    public Text m_PlayerNumber;
    public Image m_Vehicle;
    public Image m_Colour;
    public Text m_Score;
    public Text m_Kills;
    public Text m_Deaths;
}

public class ScoreboardManager : MonoBehaviour {

    public GameObject[] m_ScorePanels;
    public GameObject[] m_Players;
    public ScorePanelItems[] m_Items;

    public Sprite[] m_CarSprites;
    public int KillLimit = 0;
    CarFireControl[] m_Cars = new CarFireControl[4];

	// Use this for initialization
	void Start () {

        KillLimit = PlayerPrefs.GetInt("FFAKillLimit");
        
		
	}
	
	// Update is called once per frame
	void Update () {
        for (int i = 0; i < m_Players.Length; ++i)
        {
            if (m_Players[i].activeInHierarchy)
            {
                m_ScorePanels[i].SetActive(true);
                m_Cars[i] = m_Players[i].GetComponentInChildren<CarFireControl>();
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
        for (int i = 0; i < m_ScorePanels.Length; ++i)
        {
            if (m_ScorePanels[i].activeInHierarchy)
            {
                if (m_Cars[i])
                {
                    m_Items[i].m_Score.text = m_Cars[i].m_Score.ToString();

                    if(KillLimit > 0)
                        m_Items[i].m_Kills.text = m_Cars[i].m_Kills.ToString() + "/" + KillLimit;
                    else
                        m_Items[i].m_Kills.text = m_Cars[i].m_Kills.ToString();

                    m_Items[i].m_Deaths.text = m_Cars[i].m_Deaths.ToString();
                    Debug.Log("Populating scoreboard");
                    if (KillLimit != 0 && m_Cars[i].m_Kills >= KillLimit)
                    {
                        m_Cars[i].m_Victory = true;
                    }
                }
            }
        }
    }
}
