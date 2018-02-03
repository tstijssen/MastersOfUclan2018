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

    public Image pointer;

    public int selection = 0;

    private Vector3 selectionOffset = new Vector3(0f, 50f, 0f);

    // Use this for initialization
    void Start ()
    {
        DontDestroyOnLoad(p1);

	}
	
	// Update is called once per frame
	void Update ()
    {
		if(Input.GetAxis("DpadVert") > 0.5f && selection == 0)
        {
            pointer.transform.position = pointer.transform.position - selectionOffset;
            selection++;
        }

        if (Input.GetAxis("DpadVert") < -0.5f && selection == 1)
        {
            pointer.transform.position = pointer.transform.position + selectionOffset;
            selection--;
        }

        if (Input.GetButtonDown("Fire1"))
        {
            switch (selection)
            {
                case 0:
                    SceneManager.LoadScene("Lobby Offline");
                    break;
                case 1:
                    Application.Quit();
                    break;
            }
        }
        
    }
}
