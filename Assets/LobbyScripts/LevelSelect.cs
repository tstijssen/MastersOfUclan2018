using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LevelSelect : MonoBehaviour {

    public Text levelName;
    public Text rulesText;

    public Button levelSwitch;

    public Button rulesLeft;
    public Button rulesRight;

    public Image mapImage;
    public Sprite[] mapImages;

    public int levelSelect = 0;
    public int rulesNumber = 1;
    public int NumOfMaps;

    public Vector2[] FFARuleButtonPositions;
    public Vector2[] HATRuleButtonPositions;
    /*
 * FFA
 * 10.3, 9.7
 * 15.6, 10.4
 * 10.3, -12
 * 15.6, -11.3
 * 
 * HAT
 * 54.4, 16.8
 * 60.5, 16.6
 * 54.4, -4.9
 * 60.5, -5.1
 * 
 * */

    // Use this for initialization
    void Start ()
    {
        rulesLeft.onClick.AddListener(DecRules);
        rulesRight.onClick.AddListener(IncRules);
    }
	
	// Update is called once per frame
	void Update ()
    {
        mapImage.sprite = mapImages[levelSelect];
        switch (levelSelect)
        {
            case 0:
                levelName.text = "FFA Castle";
                PlayerPrefs.SetInt("HATScoreLimit", 0);
                PlayerPrefs.SetInt("FFAKillLimit", rulesNumber);

                levelName.transform.parent.GetComponent<Text>().text = "FFA Castle";
                if (SceneManager.GetActiveScene().name == "OnlineLobby")
                {
                    rulesText.text = "First to " + rulesNumber + " deaths";
                    rulesText.transform.parent.GetComponent<Text>().text = "First to " + rulesNumber + " deaths";
                }
                else
                {
                    rulesText.text = "First to " + rulesNumber + " kills";
                    rulesText.transform.parent.GetComponent<Text>().text = "First to " + rulesNumber + " kills";
                }

                //place ffa rule increment buttons to be directly over the number in the ui
                if (rulesNumber > 9)
                {
                    rulesRight.transform.localPosition = FFARuleButtonPositions[1];
                    rulesLeft.transform.localPosition = FFARuleButtonPositions[3];
                }
                else
                {
                    rulesRight.transform.localPosition = FFARuleButtonPositions[0];
                    rulesLeft.transform.localPosition = FFARuleButtonPositions[2];
                }
                break;
            case 1:
                levelName.text = "HAT Castle";
                PlayerPrefs.SetInt("HATScoreLimit", rulesNumber);
                PlayerPrefs.SetInt("FFAKillLimit", 0);

                levelName.transform.parent.GetComponent<Text>().text = "HAT Castle";
                rulesText.text = "Hold Hat for " + rulesNumber + " seconds";
                rulesText.transform.parent.GetComponent<Text>().text = "Hold Hat for " + rulesNumber + " seconds";

                // place hat rule increment buttons to be directly over the number in the ui
                if (rulesNumber > 9)
                {
                    rulesRight.transform.localPosition = HATRuleButtonPositions[1];
                    rulesLeft.transform.localPosition = HATRuleButtonPositions[3];
                }
                else
                {
                    rulesRight.transform.localPosition = HATRuleButtonPositions[0];
                    rulesLeft.transform.localPosition = HATRuleButtonPositions[2];
                }
                break;
            case 2:
                levelName.text = "FFA Arena";
                PlayerPrefs.SetInt("HATScoreLimit", 0);
                PlayerPrefs.SetInt("FFAKillLimit", rulesNumber);

                levelName.transform.parent.GetComponent<Text>().text = "FFA Arena";
                if (SceneManager.GetActiveScene().name == "OnlineLobby")
                {
                    rulesText.text = "First to " + rulesNumber + " deaths";
                    rulesText.transform.parent.GetComponent<Text>().text = "First to " + rulesNumber + " deaths";
                }
                else
                {
                    rulesText.text = "First to " + rulesNumber + " kills";
                    rulesText.transform.parent.GetComponent<Text>().text = "First to " + rulesNumber + " kills";
                }

                //place ffa rule increment buttons to be directly over the number in the ui
                if (rulesNumber > 9)
                {
                    rulesRight.transform.localPosition = FFARuleButtonPositions[1];
                    rulesLeft.transform.localPosition = FFARuleButtonPositions[3];
                }
                else
                {
                    rulesRight.transform.localPosition = FFARuleButtonPositions[0];
                    rulesLeft.transform.localPosition = FFARuleButtonPositions[2];
                }
                break;
            case 3:
                levelName.text = "HAT Arena";
                PlayerPrefs.SetInt("HATScoreLimit", rulesNumber);
                PlayerPrefs.SetInt("FFAKillLimit", 0);

                levelName.transform.parent.GetComponent<Text>().text = "HAT Arena";
                rulesText.text = "Hold Hat for " + rulesNumber + " seconds";
                rulesText.transform.parent.GetComponent<Text>().text = "Hold Hat for " + rulesNumber + " seconds";

                // place hat rule increment buttons to be directly over the number in the ui
                if (rulesNumber > 9)
                {
                    rulesRight.transform.localPosition = HATRuleButtonPositions[1];
                    rulesLeft.transform.localPosition = HATRuleButtonPositions[3];
                }
                else
                {
                    rulesRight.transform.localPosition = HATRuleButtonPositions[0];
                    rulesLeft.transform.localPosition = HATRuleButtonPositions[2];
                }
                break;
            case 4:
                levelName.text = "FFA Tilt";
                PlayerPrefs.SetInt("HATScoreLimit", 0);
                PlayerPrefs.SetInt("FFAKillLimit", rulesNumber);

                levelName.transform.parent.GetComponent<Text>().text = "FFA Tilt";
                if(SceneManager.GetActiveScene().name == "OnlineLobby")
                {
                    rulesText.text = "First to " + rulesNumber + " deaths";
                    rulesText.transform.parent.GetComponent<Text>().text = "First to " + rulesNumber + " deaths";
                }               
                else
                {
                    rulesText.text = "First to " + rulesNumber + " kills";
                    rulesText.transform.parent.GetComponent<Text>().text = "First to " + rulesNumber + " kills";
                }


                //place ffa rule increment buttons to be directly over the number in the ui
                if (rulesNumber > 9)
                {
                    rulesRight.transform.localPosition = FFARuleButtonPositions[1];
                    rulesLeft.transform.localPosition = FFARuleButtonPositions[3];
                }
                else
                {
                    rulesRight.transform.localPosition = FFARuleButtonPositions[0];
                    rulesLeft.transform.localPosition = FFARuleButtonPositions[2];
                }
                break;
            case 5:
                levelName.text = "FFA Beach";
                PlayerPrefs.SetInt("HATScoreLimit", 0);
                PlayerPrefs.SetInt("FFAKillLimit", rulesNumber);

                levelName.transform.parent.GetComponent<Text>().text = "FFA Beach";
                if (SceneManager.GetActiveScene().name == "OnlineLobby")
                {
                    rulesText.text = "First to " + rulesNumber + " deaths";
                    rulesText.transform.parent.GetComponent<Text>().text = "First to " + rulesNumber + " deaths";
                }
                else
                {
                    rulesText.text = "First to " + rulesNumber + " kills";
                    rulesText.transform.parent.GetComponent<Text>().text = "First to " + rulesNumber + " kills";
                }

                //place ffa rule increment buttons to be directly over the number in the ui
                if (rulesNumber > 9)
                {
                    rulesRight.transform.localPosition = FFARuleButtonPositions[1];
                    rulesLeft.transform.localPosition = FFARuleButtonPositions[3];
                }
                else
                {
                    rulesRight.transform.localPosition = FFARuleButtonPositions[0];
                    rulesLeft.transform.localPosition = FFARuleButtonPositions[2];
                }
                break;
        }

        PlayerPrefs.SetInt("Level", levelSelect);
    }

    public void SwitchLevel()
    {
        levelSelect++;
        if (levelSelect > NumOfMaps - 1)
            levelSelect = 0;
    }

    public void IncRules()
    {
        rulesNumber++;
        if(rulesNumber > 99)
        {
            rulesNumber = 1;
        }
    }

    public void DecRules()
    {
        rulesNumber--;
        if (rulesNumber < 1)
        {
            rulesNumber = 99;
        }
    }

}
