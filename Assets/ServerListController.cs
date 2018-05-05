using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using XInputDotNetPure;

public class ServerListController : MonoBehaviour {

    public OfflineControllerPlayer controllerUser;

	// Update is called once per frame
	void Update () {

        GamePadState currentState = GamePad.GetState(PlayerIndex.One);

        if (currentState.IsConnected)
        {
            controllerUser.ControllerIcon.SetActive(true);

            Button[] foundButtons = GameObject.FindObjectsOfType<Button>();

            controllerUser.ListOfButtons = foundButtons;
            controllerUser.SetButton();
        }
        else
        {
            controllerUser.ControllerIcon.SetActive(false);
        }

        controllerUser.controllerState = currentState;

    }
}
