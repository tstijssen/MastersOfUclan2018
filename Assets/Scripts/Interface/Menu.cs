using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Menu : MonoBehaviour {

    enum MenuState { Main, Setting, SubSetting}

    public GameObject p1;

    public Button startBtn;
    public Button settingsBtn;
    public Button quitBtn;

    public Button apply;
    public Button cancel;

    public GameObject settingsPanel;
    public GameObject menuPanel;

    Vector3 pointerHome;

    AudioSource soundSource;
    public AudioClip moveSelect;
    public AudioClip accept;

    public Image menuPointer;
    public Image settingPointer;

    public int selection = 0;



    bool video = false;
    bool audioSet = false;
    bool keys = false;

    private Vector3 selectionOffset = new Vector3(0f, 50f, 0f);
    public bool axisInUse = false;
    // Use this for initialization
    void Start ()
    {
        //Add Button listeners
        startBtn.onClick.AddListener(StartGame);
        settingsBtn.onClick.AddListener(Settings);
        quitBtn.onClick.AddListener(QuitGame);
        apply.onClick.AddListener(AcceptSettings);
        cancel.onClick.AddListener(CancelSettings);

        pointerHome = menuPointer.transform.position;

        DontDestroyOnLoad(p1);
        soundSource = GetComponent<AudioSource>();

	}
	
	// Update is called once per frame
	void Update ()
    {

        //Selcetor moving up
        if ((Input.GetAxis("DpadVert") > 0.5f || Input.GetAxis("Vertical") > 0.5f)
            && !axisInUse)
        {
            if (menuPanel.activeInHierarchy)
            {
                selection++;
                if (selection > 2)
                {
                    selection = 2;
                }
                else
                {
                    soundSource.PlayOneShot(moveSelect);

                }
            }
            else if (settingsPanel.activeInHierarchy)
            {
                if(!audioSet && !video && !keys)
                {
                    selection++;
                    if (selection > 2)
                    {
                        selection = 2;
                    }
                }

                if (audioSet)
                {

                }
                
            }
        }

        if ((Input.GetAxis("DpadVert") > 0.5f || Input.GetAxis("Vertical") > 0.5f)
            && !axisInUse)
        {
            selection--;
            if (selection < 0)
            {
                selection = 0;
            }
            else
            {
                soundSource.PlayOneShot(moveSelect);


            }
        }

        switch (selection)
        {
            case 0:
                menuPointer.transform.position = pointerHome;
                
                break;
            case 1:
                menuPointer.transform.position = pointerHome - selectionOffset;
                break;
            case 2:
                menuPointer.transform.position = pointerHome - selectionOffset;
                break;
        }
        
       
        //Selection made
        if (Input.GetButtonDown("Fire1"))
        {
           soundSource.PlayOneShot(accept);
           if (menuPanel.activeInHierarchy)
           {
               switch (selection)
               {
                   case 0:
                       SceneManager.LoadScene("Lobby Offline");
                       break;
                   case 1:
                       //load Settings
                       SettingMenuToggle();
                       break;
                   case 2:
                       Application.Quit();
                       break;
               }
           }
           else if (settingsPanel.activeInHierarchy)
           {
                switch (selection)
                {
                    case 0:
                        audioSet = true;
                        break;
                    case 1:
                        keys = true;
                        break;
                    case 2:
                        video = true;
                        break;
                }
            }
        }

        //Check if axis is in use
        if ((Input.GetAxis("DpadVert") > 0.5f || Input.GetAxis("DpadVert") < -0.5f) || (Input.GetAxis("Vertical") > 0.5f || Input.GetAxis("Vertical") < -0.5f))
            axisInUse = true;
        else
            axisInUse = false;
    }

    void SettingMenuToggle()
    {
        selection = 0;
        settingsPanel.SetActive(!settingsPanel.activeInHierarchy);
        //menuPanel.SetActive(!menuPanel.activeInHierarchy);
    }

    //Button Listeners
    void StartGame()
    {
        SceneManager.LoadScene("Lobby Offline");
    }

    void QuitGame()
    {
        Application.Quit();
    }

    void Settings()
    {
        SettingMenuToggle();
    }

    void AcceptSettings()
    {
        //Set playerPrefs for volume settings

        SettingMenuToggle();
    }

    void CancelSettings()
    {
        SettingMenuToggle();
    }
}
