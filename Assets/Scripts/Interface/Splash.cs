using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Splash : MonoBehaviour {
    bool sceneLoaded = false;
	// Use this for initialization
	void Start () {
        DontDestroyOnLoad(this);
	}
	
	// Update is called once per frame
	void Update ()
    {
        if (!sceneLoaded)
        {
            SceneManager.LoadScene("Menu");
            sceneLoaded = true;
        }
	}
}
