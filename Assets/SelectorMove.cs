using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using XInputDotNetPure;

public class SelectorMove : MonoBehaviour {

    enum OnButton { offline, online, quit, options};
    OnButton menuBtn;
	Menu mainMenuControl;
    GamePadState gamePad;
    Image selector;
    float alpha;
	bool canInteract;

    public GameObject offline;
    public GameObject online;
    public GameObject quit;
    public GameObject options;

    // Use this for initialization
    void Start ()
    {
        selector = GetComponent<Image>();
		canInteract = false;
		StartCoroutine (MenuChange());
		mainMenuControl = GameObject.Find ("MenuControl").GetComponent<Menu> ();
    }
	
	// Update is called once per frame
	void Update ()
    {
        gamePad = GameObject.Find("MenuControl").GetComponent<MenuControllerDetect>().state[0];
        Debug.Log(gamePad.IsConnected);

        if (gamePad.IsConnected)
        {
            Debug.Log(gamePad.ThumbSticks.Left.Y);
            Debug.Log(gamePad.DPad.Down);
        }

        selector.color += new Color(0f, 0f, 0f, alpha);

        if (selector.color.a < 0.2f)
            alpha = 0.07f;      
        else if (selector.color.a > 0.8f)
            alpha = -0.07f;

        MainMenuControl();
        
    }


    void MainMenuControl()
    {
        if (canInteract && gamePad.ThumbSticks.Left.Y < -0.5f && menuBtn == OnButton.offline)
        {
            transform.position = online.transform.position;
            menuBtn = OnButton.online;
			canInteract = false;
			StartCoroutine (MenuChange());
        }

		if (canInteract && gamePad.ThumbSticks.Left.Y < -0.5f && menuBtn == OnButton.online)
        {
            transform.position = quit.transform.position;
            menuBtn = OnButton.quit;
			canInteract = false;
			StartCoroutine (MenuChange());
        }

		if (canInteract && gamePad.ThumbSticks.Left.Y > 0.5f && menuBtn == OnButton.quit)
        {
            transform.position = online.transform.position;
            menuBtn = OnButton.online;
			canInteract = false;
			StartCoroutine (MenuChange ());
        }

		if (canInteract && gamePad.ThumbSticks.Left.Y > 0.5f && menuBtn == OnButton.online)
        {
            transform.position = offline.transform.position;
            menuBtn = OnButton.offline;
			canInteract = false;
			StartCoroutine (MenuChange());
        }

        if (canInteract && gamePad.ThumbSticks.Left.X > 0.5f && menuBtn != OnButton.options)
        {
            transform.position = options.transform.position;
            menuBtn = OnButton.options;
            canInteract = false;
            StartCoroutine(MenuChange());
        }


        if (canInteract && gamePad.ThumbSticks.Left.X < -0.5f && menuBtn == OnButton.options)
        {
            transform.position = offline.transform.position;
            menuBtn = OnButton.offline;
            canInteract = false;
            StartCoroutine(MenuChange());
        }

        if (canInteract && gamePad.Buttons.A == ButtonState.Pressed)
		{
			switch (menuBtn) {
			case OnButton.offline:
				mainMenuControl.LaunchOffline ();
				break;
			case OnButton.online:
				mainMenuControl.LaunchOnline ();
				break;
			case OnButton.quit:
				mainMenuControl.QuitGame ();
				break;
			}
			canInteract = false;
			StartCoroutine (MenuChange());
		}

    }

	IEnumerator MenuChange()
	{
		Debug.Log("Delaying");
		yield return new WaitForSeconds(0.25f);
		canInteract = true;   // After the wait is over, the player can interact with the menu again.
	}

}
