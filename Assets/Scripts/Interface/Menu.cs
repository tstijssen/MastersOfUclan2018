using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Menu : MonoBehaviour {

    public GameObject p1;
    //public GameObject p2;
    //public GameObject p3;
    //public GameObject p4;

    public Button startBtn;
    public Button settingsBtn;
    public Button quitBtn;

    public Button apply;
    public Button cancel;

    public GameObject menuPanel;
    public GameObject settingsPanel;

    AudioSource soundSource;
    public AudioClip moveSelect;
    public AudioClip accept;

    public Image menuPointer;
    public Image settingPointer;

    public int selection = 0;

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
            selection++;
            if (selection > 2)
            {
                selection = 2;
            }
            else
            {
                soundSource.PlayOneShot(moveSelect);
                menuPointer.transform.position = menuPointer.transform.position - selectionOffset;

            }
        }

        //Selector moving down
        if ((Input.GetAxis("DpadVert") < -0.5f || Input.GetAxis("Vertical") < -0.5f) 
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
                menuPointer.transform.position = menuPointer.transform.position + selectionOffset;

            }
        }

        if ((Input.GetAxis("DpadHor") < -0.5f || Input.GetAxis("Horizontal") < -0.5f)
            && !axisInUse)
        {
            selection++;
            if (selection < 0)
            {
                selection = 0;
            }
            else
            {
                soundSource.PlayOneShot(moveSelect);
                menuPointer.transform.position = menuPointer.transform.position + selectionOffset;

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
                menuPointer.transform.position = menuPointer.transform.position - selectionOffset;

            }
        }

        if (menuPanel.activeInHierarchy)
        {

        }

        if (settingsPanel.activeInHierarchy)
        {

        }



        //Check if axis is in use
        if ((Input.GetAxis("DpadVert") > 0.5f || Input.GetAxis("DpadVert") < -0.5f) || (Input.GetAxis("Vertical") > 0.5f || Input.GetAxis("Vertical") < -0.5f))
            axisInUse = true;
        else
            axisInUse = false;
       
        //Selection made
        if (Input.GetButtonDown("Fire1"))
        {
            soundSource.PlayOneShot(accept);
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
    }

    void Selector(Image pointer)
    {
        selection++;
        if (selection > 2)
        {
            selection = 2;
        }
        else
        {
            soundSource.PlayOneShot(moveSelect);
            menuPointer.transform.position = menuPointer.transform.position - selectionOffset;

        }
    }

    void SettingMenuToggle()
    {
        settingsPanel.SetActive(!settingsPanel.activeInHierarchy);
        menuPanel.SetActive(!menuPanel.activeInHierarchy);
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
