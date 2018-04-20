using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using XInputDotNetPure;

public class LevelSelection : MonoBehaviour {

    public Button left;
    public Button right;
    public Text levelText;
    public string[] levels;
    public GameObject[] Platforms;
    public GamePadState[] gamePadStates;
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
        gamePadStates = new GamePadState[Platforms.Length];
    }

    // Update is called once per frame
    void Update ()
    {
        levelText.text = levels[levelSelect];
        PlayerPrefs.SetInt("Level", levelSelect);

        for (int i = 0; i < Platforms.Length; ++i)
        {
            gamePadStates[i] = GameObject.Find("MenuControl").GetComponent<MenuControllerDetect>().state[i];
            if (gamePadStates[i].Buttons.B == ButtonState.Pressed)
            {
                
                Debug.Log("Start");
            }
        }

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

