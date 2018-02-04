using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LobbyOffline : MonoBehaviour {

    public bool player1Picked = false;
    public bool player2Picked = false;

    public GameObject PlayerOne;

    PlayerOneControl playerSelect;

    // Use this for initialization
    void Start ()
    {
        PlayerPrefs.SetInt("P1Choice", 0);
        PlayerPrefs.SetInt("P2Choice", 0);
    }

    // Update is called once per frame
    void Update()
    {

        



        // both players select vehicles
        // activate level select
        // set go option
    }
}
