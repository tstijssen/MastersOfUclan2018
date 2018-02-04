using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour {

    

    public GameObject car;
    public GameObject Tank;

    public GameObject InGameMenu;

    public bool paused = false;

    // Use this for initialization
    void Start ()
    {

        

        GameObject.Find("Player1").transform.position = new Vector3(0f, 9f, 0f);

        if (PlayerPrefs.GetFloat("P1Choice") == 1)
        {
            Instantiate(car, GameObject.Find("Player1").transform);
        }

        if (PlayerPrefs.GetFloat("P1Choice") == 0)
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
