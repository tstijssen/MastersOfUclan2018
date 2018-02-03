using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour {

    PlayerOneControl playerSelect;

    public GameObject car;
    public GameObject Tank;

    public GameObject InGameMenu;

    public bool paused = false;

    // Use this for initialization
    void Start ()
    {

        playerSelect = GameObject.Find("Player1").GetComponent<PlayerOneControl>();

        if (playerSelect.vehicle == 0)
        {
            Instantiate(car, GameObject.Find("Player1").transform);
        }

        if (playerSelect.vehicle == 1)
        {
            Instantiate(Tank, GameObject.Find("Player1").transform);
        }
    }
	
	// Update is called once per frame
	void Update ()
    {
        if (Input.GetButtonDown("Pause"))
        {
            paused = !paused;
        }




        if (paused)
        {
            Time.timeScale = 0f;
            InGameMenu.SetActive(true);
        }
        else
        {
            InGameMenu.SetActive(false);
            Time.timeScale = 1f;
        }

    }
}
