using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ButtonListeners : MonoBehaviour {

    public Button go;
    public Button back;

    public Button p1TeamLeft;
    public Button p1TeamRight;

    public Button lvlSelectLeft;
    public Button lvlSelectRight;

    // Use this for initialization
    void Start ()
    {
        go.onClick.AddListener(Go);
        back.onClick.AddListener(Back);
        p1TeamLeft.onClick.AddListener(P1TeamUp);
        p1TeamRight.onClick.AddListener(P1TeamDwn);
        lvlSelectLeft.onClick.AddListener(LvlDwn);
        lvlSelectRight.onClick.AddListener(LvlUp);
    }
	
	// Update is called once per frame
	void Update ()
    {
		
	}

    void Go()
    {
        SceneManager.LoadScene("Battle Offline");
    }

    void Back()
    {
        SceneManager.LoadScene("Menu");
    }

    void LvlUp()
    {

    }

    void LvlDwn()
    {

    }

    void P1TeamUp()
    {

    }

    void P1TeamDwn()
    {

    }
}
