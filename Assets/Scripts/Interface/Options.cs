using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Options : MonoBehaviour {

    public Button fullScrBtn;
    public Button resBtn;

    List<string> resolutionsList;
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

        //foreach (Resolution res in resolutions)
        //{
        for (int i = 0; i < resolutions.Length; ++i)
        {

            dropdownMenu.options.Add(new Dropdown.OptionData() { text = resolutions[i].ToString() });
            dropdownMenu.value = i;

        }


    }


    // Update is called once per frame
    void Update ()
    {
        volDisplay = vol.value * 100f;
        volText.text = volDisplay.ToString("F0");
        dropdownMenu.onValueChanged.AddListener(delegate { Screen.SetResolution(resolutions[dropdownMenu.value].width, resolutions[dropdownMenu.value].height, fullScrTogg); });
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

    

        //Screen.SetResolution(,  true);

        AudioListener.volume = vol.value;

        Back();
    }
    
    public void Back()
    {
        



        vol.value = AudioListener.volume;
        volText.text = volDisplay.ToString("F0");
        bool menu = GameObject.Find("MenuControl").GetComponent<Menu>().optionsMenu = false;
    }

    string ResToString(Resolution res)
    {
        return res.width + " x " + res.height;
    }
}
