using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Options : MonoBehaviour {

    public Button fullScrBtn;
    public Button resBtn;

    Resolution[] resolutions;
    public Dropdown dropdownMenu;

    public Slider vol;

    public Text resText;
    public Toggle fullScrTogg;

    bool fullScr = false;
    float volDisplay;
    public Text volText;

	// Use this for initialization
	void Start ()
    {
        vol.minValue = 0f;
        vol.maxValue = 1f;
        vol.value = 1f;
        fullScr = Screen.fullScreen;
        volText.text = volDisplay.ToString("F0");

        resolutions = Screen.resolutions;

        //Screen.SetResolution(1920, 1080, true);
        Debug.Log(resolutions.Length);

        for (int i = 0; i < resolutions.Length; i++)
        {
            dropdownMenu.options[i].text = resolutions[i].ToString();
            dropdownMenu.value = i;

            dropdownMenu.onValueChanged.AddListener(delegate { Screen.SetResolution(resolutions[dropdownMenu.value].width, resolutions[dropdownMenu.value].height, true); });
        }


    }


    // Update is called once per frame
    void Update ()
    {
        volDisplay = vol.value * 100f;

        volText.text = volDisplay.ToString("F0");
	}

    public void CycleResolution()
    {

    }

    //public void Fullscreen()
    //{
    //    fullScr = !fullScr;
    //    fullScrTogg.isOn = fullScr;
    //}

    public void Apply()
    {
        if (fullScrTogg.isOn)
            Screen.fullScreen = true;
        else
            Screen.fullScreen = false;

        AudioListener.volume = vol.value;

        Back();
    }
    
    public void Back()
    {
        vol.value = AudioListener.volume;
        volText.text = volDisplay.ToString("F0");
        bool menu = GameObject.Find("MenuControl").GetComponent<Menu>().optionsMenu = false;
    }


}
