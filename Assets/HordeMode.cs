using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class HordeMode : MonoBehaviour {

    public GameObject pauseMenu;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void ResumeGame()
    {
        pauseMenu.SetActive(!pauseMenu.activeInHierarchy);
        if (pauseMenu.activeInHierarchy)
            Time.timeScale = 0f;
        else
            Time.timeScale = 1f;
    }

    public void Restart()
    {
        SceneManager.LoadScene("Horde");
    }

    public void ToLobby()
    {
        SceneManager.LoadScene("Menu");
    }

}
