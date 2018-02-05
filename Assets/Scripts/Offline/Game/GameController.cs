using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour {

    public GameObject selector;
    private int selectionCount = 0;

    public GameObject car;
    public GameObject Tank;

    public GameObject InGameMenu;

    public bool paused = false;
    public bool AxisInUse = false;

    // Use this for initialization
    void Start ()
    {

        

        GameObject.Find("Player1").transform.position = new Vector3(-30f, 9f, 0f);

        if (PlayerPrefs.GetFloat("P1Choice") == 1)
        {
            Instantiate(car, GameObject.Find("Player1").transform);
        }

        if (PlayerPrefs.GetFloat("P1Choice") == 0)
        {
            Instantiate(Tank, GameObject.Find("Player1").transform);
        }

        GameObject.Find("Player2").transform.position = new Vector3(30f, 9f, 0f);

        if (PlayerPrefs.GetFloat("P2Choice") == 1)
        {
            Instantiate(car, GameObject.Find("Player2").transform);
        }

        if (PlayerPrefs.GetFloat("P2Choice") == 0)
        {
            Instantiate(Tank, GameObject.Find("Player2").transform);
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

            float axis = Input.GetAxis("DpadVert");

            if(axis >= -0.4f && axis <= 0.4f)
            {
                AxisInUse = false;
            }

            //Down
            if (Input.GetAxis("DpadVert") > 0.5f && !AxisInUse && selectionCount < 2)
            {
                selector.transform.position -= new Vector3(0f, 30f, 0f);
                AxisInUse = true;
                selectionCount++;
            }

            //Up
            if (Input.GetAxis("DpadVert") < -0.5f && !AxisInUse && selectionCount > 0)
            {
                selector.transform.position += new Vector3(0f, 30f, 0f);
                AxisInUse = true;
                selectionCount--;
            }

            if (Input.GetButtonDown("Fire1"))
            {
                switch (selectionCount)
                {
                    case 0:
                        paused = false;
                        break;
                    case 1:
                        SceneManager.LoadScene("Battle OFfline");
                        break;
                    case 2:
                        SceneManager.LoadScene("Lobby OFfline");
                        break;
                }
            }
            


        }
        else
        {
            InGameMenu.SetActive(false);
            Time.timeScale = 1f;
        }

    }
}
