using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using XInputDotNetPure;

public class PlatformActivator : MonoBehaviour
{
    Menu levelScreen;
    public GameObject[] Platforms;
    public GamePadState[] gamePadStates;
    bool[] playersReady = new bool[4];
    public int noPlayers;
    public int noReadyPlayers;
    public bool allReady = false;

    // countdown variables
    float timer;
    int currentTime;
    public int m_CountdownLength;
    public GameObject m_CountdownObj;
    bool m_CountdownActive;
    // Use this for initialization
    void Start()
    {
        m_CountdownActive = false;
        timer = 0f;
        gamePadStates = new GamePadState[Platforms.Length];
        for (int i = 0; i < Platforms.Length; ++i)
        {
            playersReady[i] = false;
        }
        levelScreen = GameObject.Find("MenuControl").GetComponent<Menu>();
        noPlayers = 0;
    }

    // Update is called once per frame
    void Update()
    {
        for (int i = 0; i < Platforms.Length; ++i)
        {
            gamePadStates[i] = GameObject.Find("MenuControl").GetComponent<MenuControllerDetect>().state[i];
            if (gamePadStates[i].IsConnected)
            {
                if (!Platforms[i].activeInHierarchy)
                    Platforms[i].SetActive(true);
                Platforms[i].GetComponent<OfflineControllerPlayer>().controllerState = gamePadStates[i];
            }
            else
                Platforms[i].SetActive(false);

            noPlayers = 0;
            noReadyPlayers = 0;
            for (int j = 0; j < Platforms.Length; ++j)
            {
                if (gamePadStates[j].IsConnected)
                {
                    noPlayers++;
                }

                if (Platforms[j].GetComponent<OfflineLobbyPlayer>().m_Ready)
                {
                    noReadyPlayers++;
                }
            }
        }
        if (noPlayers == noReadyPlayers && noPlayers > 0)
        {
            allReady = true;
            if(!m_CountdownActive)
            {
                m_CountdownActive = true;
                timer = 1f;
                currentTime = m_CountdownLength;
                m_CountdownObj.transform.parent.gameObject.SetActive(true);
            }
        }
        else
        {
            m_CountdownActive = false;
            allReady = false;
            m_CountdownObj.transform.parent.gameObject.SetActive(false);

        }

        if (allReady && m_CountdownActive)
        {
            timer -= Time.deltaTime;
            m_CountdownObj.GetComponent<Text>().text = currentTime.ToString();

            if (timer <= 0.0f)
            {
                currentTime--;
                timer = 1f;
            }

            if(currentTime < 0)
            {
                SceneManager.LoadScene("OfflineBattle");
            }
        }
    }
}
