using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using XInputDotNetPure;

public class Menu : MonoBehaviour {
    enum Menus {Splash, Main, OfflineLobby, LevelSelect, OnlineLobby, Loading};
    Menus menuUp = Menus.Splash; //Which menu is active

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

    //Lobby Online
    public Button onlineBack;
    public GameObject onlineLobby;

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

        //Lobby
        back.onClick.AddListener(ToMenu);
        launchGame.onClick.AddListener(LaunchGame);
        levelSelectBtn.onClick.AddListener(LevelSelect);

        onlineBack.onClick.AddListener(ToMenu);
	}
	
	// Update is called once per frame
	void Update ()
    {
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
                if (!menuPanel.activeInHierarchy)
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

                break;      
            case Menus.LevelSelect:

                shutter.GetComponent<RectTransform>().Translate((-Vector3.up * transitionSpd) * Time.deltaTime);

                if (shutter.GetComponent<RectTransform>().position.y < Screen.height / 1.6)
                {
                    transitionSpd = 0f;
                    shutter.GetComponent<RectTransform>().localPosition = new Vector3(0f, 0f, 0f);


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
                

                if (shutter.GetComponent<RectTransform>().position.y > Screen.height * 2)
                    transitionSpd = 0f;


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
                            StartCoroutine(AsynchronousLoad("Arena"));
                            break;
                        case 1:
                            StartCoroutine(AsynchronousLoad("Castle"));
                            break;
                        case 2:
                            StartCoroutine(AsynchronousLoad("Beach"));
                            break;
                    }
                }
                break;
        }
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
            if (ao.progress == 0.9f)
            {
                Debug.Log("Press Q key to start");
                loadText.text = "Press Q key to start";
                if (Input.GetKey(KeyCode.Q))
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

}