using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadLevel : MonoBehaviour {

    public GameObject[] levels = new GameObject[3];

    // Use this for initialization
    void Start () {
        levels[PlayerPrefs.GetInt("Level")].SetActive(true);
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
