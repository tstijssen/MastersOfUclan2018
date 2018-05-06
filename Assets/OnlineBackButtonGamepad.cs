using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using XInputDotNetPure;

public class OnlineBackButtonGamepad : MonoBehaviour {

    public Button m_ThisObjButton;
    bool canInteract;

    void OnEnable()
    {
        canInteract = false;
        StartCoroutine(ButtonClick());
    }

    // Update is called once per frame
    void Update () {

        GamePadState currentState = GamePad.GetState(PlayerIndex.One);

        if (currentState.IsConnected)
        {
            if(canInteract && currentState.Buttons.Back == ButtonState.Pressed)
            {
                canInteract = false;
                StartCoroutine(ButtonClick());

                m_ThisObjButton.onClick.Invoke();
            }
        }
    }

    IEnumerator ButtonClick()
    {
        yield return new WaitForSeconds(0.25f);
        canInteract = true;   // After the wait is over, the player can interact with the menu again.
    }

}
