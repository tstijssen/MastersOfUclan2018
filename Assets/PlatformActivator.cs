using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XInputDotNetPure;

public class PlatformActivator : MonoBehaviour {

	public GameObject[] Platforms;
	public GamePadState[] gamePadStates;

	// Use this for initialization
	void Start () {
		gamePadStates = new GamePadState[Platforms.Length];
	}
	
	// Update is called once per frame
	void Update () {
		for (int i = 0; i < Platforms.Length; ++i)
		{
			gamePadStates[i] = GameObject.Find("MenuControl").GetComponent<MenuControllerDetect>().state[i];
			if (gamePadStates [i].IsConnected) 
			{
				Platforms [i].SetActive (true);
				Platforms [i].GetComponent<PlatfomOptions> ().gamePad = gamePadStates[i];
			}
			else
				Platforms [i].SetActive (false);
		}
	}
}
