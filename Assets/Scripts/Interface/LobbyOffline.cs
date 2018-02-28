using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

[System.Serializable]
public struct PlayerColourInfo
{
//    public Text vehicleText;
    public Image vehicleColour;
    public Color playerColour;
}


public class LobbyOffline : MonoBehaviour {

    //Player Variables
    static Color[] Colors = new Color[] { Color.magenta, Color.red, Color.cyan, Color.blue, Color.green, Color.yellow };
    static List<int> _colorInUse = new List<int>();
    public PlayerColourInfo[] colourInfos = new PlayerColourInfo[4];

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
    int levelSelect = 0;

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

        PlayerPrefs.SetInt("Level", 1);

        PlayerPrefs.SetInt("P2In", 0);
        PlayerPrefs.SetInt("P3In", 0);
        PlayerPrefs.SetInt("P4In", 0);

        PlayerPrefs.SetInt("GameLives", 3);

        playersIn = 1;
    }

    // Update is called once per frame
    void Update()
    {



        //Player select colour
        if (Input.GetButtonDown("Brake") && !player1Picked)
        {
            PlayerPrefs.SetInt("P1Colour", ChangeColour(0));
        }
        if (Input.GetButtonDown("Brake2") && !player2Picked)
        {
            PlayerPrefs.SetInt("P2Colour", ChangeColour(1));
        }
        if (Input.GetButtonDown("Brake3") && !player3Picked)
        {
            PlayerPrefs.SetInt("P3Colour", ChangeColour(2));
        }
        if (Input.GetButtonDown("Brake4") && !player4Picked)
        {
            PlayerPrefs.SetInt("P4Colour", ChangeColour(3));
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
            LevelScreen.SetActive(true);

            levelText.text = "Tilt";
            allPlayersReady = false;       
         
            if (Input.GetAxis("DpadHor") > 0 && levelSelect < 1)
            {
                levelSelect++;
            }
            else if (Input.GetAxis("DpadHor") < 0 && levelSelect > 0)
            {
                levelSelect--;
            }
                
            switch (levelSelect)
            {
                case 0:
                    levelText.text = "Tilt";
                    break;
                case 1:
                    levelText.text = "Beach";
                    break;
            }              

            if (Input.GetButtonDown("Pause"))
            {
                PlayerPrefs.SetInt("Level", levelSelect);  
                SceneManager.LoadScene("Battle Offline");
            }                   
        }
        else
        {
            LevelScreen.SetActive(false);
            nextScrnText.text = "";
        }

        
        
        // both players select vehicles
        // activate level select
        // set go option
    }

    void LevelSelect()
    {
        //vehiclePickScreen.SetActive(false);
        LevelScreen.SetActive(true);
        levelText.text = "Tilt";
    }

    int ChangeColour(int playerNum)
    {
        int idx = System.Array.IndexOf(Colors, colourInfos[playerNum].playerColour);

        int inUseIdx = _colorInUse.IndexOf(idx);

        if (idx < 0) idx = 0;

        idx = (idx + 1) % Colors.Length;

        bool alreadyInUse = false;

        do
        {
            alreadyInUse = false;
            for (int i = 0; i < _colorInUse.Count; ++i)
            {
                if (_colorInUse[i] == idx)
                {//that color is already in use
                    alreadyInUse = true;
                    idx = (idx + 1) % Colors.Length;
                }
            }
        }
        while (alreadyInUse);

        if (inUseIdx >= 0)
        {//if we already add an entry in the colorTabs, we change it
            _colorInUse[inUseIdx] = idx;
        }
        else
        {//else we add it
            _colorInUse.Add(idx);
        }

        colourInfos[playerNum].playerColour = Colors[idx];
        Debug.Log(colourInfos[playerNum].playerColour.ToString());
        colourInfos[playerNum].vehicleColour.color = colourInfos[playerNum].playerColour;
        return idx;
    }
}

