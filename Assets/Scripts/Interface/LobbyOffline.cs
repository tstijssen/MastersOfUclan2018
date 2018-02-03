using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LobbyOffline : MonoBehaviour {

    public GameObject PlayerOne;

    public Text P1Car;

    int selection = 0;



    PlayerOneControl playerSelect;

    // Use this for initialization
    void Start () {
        DontDestroyOnLoad(PlayerOne);
        playerSelect = PlayerOne.GetComponent<PlayerOneControl>();
        P1Car.text = "Car";
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetAxis("DpadVert") > 0.5f && selection == 0)
        {
            P1Car.text = "Car";
            playerSelect.vehicle = 0;
            selection++;
        }

        if (Input.GetAxis("DpadVert") < -0.5f && selection == 1)
        {
            P1Car.text = "Tank";
            playerSelect.vehicle = 1;
            selection--;
        }



        if (Input.GetButtonDown("Brake"))
        {
            SceneManager.LoadScene("Menu");
        }
        if (Input.GetButtonDown("Fire1"))
        {
            SceneManager.LoadScene("Battle Offline");
        }
    }
}
