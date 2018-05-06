using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ScoreBoardToggle : MonoBehaviour {

    public GameObject scoreboard;
    public GameObject pauseMenu = null;
    bool pauseEnabled = false;
    bool canInteract;

    void Start()
    {
        canInteract = false;
        StartCoroutine(MenuChange());
    }

    // Update is called once per frame
    void Update () {
        
		if(Input.GetButton("ToggleScoreboard"))
        {
            scoreboard.SetActive(true);
        }
        else
        {
            scoreboard.SetActive(false);

        }

        if (pauseMenu && canInteract && Input.GetButton("Cancel"))
        {
            ShowPauseMenu();
            canInteract = false;
            StartCoroutine(MenuChange());
        }

    }

    public void ShowPauseMenu()
    {
        pauseEnabled = !pauseEnabled;
        pauseMenu.SetActive(pauseEnabled);
    }

    public void GoToMenu()
    {
        SceneManager.LoadScene("Menu");
    }

    IEnumerator MenuChange()
    {
        yield return new WaitForSeconds(0.25f);
        canInteract = true;   // After the wait is over, the player can interact with the menu again.
    }
}
