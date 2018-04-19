using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XInputDotNetPure;

public class MenuControllerDetect : MonoBehaviour {

    bool playerIndexSet = false;
    PlayerIndex playerIndex;
	public GamePadState[] state;
    GamePadState prevState;

	void Start()
	{
		state = new GamePadState[4];
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
        prevState = state[0];
		for (int i = 0; i < 4; ++i)
		{
			state[i] = GamePad.GetState((PlayerIndex)i);
		}
    }


}
