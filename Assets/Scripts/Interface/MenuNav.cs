using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuNav : MonoBehaviour {

    const int BUTTON_NUM = 6;

    public Button offCoop;
    public Button offVs;
    public Button onVs;
    public Button options;
    public Button quit;
    
    public Button[,] buttons = new Button[BUTTON_NUM/2, BUTTON_NUM/2];

    public Image selector;

    int x, y = 0;

	// Use this for initialization
	void Start ()
    {
		for (int i = 0; i < BUTTON_NUM/2; i++)
        {
            for (int j = BUTTON_NUM / 2; j < BUTTON_NUM; j++)
            {

            }
        }

        
    }
	
	// Update is called once per frame
	void Update ()
    {
		      
        selector.transform.position = buttons[x, y].transform.position;
        


	}

    

}
