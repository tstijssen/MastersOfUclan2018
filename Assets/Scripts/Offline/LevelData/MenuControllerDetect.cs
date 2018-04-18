using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XInputDotNetPure;

public class MenuControllerDetection : MonoBehaviour {


    public GameObject[] players;
    bool playerIndexSet = false;
    PlayerIndex playerIndex;
    GamePadState state;
    GamePadState prevState;
    int numOfPlayers = 0;
    // Use this for initialization
    void Start () {
        //state = new GamePadState[players.Length];
	}
	
    void FixedUpdate()
    {
        //GamePad.SetVibration(playerIndex, state[0].Triggers.Left, state[0].Triggers.Right);
    }

	// Update is called once per frame
	void Update () {
		
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
        prevState = state;
        state = GamePad.GetState(playerIndex);
        numOfPlayers = 0;
    }


}
