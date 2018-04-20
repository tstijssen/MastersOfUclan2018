using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XInputDotNetPure;

public class PlatformActivator : MonoBehaviour
{
    public GameObject levelSelectTxt;
    Menu levelScreen;
    public GameObject[] Platforms;
    public GamePadState[] gamePadStates;
    bool[] playersReady = new bool[4];
    public int noPlayers;
    public int noReadyPlayers;
    public bool allReady = false;

    // Use this for initialization
    void Start()
    {
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

                Platforms[i].GetComponent<PlatfomOptions>().gamePad = gamePadStates[i];

                //if(gamePadStates[i].Buttons.A == ButtonState.Pressed)
                //    playersReady[i] = true;


            }
            else
                Platforms[i].SetActive(false);

            //Debug.Log(gamePadStates.Length);

            noPlayers = 0;
            noReadyPlayers = 0;
            for (int j = 0; j < Platforms.Length; ++j)
            {
                if (gamePadStates[j].IsConnected)
                {
                    noPlayers++;
                }

                if (Platforms[j].GetComponent<PlatfomOptions>().isReady)
                {
                    noReadyPlayers++;
                }

                if (noPlayers == noReadyPlayers)
                {
                    levelSelectTxt.SetActive(true);
                    allReady = true;
                }

            }
        }



    }

    private void LateUpdate()
    {
        //if (allReady)
        //{
        //    for (int i = 0; i < Platforms.Length; ++i)
        //    {
        //        gamePadStates[i] = GameObject.Find("MenuControl").GetComponent<MenuControllerDetect>().state[i];
        //        if (gamePadStates[i].Buttons.B == ButtonState.Pressed)
        //        {
        //            levelScreen.menuUp = Menu.Menus.LevelSelect;
        //            Debug.Log("Start");
        //        }
        //    }

        //}

    }
}
