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
                vehicleChoice =     playerSelect.GetComponent<PlayerOneControl>().vehicle;

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
            SceneManager.LoadScene("Menu");
        }
        if (Input.GetButtonDown("Fire1"))
        {
            SceneManager.LoadScene("Battle Offline");
        }
    }
}
