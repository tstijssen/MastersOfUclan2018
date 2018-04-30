using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelSelect : MonoBehaviour {



    public Text levelName;

    public Button levelLeft;
    public Button levelRight;
    public int levelSelect = 0;
    public int NumOfMaps;
    
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
            case 3:
                levelName.text = "HAT Cross";
                break;
        }

        PlayerPrefs.SetInt("Level", levelSelect);
    }


    public void IncLvl()
    {
        levelSelect++;
        if (levelSelect > NumOfMaps - 1)
            levelSelect = 0;
    }

    public void DecLvl()
    {
        levelSelect--;
        if (levelSelect < 0)
            levelSelect = NumOfMaps - 1;
    }



}
