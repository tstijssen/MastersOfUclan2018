using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreBoardToggle : MonoBehaviour {

    public GameObject scoreboard;

    bool scoresEnabled = false;

	// Update is called once per frame
	void Update () {
        
		if(Input.GetKey(KeyCode.Tab))
        {
            scoreboard.SetActive(true);
        }
        else
        {
            scoreboard.SetActive(false);

        }
    }
}
