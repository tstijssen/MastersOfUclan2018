using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using XInputDotNetPure;

public class OfflineControllerPlayer : MonoBehaviour {

    public Button[] ListOfButtons;
    public GameObject ControllerIcon;
    public PlayerIndex ControllerIndex;
    public float ThumbstickMargin;
    public TeamColours IndicatorColour;

    [SerializeField]
    private Button CurrentButton;
    
    int btnIndex = 1;
    bool canInteract;
    float colourAmount;
    public GamePadState controllerState;

	// Use this for initialization
	void Start ()
    {
        CurrentButton = ListOfButtons[btnIndex];
        colourAmount = 0.07f;
        switch (IndicatorColour)
        {
            case TeamColours.Red:
                ControllerIcon.GetComponent<Image>().color = Color.red;
                break;
            case TeamColours.Blue:
                ControllerIcon.GetComponent<Image>().color = Color.blue;
                break;
            case TeamColours.Green:
                ControllerIcon.GetComponent<Image>().color = Color.green;
                break;
            case TeamColours.Yellow:
                ControllerIcon.GetComponent<Image>().color = Color.yellow;
                break;
        }
        ControllerIcon.transform.position = new Vector3(CurrentButton.transform.position.x - 1, CurrentButton.transform.position.y, CurrentButton.transform.position.z);

    }

    public void SetButton()
    {
        CurrentButton = ListOfButtons[btnIndex];
    }

    void OnEnable()
    {
        canInteract = false;
        StartCoroutine(ButtonClick());
    }

    // Update is called once per frame
    void LateUpdate ()
    {

        if (controllerState.IsConnected)
            ControllerIcon.SetActive(true);
        else
            ControllerIcon.SetActive(false);

        if (canInteract && controllerState.IsConnected)
        {
            if(controllerState.ThumbSticks.Left.Y > ThumbstickMargin)
            {
                if (btnIndex < ListOfButtons.Length - 1)
                    btnIndex++;

                CurrentButton = ListOfButtons[btnIndex];
                ControllerIcon.transform.position = new Vector3(CurrentButton.transform.position.x - 1, CurrentButton.transform.position.y, CurrentButton.transform.position.z);

                canInteract = false;
                StartCoroutine(ButtonClick());
            }

            if (controllerState.ThumbSticks.Left.Y < -ThumbstickMargin)
            {
                if (btnIndex > 0)
                    btnIndex--;

                CurrentButton = ListOfButtons[btnIndex];
                ControllerIcon.transform.position = new Vector3(CurrentButton.transform.position.x - 1, CurrentButton.transform.position.y, CurrentButton.transform.position.z);

                canInteract = false;
                StartCoroutine(ButtonClick());
            }

            if (CurrentButton.interactable && controllerState.Buttons.A == ButtonState.Pressed)
            {
                Debug.Log("Pressing A");
                CurrentButton.onClick.Invoke();
                canInteract = false;
                StartCoroutine(ButtonClick());
            }
        }
	}

    IEnumerator ButtonClick()
    {
        Debug.Log("Delaying");
        yield return new WaitForSeconds(0.25f);
        canInteract = true;   // After the wait is over, the player can interact with the menu again.
    }
}
