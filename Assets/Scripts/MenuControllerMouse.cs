using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XInputDotNetPure;

public class MenuControllerMouse : MonoBehaviour {

    public GameObject[] targets;
    public GameObject container;
    GamePadState gamePad;


    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        gamePad = container.GetComponent<MenuControllerDetect>().state;

        transform.Translate(new Vector3(gamePad.ThumbSticks.Left.X, gamePad.ThumbSticks.Left.Y, 0.0f));
	}
}
