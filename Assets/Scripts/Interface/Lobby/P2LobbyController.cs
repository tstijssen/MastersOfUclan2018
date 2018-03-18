using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class P2LobbyController : MonoBehaviour {

    Sprite[] spriteArray;
    public Image carImg;
    public Image teamImg;

    public Text p2Car;
    public Text p2Team;

    public Button p2CarLeft;
    public Button p2CarRight;
    int p2CarChoice = 0;

    public Button p2TeamLeft;
    public Button p2TeamRight;
    int p2TeamChoice = 0;


    // Use this for initialization
    void Start ()
    {
        //carImg = GameObject.Find("VehiclePic").GetComponent<Image>();
        //teamImg = GameObject.Find("Team").GetComponent<Image>();
        spriteArray = GameObject.Find("OfflineLobby").GetComponent<LobbyOverlord>().spriteList;
        carImg.sprite = spriteArray[p2CarChoice];
        p2CarLeft.onClick.AddListener(DecCar);
        p2CarRight.onClick.AddListener(IncCar);
        p2TeamLeft.onClick.AddListener(DecTeam);
        p2TeamRight.onClick.AddListener(IncTeam);
    }
	
	// Update is called once per frame
	void Update ()
    {
		switch(p2CarChoice)
        {
            case 0:
                p2Car.text = "Gun Car";
                break;
            case 1:
                p2Car.text = "Train";
                break;
            case 2:
                p2Car.text = "Boat";
                break;
            case 3:
                p2Car.text = "Future Car";
                break;
        }

        switch (p2TeamChoice)
        {
            case 0:
                p2Team.text = "Red";
                teamImg.color = Color.red;
                break;
            case 1:
                p2Team.text = "Blue";
                teamImg.color = Color.blue;
                break;
            case 2:
                p2Team.text = "Yellow";
                teamImg.color = Color.yellow;
                break;
            case 3:
                p2Team.text = "Green";
                teamImg.color = Color.green;
                break;
        }    

        PlayerPrefs.SetInt("P2Car", p2CarChoice);
        PlayerPrefs.SetInt("P2Team", p2TeamChoice);
    }



    void IncCar()
    {
        p2CarChoice++;
        if (p2CarChoice > 3)
            p2CarChoice = 0;

        carImg.sprite = spriteArray[p2CarChoice];
    }

    void DecCar()
    {
        p2CarChoice--;
        if (p2CarChoice < 0)
            p2CarChoice = 3;

        carImg.sprite = spriteArray[p2CarChoice];
    }

    void IncTeam()
    {
        p2TeamChoice++;
        if (p2TeamChoice > 3)
            p2TeamChoice = 0;
    }

    void DecTeam()
    {
        p2TeamChoice--;
        if (p2TeamChoice < 0)
            p2TeamChoice = 3;
    }
}
