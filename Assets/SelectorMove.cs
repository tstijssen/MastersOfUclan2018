using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using XInputDotNetPure;

public class SelectorMove : MonoBehaviour {

    enum OnButton { offline, online, quit};
    OnButton menuBtn;

    GamePadState gamePad;
    Image selector;
    float alpha;

    public GameObject offline;
    public GameObject online;
    public GameObject quit;

    // Use this for initialization
    void Start ()
    {
        selector = GetComponent<Image>();
    }
	
	// Update is called once per frame
	void Update ()
    {
        gamePad = GameObject.Find("MenuControl").GetComponent<LocalPlayerSetup>().m_GamePadState;
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
        if (gamePad.ThumbSticks.Left.Y < -0.5f && menuBtn == OnButton.offline)
        {
            transform.position = online.transform.position;
            menuBtn = OnButton.online;
        }

        if (gamePad.ThumbSticks.Left.Y < -0.5f && menuBtn == OnButton.online)
        {
            transform.position = quit.transform.position;
            menuBtn = OnButton.quit;
        }

        if (gamePad.ThumbSticks.Left.Y > 0.5f && menuBtn == OnButton.quit)
        {
            transform.position = online.transform.position;
            menuBtn = OnButton.online;
        }

        if (gamePad.ThumbSticks.Left.Y > 0.5f && menuBtn == OnButton.online)
        {
            transform.position = offline.transform.position;
            menuBtn = OnButton.offline;
        }
    }



}
