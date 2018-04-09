using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SplitSceen : MonoBehaviour {

    public Camera[] cams;
    public Camera cam1;
    public Camera cam2;
    public Camera cam3;
    public Camera cam4;

    public int NumOfPlayers;

    private void Start()
    {
        ChangeSplitScreen();
    }

    private void Update()
    {
        ChangeSplitScreen();

    }

    public void ChangeSplitScreen()
    {

        for (int i = 0; i < cams.Length; ++i)
        {
            if (cams[i].isActiveAndEnabled)
            {
                
            }
        }

        switch (NumOfPlayers)
        {
            case 1: // single camera [  ]
                cam1.rect = new Rect(0,0,1,1);

                cam2.gameObject.SetActive(false);
                cam3.gameObject.SetActive(false);
                cam4.gameObject.SetActive(false);
                break;
            case 2: // horizontal side by side [][]
                cam1.rect = new Rect(0, 0, 0.5f, 1);
                cam2.rect = new Rect(0.5f, 0, 0.5f, 1);

                cam2.gameObject.SetActive(true);
                cam3.gameObject.SetActive(false);
                cam4.gameObject.SetActive(false);
                break;

            case 3: // single vertical on left, two vertical on right []:
                cam1.rect = new Rect(0, 0, 0.5f, 0.5f);
                cam2.rect = new Rect(0, 0.5f, 0.5f, 0.5f);
                cam3.rect = new Rect(0.5f, 0.0f, 0.5f, 0.5f);

                cam2.gameObject.SetActive(true);
                cam3.gameObject.SetActive(true);
                cam4.gameObject.SetActive(false);
                break;

            case 4: // 2x2 grid ::
                cam1.rect = new Rect(0, 0, 0.5f, 0.5f);
                cam2.rect = new Rect(0, 0.5f, 0.5f, 0.5f);
                cam3.rect = new Rect(0.5f, 0, 0.5f, 0.5f);
                cam4.rect = new Rect(0.5f, 0.5f, 0.5f, 0.5f);

                cam2.gameObject.SetActive(true);
                cam3.gameObject.SetActive(true);
                cam4.gameObject.SetActive(true);
                break;
            default:
                break;
        }
    }

}
