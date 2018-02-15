using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour {

    public GameObject selector;
    private int selectionCount = 0;

    public GameObject Player1;
    public GameObject Player2;
    public GameObject Player3;
    public GameObject Player4;

    public Text Player1Lives;
    public Text Player2Lives;
    public Text Player3Lives;
    public Text Player4Lives;

    //level list
    public GameObject Tilt;

    //public GameObject car;
    public GameObject Tank;

    public GameObject InGameMenu;

    public bool paused = false;
    public bool AxisInUse = false;

    // Use this for initialization
    void Start ()
    {
        if(PlayerPrefs.GetInt("P2In") == 1)
        {
            Player2.SetActive(true);
        }
        if (PlayerPrefs.GetInt("P3In") == 1)
        {
            Player3.SetActive(true);
        }
        if (PlayerPrefs.GetInt("P4In") == 1)
        {
            Player4.SetActive(true);
        }

        Instantiate(Tilt, GameObject.Find("Level").transform);


        if (PlayerPrefs.GetInt("Level") == 0)
        {
        }

        if (PlayerPrefs.GetFloat("P1Choice") == 0)
        {
            Instantiate(Tank, Player1.transform);
        }

        if (PlayerPrefs.GetFloat("P2Choice") == 0)
        {
            Instantiate(Tank, Player2.transform);
        }
        //if (PlayerPrefs.GetFloat("P3Choice") == 0)
        //{
        //    Instantiate(Tank, Player3.transform);
        //}
        //if (PlayerPrefs.GetFloat("P4Choice") == 0)
        //{
        //    Instantiate(Tank, Player4.transform);
        //}
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

    public void UpdateText()
    {
        Player1Lives.text = "Stock: " + PlayerPrefs.GetInt("P1Lives").ToString();
    }
}
