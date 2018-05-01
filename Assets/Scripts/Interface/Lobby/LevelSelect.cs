using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelSelect : MonoBehaviour {



    public Text levelName;
    public Text rulesText;

    public Button levelLeft;
    public Button levelRight;

    public Button rulesLeft;
    public Button rulesRight;

    public int levelSelect = 0;
    public int rulesNumber = 1;
    public int NumOfMaps;
    
    // Use this for initialization
    void Start ()
    {
        levelLeft.onClick.AddListener(DecLvl);
        levelRight.onClick.AddListener(IncLvl);
        rulesLeft.onClick.AddListener(DecRules);
        rulesRight.onClick.AddListener(IncRules);
    }
	
	// Update is called once per frame
	void Update ()
    {
        switch (levelSelect)
        {
            case 0:
                levelName.text = "CTF Bridges";
                rulesText.text = "First team to " + rulesNumber + " captures";
                break;
            case 1:
                levelName.text = "FFA Bridges";
                rulesText.text = "First to " + rulesNumber + " kills";
                break;
            case 2:
                levelName.text = "FFA Tilt";
                rulesText.text = "First to " + rulesNumber + " kills";
                break;
            case 3:
                levelName.text = "HAT Cross";
                rulesText.text = "Hold Hat for " + rulesNumber + " seconds";
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

    public void IncRules()
    {
        rulesNumber++;
        if(rulesNumber > 100)
        {
            rulesNumber = 0;
        }
    }

    public void DecRules()
    {
        rulesNumber--;
        if (rulesNumber < 0)
        {
            rulesNumber = 99;
        }
    }

}
