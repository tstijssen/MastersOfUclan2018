using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SplitSceen : MonoBehaviour {

    public Camera[] cams;
    public Camera backupCam;
    public int NumOfPlayers;
    int m_PlayerCount;

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

        int index = 0;
        switch (NumOfPlayers)
        {
            case 1: // single camera
                for (int i = 0; i < cams.Length; ++i)
                {
                    if (cams[i].isActiveAndEnabled)
                    {
                        cams[i].rect = new Rect(0, 0, 1, 1);
                    }
                }
                //backupCam.gameObject.SetActive(false);
                break;
            case 2: // two cameras one above the other
                for (int i = 0; i < cams.Length; ++i)
                {
                    if (cams[i].isActiveAndEnabled)
                    {
                        index++;
                        if (index == 1)
                            cams[i].rect = new Rect(0f, 0f, 1f, 0.5f);
                        else
                            cams[i].rect = new Rect(0f, 0.5f, 1f, 0.5f);
                    }
                }
                //backupCam.gameObject.SetActive(false);

                break;

            case 3: // 3 cameras in a 2x2 grid, with a overlook camera in the 4th spot
                for (int i = 0; i < cams.Length; ++i)
                {
                    if (cams[i].isActiveAndEnabled)
                    {
                        index++;
                        if (index == 1)
                            cams[i].rect = new Rect(0f, 0f, 0.5f, 0.5f);
                        else if(index == 2)
                            cams[i].rect = new Rect(0f, 0.5f, 0.5f, 0.5f);
                        else
                            cams[i].rect = new Rect(0.5f, 0.0f, 0.5f, 0.5f);

                        backupCam.gameObject.SetActive(true);
                        backupCam.rect = new Rect(0.5f, 0.5f, 0.5f, 0.5f);
                    }
                }
                break;

            case 4: // 4 cameras in a 2x2 grid
                for (int i = 0; i < cams.Length; ++i)
                {
                    if (cams[i].isActiveAndEnabled)
                    {
                        index++;
                        if (index == 1)
                            cams[i].rect = new Rect(0, 0, 0.5f, 0.5f);
                        else if (index == 2)
                            cams[i].rect = new Rect(0, 0.5f, 0.5f, 0.5f);
                        else if (index == 3)
                            cams[i].rect = new Rect(0.5f, 0, 0.5f, 0.5f);
                        else
                            cams[i].rect = new Rect(0.5f, 0.5f, 0.5f, 0.5f);
                    }
                }
                //backupCam.gameObject.SetActive(false);

                break;
            default:
                break;
        }
    }

}
