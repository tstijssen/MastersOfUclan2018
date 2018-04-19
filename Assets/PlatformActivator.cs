using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XInputDotNetPure;

public class PlatformActivator : MonoBehaviour {
    public GameObject levelSelectTxt;

	public GameObject[] Platforms;
	public GamePadState[] gamePadStates;
    bool[] playersReady = new bool[4];
    int noPlayers;

    // Use this for initialization
    void Start()
    {
        gamePadStates = new GamePadState[Platforms.Length];
        for (int i = 0; i < Platforms.Length; ++i)
        {
            playersReady[i] = false;
        }

        noPlayers = 0;
    }
	
	// Update is called once per frame
	void Update () {
		for (int i = 0; i < Platforms.Length; ++i)
		{
			gamePadStates[i] = GameObject.Find("MenuControl").GetComponent<MenuControllerDetect>().state[i];
			if (gamePadStates [i].IsConnected) 
			{
                if(!Platforms[i].activeInHierarchy)
				    Platforms [i].SetActive (true);

                Platforms [i].GetComponent<PlatfomOptions> ().gamePad = gamePadStates[i];

                if(gamePadStates[i].Buttons.A == ButtonState.Pressed)
                    playersReady[i] = true;

                for (int j = 0; j < Platforms.Length; ++j)
                {
                    if (!playersReady[j])
                        return;

                    levelSelectTxt.SetActive(true);

                }
            }
			else
				Platforms [i].SetActive (false);
		}



	}
}
