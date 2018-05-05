using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XInputDotNetPure;

public class OnlineLobbyControllerDetect : MonoBehaviour {

    public OfflineControllerPlayer controllerUser;
	
	// Update is called once per frame
	void Update () {
        GamePadState currentState = GamePad.GetState(PlayerIndex.One);

        if (currentState.IsConnected)
        {
            controllerUser.ControllerIcon.SetActive(true);
            
        }
        else
        {
            controllerUser.ControllerIcon.SetActive(false);
        }
        controllerUser.controllerState = currentState;

    }
}
