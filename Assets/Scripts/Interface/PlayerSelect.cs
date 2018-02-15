using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayerSelect : MonoBehaviour {

    public GameObject playerSelect;
    float moveVertical;
    float turning;
    float shootHorizontal;
    float shootVertical;
    float DpadHor;
    float dPadVert;
    string colorChange;
    int vehicleChoice;
    string fire;

    int selection = 0;

    static Color[] Colors = new Color[] { Color.magenta, Color.red, Color.cyan, Color.blue, Color.green, Color.yellow };
    static List<int> _colorInUse = new List<int>();


    public Text vehicleText;
    public Image vehicleColour;
    public Color playerColour;
    public string m_PlayerName;

    // Use this for initialization
    void Start ()
    {
        m_PlayerName = transform.parent.name;
        playerSelect = gameObject.transform.parent.gameObject;
    }
	
	// Update is called once per frame
	void Update ()
    {
        //Get Controller
        switch (m_PlayerName)
        {
            case "Player1":               
                moveVertical =      playerSelect.GetComponent<PlayerOneControl>().moveVertical;
                turning =           playerSelect.GetComponent<PlayerOneControl>().turning;
                shootHorizontal =   playerSelect.GetComponent<PlayerOneControl>().shootHorizontal;
                shootVertical =     playerSelect.GetComponent<PlayerOneControl>().shootVertical;
                fire =              playerSelect.GetComponent<PlayerOneControl>().fire;
                DpadHor =           playerSelect.GetComponent<PlayerOneControl>().dPadHor;
                dPadVert =          playerSelect.GetComponent<PlayerOneControl>().dPadVert;
                vehicleChoice =     playerSelect.GetComponent<PlayerOneControl>().vehicle;
                colorChange =       playerSelect.GetComponent<PlayerOneControl>().brake;

                break;
            case "Player2":
                //moveVertical = transform.parent.GetComponent<PlayerTwoControl>().moveVertical;
                //turning = transform.parent.GetComponent<PlayerTwoControl>().turning;
                //shootHorizontal = transform.parent.GetComponent<PlayerTwoControl>().shootHorizontal;
                //shootVertical = transform.parent.GetComponent<PlayerTwoControl>().shootVertical;
                //fire = transform.parent.GetComponent<PlayerTwoControl>().fire;
                //DpadHor = transform.parent.GetComponent<PlayerTwoControl>().dPadHor;
                //dPadVert = transform.parent.GetComponent<PlayerTwoControl>().dPadVert;
                break;
        }

        if (dPadVert > 0.5f && selection == 0)
        {
            vehicleText.text = "Car";
            selection++;
            PlayerPrefs.SetFloat("P1Choice", selection);
            //playerSelect.GetComponent<PlayerOneControl>().vehicle = selection;
        }

        if (dPadVert < -0.5f && selection == 1)
        {
            vehicleText.text = "Tank";
            selection--;
            PlayerPrefs.SetFloat("P1Choice", selection);
        }

        if (Input.GetButtonDown("Brake"))
        {
            ChangeColour();
            //SceneManager.LoadScene("Menu");
        }
        if (Input.GetButtonDown("Fire1"))
        {
            SceneManager.LoadScene("Battle Offline");
        }
    }

    void ChangeColour()
    {
        int idx = System.Array.IndexOf(Colors, playerColour);

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

        playerColour = Colors[idx];
        Debug.Log(playerColour.ToString());
        PlayerPrefs.SetInt("P1Colour", idx);
        vehicleColour.color = playerColour;
    }
}
