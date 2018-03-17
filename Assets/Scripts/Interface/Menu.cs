using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Menu : MonoBehaviour {
    enum Menus {Splash, Main, OfflineLobby, Loading};
    Menus menuUp = Menus.Splash; //Which menu is active

    public float timer;

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
    public Button quit;
    public GameObject menuPanel;

    //Transitions
    float transitionSpd;
    public GameObject shutter;
    public AudioClip shutterNoise;
    public AudioClip shutterNoiseOpen;

    //Lobby Offline
    public Button back;
    public Button launch;
    public GameObject offlineLobby;

    //Loading
    public GameObject loadingGif;
    public Text loadText;
    public Image loadAnim;
    AsyncOperation loadLevel;
    bool loading = false;


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
        quit.onClick.AddListener(QuitGame);

        //Lobby
        back.onClick.AddListener(ToMenu);
        launch.onClick.AddListener(LaunchGame);
	}
	
	// Update is called once per frame
	void Update ()
    {
        switch (menuUp)
        {
            case Menus.Splash:
        timer += Time.deltaTime;
                if(timer > 3f)
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
                    splashTitle.SetActive(false);
                }

                shutter.GetComponent<RectTransform>().Translate((-Vector3.up * transitionSpd) * Time.deltaTime);

                break;
            case Menus.OfflineLobby:
                if (!offlineLobby.activeInHierarchy)
                    offlineLobby.SetActive(true);

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
                }

                shutter.GetComponent<RectTransform>().Translate((-Vector3.up * transitionSpd) * Time.deltaTime);
                



                if (!loading)
                {
                    loadText.text = "Loading";
                    switch (PlayerPrefs.GetInt("Level"))
                    {
                        case 1:
                            StartCoroutine(AsynchronousLoad("Tilt"));

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
    void LaunchOffline()
    {
        transitionSpd = 3000f;
        soundSource.PlayOneShot(accept);
        soundSource.PlayOneShot(shutterNoiseOpen);
        menuUp = Menus.OfflineLobby;
        stateText.text = menuUp.ToString();
    }

    void ToMenu()
    {
        transitionSpd = 3000f;
        soundSource.PlayOneShot(accept);
        soundSource.PlayOneShot(shutterNoise);
        menuUp = Menus.Main;
        stateText.text = menuUp.ToString();
    }

    void LaunchGame()
    {
        transitionSpd = 3000f;
        soundSource.PlayOneShot(accept);
        soundSource.PlayOneShot(shutterNoise);
        menuUp = Menus.Loading;
        stateText.text = menuUp.ToString();
    }

    void QuitGame()
    {
        soundSource.PlayOneShot(accept);
        Application.Quit();
    }

}