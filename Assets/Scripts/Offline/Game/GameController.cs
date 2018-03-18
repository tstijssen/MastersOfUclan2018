using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour
{

    public GameObject Player1;
    public GameObject Player2;
    public GameObject Player3;
    public GameObject Player4;

    //public Text Player1Lives;
    //public Text Player2Lives;
    //public Text Player3Lives;
    //public Text Player4Lives;

    public GameObject laserCar;
    public GameObject bulletCar;
    public GameObject boat;
    public GameObject train;


    public GameObject EndMenu;
    public Text endText;
    public GameObject InGameMenu;
    public Button resume;
    public Button restart;
    public Button quitToLobby;


    public bool paused = false;
    public bool AxisInUse = false;

    // Use this for initialization
    void Start ()
    {
        //Activate and place Players

        resume.onClick.AddListener(ResumeGame);
        restart.onClick.AddListener(RestartGame);
        quitToLobby.onClick.AddListener(LoadLobby);


        if(PlayerPrefs.GetInt("P2In") == 1)
        {
            Player2.SetActive(true);
            Player2.transform.position = new Vector3(0f, 7f, 10f);
        }
        if (PlayerPrefs.GetInt("P3In") == 1)
        {
            Player3.SetActive(true);
            Player3.transform.position = new Vector3(10f, 7f, 0f);
        }
        if (PlayerPrefs.GetInt("P4In") == 1)
        {
            Player4.SetActive(true);
            Player4.transform.position = new Vector3(0f, 7f, -10f);
        }

        //Load Player Vehicles
        switch(PlayerPrefs.GetInt("P1Car"))
        {
            case 0:
                Instantiate(bulletCar, Player1.transform);
                break;
            case 1:
                Instantiate(train, Player1.transform);
                break;
            case 2:
                Instantiate(boat, Player1.transform);
                break;
            case 3:
                Instantiate(laserCar, Player1.transform);
                break;
        }

        switch (PlayerPrefs.GetInt("P2Car"))
        {
            case 0:
                Instantiate(bulletCar, Player2.transform);
                break;
            case 1:
                Instantiate(train, Player2.transform);
                break;
            case 2:
                Instantiate(boat, Player2.transform);
                break;
            case 3:
                Instantiate(laserCar, Player2.transform);
                break;
        }


        //Set Lives
        PlayerPrefs.SetInt("P1Lives", PlayerPrefs.GetInt("GameLives"));
        PlayerPrefs.SetInt("P2Lives", PlayerPrefs.GetInt("GameLives"));
    }
	
	// Update is called once per frame
	void Update ()
    {

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            paused = !paused;
           
        }


        if (paused)
        {
            InGameMenu.SetActive(true);
            Time.timeScale = 0f;
        }
        else
        {
            InGameMenu.SetActive(false);
            Time.timeScale = 1f;
        }


    }


    void ResumeGame()
    {
        paused = !paused;
    }

    void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex, LoadSceneMode.Single);
    }

    void LoadLobby()
    {
        SceneManager.LoadScene("Menu");
    }


}

