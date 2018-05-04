using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightFlicker : MonoBehaviour {

    Light light;

	// Use this for initialization
	void Start () {
        light = GetComponent<Light>();
	}
	
	// Update is called once per frame
	void Update () {
        if (Random.value > 0.25) //a random chance
        {
            if (light.enabled == true) //if the light is on...
            {
                light.enabled = false; //turn it off
            }
            else
            {
                light.enabled = true; //turn it on
            }
        }
    }
}
