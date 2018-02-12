using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LobbyOffline : MonoBehaviour {

    //Player Variables
    static Color[] Colors = new Color[] { Color.magenta, Color.red, Color.cyan, Color.blue, Color.green, Color.yellow };

    public int playersIn = 1;
    public int playersReady = 0;
    bool allPlayersReady = false;

    public GameObject player1;
    public GameObject player2;
    public GameObject player3;
    public GameObject player4;

    public Text p1Text;
    public Text p2Text;
    public Text p3Text;
    public Text p4Text;

    public Image p1car;
    public Image p2car;
    public Image p3car;
    public Image p4car;

    public Toggle p1Ready;
    public Toggle p2Ready;
    public Toggle p3Ready;
    public Toggle p4Ready;

    bool player1Picked = false;
    bool player2Picked = false;
    bool player3Picked = false;
    bool player4Picked = false;

    int p1Pick;
    int p2Pick;
    int p3Pick;
    int p4Pick;

    public Text nextScrnText;

    GameObject vehiclePickScreen;

    //Level Variables
    public GameObject LevelScreen;
    public Text levelText;

    // Use this for initialization
    void Start ()
    {
        vehiclePickScreen = GameObject.Find("VehicleSelect");
        PlayerPrefs.SetInt("P1Choice", 0);
        PlayerPrefs.SetInt("P2Choice", 0);
        PlayerPrefs.SetInt("P3Choice", 0);
        PlayerPrefs.SetInt("P4Choice", 0);

        PlayerPrefs.SetInt("Level", 0);

        PlayerPrefs.SetInt("P2In", 0);
        PlayerPrefs.SetInt("P3In", 0);
        PlayerPrefs.SetInt("P4In", 0);

        playersIn = 1;
    }

    // Update is called once per frame
    void Update()
    {
        


        //Player select vehicle
        if (Input.GetAxis("Vertical") > 0.3f || Input.GetAxis("DpadVert") > 0.3f)
        {
            p1Pick++;
            switch (p1Pick)
            {
                case 0:
                    p1Text.text = "Tank";
                    break;
                case 1:
                    p1Text.text = "Car";
                    break;
            } 
            
            if(p1Pick > 1)
            {
                p1Pick = 0;
            }
        }
        if (Input.GetAxis("Vertical2") > 0.3f || Input.GetAxis("DpadVert2") > 0.3f)
        {
            p2Pick++;
            switch (p2Pick)
            {
                case 0:
                    p2Text.text = "Tank";
                    break;
            }

            if (p2Pick > 1)
            {
                p2Pick = 0;
            }
        }
        if (Input.GetAxis("Vertical3") > 0.3f || Input.GetAxis("DpadVert3") > 0.3f)
        {
            p3Pick++;
            switch (p3Pick)
            {
                case 0:
                    p3Text.text = "Tank";
                    break;
            }

            if (p3Pick > 1)
            {
                p3Pick = 0;
            }
        }
        if (Input.GetAxis("Vertical4") > 0.3f || Input.GetAxis("DpadVert4") > 0.3f)
        {
            p4Pick++;
            switch (p4Pick)
            {
                case 0:
                    p4Text.text = "Tank";
                    break;
            }

            if (p4Pick > 1)
            {
                p4Pick = 0;
            }
        }

        //Players set ready
        //Ready
        if (Input.GetButtonDown("Fire1") && !player1Picked)
        {
            p1Ready.isOn = true;
            player1Picked = true;
            playersReady++;
        }
        if (Input.GetButtonDown("Fire2") && !player2Picked && player2.activeInHierarchy)
        {
            p2Ready.isOn = true;
            player2Picked = true;
            playersReady++;
        }
        if (Input.GetButtonDown("Fire3") && !player3Picked && player3.activeInHierarchy)
        {
            p3Ready.isOn = true;
            player3Picked = true;
            playersReady++;
        }
        if (Input.GetButtonDown("Fire4") && !player4Picked && player4.activeInHierarchy)
        {
            p4Ready.isOn = true;
            player4Picked = true;
            playersReady++;
        }
        //Unready
        if (Input.GetButtonDown("Brake") && player1Picked)
        {
            p1Ready.isOn = false;
            player1Picked = false;
            playersReady--;
        }
        if (Input.GetButtonDown("Brake2") && player2Picked && player2.activeInHierarchy)
        {
            p2Ready.isOn = false;
            player2Picked = false;
            playersReady--;
        }
        if (Input.GetButtonDown("Brake3") && player3Picked && player3.activeInHierarchy)
        {
            p3Ready.isOn = false;
            player3Picked = false;
            playersReady++;
        }
        if (Input.GetButtonDown("Brake4") && player4Picked && player4.activeInHierarchy)
        {
            p4Ready.isOn = false;
            player4Picked = false;
            playersReady--;
        }


        //Players join
        if (Input.GetButtonDown("Fire2") && !player2.activeInHierarchy)
        {
            PlayerPrefs.SetInt("P2In", 1);
            player2.SetActive(true);
            playersIn++;
        }
        if (Input.GetButtonDown("Fire3") && !player3.activeInHierarchy)
        {
            PlayerPrefs.SetInt("P3In", 1);
            player3.SetActive(true);
            playersIn++;
        }
        if (Input.GetButtonDown("Fire4") && !player4.activeInHierarchy)
        {
            PlayerPrefs.SetInt("P4In", 1);
            player4.SetActive(true);
            playersIn++;
        }



        //Advance selections
        if (playersIn == playersReady)
        {
            nextScrnText.text = "Press Start to Advance";
            if (!allPlayersReady)
            {
                if (Input.GetButtonDown("Pause"))
                {
                    allPlayersReady = true;
                    LevelSelect();
                }
            }
            else
            {
                if (Input.GetAxis("Vertical") > 0 || Input.GetAxis("DpadVert") > 0)
                {
                 //   PlayerPrefs.SetInt("Level", PlayerPrefs.GetInt("Level") + 1);
                    switch (PlayerPrefs.GetInt("Level"))
                    {
                        case 0:
                            levelText.text = "Tilt";
                            break;
                        case 1:
                            levelText.text = "Future";
                            break;
                    }
                }

                if (Input.GetButtonDown("Pause"))
                {
                    SceneManager.LoadScene("Battle Offline");
                }
            }           
        }
        else
        {
            nextScrnText.text = "";
        }

        
        
        // both players select vehicles
        // activate level select
        // set go option
    }

    void LevelSelect()
    {
        vehiclePickScreen.SetActive(false);
        LevelScreen.SetActive(true);
        levelText.text = "Tilt";
    }
}
