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
    int vehicleChoice;
    string playerSelection;
    string fire;

    int selection = 0;

    public Text vehicleText;

    
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
                playerSelection =   "P1Choice";
                break;
            case "Player2":
                moveVertical =      playerSelect.GetComponent<PlayerTwoControl>().moveVertical;
                turning =           playerSelect.GetComponent<PlayerTwoControl>().turning;
                shootHorizontal =   playerSelect.GetComponent<PlayerTwoControl>().shootHorizontal;
                shootVertical =     playerSelect.GetComponent<PlayerTwoControl>().shootVertical;
                fire =              playerSelect.GetComponent<PlayerTwoControl>().fire;
                DpadHor =           playerSelect.GetComponent<PlayerTwoControl>().dPadHor;
                dPadVert =          playerSelect.GetComponent<PlayerTwoControl>().dPadVert;
                playerSelection =   "P2Choice";
                break;
        }





        if (dPadVert > 0.5f && selection == 0)
        {
            vehicleText.text = "Car";
            selection++;
            PlayerPrefs.SetFloat(playerSelection, selection);
        }

        if (dPadVert < -0.5f && selection == 1)
        {
            vehicleText.text = "Tank";
            selection--;
            PlayerPrefs.SetFloat(playerSelection, selection);
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
