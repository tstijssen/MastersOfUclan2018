using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AIHP : MonoBehaviour {

    public int health;
    public Slider hpBar;

	// Use this for initialization
	void Start ()
    {
	}
	
	// Update is called once per frame
	void Update () {
		
        hpBar.value = health;
	}
}
