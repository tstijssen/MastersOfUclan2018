using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XInputDotNetPure;

public class ControllerDetection : MonoBehaviour {


    public GameObject[] players;
    bool playerIndexSet = false;
    PlayerIndex playerIndex;
    GamePadState[] state;
    GamePadState prevState;
    int numOfPlayers = 0;
    public GameObject pauseMenu;
    public GameObject pauseCam;
    // Use this for initialization
    void Start () {
        state = new GamePadState[players.Length];
	}

	// Update is called once per frame
	void Update () {

        pauseCam.SetActive(pauseMenu.activeInHierarchy);

        if (!playerIndexSet || prevState.IsConnected)
        {
            for (int i = 0; i < 4; ++i)
            {
                PlayerIndex testIndex = (PlayerIndex)i;

                GamePadState testState = GamePad.GetState(testIndex);

                if (testState.IsConnected)
                {
                    Debug.Log(string.Format("GamePad found {0}", testIndex));
                    playerIndex = testIndex;
                    playerIndexSet = true;
                    

                }
            }
        }
        prevState = state[0];
        numOfPlayers = 0;
        for (int i = 0; i < state.Length; ++i)
        {
            state[i] = GamePad.GetState((PlayerIndex)i);
            GetComponent<SplitSceen>().cams[i].gameObject.SetActive(false);

            if (state[i].IsConnected)
            {
                players[i].SetActive(true);
                players[i].GetComponent<LocalPlayerSetup>().m_GamePadState = state[i];
                numOfPlayers++;
                GetComponent<SplitSceen>().NumOfPlayers = numOfPlayers;
                GetComponent<SplitSceen>().cams[i].gameObject.SetActive(true);
                players[i].gameObject.SetActive(!pauseMenu.activeInHierarchy);
            }
        }
    }


}
