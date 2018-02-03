using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LobbyOffline : MonoBehaviour {


    public Text P1Car;

    int selection = 0;

	// Use this for initialization
	void Start () {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetAxis("DpadVert") > 0.5f && selection == 0)
        {
            P1Car.text = "Car";
            selection++;
        }

        if (Input.GetAxis("DpadVert") < -0.5f && selection == 1)
        {
            P1Car.text = "Tank";
            selection--;
        }



        if (Input.GetButtonDown("Brake"))
        {
            SceneManager.LoadScene("Menu");
        }
    }
}
