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
    //public Button[][] menuButtons = new Button[3][3];
    public SelectorMove selector;

    //Transitions
    float transitionSpd;
    public GameObject shutter;
    public AudioClip shutterNoise;
    public AudioClip shutterNoiseOpen;

    public GameObject offlineLobby;
    public GameObject getPlayersReady;
    public bool canInteract = false;   //whether the controller inputs are used this frame

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
    bool pastSplashScreen = false;

    // Use this for initialization
    void Start ()
    {
        canInteract = false;
        StartCoroutine(MenuChange());
        //DontDestroyOnLoad(gameObject);

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
	}
	
	// Update is called once per frame
	void Update ()
    {
  
        gamePad = GetComponent<MenuControllerDetect>().state[0];

        optionsUI.SetActive(optionsMenu);
       
        
        switch (menuUp)
        {
            case Menus.Splash:
                timer += Time.deltaTime;
                if(timer > 0f)
                {
                    transitionSpd = 3000f;
                    menuUp = Menus.Main;
                    stateText.text = menuUp.ToString();
                    soundSource.PlayOneShot(shutterNoise);
                }
                break;
            case Menus.Main:
                if (!menuPanel.activeInHierarchy && !optionsMenu)
                {
                    menuPanel.SetActive(true);
                    selector.StartWait();
                }
                else if (optionsMenu)
                    menuPanel.SetActive(false);

                if (shutter.GetComponent<RectTransform>().position.y < Screen.height /1.6)
                {
                    transitionSpd = 0f;
                    shutter.GetComponent<RectTransform>().localPosition = new Vector3(0f, 0f, 0f);

                    if(splashTitle.activeInHierarchy)
                    if(!pastSplashScreen)
                    {
                        StartCoroutine(MenuChange());
                        pastSplashScreen = true;
                    }

                    if (splashTitle.activeInHierarchy)
                        splashTitle.SetActive(false);

                    if (offlineLobby.activeInHierarchy)
                        offlineLobby.SetActive(false);

                    //if (onlineLobby.activeInHierarchy)
                    //    onlineLobby.SetActive(false);
                }

                shutter.GetComponent<RectTransform>().Translate((-Vector3.up * transitionSpd) * Time.deltaTime);
                break;
            case Menus.OfflineLobby:
                if (!offlineLobby.activeInHierarchy)
                   offlineLobby.SetActive(true);

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
                    //transitionSpd = 0f;

                    Debug.Log("Shutter down");

                    if (offlineLobby.activeInHierarchy)
                        offlineLobby.SetActive(false);

                    transitionSpd = -3000f;
                }

                Debug.LogError(transitionSpd);
                if (shutter.GetComponent<RectTransform>().position.y > Screen.height * 2)
                {
                    //transitionSpd = 0f;
                    //Debug.Log("ShutterIfReached!!");
                }
 
                break;
            //case Menus.OnlineLobby:
            //    if (!onlineLobby.activeInHierarchy)
            //        onlineLobby.SetActive(true);

            //    if (shutter.GetComponent<RectTransform>().position.y > Screen.height * 2)
            //        transitionSpd = 0f;

            //    shutter.GetComponent<RectTransform>().Translate((Vector3.up * transitionSpd) * Time.deltaTime);
            //    break;
            case Menus.Loading:

                if (menuPanel.activeInHierarchy)
                    menuPanel.SetActive(false);

                if (!loadingGif.activeInHierarchy)
                    loadingGif.SetActive(true);

                if (shutter.GetComponent<RectTransform>().position.y < Screen.height / 1.6)
                {

                    transitionSpd = 0f;
                    shutter.GetComponent<RectTransform>().localPosition = new Vector3(0f, 0f, 0f);
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
        yield return new WaitForSeconds(0.25f);
        canInteract = true;   // After the wait is over, the player can interact with the menu again.
        selector.canInteract = true;
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
        SceneManager.LoadScene("OnlineLobby");
    }

	public void ToMenu()
    {
        Debug.Log("Going to menu!");
        selector.canInteract = false;
        transitionSpd = 3000f;
        soundSource.PlayOneShot(accept);
        soundSource.PlayOneShot(shutterNoise);
        menuUp = Menus.Main;
        stateText.text = menuUp.ToString();
        canInteract = false;
        StartCoroutine(MenuChange());
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