using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class P1LobbyController : MonoBehaviour {

    Sprite[] spriteArray;
    public Image carImg;
    public Image teamImg;

    public Text p1Car;
    public Text p1Team;

    public Button p1CarLeft;
    public Button p1CarRight;
    int p1CarChoice = 0;

    public Button p1TeamLeft;
    public Button p1TeamRight;
    int p1TeamChoice = 0;


    // Use this for initialization
    void Start ()
    {
        //carImg = GameObject.Find("VehiclePic").GetComponent<Image>();
        //teamImg = GameObject.Find("Team").GetComponent<Image>();
        spriteArray = GameObject.Find("OfflineLobby").GetComponent<LobbyOverlord>().spriteList;
        carImg.sprite = spriteArray[p1CarChoice];
        p1CarLeft.onClick.AddListener(DecCar);
        p1CarRight.onClick.AddListener(IncCar);
        p1TeamLeft.onClick.AddListener(DecTeam);
        p1TeamRight.onClick.AddListener(IncTeam);
    }
	
	// Update is called once per frame
	void Update ()
    {
		switch(p1CarChoice)
        {
            case 0:
                p1Car.text = "Gun Car";
                break;
            case 1:
                p1Car.text = "Train";
                break;
            case 2:
                p1Car.text = "Boat";
                break;
            case 3:
                p1Car.text = "Future Car";
                break;
        }

        switch (p1TeamChoice)
        {
            case 0:
                p1Team.text = "Red";
                teamImg.color = Color.red;
                break;
            case 1:
                p1Team.text = "Blue";
                teamImg.color = Color.blue;
                break;
            case 2:
                p1Team.text = "Yellow";
                teamImg.color = Color.yellow;
                break;
            case 3:
                p1Team.text = "Green";
                teamImg.color = Color.green;
                break;
        }

        

        PlayerPrefs.SetInt("P1Car", p1CarChoice);
        PlayerPrefs.SetInt("P1Team", p1TeamChoice);
    }



    void IncCar()
    {
        p1CarChoice++;
        if (p1CarChoice > 3)
            p1CarChoice = 0;

        carImg.sprite = spriteArray[p1CarChoice];
    }

    void DecCar()
    {
        p1CarChoice--;
        if (p1CarChoice < 0)
            p1CarChoice = 3;

        carImg.sprite = spriteArray[p1CarChoice];
    }

    void IncTeam()
    {
        p1TeamChoice++;
        if (p1TeamChoice > 3)
            p1TeamChoice = 0;
    }

    void DecTeam()
    {
        p1TeamChoice--;
        if (p1TeamChoice < 0)
            p1TeamChoice = 3;
    }
}
