using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using XInputDotNetPure;

public class Menu : MonoBehaviour {
    public enum Menus {Splash, Main, OfflineLobby, LevelSelect, OnlineLobby, Loading};
    public Menus menuUp = Menus.Splash; //Which menu is active

    public float timer;
    GamePadState gamePad;

    //DEV
    public Text stateText;

    //Splash
    GameObject splashTitle;

    //Sound
    AudioSource soundSource;
    public AudioClip moveSelect;
    public AudioClip accept;

    //Main Menu
    public Button offline;
    public Button online;
    public Button quit;
    public GameObject menuPanel;

    //Transitions
    float transitionSpd;
    public GameObject shutter;
    public AudioClip shutterNoise;
    public AudioClip shutterNoiseOpen;

    //Lobby Offline
    public Button back;
    public Button levelSelectBtn;
    public GameObject offlineLobbyUI;
    public GameObject offlineLobby;
    public GameObject getPlayersReady;
    bool players;
    bool canInteract = false;   //whether the controller inputs are used this frame

    //Lobby Online
    public Button onlineBack;
    public GameObject onlineLobby;

    //Options
    public GameObject optionsUI;
    public Button optionsBtn;
    public bool optionsMenu = false;

    //Loading
    public GameObject loadingGif;
    public Text loadText;
    public Image loadAnim;
    AsyncOperation loadLevel;
    bool loading = false;

    //Level Select
    public GameObject levelSelect;
    public GameObject levelSelectUI;
    public Button launchGame;

    // Use this for initialization
    void Start ()
    {
        canInteract = false;
        StartCoroutine(MenuChange());

        //DEV
        stateText.text = menuUp.ToString();

        //Splash
        splashTitle = GameObject.Find("DevSplash");

        //Timing
        timer = 0f;

        //Audio
        soundSource = GetComponent<AudioSource>();

        //Main Menu
        offline.onClick.AddListener(LaunchOffline);
        online.onClick.AddListener(LaunchOnline);
        quit.onClick.AddListener(QuitGame);
        optionsBtn.onClick.AddListener(LoadOptions);

        //Lobby
        back.onClick.AddListener(ToMenu);
        launchGame.onClick.AddListener(LaunchGame);
        levelSelectBtn.onClick.AddListener(LevelSelect);
        onlineBack.onClick.AddListener(ToMenu);
        
	}
	
	// Update is called once per frame
	void Update ()
    {
        gamePad = GetComponent<MenuControllerDetect>().state[0];
        players = getPlayersReady.GetComponent<PlatformActivator>().allReady;
        optionsUI.SetActive(optionsMenu);
        menuPanel.SetActive(!optionsMenu);
        

        switch (menuUp)
        {
            case Menus.Splash:
                timer += Time.deltaTime;
                if(timer > 1f)
                {
                    transitionSpd = 3000f;
                    menuUp = Menus.Main;
                    stateText.text = menuUp.ToString();
                    soundSource.PlayOneShot(shutterNoise);
                }
                break;
            case Menus.Main:
                if (!menuPanel.activeInHierarchy && !optionsMenu)
                    menuPanel.SetActive(true);             

                if (shutter.GetComponent<RectTransform>().position.y < Screen.height /1.6)
                {
                    transitionSpd = 0f;
                    shutter.GetComponent<RectTransform>().localPosition = new Vector3(0f, 0f, 0f);

                    if(splashTitle.activeInHierarchy)
                        splashTitle.SetActive(false);

                    if (offlineLobby.activeInHierarchy)
                        offlineLobby.SetActive(false);

                    if (onlineLobby.activeInHierarchy)
                        onlineLobby.SetActive(false);
                }

                shutter.GetComponent<RectTransform>().Translate((-Vector3.up * transitionSpd) * Time.deltaTime);
                break;
            case Menus.OfflineLobby:
                if (!offlineLobby.activeInHierarchy)
                   offlineLobby.SetActive(true);

                if (!offlineLobbyUI.activeInHierarchy)
                    offlineLobbyUI.SetActive(true);

                if (shutter.GetComponent<RectTransform>().position.y > Screen.height * 2)
                    transitionSpd = 0f;

                shutter.GetComponent<RectTransform>().Translate((Vector3.up * transitionSpd) * Time.deltaTime);

                if (menuPanel.activeInHierarchy)
                    menuPanel.SetActive(false);

                if (players)
                {
                    canInteract = getPlayersReady.GetComponent<PlatformActivator>().Platforms[0].GetComponent<PlatfomOptions>().canInteract;
                    if (canInteract && gamePad.Buttons.Y == ButtonState.Pressed)
                    {
                        LevelSelect();
                        Debug.Log("Start");
                        canInteract = false;
                        StartCoroutine(MenuChange());
                    }
                    Debug.Log("players ready");
                }

                break;      
            case Menus.LevelSelect:

                shutter.GetComponent<RectTransform>().Translate((-Vector3.up * transitionSpd) * Time.deltaTime);

                if (shutter.GetComponent<RectTransform>().position.y < Screen.height / 1.6)
                {
                    //transitionSpd = 0f;

                    Debug.Log("Shutter down");

                    if (offlineLobby.activeInHierarchy)
                        offlineLobby.SetActive(false);

                    if (offlineLobbyUI.activeInHierarchy)
                        offlineLobbyUI.SetActive(false);

                    if (!levelSelect.activeInHierarchy)
                        levelSelect.SetActive(true);

                    if (!levelSelectUI.activeInHierarchy)
                        levelSelectUI.SetActive(true);

                    transitionSpd = -3000f;
                }

                Debug.LogError(transitionSpd);
                if (shutter.GetComponent<RectTransform>().position.y > Screen.height * 2)
                {
                    transitionSpd = 0f;
                    Debug.Log("ShutterIfReached!!");
                }
   
                if (players)
                {
                    if (canInteract && gamePad.Buttons.Y == ButtonState.Pressed)
                    {
                        LaunchGame();
                        Debug.Log("Start");
                    }
                    Debug.Log("players ready");
                }


                break;
            case Menus.OnlineLobby:
                if (!onlineLobby.activeInHierarchy)
                    onlineLobby.SetActive(true);

                if (shutter.GetComponent<RectTransform>().position.y > Screen.height * 2)
                    transitionSpd = 0f;

                shutter.GetComponent<RectTransform>().Translate((Vector3.up * transitionSpd) * Time.deltaTime);
                break;
            case Menus.Loading:

                if (menuPanel.activeInHierarchy)
                    menuPanel.SetActive(false);

                if (!loadingGif.activeInHierarchy)
                    loadingGif.SetActive(true);

                if (shutter.GetComponent<RectTransform>().position.y < Screen.height / 1.6)
                {

                    transitionSpd = 0f;
                    shutter.GetComponent<RectTransform>().localPosition = new Vector3(0f, 0f, 0f);
                    if (levelSelect.activeInHierarchy)
                        offlineLobby.SetActive(false);

                    if (levelSelectUI.activeInHierarchy)
                        levelSelectUI.SetActive(false);
                }

                shutter.GetComponent<RectTransform>().Translate((-Vector3.up * transitionSpd) * Time.deltaTime);               

                if (!loading)
                {
                    loadText.text = "Loading";
                    switch (PlayerPrefs.GetInt("Level"))
                    {
                        case 0:
                            StartCoroutine(AsynchronousLoad("FFA-Bridges"));
                            break;
                        case 1:
                            StartCoroutine(AsynchronousLoad("FFA-Tilt"));
                            break;
                    }
                }
                break;
        }
    }

    IEnumerator MenuChange()
    {
        Debug.Log("Delaying");
        yield return new WaitForSeconds(0.25f);
        canInteract = true;   // After the wait is over, the player can interact with the menu again.
    }

    IEnumerator AsynchronousLoad(string scene)
    {
        loading = true;
        yield return null;

        AsyncOperation ao = SceneManager.LoadSceneAsync(scene);
        ao.allowSceneActivation = false;

        while (!ao.isDone)
        {
            // [0, 0.9] > [0, 1]
            float progress = Mathf.Clamp01(ao.progress / 0.9f);
            Debug.Log("Loading progress: " + (progress * 100) + "%");

            // Loading completed
            if (ao.progress >= 0.9f)
            {
                Debug.Log("Loading Completed");
                    ao.allowSceneActivation = true;
            }

            yield return null;
        }
    }


    //Button Listeners
    public void LaunchOffline()
    {
        transitionSpd = 3000f;
        soundSource.PlayOneShot(accept);
        soundSource.PlayOneShot(shutterNoiseOpen);
        menuUp = Menus.OfflineLobby;
        stateText.text = menuUp.ToString();
    }

	public void LaunchOnline()
    {
        transitionSpd = 3000f;
        soundSource.PlayOneShot(accept);
        soundSource.PlayOneShot(shutterNoiseOpen);
        menuUp = Menus.OnlineLobby;
        stateText.text = menuUp.ToString();
    }

	public void ToMenu()
    {
        transitionSpd = 3000f;
        soundSource.PlayOneShot(accept);
        soundSource.PlayOneShot(shutterNoise);
        menuUp = Menus.Main;
        stateText.text = menuUp.ToString();
    }

	public void LaunchGame()
    {

            transitionSpd = 3000f;
            soundSource.PlayOneShot(accept);
            soundSource.PlayOneShot(shutterNoise);
            menuUp = Menus.Loading;
            stateText.text = menuUp.ToString();
        
    }


    public void LevelSelect()
    {

            transitionSpd = 3000f;
            soundSource.PlayOneShot(accept);
            soundSource.PlayOneShot(shutterNoise);
            menuUp = Menus.LevelSelect;
            stateText.text = menuUp.ToString();
        
    }

	public void QuitGame()
    {
        soundSource.PlayOneShot(accept);
        Application.Quit();
    }

    public void LoadOptions()
    {
        optionsMenu = true;
    }

}