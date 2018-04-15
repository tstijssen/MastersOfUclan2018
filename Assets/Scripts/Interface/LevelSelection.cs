using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelSelection : MonoBehaviour {

    public Button left;
    public Button right;
    public Text levelText;
    public string[] levels;

    int levelSelect;

	// Use this for initialization
	void Start ()
    {
        left.onClick.AddListener(DecChoice);
        right.onClick.AddListener(IncChoice);
    }

    private void OnEnable()
    {
        levelSelect = 0;
    }

    // Update is called once per frame
    void Update ()
    {
        levelText.text = levels[levelSelect];

        PlayerPrefs.SetInt("Level", levelSelect);


    }

    void IncChoice()
    {

            levelSelect++;

            if (levelSelect > 2)
                levelSelect = 0;

        
    }

    void DecChoice()
    {
        levelSelect--;
    
        if (levelSelect < 0)
            levelSelect = 2;
    }
}

