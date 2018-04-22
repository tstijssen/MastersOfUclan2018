using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelSelect : MonoBehaviour {



    public Text levelName;

    public Button levelLeft;
    public Button levelRight;
    int levelSelect = 0;

    // Use this for initialization
    void Start ()
    {
        levelLeft.onClick.AddListener(DecLvl);
        levelRight.onClick.AddListener(IncLvl);

    }
	
	// Update is called once per frame
	void Update ()
    {
        switch (levelSelect)
        {
            case 0:
                levelName.text = "CTF Bridges";
                break;
            case 1:
                levelName.text = "FFA Bridges";
                break;
            case 2:
                levelName.text = "FFA Tilt";
                break;
        }

        PlayerPrefs.SetInt("Level", levelSelect);
    }


    void IncLvl()
    {
        levelSelect++;
        if (levelSelect > 2)
            levelSelect = 0;
    }

    void DecLvl()
    {
        levelSelect--;
        if (levelSelect < 0)
            levelSelect = 2;
    }



}
