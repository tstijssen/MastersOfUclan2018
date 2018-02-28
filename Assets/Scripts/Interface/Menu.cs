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

    public GameObject menuPanel;
    public GameObject settingsPanel;

    AudioSource soundSource;
    public AudioClip moveSelect;
    public AudioClip accept;

    public Image pointer;

    public int selection = 0;

    private Vector3 selectionOffset = new Vector3(0f, 50f, 0f);
    public bool axisInUse = false;
    // Use this for initialization
    void Start ()
    {
        DontDestroyOnLoad(p1);
        soundSource = GetComponent<AudioSource>();
	}
	
	// Update is called once per frame
	void Update ()
    {
        //Check if axis is in use
        if (Input.GetAxis("DpadVert") > 0.5f || Input.GetAxis("DpadVert") < -0.5f)
            axisInUse = true;
        else
            axisInUse = false;

        //Selcetor moving up
        if (Input.GetAxis("DpadVert") > 0.5f && !axisInUse)
        {
            selection++;
            if (selection > 2)
            {
                selection = 2;
            }
            else
            {
                soundSource.PlayOneShot(moveSelect);
                pointer.transform.position = pointer.transform.position - selectionOffset;

            }
        }

        //Selector moving down
        if (Input.GetAxis("DpadVert") < -0.5f && !axisInUse)
        {
            selection--;
            if (selection < 0)
            {
                selection = 0;
            }
            else
            {
                soundSource.PlayOneShot(moveSelect);
                pointer.transform.position = pointer.transform.position + selectionOffset;

            }
        }

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
                    settingsPanel.SetActive(true);
                    menuPanel.SetActive(false);
                    break;
                case 2:
                    Application.Quit();
                    break;
            }
        }

       


    }




}
