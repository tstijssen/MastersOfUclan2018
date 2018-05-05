using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadLevel : MonoBehaviour {

    public GameObject[] levels;
    public Material[] skyBoxes;

    // Load level before any other script starts
    void Awake () {
        int levelNo = PlayerPrefs.GetInt("Level");
        GameObject go = Instantiate(levels[levelNo]);
        RenderSettings.skybox = skyBoxes[levelNo];
	}
}
