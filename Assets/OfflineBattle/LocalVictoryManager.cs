using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using XInputDotNetPure;

public class LocalVictoryManager : MonoBehaviour {

    public GameObject[] m_Players;
    public GameObject m_Panel;
    public Text m_VictoryDesc;
    public Text m_VictoryData;
    public Button m_MenuButton;
    public GamePadState controllerState;

    void Start()
    {
        m_MenuButton.onClick.AddListener(LoadMenuScene);
    }

    void LoadMenuScene()
    {
        SceneManager.LoadSceneAsync("Menu");
    }

    public void SetupFromOnline(int pNum, int pScore)
    {
        m_Panel.SetActive(true);
        m_VictoryDesc.text = "Player " + pNum + " has won!";
        m_VictoryData.text = "Score: " + pScore;
    }

    // Update is called once per frame
    void Update()
    {
        controllerState = GamePad.GetState(PlayerIndex.One);

        if(m_Panel.activeInHierarchy)
        {
            if (controllerState.Buttons.A == ButtonState.Pressed)
            {
                SceneManager.LoadScene("OfflineBattle");
            }
            if (controllerState.Buttons.B == ButtonState.Pressed)
            {
                SceneManager.LoadScene("Menu");
            }
        }


        if (m_Players.Length == 0)
        {
            m_Players = GameObject.FindGameObjectsWithTag("Player");
        }
        else
        {
            for (int i = 0; i < m_Players.Length; ++i)
            {
                if (m_Players[i].activeInHierarchy)
                {
                    CarFireControl car = m_Players[i].GetComponentInChildren<CarFireControl>();
                    if (car && car.gameObject.activeInHierarchy && car.m_Victory)
                    {
                        // a player has won the game
                        m_Panel.SetActive(true);

                        //for (int p = 0; p < m_Players.Length; ++p)
                        //{
                        //    m_Players[p].GetComponentInChildren<CarFireControl>().Respawn();
                        //}

                       

                        m_VictoryDesc.text = "Player " + (i + 1) + " has won!";
                        m_VictoryData.text = "Score: " + car.m_Score + "\nKills: " + car.m_Kills + "\nDeaths: " + car.m_Deaths;
                    }
                }
            }
        }
        
    }
}
